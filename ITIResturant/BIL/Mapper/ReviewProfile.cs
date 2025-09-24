using Restaurant.BLL.ModelVM.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Mapper
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewVm>();
            CreateMap<ReviewCreateVm, Review>();
        }
    }
}
