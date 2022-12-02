using LotteryAPI.Model;
using LotteryAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/participants")]
    [Authorize]
    public class ParticipantsController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ParticipantsController> _logger;
        public ParticipantsController(AppDBContext dbContext, IMapper mapper, ILogger<ParticipantsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize]
        public async Task<List<ParticipantDTO>> Get()
        {
            return _mapper.Map<List<ParticipantDTO>>(await _dbContext.Participants.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ParticipantDTO>> Get(int id)
        {
            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Id == id);
            if (participant == null)
            {
                NotFound();
            }
            return _mapper.Map<ParticipantDTO>(participant);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> Post([FromBody] ParticipantDTO participant)
        {
            var existParticipant = await _dbContext.Participants.AnyAsync(x => x.Email == participant.Email);
            if (existParticipant)
            {
                return BadRequest($"Theres already a participant with the email {participant.Email}");
            }
            _dbContext.Add(_mapper.Map<Participant>(participant));
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] ParticipantDTO participant, int id)
        {
            var exists = await _dbContext.Participants.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            var participantObj = _mapper.Map<Participant>(participant);
            participantObj.Id = id;
            _dbContext.Update(participantObj);
            return Ok();
        }

        /*[HttpDelete("{id:int}")]
        //[Authorize]
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
        }*/
    }
}
