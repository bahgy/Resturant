


namespace Restaurant.BLL.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Create Order mapping (VM -> Entity)
            CreateMap<CreateOrderVM, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.PromoCode, opt => opt.Ignore()) // handled separately if exists
                .ForMember(dest => dest.Payments, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending")) // default status
                .ForMember(dest => dest.TimeRequst, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Update Order mapping (VM -> Entity)
            CreateMap<UpdateOrderVM, Order>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // Order entity -> OrderVM (to show details)
            CreateMap<Order, OrderVM>()
     .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
     .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
     .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
     .ForMember(dest => dest.PromoCode, opt => opt.MapFrom(src => src.PromoCode != null ? src.PromoCode.Code : null))
     .ForMember(dest => dest.PromoCodeDescription, opt => opt.MapFrom(src => src.PromoCode != null ? src.PromoCode.Description : null))
     .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.OrderItems.Count));


            // OrderStatusUpdate mapping
            CreateMap<OrderStatusUpdateVM, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.EstimatDelivryTime, opt => opt.MapFrom(src => src.EstimatDelivryTime ?? DateTime.MinValue))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // OrderItem mapping
            CreateMap<CreateOrderItemVM, OrderItem>();
            CreateMap<OrderItem, OrderItemVM>();
        }
    }
}
        
