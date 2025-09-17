

namespace Restaurant.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; }
        public List< ShopingCartItem> shopingCartItems { get; set; } 
        public List<OrderItem> OrderItems { get; set; }
    }
                                                                
}
