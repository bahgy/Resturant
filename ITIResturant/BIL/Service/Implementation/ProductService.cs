

using Restaurant.BLL.Helper;

namespace Restaurant.BLL.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<(bool hasError, string? message, IEnumerable<ProductVM>? products)> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            if (products == null || !products.Any())
                return (true, "No products found.", null);

            var result = _mapper.Map<IEnumerable<ProductVM>>(products);
            return (false, null, result);
        }

        public async Task<(bool hasError, string? message, ProductVM? product)> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return (true, "Product not found.", null);

            var result = _mapper.Map<ProductVM>(product);
            return (false, null, result);
        }

        public async Task<(bool hasError, string? message)> CreateAsync(CreateProductVM model)
        {
            try
            {
                var product = _mapper.Map<Product>(model);
                if (model.ImageFile != null)
                {
                    string fileName = UploadImage.UploadFile("Files", model.ImageFile);
                    product.ImageUrl = fileName; 
                }
                await _repo.AddAsync(product);
                return (false, null);
            }
            catch (Exception ex)
            {
                return (true, $"Error creating product: {ex.Message}");
            }
        }

        public async Task<(bool hasError, string? message)> UpdateAsync(EditProductVM model)
        {
            var product = await _repo.GetByIdAsync(model.Id);
            if (product == null)
                return (true, "Product not found.");

            try
            {
                _mapper.Map(model, product);
                if (model.ImageFile != null)
                {
                    // remove old file
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        UploadImage.RemoveFile("Files", product.ImageUrl);
                    }

                    string fileName = UploadImage.UploadFile("Files", model.ImageFile);
                    product.ImageUrl = fileName;
                }
                await _repo.UpdateAsync(product);
                return (false, null);
            }
            catch (Exception ex)
            {
                return (true, $"Error updating product: {ex.Message}");
            }
        }

        public async Task<(bool hasError, string? message)> DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return (true, "Product not found.");

            try
            {
                await _repo.DeleteAsync(id);
                return (false, null);
            }
            catch (Exception ex)
            {
                return (true, $"Error deleting product: {ex.Message}");
            }
        }
    }
}
