

namespace Restaurant.DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 
        public List<Product>Products { get; set; }
    }
}
