global using Restaurant.BLL.ModelVM.Review;

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
