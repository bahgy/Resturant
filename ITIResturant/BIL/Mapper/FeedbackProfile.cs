using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Mapper
{
   public class FeedbackProfile :Profile
    {
        public FeedbackProfile()
        {
            //Entity  VM
            CreateMap<Feedback, GetAllFeedbackVM>()
                  .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FirstName : ""))
                .ForMember(dest => dest.OrderDescription, opt => opt.MapFrom(src => src.Order != null ? $"Order #{src.Order.Id}" : ""));
            CreateMap<Feedback, UpdateFeedbackVM>();

            CreateMap<Feedback, CreateFeedbackVM>()
            .ForMember(dest => dest.SubmittedDate, opt => opt.MapFrom(src => DateTime.Now)); // Auto-set submission date

            // VMs -> Entity
            CreateMap<CreateFeedbackVM, Feedback>();
            





        }


      
    }
}
