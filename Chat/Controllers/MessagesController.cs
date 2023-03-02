using ChatWS.Hubs;
using ChatWS.Models.Requests;
using ChatWS.Services;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesService _messageService;
        public MessagesController(MessagesService service)
        {
            _messageService = service;
        }

        [HttpPost]
        public async Task Send([FromBody] SendMessageRequest sendRequest)
        {

            await _messageService.AddMessage(sendRequest);
        }
    }
}
