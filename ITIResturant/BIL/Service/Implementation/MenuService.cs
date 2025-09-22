using Restaurant.BLL.ModelVM.MenuVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProductVM = Restaurant.BLL.ModelVM.MenuVM.ProductVM;

namespace Restaurant.BLL.Service.Implementation
{
    public class MenuService:IMenuService
    {
        private readonly IMenuRepo _MenuRepo;

        public MenuService(IMenuRepo categoryRepo)
        {
            _MenuRepo = categoryRepo;
        }

        public async Task<List<MenuVM>> GetMenuAsync()
        {
            var categories = await _MenuRepo.GetAllCategoriesWithProductsAsync();

            return categories.Select(c => new MenuVM
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                Products = c.Products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToList()
            }).ToList();
        }
    }
}
