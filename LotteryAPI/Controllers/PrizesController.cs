using Microsoft.AspNetCore.Mvc;
using LotteryAPI.Model;
using LotteryAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/prizes")]
    public class PrizesController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PrizesController> _logger;

        public PrizesController(AppDBContext dbContext, IMapper mapper, ILogger<PrizesController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<PrizeDTO>> Get()
        {
            _logger.LogInformation("Loading prizes");
            return _mapper.Map<List<PrizeDTO>>(await _dbContext.Prizes.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PrizeDTO>> Get(int id)
        {
            var prize = await _dbContext.Prizes.FirstOrDefaultAsync(x => x.Id == id);
            if (prize == null)
            {
                return NotFound();
            }
            return _mapper.Map<PrizeDTO>(prize);
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> Put([FromBody] PrizeDTO prize, int id)
        {
            var exists = await _dbContext.Prizes.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            var prizeObj = _mapper.Map<Prize>(prize);
            prizeObj.Id = id;
            _dbContext.Update(prizeObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> Post([FromBody] PrizeDTO prize)
        {
            var prizeObj = _mapper.Map<Prize>(prize);
            _dbContext.Add(prizeObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
