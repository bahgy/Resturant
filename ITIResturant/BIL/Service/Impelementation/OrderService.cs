using AutoMapper;
using BIL.ModelVM.Order;
using BIL.Service.Abstraction;
using DAL.Entities;
using DAL.Repos.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL.Service.Implementation
{
    public class OrderService:IOrderService
    {

        private readonly IOrderRepo _orderRepository;
        private readonly IOrderItemRepo _orderItemRepository;
        private readonly ICustomerRepo _customerRepository;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IMapper _mapper;

        public OrderService( IOrderRepo orderRepository,IOrderItemRepo orderItemRepository
            ,ICustomerRepo customerRepository,IPromoCodeService promoCodeService,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _customerRepository = customerRepository;
            _promoCodeService = promoCodeService;
            _mapper = mapper;
        }

        public async Task<OrderVM> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            return _mapper.Map<OrderVM>(order);
        }

        public async Task<IEnumerable<OrderVM>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderVM>>(orders);
        }

        public async Task<IEnumerable<OrderVM>> GetByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<OrderVM>>(orders);
        }

        public async Task<IEnumerable<OrderVM>> GetByStatusAsync(string status)
        {
            var orders = await _orderRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderVM>>(orders);
        }

        public async Task<OrderVM> CreateAsync(CreateOrderVM viewModel)
        {
           
            if (!await _customerRepository.ExistsAsync(viewModel.CustomerId))
                return null;

            var order = new Order
            {
                customerId = viewModel.CustomerId,
                DelivryAddress = viewModel.DelivryAddress,
                PaymentMethod = viewModel.PaymentMethod,
                status = "Pending",
                paymentSTate = "Pending",
                TimeRequst = DateTime.UtcNow,
                DiscountAmount = 0, 
                OrderItems = new List<OrderItem>()
            };

            
            decimal totalAmount = 0;
            foreach (var itemViewModel in viewModel.OrderItems)
            {
                var orderItem = _mapper.Map<OrderItem>(itemViewModel);
                orderItem.OrderId = order.Id; 
                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.Price * orderItem.Quantity;
            }

            order.TotalAmount = totalAmount;

            
            if (!string.IsNullOrEmpty(viewModel.PromoCode))
            {
                var promoResult = await _promoCodeService.ValidatePromoCodeAsync(viewModel.PromoCode, totalAmount);
                if (promoResult.IsValid)
                {
                    order.PromoCodeId = promoResult.PromoCode.Id;
                    order.DiscountAmount = promoResult.DiscountAmount;
                    order.TotalAmount -= order.DiscountAmount;

                    
                    await _promoCodeService.IncrementUsageAsync(viewModel.PromoCode);
                }
            }

            var created = await _orderRepository.AddAsync(order);
            if (!created) return null;

            
            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = order.Id;
                await _orderItemRepository.AddAsync(orderItem);
            }

            return await GetByIdAsync(order.Id);
        }

        public async Task<bool> UpdateAsync(UpdateOrderVM viewModel)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(viewModel.Id);
            if (existingOrder == null) return false;

            
            if (!string.IsNullOrEmpty(viewModel.DelivryAddress))
                existingOrder.DelivryAddress = viewModel.DelivryAddress;

            if (!string.IsNullOrEmpty(viewModel.Status))
                existingOrder.status = viewModel.Status;

            if (!string.IsNullOrEmpty(viewModel.PaymentState))
                existingOrder.paymentSTate = viewModel.PaymentState;

            if (!string.IsNullOrEmpty(viewModel.PaymentMethod))
                existingOrder.PaymentMethod = viewModel.PaymentMethod;

            return await _orderRepository.UpdateAsync(existingOrder);
        }

        public async Task<bool> DeleteAsync(int id)
        {
          
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(id);
            foreach (var item in orderItems)
            {
                await _orderItemRepository.DeleteAsync(item.Id);
            }

           
            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateStatusAsync(OrderStatusUpdateVM viewModel)
        {
            var result = await _orderRepository.UpdateOrderStatusAsync(viewModel.OrderId, viewModel.Status);

            if (result && viewModel.EstimatDelivryTime.HasValue)
            {
                var order = await _orderRepository.GetByIdAsync(viewModel.OrderId);
                if (order != null)
                {
                    order.EstimatDelivryTime = viewModel.EstimatDelivryTime.Value;
                    await _orderRepository.UpdateAsync(order);
                }
            }

            return result;
        }

        public async Task<bool> UpdatePaymentStateAsync(int orderId, string paymentState)
        {
            return await _orderRepository.UpdatePaymentStateAsync(orderId, paymentState);
        }

        public async Task<bool> ApplyPromoCodeAsync(int orderId, string promoCode)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            
            decimal orderAmountWithoutDiscount = order.TotalAmount + order.DiscountAmount;

            var promoResult = await _promoCodeService.ValidatePromoCodeAsync(promoCode, orderAmountWithoutDiscount);
            if (!promoResult.IsValid) return false;

            order.PromoCodeId = promoResult.PromoCode.Id;
            order.DiscountAmount = promoResult.DiscountAmount;
            order.TotalAmount = orderAmountWithoutDiscount - order.DiscountAmount;

            
            await _promoCodeService.IncrementUsageAsync(promoCode);

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> RemovePromoCodeAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.PromoCodeId == null) return false;

            
            order.TotalAmount += order.DiscountAmount;
            order.PromoCodeId = null;
            order.DiscountAmount = 0;

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<decimal> CalculateOrderTotalWithDiscountAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return 0;

            return order.TotalAmount - order.DiscountAmount;
        }
    }
}

