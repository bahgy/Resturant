using Restaurant.BLL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.BLL.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // RegisterVM → Customer
            CreateMap<RegisterVM, Customer>()
                                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))

                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.UserType,
                           opt => opt.MapFrom(src => src.IsAdmin ? UserTypeEnum.Admin : UserTypeEnum.Customer))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.EmailVerified, opt => opt.Ignore());

            // Customer → RegisterVM
            CreateMap<Customer, RegisterVM>().ReverseMap();
        }
    }
}
