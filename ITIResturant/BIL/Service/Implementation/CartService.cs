using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.BLL.ModelVM.CartVM;
using Restaurant.DAL.Entities;

namespace Restaurant.BLL.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepo;
        private readonly IMapper _mapper;

        public CartService(ICartRepo cartRepository, IMapper mapper)
        {
            _cartRepo = cartRepository;
            _mapper = mapper;
        }

        public async Task<(bool success, string? message)> AddToCartAsync(int productId, int customerId, int quantity = 1)
        {
            var cart = await _cartRepo.GetCartByCustomerIdAsync(customerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customerId,
                    ShopingCartItem = new List<ShopingCartItem>()
                };
                await _cartRepo.AddCartAsync(cart);
            }

            var existingItem = cart.ShopingCartItem
                .FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.ShopingCartItem.Add(new ShopingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _cartRepo.SaveChangesAsync();
            return (true, "Product added to cart successfully.");
        }

        public async Task<CartVM> GetCartAsync(int customerId)
        {
            var cart = await _cartRepo.GetCartByCustomerIdAsync(customerId);

            if (cart == null || cart.ShopingCartItem == null || !cart.ShopingCartItem.Any())
            {
                return new CartVM
                {
                    Items = new List<CartItemVM>()
                };
            }

            var cartVM = _mapper.Map<CartVM>(cart);

            return cartVM;
        }
        public async Task<(bool success, string? message)> UpdateQuantityAsync(int customerId, int productId, int quantity)
        {
            var cart = await _cartRepo.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return (false, "Cart not found");

            var item = cart.ShopingCartItem.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return (false, "Item not found in cart");

            if (quantity <= 0)
                cart.ShopingCartItem.Remove(item);
            else
                item.Quantity = quantity;

            await _cartRepo.SaveChangesAsync();
            return (true, "Quantity updated");
        }

        public async Task<(bool success, string? message)> RemoveItemAsync(int customerId, int productId)
        {
            var cart = await _cartRepo.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return (false, "Cart not found");

            var item = cart.ShopingCartItem.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return (false, "Item not found in cart");

            cart.ShopingCartItem.Remove(item);

            await _cartRepo.SaveChangesAsync();
            return (true, "Item removed from cart");
        }

        public async Task<(bool success, string? message)> ClearCartAsync(int customerId)
        {
            try
            {
                await _cartRepo.ClearCartAsync(customerId);
                return (true, "Cart cleared successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error clearing cart: {ex.Message}");
            }
        }


    }
}
