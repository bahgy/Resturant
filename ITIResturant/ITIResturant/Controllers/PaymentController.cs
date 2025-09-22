using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.BLL.ModelVMOrder;
using Restaurant.BLL.ModelVMOrderItem;
using Restaurant.BLL.Service.Abstraction;
using Restaurant.BLL.Service.Implementation;
using Restaurant.DAL.Enum;
using Resturant.BLL.ModelVm.PaymentVm;
using Resturant.BLL.Service.Abstraction;

namespace ResturantProject.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        public PaymentController(IOrderService orderService, IPaymentService paymentService, IUserService userService, ICartService cartService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _userService = userService;
            _cartService = cartService;
        }

        // GET: /Payment/Checkout/5
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var customerId = _userService.GetCurrentCustomerId(User);
            if (customerId == null) return RedirectToAction("Login", "Account");

            var cart = await _cartService.GetCartAsync(customerId.Value);
            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            // Create order from cart
            var orderVm = await _orderService.CreateAsync(new CreateOrderVM
            {
                CustomerId = customerId.Value,
                DelivryAddress = "Set from checkout form later", // placeholder
                PaymentMethod = PaymentMethod.CashOnDelivery, // default, can change on form
                OrderItems = cart.Items.Select(i => new CreateOrderItemVM
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });

            // Now prepare payment form
            var vm = new CreatePaymentVm
            {
                OrderId = orderVm.Id,
                PaymentMethod = orderVm.PaymentMethod,
                Amount = orderVm.FinalAmount
            };

            return View("PaymentFormWithOrderDetails", vm);
        }


        // POST: /Payment/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(CreatePaymentVm model)
        {
            if (!ModelState.IsValid)
                return View("PaymentFormWithOrderDetails", model);

            var order = await _orderService.GetByIdAsync(model.OrderId);
            if (order == null) return NotFound();

            var paymentOrder = new Order
            {
                Id = order.Id,
                customerId = order.CustomerId
            };

            var payment = await _paymentService.ProcessPaymentAsync(paymentOrder, model.Amount, model.PaymentMethod);

            if (payment.IsSuccsessful)
            {
                return RedirectToAction(nameof(Success), new { id = payment.PaymentID });
            }

            ModelState.AddModelError(string.Empty, "Payment failed. Please try again.");
            return View("PaymentFormWithOrderDetails", model);
        }

        // GET: /Payment/Success/10
        [HttpGet]
        public IActionResult Success(int id)
        {
            ViewBag.PaymentId = id;
            return View();
        }
    }
}
