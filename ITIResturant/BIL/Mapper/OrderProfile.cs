


namespace Restaurant.BLL.Mapper
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            // Entity to ViewModel
            CreateMap<Order, OrderVM>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.PaymentState, opt => opt.MapFrom(src => src.paymentSTate))
                .ForMember(dest => dest.CustomerName,
                           opt => opt.MapFrom(src => src.Customer != null ?
                               $"{src.Customer.FirstName} {src.Customer.LastName}" : "Unknown Customer"))
                .ForMember(dest => dest.CustomerEmail,
                           opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Email : null))
                .ForMember(dest => dest.CustomerPhone,
                           opt => opt.MapFrom(src => src.Customer != null ? src.Customer.PhoneNumber : null))
                .ForMember(dest => dest.PromoCode,
                           opt => opt.MapFrom(src => src.PromoCode != null ? src.PromoCode.Code : null))
                .ForMember(dest => dest.PromoCodeDescription,
                           opt => opt.MapFrom(src => src.PromoCode != null ? src.PromoCode.Description : null))
                .ForMember(dest => dest.TotalItems,
                           opt => opt.MapFrom(src => src.OrderItems != null ? src.OrderItems.Count : 0))
                .ForMember(dest => dest.FinalAmount,
                           opt => opt.MapFrom(src => src.TotalAmount - src.DiscountAmount))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount));

            // ViewModel to Entity
            CreateMap<UpdateOrderVM, Order>()
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.paymentSTate, opt => opt.MapFrom(src => src.PaymentState))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
        
