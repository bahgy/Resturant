

using Restaurant.BLL.ModelVM.Review;

namespace Restaurant.BLL.Service.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepo reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewVm>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewVm>>(reviews);
        }

        public async Task<ReviewVm?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return _mapper.Map<ReviewVm>(review);
        }

        public async Task AddAsync(string userFullName, ReviewCreateVm model)
        {
            var review = _mapper.Map<Review>(model);
            review.UserName = userFullName;
            review.CreatedAt = DateTime.Now;

            await _reviewRepository.AddAsync(review);
        }

        public async Task DeleteAsync(int id)
        {
            await _reviewRepository.DeleteAsync(id);
        }
    }
}
