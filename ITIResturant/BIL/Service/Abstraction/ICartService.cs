
namespace Restaurant.BLL.Service.Abstraction
{
    public interface ICartService
    {
        IEnumerable<CartVM> GetAll();
        CartVM GetById(int id);
        void Add(CartVM cartVM);
        void Update(CartVM cartVM);
        void Delete(int id);

    }
}
