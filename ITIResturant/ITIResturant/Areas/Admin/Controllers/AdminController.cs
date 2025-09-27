

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.BLL.ModelVM.AdminVM;
using Restaurant.BLL.ModelVMOrderItem;
using Restaurant.PL.Filters;

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IOrderItemService _orderItemService;
        public AdminController(IUserService userService, IOrderService orderService, IProductService productService, IOrderItemService orderItemService)
        {
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
            _orderItemService = orderItemService;
        }

        // GET: /Admin
        public async Task<IActionResult> Index(string period = "week")
        {
            var model = new AdminDashboardVM();

            // --- set report period
            if (period == "today")
            {
                model.ReportStart = DateTime.Today;
                model.ReportEnd = DateTime.Today.AddDays(1).AddTicks(-1);
            }
            else if (period == "week")
            {
                model.ReportStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                model.ReportEnd = DateTime.Today.AddDays(1).AddTicks(-1);
            }
            else if (period == "month")
            {
                model.ReportStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                model.ReportEnd = model.ReportStart.AddMonths(1).AddTicks(-1);
            }

            // --- users / products counts
            var getUsers = await _userService.GetAllUsersAsync();
            model.Users = getUsers.users?.Count() ?? 0;

            var getProducts = await _productService.GetAllAsync();
            model.TotalProducts = getProducts.products?.Count() ?? 0;

            // --- orders 
            var getOrders = await _orderService.GetAllAsync();
            var allOrders = getOrders.Data ?? Enumerable.Empty<BLL.ModelVMOrder.OrderVM>();

            var filteredOrders = allOrders
                .Where(o => o.TimeRequst >= model.ReportStart && o.TimeRequst <= model.ReportEnd)
                .ToList();

            model.TotalOrders = filteredOrders.Count;
            model.PendingOrders = filteredOrders.Count(o => o.Status == DAL.Enum.OrderStatus.Pending);
            model.CompletedOrders = filteredOrders.Count(o => o.Status == DAL.Enum.OrderStatus.Delivered);

            // average 
            model.AverageOrderVal = filteredOrders
                .Select(o => o.FinalAmount)
                .DefaultIfEmpty(0m)
                .Average();

            // total income in the period
            model.TotalIncome = filteredOrders
                .Select(o => o.FinalAmount)
                .DefaultIfEmpty(0m)
                .Sum();

            // --- top products 
            var getOrderItems = await _orderItemService.GetAllAsync();
            var allOrderItems = getOrderItems ?? Enumerable.Empty<OrderItemVM>();

           
            var filteredOrderIds = new HashSet<int>(
                filteredOrders.Select(o => o.Id)
            );

            model.MostOrderedProducts = allOrderItems
                .Where(oi => filteredOrderIds.Contains(oi.OrderId)) 
                .GroupBy(oi => oi.ProductName)
                .Select(g =>
                {
                    var totalQtyDecimal = g.Sum(x => x.Quantity);
                    var totalQtyInt = Convert.ToInt32(Math.Round(totalQtyDecimal)); 
                    return new TopProductVM
                    {
                        ProductName = g.Key,
                        Quantity = totalQtyInt
                    };
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToList();


            return View(model);
        }

    }


}
