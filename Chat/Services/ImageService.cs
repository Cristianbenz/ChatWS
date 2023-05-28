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

            string imagePath = filePath + "\\avatar.jpg";

            using (var stream = System.IO.File.Create(imagePath))
            {
                await file.CopyToAsync(stream);
            }
        }

        public string Get(int userId)
        {
            var filePath = this.GetFilePath(userId);
            string hostUrl = "https://cbenzchat.vercel.app";
            string avatarPath = filePath + "\\avatar.jpg";

            if (!System.IO.File.Exists(avatarPath))
            {
                return hostUrl + "/Uploads/Common/noavatar.jpg";
            }
            else
            {
                return hostUrl + "/Uploads/Users/" + userId + "/avatar.jpg";
            }
        }

        public void Delete(int userId)
        {
            var filePath = this.GetFilePath(userId);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.Directory.Delete(filePath);
            }
        }

        private string GetFilePath(int userId)
        {
            return Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\Users\\" + userId;
        }
    }
}
