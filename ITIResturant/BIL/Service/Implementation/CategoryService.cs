

namespace Restaurant.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly RestaurantDbContext _db;
        private readonly IMapper _mapper;

        public CategoryService(RestaurantDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<(bool, string?, List<CategoryVM>?)> GetAllAsync()
        {
            var categories = await _db.Categories.ToListAsync();
            var result = _mapper.Map<List<CategoryVM>>(categories);
            return (false, null, result);
        }

        public async Task<(bool, string?, CategoryVM?)> GetByIdAsync(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return (true, "Category not found", null);

            var result = _mapper.Map<CategoryVM>(category);
            return (false, null, result);
        }

        public async Task<(bool, string?)> CreateAsync(CategoryVM model)
        {
            // 🔹 Check unique name
            var exists = await _db.Categories
                .AnyAsync(c => c.Name.ToLower() == model.Name.ToLower());

            if (exists)
                return (true, "Category name already exists. Please choose another name.");

            var entity = _mapper.Map<Category>(model);
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync();

            return (false, null);
        }

        public async Task<(bool, string?)> UpdateAsync(CategoryVM model)
        {
            // 🔹 Check if category exists
            var category = await _db.Categories.FindAsync(model.Id);
            if (category == null)
                return (true, "Category not found");

            // 🔹 Ensure unique name (exclude current Id)
            var exists = await _db.Categories
                .AnyAsync(c => c.Name.ToLower() == model.Name.ToLower() && c.Id != model.Id);

            if (exists)
                return (true, "Category name already exists. Please choose another name.");

            _mapper.Map(model, category);
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();

            return (false, null);
        }

        public async Task<(bool, string?)> DeleteAsync(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
                return (true, "Category not found");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return (false, null);
        }
    }
}
