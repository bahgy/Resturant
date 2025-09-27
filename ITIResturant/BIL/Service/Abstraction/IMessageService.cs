using Restaurant.BLL.ModelVM.ChatVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.BLL.Service.Abstraction
{
    public interface IMessageService
    {
        Task<(bool hasError, string? message, IEnumerable<MessageUserListVM>? users)> GetChatsForCustomer();
        Task<(bool hasError, string? message, IEnumerable<MessageUserListVM>? users)> GetChatsForDelivery();

        Task<(bool hasError, string? message, ChatVM? chat)> GetMessageByOrder(int orderId);
        //Task<(bool hasError, string? message)> SendMessage(int orderId, string text);
        Task<(bool hasError, string? message)> SendMessage(int receiverId, string text, int? orderId = null);


    }
}
