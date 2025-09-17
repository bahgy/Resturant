

namespace Restaurant.DAL.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        // Foreign Key
        public int CustomerId { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
        public List<ShopingCartItem> ShopingCartItem { get; set; }
    }

}
