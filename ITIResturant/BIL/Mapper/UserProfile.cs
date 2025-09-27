using Restaurant.BLL.Helper;

namespace Restaurant.BLL.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // UpdateProfileVM → AppUser
            CreateMap<UpdateProfileVM, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore());

            CreateMap<AppUser, UpdateProfileVM>().ReverseMap();

            // AppUser → UserVM
            CreateMap<AppUser, UserVM>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // UserVM → AppUser
            CreateMap<UserVM, AppUser>()
                .ForMember(dest => dest.UserType,
                           opt => opt.MapFrom(src => Enum.Parse<UserTypeEnum>(src.UserType.ToString())))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))

                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore());
        }
    }
}
