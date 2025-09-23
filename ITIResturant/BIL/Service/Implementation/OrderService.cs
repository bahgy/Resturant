

using Hangfire;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Restaurant.DAL.Enum;
using System.Web.Mvc;

namespace Restaurant.BLL.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IOrderItemRepo _orderItemRepository;
        private readonly ICustomerRepo _customerRepository;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepo orderRepository,
            IOrderItemRepo orderItemRepository,
            ICustomerRepo customerRepository,
            IPromoCodeService promoCodeService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _customerRepository = customerRepository;
            _promoCodeService = promoCodeService;
            _mapper = mapper;
        }

        public async Task<(bool IsError, string ErrorMessage, OrderVM Data)> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return (true, "Order not found", null);

            return (false, string.Empty, _mapper.Map<OrderVM>(order));
        }

        public async Task<(bool IsError, string ErrorMessage, IEnumerable<OrderVM> Data)> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return (false, string.Empty, _mapper.Map<IEnumerable<OrderVM>>(orders));
        }

        public async Task<(bool IsError, string ErrorMessage, IEnumerable<OrderVM> Data)> GetByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return (false, string.Empty, _mapper.Map<IEnumerable<OrderVM>>(orders));
        }

        public async Task<(bool IsError, string ErrorMessage, IEnumerable<OrderVM> Data)> GetByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository.GetByStatusAsync(status);
            return (false, string.Empty, _mapper.Map<IEnumerable<OrderVM>>(orders));
        }

        public async Task<(bool IsError, string ErrorMessage, OrderVM Data)> CreateAsync(CreateOrderVM viewModel)
        {
            if (!await _customerRepository.ExistsAsync(viewModel.CustomerId))
                return (true, "Customer not found", null);

            var order = new Order
            {
                CustomerId = viewModel.CustomerId,
                DelivryAddress = viewModel.DelivryAddress,
                PaymentMethod = viewModel.PaymentMethod,
                Status = OrderStatus.Pending,
                PaymentState = PaymentStatus.Pending,
                TimeRequst = DateTime.UtcNow,
                DiscountAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;
            foreach (var itemViewModel in viewModel.OrderItems)
            {
                var orderItem = _mapper.Map<OrderItem>(itemViewModel);
                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.Price * orderItem.Quantity;
            }

            order.TotalAmount = (totalAmount * 0.085m + 5m + totalAmount);

            if (!string.IsNullOrEmpty(viewModel.PromoCode))
            {
                var promoResult = await _promoCodeService.ValidatePromoCodeAsync(viewModel.PromoCode, totalAmount);
                if (promoResult.IsValid)
                {
                    order.PromoCodeId = promoResult.PromoCode.Id;
                    order.DiscountAmount = promoResult.DiscountAmount;
                }
            }

            var created = await _orderRepository.AddAsync(order);
            if (!created)
                return (true, "Failed to create order", null);


            var createdOrder = await _orderRepository.GetByIdAsync(order.Id);

            // Schedule background job: after 3 min -> mark as Delivered
            BackgroundJob.Schedule<IOrderService>(
                service => service.UpdateStatusAsync(new OrderStatusUpdateVM
                {
                    OrderId = createdOrder.Id,
                    Status = OrderStatus.Delivered
                }), TimeSpan.FromSeconds(10));

            return (false, string.Empty, _mapper.Map<OrderVM>(createdOrder));
        }

        public async Task<(bool IsError, string ErrorMessage, bool Data)> UpdateAsync(UpdateOrderVM viewModel)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(viewModel.Id);
            if (existingOrder == null)
                return (true, "Order not found", false);

            if (!string.IsNullOrEmpty(viewModel.DelivryAddress))
                existingOrder.DelivryAddress = viewModel.DelivryAddress;

            if (!string.IsNullOrEmpty(viewModel.Status.ToString()))
                existingOrder.Status = viewModel.Status;

            if (viewModel.PaymentState.HasValue)
                existingOrder.PaymentState = viewModel.PaymentState.Value;

            if (viewModel.PaymentMethod.HasValue)
                existingOrder.PaymentMethod = viewModel.PaymentMethod.Value;

            var updated = await _orderRepository.UpdateAsync(existingOrder);
            if (!updated)
                return (true, "Failed to update order", false);

            return (false, string.Empty, true);
        }

        public async Task<(bool IsError, string ErrorMessage, bool Data)> DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return (true, "Order not found", false);

            var orderItems = await _orderItemRepository.GetByOrderIdAsync(id);
            foreach (var item in orderItems)
            {
                await _orderItemRepository.DeleteAsync(item.Id);
            }

            var deleted = await _orderRepository.DeleteAsync(id);
            if (!deleted)
                return (true, "Failed to delete order", false);

            return (false, string.Empty, true);
        }

        public async Task<(bool IsError, string ErrorMessage, bool Data)> UpdateStatusAsync(OrderStatusUpdateVM viewModel)
        {
            var order = await _orderRepository.GetByIdAsync(viewModel.OrderId);
            if (order == null)
                return (true, "Order not found", false);

            if (order.Status != OrderStatus.Pending)
                return (true, $"Cannot change status because order is already {order.Status}", false);

            // Proceed with update
            var result = await _orderRepository.UpdateOrderStatusAsync(viewModel.OrderId, viewModel.Status);

            if (!result)
                return (true, "Failed to update status", false);

            if (viewModel.EstimatDelivryTime.HasValue)
            {
                order.EstimatDelivryTime = viewModel.EstimatDelivryTime.Value;
                await _orderRepository.UpdateAsync(order);
            }

            return (false, string.Empty, true);
        }


        public async Task<(bool IsError, string ErrorMessage, bool Data)> UpdatePaymentStateAsync(int orderId, PaymentStatus paymentState)
        {
            var result = await _orderRepository.UpdatePaymentStateAsync(orderId, paymentState);
            if (!result)
                return (true, "Failed to update payment state", false);

            return (false, string.Empty, true);
        }

        public async Task<(bool IsError, string ErrorMessage, bool Data)> ApplyPromoCodeAsync(int orderId, string promoCode)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return (true, "Order not found", false);

            decimal orderAmountWithoutDiscount = order.TotalAmount + order.DiscountAmount;

            var promoResult = await _promoCodeService.ValidatePromoCodeAsync(promoCode, orderAmountWithoutDiscount);
            if (!promoResult.IsValid)
                return (true, "Invalid promo code", false);

            order.PromoCodeId = promoResult.PromoCode.Id;
            order.DiscountAmount = promoResult.DiscountAmount;
            order.TotalAmount = orderAmountWithoutDiscount - order.DiscountAmount;

            await _promoCodeService.IncrementUsageAsync(promoCode);

            var updated = await _orderRepository.UpdateAsync(order);
            if (!updated)
                return (true, "Failed to apply promo code", false);

            return (false, string.Empty, true);
        }

        public async Task<(bool IsError, string ErrorMessage, bool Data)> RemovePromoCodeAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return (true, "Order not found", false);

            if (order.PromoCodeId == null)
                return (true, "No promo code applied", false);

            order.TotalAmount += order.DiscountAmount;
            order.PromoCodeId = null;
            order.DiscountAmount = 0;

            var updated = await _orderRepository.UpdateAsync(order);
            if (!updated)
                return (true, "Failed to remove promo code", false);

            return (false, string.Empty, true);
        }

        public async Task<(bool IsError, string ErrorMessage, decimal Data)> CalculateOrderTotalWithDiscountAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return (true, "Order not found", 0);

            return (false, string.Empty, order.TotalAmount - order.DiscountAmount);
        }
    }
}

