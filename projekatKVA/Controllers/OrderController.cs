using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projekatKVA.DTOs;
using projekatKVA.Models;

namespace projekatKVA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ShopContext _dbContext;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;

        public OrderController(ILogger<OrderController> logger, ShopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Checkout([FromBody] List<OrderItemDTO> orderItems)
        {
            if (orderItems == null || !orderItems.Any())
            {
                return BadRequest("No items in the order");
            }

            try
            {
                var userId = User.FindFirst("id")?.Value; // Pretpostavljamo da je user ID u JWT tokenu

                var order = new Order
                {
                    
                     // Datum i vreme kreiranja porudžbine
                    OrderItems = orderItems.Select(item => new OrderItem
                    {
                        ItemId = item.ItemId,
                        ItemName = item.ItemName,
                        ItemPrice = item.ItemPrice,
                        Quantity = item.Quantity
                    }).ToList()
                };

                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                // Ovde možete dodati dodatne akcije ili notifikacije

                return Ok(new { message = "Order placed successfully", orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
