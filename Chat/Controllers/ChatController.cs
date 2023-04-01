using ChatWS.Models.Exceptions;
using ChatWS.Models.Requests;
using ChatWS.Models.Responses;
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

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            Response response = new Response();
            try
            {
                var chatId = await _chatService.Create(request);
                response.Success = 1;
                response.Data = new {
                    id = chatId
                };
                return Ok(response);
            }
            catch (NotExistException ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserChats(int userId)
        {
            Response response = new Response();
            try
            {
                var result = _chatService.GetUserChats(userId);
                response.Success = 1;
                response.Data = result;
                return Ok(response);
            }
            catch (NotExistException ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
            catch (Exception)
            {
                return StatusCode(500, response);
            }
        }
    }
}
