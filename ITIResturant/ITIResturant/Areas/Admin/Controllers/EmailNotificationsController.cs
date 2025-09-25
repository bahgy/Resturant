using Castle.Core.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EmailNotificationsController : Controller
    {
        private readonly RestaurantDbContext _db;
        private readonly EmailSender _emailSender;

        public EmailNotificationsController(RestaurantDbContext db, EmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var customers = _db.Customers
                .Where(c => (bool)c.SendEmailNotification)
                .ToList();
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmails(List<int> selectedUserIds, string subject, string message)
        {
            var customers = _db.Customers
                .Where(c => selectedUserIds.Contains(c.Id))
                .ToList();

            foreach (var customer in customers)
            {
                await _emailSender.SendEmailAsync(customer.Email, subject, message);
            }

            TempData["Success"] = "Emails sent successfully!";
            return RedirectToAction("Index");
        }
    }
}
