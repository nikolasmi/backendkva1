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
    }
}
