using DB;

namespace ChatWS.Services
{
    public class ImageService
    {
        public async Task Add(int userId, IFormFile file)
        {
            var filePath = this.GetFilePath(userId);

            if (!System.IO.File.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }

            if (System.IO.File.Exists(filePath))
            {
                System.IO.Directory.Delete(filePath);
            }

            string imagePath = filePath + "/avatar";

            using (var stream = System.IO.File.Create(imagePath))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task Delete(int userId)
        {
            var filePath = this.GetFilePath(userId);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.Directory.Delete(filePath);
            }
        }

        private string GetFilePath(int userId)
        {
            return Directory.GetCurrentDirectory() + "/wwwroot/Uploads/Users/" + userId;
        }
    }
}
