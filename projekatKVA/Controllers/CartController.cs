using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projekatKVA.DTOs;
using projekatKVA.Models;
using projekatKVA.Token;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using projekatKVA.Token;
//using System.Data.Entity;
//using System.Data.Entity;

namespace projekatKVA.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ShopContext _dbContext;
        private readonly ILogger<CartController> _logger;
        private readonly IMapper _mapper;

        public CartController(ILogger<CartController> logger, ShopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDTO>>> GetItemsFromCart()
        {

            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string userId = TokenHelper.GetUserIdFromToken(token);

            if (userId == null)
            {
                return Unauthorized();
            }


            var carts = await _dbContext.Carts
                                        .Include(c => c.Item)
                                        .ToListAsync();

            var cartDTOs = carts.Select(cart => new CartDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                ItemName = cart.Item.Name,
                ItemPicturePath = cart.Item.PicturePath,
                ItemPrice = (int)cart.Item.Price,
                Quantity = cart.Quantity,
                ItemId = cart.ItemId
            }).ToList();

            return Ok(cartDTOs);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            // Pronađi odgovarajući item u bazi podataka
            var item = await _dbContext.Items.FindAsync(request.ItemId);

            if (item == null)
            {
                return NotFound("Proizvod nije pronađen.");
            }

            // Provjeri je li korisnik već dodao ovaj proizvod u korpu
            var existingCartItem = await _dbContext.Carts.FirstOrDefaultAsync(c => c.ItemId == request.ItemId);

            if (existingCartItem != null)
            {
                // Ažuriraj količinu ako je proizvod već dodan u korpu
                existingCartItem.Quantity++;
            }
            else
            {
                // Kreiraj novi zapis u korpi
                var newCartItem = _mapper.Map<CartDTO, Cart>(new CartDTO
                {
                    ItemId = item.ItemId,
                    UserId = 1,
                    ItemName = item.Name,
                    ItemPicturePath = item.PicturePath,
                    ItemPrice = (int)item.Price,
                    Quantity = 1 // Početna količina je 1
                });

                _dbContext.Carts.Add(newCartItem);
            }

            // Sačuvaj promjene u bazi podataka
            await _dbContext.SaveChangesAsync();

            return Ok("Proizvod uspješno dodat u korpu.");
        }

        public class AddToCartRequest
        {
            public int ItemId { get; set; }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFromCart([FromBody] AddToCartRequest request)
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
            var cartItem = await _dbContext.Carts.FirstOrDefaultAsync(c => c.ItemId == request.ItemId); //&& c.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Stavka nije pronađena u korpi.");
            }

            // Ukloni stavku iz korpe
            _dbContext.Carts.Remove(cartItem);

            // Sačuvaj promjene u bazi podataka
            await _dbContext.SaveChangesAsync();

            return Ok("Stavka uspješno obrisana iz korpe.");
        }









        //[HttpPost]
        //// Dodajemo autorizaciju jer pretpostavljamo da je pristup dodavanju u korpu zaštićen
        //public async Task<ActionResult> AddToCart([FromBody] AddToCartRequest request)
        //{
        //    // Pronađi korisnika na osnovu identifikatora u JWT tokenu
        //    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (!int.TryParse(userIdString, out int userId))
        //    {
        //        return BadRequest("Neispravan identifikator korisnika.");
        //    }

        //    // Pronađi odgovarajući item u bazi podataka
        //    var item = await _dbContext.Items.FindAsync(request.ItemId);

        //    if (item == null)
        //    {
        //        return NotFound("Proizvod nije pronađen.");
        //    }

        //    // Provjeri je li korisnik već dodao ovaj proizvod u korpu
        //    var existingCartItem = await _dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == request.ItemId);

        //    if (existingCartItem != null)
        //    {
        //        // Ažuriraj količinu ako je proizvod već dodan u korpu
        //        existingCartItem.Quantity++;
        //    }
        //    else
        //    {
        //        // Dodaj novi proizvod u korpu ako ga korisnik još nije dodao
        //        var newCartItem = new Cart
        //        {
        //            UserId = userId,
        //            ItemId = item.ItemId,
        //            Quantity = 1 // Početna količina je 1
        //        };

        //        _dbContext.Carts.Add(newCartItem);
        //    }

        //    // Sačuvaj promjene u bazi podataka
        //    await _dbContext.SaveChangesAsync();

        //    return Ok("Proizvod uspješno dodat u korpu.");
        //}




        //[HttpPost]
        //public async Task<IActionResult> AddToCart([FromBody] CartItemRequest response)
        //{
        //    Console.WriteLine(response.ItemId);
        //    try
        //    {
        //        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        var userId1 = Int32.Parse(userId);


        //        var userExists = await _dbContext.Users.AnyAsync(u => u.UserId == userId1);
        //        if (!userExists)
        //        {
        //            return NotFound("Korisnik nije pronađen.");
        //        }

        //       var item = await _dbContext.Items.FindAsync(response.ItemId);
        //        if (item == null)
        //        {
        //            return NotFound("Proizvod nije pronađen.");
        //        }

        //        // Dodavanje proizvoda u korpu
        //        var cartItem = new Cart
        //        {
        //            UserId = Int32.Parse(userId),
        //            ItemId = response.ItemId,
        //            Quantity = 1 // Postavljamo početnu količinu na 1, možete prilagoditi prema potrebama
        //        };

        //        _dbContext.Carts.Add(cartItem);
        //        await _dbContext.SaveChangesAsync();

        //        return Ok("Proizvod uspešno dodat u korpu.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Došlo je do greške prilikom dodavanja proizvoda u korpu.");
        //    }
        //}
    }
}
