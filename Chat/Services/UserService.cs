using ChatWS.Models;
using ChatWS.Models.Exceptions;
using ChatWS.Models.Requests;
using ChatWS.Models.Responses;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Serialization;

namespace ChatWS.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        private readonly ImageService _imageService;

        public UserService(AppDbContext context, ImageService imageSerivce)
        {
            _db = context;
            _imageService = imageSerivce;
        }

        public List<SearchUserResponse> GetUsers(string name)
        {
            var users = _db.Users.Where(user => user.Name.Contains(name))
                .Select(user => new SearchUserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Picture = _imageService.Get(user.Id)
                })
                .ToList();

            if (users.IsNullOrEmpty())
            {
                throw new NotExistException("User not exist");
            }
            else
            {
                return users;
            }
        }

        public async Task AddContact(AddContactRequest request)
        {
            var user = _db.Users.Where(user => user.Id == request.UserId).FirstOrDefault();
            var contact = _db.Users.Where(user => user.Id == request.ContactId).FirstOrDefault();
            if (user != null && contact != null)
            {
                user.Contacts.Add(contact);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new NotExistException("User not exist");
            }

        }
    }
}
