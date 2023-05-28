using ChatWS.Models.Responses;
using ChatWS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private ImageService _imageService;
        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> UploadAvatar(int userId, [FromForm] IFormCollection file)
        {
            var response = new Response();
            try
            {
                await _imageService.Add(userId, file.Files[0]);
                response.Success = 1;
                response.Message = "Image uploaded successfully";
                return Ok(response);
            }
            catch (Exception)
            {

                return StatusCode(500, response);
            }
            
        }
    }
}
