using Restaurant.BLL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Mapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // UserVM → Customer
            CreateMap<UserVM, Customer>()
                                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))

                .ForMember(dest => dest.DefaultDeliveryAddress, opt => opt.MapFrom(src => src.Address))
                
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Customer → UserVM
            CreateMap<Customer, UserVM>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.DefaultDeliveryAddress))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(_ => "Customer"))
                .ReverseMap();
        }
    }
}
