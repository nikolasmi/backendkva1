using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projekatKVA.DTOs;
using projekatKVA.Models;
using System.Data.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace projekatKVA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ShopContext _dbContext;
        private readonly ILogger<OrderItemController> _logger;
        private readonly IMapper _mapper;

        public OrderItemController(ILogger<OrderItemController> logger, ShopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDTO> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
            {
                return BadRequest("Order items cannot be empty");
            }

            try
            {
                // Dobijanje korisničkog ID-a iz tokena
                var userId = User.FindFirst("sub")?.Value;

                // Kreiranje novog reda porudžbine
                var order = new Order
                {
                    UserId = Int32.Parse(userId),
                    OrderItems = orderItems.Select(oi => new OrderItem
                    {
                        ItemId = oi.ItemId,
                        ItemName = oi.ItemName,
                        ItemPrice = oi.ItemPrice,
                        Quantity = oi.Quantity
                    }).ToList()
                };

                // Dodavanje porudžbine u bazu podataka
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating order: {ex.Message}");
            }
        }

    }
}
