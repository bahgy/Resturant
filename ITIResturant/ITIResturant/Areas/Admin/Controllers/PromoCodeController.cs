
namespace Restaurant.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class PromoCodeController : Controller
    {
        IPromoCodeService promoCodeService;
        public PromoCodeController(IPromoCodeService PromoCodeService)
        {
            promoCodeService = PromoCodeService;
        }
        public async Task<IActionResult> Index()
        {
            var PromoCodes = await promoCodeService.GetAllAsync();

            return View(PromoCodes);
        }
        public async Task<IActionResult> Details(int id)
        {
            var promoCode = await promoCodeService.GetByIdAsync(id);

            return View(promoCode);
        }
        public async Task<IActionResult> Delete(int id)
        {   
            await promoCodeService.DeleteAsync(id);

            return RedirectToAction("Index");
        }
        
      
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(CreatePromoCodeVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await promoCodeService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var promoCode = await promoCodeService.GetByIdAsync(id);
            if (promoCode == null)
                return NotFound();

            var updateModel = new UpdatePromoCodeVM
            {
                Id = promoCode.Id,
                Code = promoCode.Code,
                Description = promoCode.Description,
                DiscountValue = promoCode.DiscountValue,
                DiscountType = promoCode.DiscountType,
                ValidFromTime = promoCode.ValidFromTime,
                ValidTo = promoCode.ValidTo,
                MaxUsedTime = promoCode.MaxUsedTime,
                MinimumOrderAmount = promoCode.MinimumOrderAmount
            };
            return View(updateModel);
        }

      
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePromoCodeVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await promoCodeService.UpdateAsync(model);
            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
