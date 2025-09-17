
namespace Restaurant.PL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            //  Register
            CreateMap<RegisterVM, Customer>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.IsAdmin
                    ? UserTypeEnum.Admin
                    : UserTypeEnum.Customer));

            CreateMap<Customer, RegisterVM>().ReverseMap();

            //  Profile - Update Info
            CreateMap<UpdateProfileVM, Customer>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<Customer, UpdateProfileVM>().ReverseMap();

            // AppUser → UserVM
            CreateMap<AppUser, UserVM>()
                .ForMember(dest => dest.UserType,
                           opt => opt.MapFrom(src => src.UserType.ToString()));

            // UserVM → AppUser
            CreateMap<UserVM, AppUser>()
                .ForMember(dest => dest.UserType,
                           opt => opt.MapFrom(src => Enum.Parse<UserTypeEnum>(src.UserType)));
            // VM → Customer
            CreateMap<UserVM, Customer>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Identity requires UserName
                .ForMember(dest => dest.DefaultDeliveryAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.EmailVerified, opt => opt.Ignore()) // keep default
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            // Customer → VM
            CreateMap<Customer, UserVM>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.DefaultDeliveryAddress))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(_ => "Customer")); // enforce type
            CreateMap<Customer, UserVM>().ReverseMap();
        }
    }
}
