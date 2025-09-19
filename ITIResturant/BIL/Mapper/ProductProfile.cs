

namespace Restaurant.BLL.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateProductVM, Product>();
            CreateMap<EditProductVM, Product>();
            CreateMap<Product, EditProductVM>();
        }
    }
}
