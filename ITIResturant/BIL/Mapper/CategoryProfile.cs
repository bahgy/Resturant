
namespace Restaurant.BLL.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Entity → VM
            CreateMap<Category, CategoryVM>();

            // VM → Entity
            CreateMap<CategoryVM, Category>();
        }
    }
}
