

namespace Restaurant.BLL.Model_VM.Cart
{
    public class CartVM
    {

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ItemsCount { get; set; }
    }
}
