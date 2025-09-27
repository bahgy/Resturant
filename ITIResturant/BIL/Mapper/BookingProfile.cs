using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Mapper
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            // Entity → ViewModels
            CreateMap<Booking, GetAllBookingVM>();
            CreateMap<Booking, EditBookingVM>();

            // ViewModels → Entity
            CreateMap<CreateBookingVM, Booking>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // enum
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId.Value));

            CreateMap<EditBookingVM, Booking>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // enum
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId));
        }
    }
}
