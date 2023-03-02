using MongoDB.Driver;
using DB;
using Microsoft.AspNetCore.Mvc;
using ChatWS.Models.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatWS.Models.Requests;
using ChatWS.Models;

namespace ChatWS.Services
{
    public class AuthService
    {
        private readonly IJwtConfig _jwtConfig;
        private readonly IMongoCollection<User> _users;
        public AuthService(IJwtConfig authConfig, IDbConfig dbConfig)
        {
            var client = new MongoClient(dbConfig.Server);
            var database = client.GetDatabase(dbConfig.Database);
            _users = database.GetCollection<User>("User");
            _jwtConfig = authConfig;
        }

        public string Add(SignUpRequest requestInfo)
        {
            var user = _users.Find(user => user.Email == requestInfo.Email).FirstOrDefault();

            if (user == null)
            {
                User newUser = new User()
                {
                    Email = requestInfo.Email,
                    Name = requestInfo.Name,
                    Picture = requestInfo.Picture,
                    Password = Encrypt.GetSHA256(requestInfo.Password),
                };
                _users.InsertOne(newUser);
                return newUser.Id;
            }
            else
            {
                throw new AlreadyExistsException("User already exist");
            }

        }
        public void SignIn(SignInRequest requestInfo)
        {
            var user = _users.Find(user => user.Email == requestInfo.Email && user.Password == Encrypt.GetSHA256(requestInfo.Password)).FirstOrDefault();

            if (user != null)
            {
                getToken(user);
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
