using Restaurant.BLL.ModelVM.ChatVM;

namespace Restaurant.PL.Controllers
{
    [Authorize(Roles =  "Customer")]
    [ServiceFilter(typeof(ValidateUserExistsFilter))]
    public class CustomerChatController : Controller
    {
        private readonly IMessageService _MessageService;
        public CustomerChatController(IMessageService MessageService) => _MessageService = MessageService;

        public async Task<IActionResult> Index()
        {
            var (hasError, message, users) = await _MessageService.GetChatsForCustomer();
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return View(new List<MessageUserListVM>());
            }
            return View(users);
        }

        // now receives orderId, not delivery id
        public async Task<IActionResult> Chat(int orderId)
        {
            var (hasError, message, chat) = await _MessageService.GetMessageByOrder(orderId);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return View(new List<MessageUserListVM>());
            }
            return View(chat);
        }
    }
}