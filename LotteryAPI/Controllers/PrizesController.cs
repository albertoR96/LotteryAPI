using Microsoft.AspNetCore.Mvc;
using LotteryAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/prizes")]
    public class PrizesController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public PrizesController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Prize>> Get()
        {
            return await _dbContext.Prizes.ToListAsync();
        }
    }
}
