using Microsoft.AspNetCore.Http;
namespace Restaurant.BLL.ModelVM.ProductVM

{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please upload image to product.")]
        public IFormFile? ImageFile { get; set; }
    }
}
