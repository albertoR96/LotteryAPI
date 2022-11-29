using LotteryAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/participants")]
    public class ParticipantsController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public ParticipantsController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Participant>> Get()
        {
            return await _dbContext.Participants.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Participant participant)
        {
            var participantObj = new Participant();
            participantObj.Name = participant.Name;
            _dbContext.Add(participantObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _dbContext.Participants.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            _dbContext.Remove(new Participant { Id = id });
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
