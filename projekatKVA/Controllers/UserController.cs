using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekatKVA.DTOs;
using projekatKVA.Models;

namespace projekatKVA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ShopContext _dbContext;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, ShopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var user = await _dbContext.Users.ToListAsync();

            var userDTO = _mapper.Map<List<UserDTO>>(user);

            return userDTO;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserDTO dto)
        {
            User user = _mapper.Map<User>(dto);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddOrUpdateUser([FromBody] UserDTO dto)
        {
            try
            {
                // Provera da li korisnik već postoji u bazi
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username || u.Email == dto.Email);

                if (existingUser != null)
                {
                    // Ažuriranje postojećeg korisnika
                    _mapper.Map(dto, existingUser); // Ako koristite AutoMapper za mapiranje podataka
                    _dbContext.Users.Update(existingUser);
                    await _dbContext.SaveChangesAsync();

                    var updatedUserDTO = _mapper.Map<UserDTO>(existingUser);
                    return Ok(updatedUserDTO);
                }
                else
                {
                    // Dodavanje novog korisnika
                    var newUser = _mapper.Map<User>(dto);
                    await _dbContext.Users.AddAsync(newUser);
                    await _dbContext.SaveChangesAsync();

                    var newUserDTO = _mapper.Map<UserDTO>(newUser);
                    return Ok(newUserDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Greška prilikom dodavanja/ažuriranja korisnika: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
