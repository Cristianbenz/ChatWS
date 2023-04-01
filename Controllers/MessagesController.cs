using ChatWS.Hubs;
using ChatWS.Models.Requests;
using ChatWS.Models.Responses;
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

        [HttpGet("{chatId}")]
        public IActionResult GetChatMessages(int chatId)
        {
            Response response = new Response();
            try
            {
                var result = _messageService.GetChatMessages(chatId);
                response.Success = 1;
                response.Data = result;
                return Ok(response);

            }
            catch (Exception)
            {
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendMessageRequest sendRequest)
        {
            try
            {
                Response response = new Response();
                var result = await _messageService.AddMessage(sendRequest);
                response.Success = 1;
                response.Data = result;
                return Created("Created", response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
        }
    }
}
