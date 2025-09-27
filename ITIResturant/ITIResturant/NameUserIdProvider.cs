using Microsoft.AspNetCore.SignalR;

namespace Restaurant.PL
{
    public class NameUserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
