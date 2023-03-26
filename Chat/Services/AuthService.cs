using DB;
using Microsoft.AspNetCore.Mvc;
using ChatWS.Models.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatWS.Models.Requests;
using ChatWS.Models;
using ChatWS.Models.Responses;

namespace ChatWS.Services
{
    public class AuthService
    {
        private AppDbContext _db;
        private IJwtConfig _jwtConfig;

        public AuthService(AppDbContext dbContext, IJwtConfig config)
        {
            _db = dbContext;
            _jwtConfig = config;
        }

        public async Task<int> Add(SignUpRequest requestInfo)
        {
            var user = _db.Users.Where(user => user.Email == requestInfo.Email).FirstOrDefault();

            if (user == null)
            {
                User newUser = new User()
                {
                    Email = requestInfo.Email,
                    Name = requestInfo.Name,
                    Picture = requestInfo.Picture,
                    Password = Encrypt.GetSHA256(requestInfo.Password),
                };
                var result = _db.Users.Add(newUser);
                await _db.SaveChangesAsync();
                return result.Entity.Id;
            }
            else
            {
                throw new AlreadyExistsException("User already exist");
            }

        }
        public SignInResponse SignIn(SignInRequest requestInfo)
        {
            var user = _db.Users.Where(user => user.Email == requestInfo.Email && user.Password == Encrypt.GetSHA256(requestInfo.Password)).FirstOrDefault();

            if (user != null)
            {
                SignInResponse response = new SignInResponse();
                string result = getToken(user);
                response.Id = user.Id;
                response.Token = result;
                return response;
            }

            throw new NotExistException("User not exist");
        }

        private string getToken(User credentials)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);
            var signToken = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim("Id", credentials.Id.ToString()),
                        new Claim("Email", credentials.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = signToken,
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
