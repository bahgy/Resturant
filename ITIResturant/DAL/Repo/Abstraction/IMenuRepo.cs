
namespace Restaurant.DAL.Repo.Abstraction
{
    public interface IMenuRepo
    {
        Task<List<Category>> GetAllCategoriesWithProductsAsync();
    }
}
