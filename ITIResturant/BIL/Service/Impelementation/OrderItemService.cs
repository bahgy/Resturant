using AutoMapper;
using Restaurant.BLL.ModelVM.OrderItem;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Repos.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Implementation
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepo _orderItemRepository;
        private readonly IOrderRepo _orderRepository;
        private readonly IProductRepo _productRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepo orderItemRepository,IOrderRepo orderRepository,IProductRepo productRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderItemVM> GetByIdAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null) return null;

            return _mapper.Map<OrderItemVM>(orderItem);
        }

        public async Task<IEnumerable<OrderItemVM>> GetAllAsync()
        {
            var orderItems = await _orderItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderItemVM>>(orderItems);
        }

        public async Task<IEnumerable<OrderItemVM>> GetByOrderIdAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemVM>>(orderItems);
        }

        public async Task<IEnumerable<OrderItemVM>> GetByProductIdAsync(int productId)
        {
            var orderItems = await _orderItemRepository.GetByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<OrderItemVM>>(orderItems);
        }

        public async Task<bool> CreateAsync(CreateOrderItemVM viewModel)
        {
            
            if (!await _productRepository.ExistsAsync(viewModel.ProductId))
                return false;

            if (!await _orderRepository.ExistsAsync(viewModel.OrderId))
                return false;

            var orderItem = _mapper.Map<OrderItem>(viewModel);
            return await _orderItemRepository.AddAsync(orderItem);
        }

        public async Task<bool> UpdateAsync(UpdateOrderItemVM viewModel)
        {
            var existingOrderItem = await _orderItemRepository.GetByIdAsync(viewModel.Id);
            if (existingOrderItem == null) return false;

            
            _mapper.Map(viewModel, existingOrderItem);

            return await _orderItemRepository.UpdateAsync(existingOrderItem);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderItemRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _orderItemRepository.ExistsAsync(id);
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            return await _orderItemRepository.GetTotalAmountByOrderIdAsync(orderId);
        }
    }
}

