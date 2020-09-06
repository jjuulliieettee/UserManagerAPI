using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UserManagerAPI.Dtos;
using UserManagerAPI.Hubs;

namespace UserManagerAPI.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController( IHubContext<ChatHub> hubContext )
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage( MessageDto input )
        {
            var username = User.Identity.Name;
            await _hubContext.Clients.All.SendAsync( "Send", new
                    { message = input.Message, user = username, date = DateTime.Now } 
            );
            return Ok();
        }
    }
}
