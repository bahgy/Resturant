using BIL.Model_VM.Feedback;
using BIL.Service.Abstraction;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITIResturant.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }


        // GET: Feedback
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _service.GetAllAsync();
            return View(feedbacks);
        }

        // GET: Feedback/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var feedback = await _service.GetByIdAsync(id);
            if (feedback == null) return NotFound();
            return View(feedback);
        }

        // GET: Feedback/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeedbackVM vm)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(vm);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Feedback/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var feedback = await _service.GetByIdAsync(id);
            if (feedback == null) return NotFound();

            var vm = new UpdateFeedbackVM
            {
                Id = feedback.Id,
                Comment = feedback.Comment,
                Rating = feedback.Rating
                
                
            };

            return View(vm);
        }

        // POST: Feedback/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateFeedbackVM vm)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(vm);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Feedback/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await _service.GetByIdAsync(id);
            if (feedback == null) return NotFound();
            return View(feedback);
        }

        // POST: Feedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
