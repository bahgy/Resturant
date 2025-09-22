[Authorize]
[ServiceFilter(typeof(ValidateUserExistsFilter))]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;

    public CartController(ICartService cartService, IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var customerId = _userService.GetCurrentCustomerId(User);
        if (customerId == null)
        {
            return RedirectToAction("Login", "Account"); 
        }

        var result = await _cartService.GetCartAsync(customerId.Value);

        return View(result); // result will be a CartVM with items
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var customerId = _userService.GetCurrentCustomerId(User);
        if (customerId == null)
        {
            return Json(new { success = false, message = "You must login first." });
        }

        var addResult = await _cartService.AddToCartAsync(productId, customerId.Value, quantity);
        if (!addResult.success)
        {
            return Json(new { success = false, message = addResult.message });
        }

        var cart = await _cartService.GetCartAsync(customerId.Value);
        var count = cart.Items.Sum(i => i.Quantity);

        var addedItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        return Json(new
        {
            success = true,
            message = $"{addedItem?.ProductName} added to cart!",
            cartCount = count,
            product = new
            {
                id = addedItem?.ProductId,
                name = addedItem?.ProductName,
                imageUrl = addedItem?.ImageUrl,
                quantity = addedItem?.Quantity
            }
        });
    }


    [HttpGet]
    public async Task<IActionResult> GetCartCount()
    {
        var customerId = _userService.GetCurrentCustomerId(User);
        if (customerId == null)
            return Json(0);

        var cart = await _cartService.GetCartAsync(customerId.Value);
        var count = cart.Items.Sum(i => i.Quantity);

        return new JsonResult(count); 
    }
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        var customerId = _userService.GetCurrentCustomerId(User);
        if (customerId == null)
            return Json(new { success = false, message = "You must login first." });

        var result = await _cartService.UpdateQuantityAsync(customerId.Value, productId, quantity);

        if (!result.success)
            return Json(new { success = false, message = result.message });

        var cart = await _cartService.GetCartAsync(customerId.Value);

        return Json(new
        {
            success = true,
            message = "Quantity updated.",
            cart = new
            {
                itemsCount = cart.Items.Sum(i => i.Quantity),
                subtotalFormatted = cart.Subtotal.ToString("C"),
                taxFormatted = cart.Tax.ToString("C"),
                deliveryFeeFormatted = cart.DeliveryFee.ToString("C"),
                grandTotalFormatted = cart.GrandTotal.ToString("C"),
                items = cart.Items.Select(i => new
                {
                    productId = i.ProductId,
                    totalPriceFormatted = i.TotalPrice.ToString("C")
                })
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(int productId)
    {
        var customerId = _userService.GetCurrentCustomerId(User);
        if (customerId == null)
            return Json(new { success = false, message = "You must login first." });

        var result = await _cartService.RemoveItemAsync(customerId.Value, productId);

        if (!result.success)
            return Json(new { success = false, message = result.message });

        var cart = await _cartService.GetCartAsync(customerId.Value);

        return Json(new
        {
            success = true,
            message = "Item removed.",
            cart = new
            {
                itemsCount = cart.Items.Sum(i => i.Quantity),
                subtotalFormatted = cart.Subtotal.ToString("C"),
                taxFormatted = cart.Tax.ToString("C"),
                deliveryFeeFormatted = cart.DeliveryFee.ToString("C"),
                grandTotalFormatted = cart.GrandTotal.ToString("C"),
                items = cart.Items.Select(i => new
                {
                    productId = i.ProductId,
                    totalPriceFormatted = i.TotalPrice.ToString("C")
                })
            }
        });
    }

}
