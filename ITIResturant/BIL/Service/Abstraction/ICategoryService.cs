using Restaurant.BLL.ModelVM.CategoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface ICategoryService
    {
        Task<(bool hasError, string? message, List<CategoryVM>? categories)> GetAllAsync();
        Task<(bool hasError, string? message, CategoryVM? category)> GetByIdAsync(int id);
        Task<(bool hasError, string? message)> CreateAsync(CategoryVM model);
        Task<(bool hasError, string? message)> UpdateAsync(CategoryVM model);
        Task<(bool hasError, string? message)> DeleteAsync(int id);
    }
}
