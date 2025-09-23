using Microsoft.AspNetCore.Mvc;
using Restaurant.BLL.ModelVMOrder;
using Restaurant.DAL.Enum;

namespace Restaurant.PL.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(ValidateUserExistsFilter))]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        // GET: /Order/OrderHistory
        public async Task<IActionResult> OrderHistory()
        {
            var customerId = _userService.GetCurrentCustomerId(User); 
            if (customerId == null)
                return RedirectToAction("Login", "Account");
            var (isError, _, orders) = await _orderService.GetByCustomerIdAsync((int)customerId);

            if (isError)
                return View(new List<OrderVM>());

            return View(orders);
        }

        // POST: /Order/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var (isError, message, order) = await _orderService.GetByIdAsync(id);

            if (isError || order == null)
                return Json(new { success = false, message = "Order not found" });

            if (order.Status != OrderStatus.Pending) // or UnderMade
                return Json(new { success = false, message = "Cannot cancel after preparation started" });

            var (cancelError, cancelMsg, _) = await _orderService.UpdateStatusAsync(
                new OrderStatusUpdateVM { OrderId = id, Status = OrderStatus.Cancelled });

            if (cancelError)
                return Json(new { success = false, message = cancelMsg });

            return Json(new { success = true, message = "Order canceled successfully" });
        }
    }
}
