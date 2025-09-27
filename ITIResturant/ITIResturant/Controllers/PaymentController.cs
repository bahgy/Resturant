using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.BLL.ModelVMOrder;
using Restaurant.BLL.ModelVMOrderItem;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.DAL.Enum;
using Resturant.BLL.ModelVM.PaymentVM;
using Resturant.BLL.Service.Abstraction;

namespace Restaurant.PL.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(ValidateUserExistsFilter))]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;

        public PaymentController(
            IOrderService orderService,
            IPaymentService paymentService,
            IUserService userService,
            ICartService cartService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _userService = userService;
            _cartService = cartService;
        }

        // GET: /Payment/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var customerId = _userService.GetCurrentCustomerId(User);
            if (customerId == null)
                return RedirectToAction("Login", "Account");

            var cart = await _cartService.GetCartAsync(customerId.Value);
            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            var total = cart.Items.Sum(i => i.Price * i.Quantity);

            var vm = new CreatePaymentVM
            {
                Amount = total * 0.085m + total + 5m, // total with tax + fee
                PaymentMethod = PaymentMethod.CashOnDelivery // default
            };

            return View("Checkout", vm);
        }

        // POST: /Payment/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(CreatePaymentVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(ms => new
                    {
                        field = ms.Key,
                        errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    });

                return Json(new { success = false, message = "Invalid data", details = errors });
            }

            var customerId = _userService.GetCurrentCustomerId(User);
            if (customerId == null)
                return Json(new { success = false, message = "User not logged in" });

            var cart = await _cartService.GetCartAsync(customerId.Value);
            if (cart == null || !cart.Items.Any())
                return Json(new { success = false, message = "Cart is empty" });

            // Create order via order service
            var (orderError, orderMessage, orderVM) = await _orderService.CreateAsync(new CreateOrderVM
            {
                CustomerId = customerId.Value,
                DelivryAddress = model.Address,
                PaymentMethod = model.PaymentMethod,
                OrderItems = cart.Items.Select(i => new CreateOrderItemVM
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });

            if (orderError || orderVM == null || orderVM.Id <= 0)
            {
                return Json(new { success = false, message = string.IsNullOrEmpty(orderMessage) ? "Order creation failed." : orderMessage });
            }

            // Process payment after order is created
            var (paymentError, paymentMessage, paymentVM) = await _paymentService.ProcessPaymentAsync(orderVM, model);

            if (!paymentError && paymentVM != null && paymentVM.Status == PaymentStatus.Paid)
            {
                await _cartService.ClearCartAsync(orderVM.CustomerId);
                return Json(new { success = true, paymentId = paymentVM.Id });
            }

            return Json(new { success = false, message = paymentMessage });
        }

        // GET: /Payment/Success/{id}
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var (error, message, payment) = await _paymentService.GetPaymentByIdAsync(id);

            if (error || payment == null)
                return RedirectToAction("Checkout");

            ViewBag.PaymentId = payment.Id;
            return View(payment);
        }
    }
}
