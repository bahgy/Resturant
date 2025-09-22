using Restaurant.BLL.ModelVM.CartVM;
namespace Restaurant.BLL.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // Map ShopingCartItem → CartItemVM
            CreateMap<ShopingCartItem, CartItemVM>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl)) 
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            // Map Cart → CartVM
            CreateMap<Cart, CartVM>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ShopingCartItem));
        }
    }
}
