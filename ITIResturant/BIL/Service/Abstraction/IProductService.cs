namespace Restaurant.BLL.Service.Abstraction
{
    public interface IProductService
    {
        Task<(bool hasError, string? message, IEnumerable<ProductVM>? products)> GetAllAsync();
        Task<(bool hasError, string? message, ProductVM? product)> GetByIdAsync(int id);
        Task<(bool hasError, string? message)> CreateAsync(CreateProductVM model);
        Task<(bool hasError, string? message)> UpdateAsync(EditProductVM model);
        Task<(bool hasError, string? message)> DeleteAsync(int id);
    }
}
