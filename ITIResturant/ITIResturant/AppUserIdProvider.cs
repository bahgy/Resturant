using Microsoft.AspNetCore.SignalR;

namespace Restaurant.PL
{
    public class AppUserIdProvider: IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("Id")?.Value;
        }
    }
}
