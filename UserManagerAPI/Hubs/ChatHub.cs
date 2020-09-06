using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace UserManagerAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        
    }
}
