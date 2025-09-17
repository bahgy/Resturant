

namespace Restaurant.BLL.Mapper
{
    public class PromoCodeProfile : Profile
    {
        public PromoCodeProfile()
        {
            // Entity to ViewModel
            CreateMap<PromoCode, PromoCodeVM>()
                .ForMember(dest => dest.DiscountType,
                           opt => opt.MapFrom(src => src.DiscountType.ToString()))
                .ForMember(dest => dest.MaxUsedTime,
                           opt => opt.MapFrom(src => src.MaxUsedtime));

            // ViewModel to Entity
            CreateMap<CreatePromoCodeVM, PromoCode>()
                .ForMember(dest => dest.DiscountType,
                           opt => opt.MapFrom(src =>
                               src.DiscountType == "Percentage" ? DiscountType.Percentage : DiscountType.FixedAmount))
                .ForMember(dest => dest.MaxUsedtime,
                           opt => opt.MapFrom(src => src.MaxUsedTime));

            CreateMap<UpdatePromoCodeVM, PromoCode>()
                .ForMember(dest => dest.MaxUsedtime,
                           opt => opt.MapFrom(src => src.MaxUsedTime))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

