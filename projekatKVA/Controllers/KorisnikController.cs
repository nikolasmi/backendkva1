using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using projekatKVA.DTOs;
using projekatKVA.Models;
using projekatKVA.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace projekatKVA.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class KorisnikController : ControllerBase
    {

        private readonly IUserService _userService;
        public KorisnikController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(LoginModel user)
        {
            var token = _userService.Login(user);
            if (token == null || token == string.Empty)
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }
            return Ok(token);
        }

        [HttpGet("name")]
        public IActionResult GetName()
        {
            string name = User.FindFirstValue(ClaimTypes.Name);
            return Ok(name);
        }
    }
}
