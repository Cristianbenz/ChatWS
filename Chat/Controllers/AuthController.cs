using ChatWS.Models;
using ChatWS.Models.Exceptions;
using ChatWS.Models.Requests;
using ChatWS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        public AuthController(AuthService authService)
        {
            _service = authService;
        }

        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            Response OResponse = new Response();
            try
            {
                string id = _service.Add(request);
                OResponse.Data = new { id };
            }
            catch (AlreadyExistsException ex)
            {

                OResponse.Message = ex.Message;
                return BadRequest(OResponse);
            }
            catch (ArgumentException ex)
            {
                OResponse.Message = "User information is missing";
                return BadRequest(OResponse);
            }
            catch (Exception ex)
            {
                OResponse.Message = ex.Message;
                return StatusCode(500, OResponse);
            }

            OResponse.Success = 1;
            return Created("User created successfully", OResponse);
        }

        [HttpPost]
        public IActionResult SignIn([FromBody] SignInRequest request)
        {
            Response OResponse = new Response();
            try
            {
                _service.SignIn(request);
            }
            catch (NotExistException ex)
            {

                OResponse.Message = ex.Message;
                return BadRequest(OResponse);
            }
            catch (ArgumentException ex)
            {
                OResponse.Message = "User information is missing";
                return BadRequest(OResponse);
            }
            catch (Exception ex)
            {
                OResponse.Message = ex.Message;
                return StatusCode(500, OResponse);
            }

            OResponse.Success = 1;
            return Ok(OResponse);
        }
    }
}
