using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.BLL.Service.Abstraction;
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

        public PaymentController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        // GET: /Payment/Checkout/5
        [HttpGet]
        public async Task<IActionResult> Checkout(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null) return NotFound();

            var vm = new CreatePaymentVm
            {
                OrderId = order.Id,
                PaymentMethod = order.PaymentMethod,
                Amount = order.TotalAmount
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
