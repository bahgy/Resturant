
using Restaurant.BLL.ModelVM.ChatVM;

namespace Restaurant.PL.Controllers
    {
    [Authorize(Roles ="Delivery")]
    [ServiceFilter(typeof(ValidateUserExistsFilter))]
    public class DeliveryChatController : Controller
        {
            private readonly IMessageService _MessageService;

            public DeliveryChatController(IMessageService MessageService)
            {
                _MessageService = MessageService;
            }

        public async Task<IActionResult> Index()
        {
            var (hasError, message, users) = await _MessageService.GetChatsForDelivery();
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return View(new List<MessageUserListVM>()); 
            }
            return View(users);
        }


        public async Task<IActionResult> Chat(int orderId)
        {
            var (hasError, message, chat) = await _MessageService.GetMessageByOrder(orderId);
            if (hasError)
            {
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Index");  // back to chat list with error
            }
            return View(chat);
        }




    }
}
