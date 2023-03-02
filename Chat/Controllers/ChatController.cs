using ChatWS.Models;
using ChatWS.Models.Exceptions;
using ChatWS.Models.Requests;
using ChatWS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        public ChatController(ChatService service)
        {
            _chatService = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            Response response = new Response();
            try
            {
                var chat = _chatService.GetChat(id);
                response.Success = 1;
                response.Data= chat;
                return Ok(response);
            }
            catch (NotExistException ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
        }

        [HttpPost]
        public IActionResult CreateChat([FromBody] CreateChatRequest request)
        {
            Response response = new Response();
            try
            {
                var chat = _chatService.Create(request);
                response.Success = 1;
                response.Data = chat;
                return Ok(response);
            }
            catch (NotExistException ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
        }
    }
}
