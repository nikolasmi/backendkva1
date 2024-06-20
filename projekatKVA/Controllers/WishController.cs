using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projekatKVA.DTOs;
using projekatKVA.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using projekatKVA.Token;
using static projekatKVA.Controllers.CartController;

namespace projekatKVA.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly ShopContext _dbContext;
        private readonly ILogger<WishController> _logger;
        private readonly IMapper _mapper;

        public WishController(ILogger<WishController> logger, ShopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishDTO>>> GetItemsFromWishlist()
        {

            //string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //string userId = TokenHelper.GetUserIdFromToken(token);

            //if (userId == null)
            //{
            //    return Unauthorized();
            //}



            var wishes = await _dbContext.Wishes
                                        .Include(c => c.Item)
                                        .ToListAsync();

            var wishDTOs = wishes.Select(wish => new WishDTO
            {
                WishId = wish.WishId,
                UserId = wish.UserId,
                ItemName = wish.Item.Name,
                ItemPicturePath = wish.Item.PicturePath,
                ItemPrice = (int)wish.Item.Price,
                ItemId = (int)wish.Item.ItemId,
            }).ToList();

            return Ok(wishDTOs);
        }


        [HttpPost]
        public async Task<ActionResult> AddToWishlist([FromBody] AddToWishlistRequest request)
        {


            // Pronađi odgovarajući item u bazi podataka
            var item = await _dbContext.Items.FindAsync(request.ItemId);

            if (item == null)
            {
                return NotFound("Proizvod nije pronađen.");
            }

            // Provjeri je li korisnik već dodao ovaj proizvod u korpu
            var existingWishItem = await _dbContext.Wishes.FirstOrDefaultAsync(c => c.ItemId == request.ItemId);

            if (existingWishItem != null)
            {
                // Ažuriraj količinu ako je proizvod već dodan u korpu
                var itemT =  _dbContext.Wishes.Remove(existingWishItem);
            }
            else
            {
                // Kreiraj novi zapis u korpi
                var newWishItem = _mapper.Map<WishDTO, Wish>(new WishDTO
                {
                    ItemId = item.ItemId,
                    UserId = 1,
                    ItemName = item.Name,
                    ItemPicturePath = item.PicturePath,
                    ItemPrice = (int)item.Price,
                });

                _dbContext.Wishes.Add(newWishItem);
            }

            // Sačuvaj promjene u bazi podataka
            await _dbContext.SaveChangesAsync();

            return Ok("Proizvod uspješno dodat u listu zelja.");
        }

        public class AddToWishlistRequest
        {
            public int ItemId { get; set; }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFromWishlist([FromBody] AddToWishlistRequest request)
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string userId = TokenHelper.GetUserIdFromToken(token);

            if (userId == null)
            {
                return Unauthorized();
            }

            //if (!int.TryParse(userIdString, out int userId))
            //{
            //    return BadRequest("Neispravan identifikator korisnika.");
            //}

            // Pronađi odgovarajući item u korpi na osnovu ItemId i UserId
            var wishItem = await _dbContext.Wishes.FirstOrDefaultAsync(c => c.ItemId == request.ItemId); //&& c.UserId == userId);

            if (wishItem == null)
            {
                return NotFound("Stavka nije pronađena u listi zelja.");
            }

            // Ukloni stavku iz korpe
            _dbContext.Wishes.Remove(wishItem);

            // Sačuvaj promjene u bazi podataka
            await _dbContext.SaveChangesAsync();

            return Ok("Stavka uspješno obrisana iz liste zelja.");
        }
    }
}
