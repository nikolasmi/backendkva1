using Microsoft.IdentityModel.Tokens;
using projekatKVA.DTOs;
using projekatKVA.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace projekatKVA.Services
{
    public class UserService : IUserService
    {
        
        private readonly ShopContext _dbContext;


            private readonly IConfiguration _configuration;

            public UserService(IConfiguration configuration, ShopContext dbContext)
            {
                _configuration = configuration;
                _dbContext = dbContext;
            }

            public string Login(LoginModel user)
            {
                var LoginUser = _dbContext.Users.SingleOrDefault(x => x.Username == user.Username && x.Password == user.Password);

                if (LoginUser == null)
                {
                    return string.Empty;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Username)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string userToken = tokenHandler.WriteToken(token);
                return userToken;
            }
    }
}
