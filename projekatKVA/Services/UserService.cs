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
            var key = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("id", LoginUser.UserId.ToString()), // Dodajte userId claim
            };

            var token = new JwtSecurityToken(
              issuer: "https://localhost:7062/",
              audience: "https://localhost:7062/",
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);


            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //    new Claim(ClaimTypes.Name, user.Username),
            //    }),
            //    Expires = DateTime.UtcNow.AddMinutes(30),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //string userToken = tokenHandler.WriteToken(token);
        }

        //private static ClaimsIdentity GenerateClaims(User user)
        //{
        //    var claims = new ClaimsIdentity();
        //    claims.AddClaim(new Claim(ClaimTypes.Name, user.Us));

        //    return claims;
        //}
    }
}
