
using Restaurant.BLL.ModelVM.MenuVM;
using Restaurant.BLL.ModelVM.Review;

namespace Restaurant.BLL.ModelVM.HomeIndexVm
{
    public class HomeIndexVm
    {
        public IEnumerable<MenuVM.MenuVM> Menu { get; set; }
        public IEnumerable<ReviewVm>? Reviews { get; set; }
    }
}
