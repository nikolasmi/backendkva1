using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projekatKVA.DTOs;
using projekatKVA.Models;
using projekatKVA.Services;

namespace projekatKVA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ShopContext _dbContext;
        private readonly IUserService _userService;
        public AuthController(ShopContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var korisnik = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Username = model.Username,
                Password = model.Password,
                FavouriteItems = model.FavouriteItems,
            };

            var result = await _dbContext.Users.AddAsync(korisnik);
            await _dbContext.SaveChangesAsync();

            return Ok("dodato");
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginModel model)
        {
            var token = _userService.Login(model);
            if (token == null || token == string.Empty)
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }

            HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30)
            });
            return Ok(token);

        }

    }
}
