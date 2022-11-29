using LotteryAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public GamesController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Game>> Get()
        {
            return await _dbContext.Games.ToListAsync();
        }

        [HttpPut("{id:int, add_participant}")]
        public async Task<ActionResult> Put(int id, Participant participantObj)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            game.Participants.Add(participantObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Game gameObj)
        {
            var game = new Game();
            game.Name = gameObj.Name;
            _dbContext.Add(game);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _dbContext.Games.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            _dbContext.Remove(new Game { Id = id });
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
