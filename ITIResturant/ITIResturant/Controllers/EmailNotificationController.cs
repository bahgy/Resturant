using BIL.Service.Abstraction;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITIResturant.Controllers
{
    public class EmailNotificationController : Controller
    {

        private readonly IEmailNotificationService _service;

        public EmailNotificationController(IEmailNotificationService service)
        {
            _service = service;
        }
        //==============================================================

        // GET: /EmailNotification
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var notifications = await _service.GetAllAsync();
            return View(notifications);
        }

        // GET: /EmailNotification/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /EmailNotification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailNotification notification)
        {
            if (!ModelState.IsValid)
                return View(notification);

            await _service.AddAsync(notification);
            return RedirectToAction(nameof(Index));
        }
        //==============================================================

        // GET: /EmailNotification/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var notification = await _service.GetByIdAsync(id);
            if (notification == null)
                return NotFound();

            return View(notification);
        }

        // POST: /EmailNotification/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmailNotification notification)
        {
            if (!ModelState.IsValid)
                return View(notification);

            await _service.UpdateAsync(notification);
            return RedirectToAction(nameof(Index));
        }

        //=========================================================

        // GET: /EmailNotification/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _service.GetByIdAsync(id);
            if (notification == null)
                return NotFound();

            return View(notification);
        }

        // POST: /EmailNotification/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
