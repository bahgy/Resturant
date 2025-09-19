
namespace Restaurant.BLL.Service.Implementation
{
    public class CartService : ICartService
    {
        private readonly RestaurantDbContext _context;

        public CartService(RestaurantDbContext context)
        {
            _context = context;
        }

        public void Add(CartVM cartVM)
        {
            
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CartVM> GetAll()
        {
            throw new NotImplementedException();
        }

        public CartVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CartVM cartVM)
        {
            throw new NotImplementedException();
        }
    }
}
