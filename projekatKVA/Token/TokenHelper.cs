using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace projekatKVA.Token
{
    public class TokenHelper
    {
        public static string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid token");
            }

            var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "id"); // Koristite "id" umesto "userId"
            return userIdClaim?.Value;
        }
    }
}
