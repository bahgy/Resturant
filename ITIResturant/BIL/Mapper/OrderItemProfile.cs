

namespace Restaurant.BLL.Mapper
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {

            CreateMap<OrderItem, OrderItemVM>()
       .ForMember(dest => dest.ProductName,
                  opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : "Unknown Product"))
       .ForMember(dest => dest.ProductDescription,
                  opt => opt.MapFrom(src => src.Product != null ? src.Product.Description : null))
       .ForMember(dest => dest.OrderStatus,
                  opt => opt.MapFrom(src => src.Order != null ? src.Order.status : null));
            CreateMap<CreateOrderItemVM, OrderItem>();
            CreateMap<UpdateOrderItemVM, OrderItem>();
        }
    }
}
