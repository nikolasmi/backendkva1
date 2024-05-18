using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekatKVA.DTOs;
using projekatKVA.Models;
//using System.Data.Entity;

namespace projekatKVA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly ShopContext _dbContext;
        private readonly ILogger<ItemController> _logger;
        private readonly IMapper _mapper;

        public ItemController(ILogger<ItemController> logger, ShopContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {
            var item = await _dbContext.Items.ToListAsync();

            var itemDTO = _mapper.Map<List<ItemDTO>>(item);

            return itemDTO;
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemDTO>>> GetFilterItem(string? manufacturer, string? size, string? type, double? price)
        {
            IQueryable<Item> query = _dbContext.Items;

            // Primena filtera na osnovu opcionalnih parametara
            if (manufacturer != null)
            {
                query = query.Where(x => x.Manufacturer == manufacturer);
            }
            if (size != null)
            {
                query = query.Where(x => x.Size == size);
            }
            if (type != null)
            {
                query = query.Where(x => x.Type == type);
            }
            if (price != null)
            {
                query = query.Where(x => x.Price <= price);
            }

            var items = await query.ToListAsync();

            if (items.Count == 0)
            {
                return NotFound();
            }

            var itemDTOs = _mapper.Map<List<ItemDTO>>(items);

            return Ok(itemDTOs);
        }

    }
}
