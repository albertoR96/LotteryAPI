using AutoMapper;
using LotteryAPI.DTOs;
using LotteryAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GamesController> _logger;

        public GamesController(AppDBContext dbContext, IMapper mapper, ILogger<GamesController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<GameDTO>> Get()
        {
            _logger.LogInformation("Loading games");
            return _mapper.Map<List<GameDTO>>(await _dbContext.Games.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GameDTO>> Get(int id)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            return _mapper.Map<GameDTO>(game);
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> Put(int id, [FromBody] GameDTO game)
        {
            var exists = await _dbContext.Games.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                _logger.LogInformation("Game not found");
                return NotFound();
            }
            var gameObj = _mapper.Map<Game>(game);
            gameObj.Id = id;
            _dbContext.Update(gameObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> Post([FromBody] GameDTO gameObj, [FromHeader] HttpHeaders headers)
        {
            _logger.LogInformation(headers.ToString());
            /*var game = _mapper.Map<Game>(gameObj);
            game.GameStatus = "Created";
            _dbContext.Add(game);
            _logger.LogInformation("Adding game");
            await _dbContext.SaveChangesAsync();*/
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _dbContext.Games.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                _logger.LogInformation("Game not found");
                return NotFound();
            }
            _dbContext.Remove(new Game { Id = id });
            _logger.LogInformation("Removing game");
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("addParticipant")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> AddParticipant([FromBody] ParticipantGameDTO participantGame)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == participantGame.gameId);
            if (game == null)
            {
                return NotFound();
            }

            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Id == participantGame.participantId);
            if (participant == null)
            {
                return NotFound();
            }

            var exists = await _dbContext.ParticipantGames.AnyAsync(x => x.ParticipantId == participantGame.participantId && x.GameId == participantGame.gameId);
            if (exists)
            {
                return BadRequest("Participant is already registered in game");
            }

            var existNumber = await _dbContext.ParticipantGames.AnyAsync(x => x.GameId == participantGame.gameId && x.GameNumber == participantGame.gameNumber);
            if (exists)
            {
                return BadRequest("Number is already registered in game");
            }

            var participantsList = await _dbContext.ParticipantGames.Where(x => x.GameId == participantGame.gameId).ToListAsync();

            if (participantsList.Count() >= 54)
            {
                return BadRequest("There can not be more than 54 participants in game");
            }

            if (participantsList.Count() == 54)
            {
                game.GameStatus = "Ready";
            } else
            {
                game.GameStatus = "Waiting";
            }

            var gameParticipant = new ParticipantGame();
            gameParticipant.GameId = participantGame.gameId;
            gameParticipant.ParticipantId = participantGame.participantId;
            gameParticipant.GameNumber = participantGame.gameNumber;

            _dbContext.Add(gameParticipant);
            _dbContext.Update(game);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("removeParticipant")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> RemoveParticipant([FromBody] ParticipantGameDTO participantGame)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == participantGame.gameId);
            if (game == null)
            {
                return NotFound();
            }

            var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Id == participantGame.participantId);
            if (participant == null)
            {
                return NotFound();
            }

            var participantGameObj = await _dbContext.ParticipantGames.FirstOrDefaultAsync(x => x.ParticipantId == participantGame.participantId && x.GameId == participantGame.gameId);
            if (participantGameObj == null)
            {
                return BadRequest("Participant is not registered in game");
            }

            game.GameStatus = "Waiting";

            _dbContext.Remove(participantGameObj);
            _dbContext.Update(game);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("addPrize")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> AddPrize(GamePrizeDTO gamePrize)
        {
            var gameExist = await _dbContext.Games.AnyAsync(x => x.Id == gamePrize.gameId);
            if (!gameExist)
            {
                return BadRequest("Game does not exist");
            }

            var prizeExist = await _dbContext.Prizes.AnyAsync(x => x.Id == gamePrize.prizeId);
            if (!prizeExist)
            {
                return BadRequest("Game does not exist");
            }

            var gamePrizeExists = await _dbContext.GamePrizes.AnyAsync(x => x.GameId == gamePrize.gameId && x.PrizeId == gamePrize.prizeId);
            if (gamePrizeExists)
            {
                return BadRequest("Game already has the prize");
            }

            var newGamePrize = new GamePrize();
            newGamePrize.PrizeId = gamePrize.prizeId;
            newGamePrize.GameId = gamePrize.gameId;
            _dbContext.Add(newGamePrize);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("removePrize")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult> RemovePrize([FromBody] GamePrizeDTO gamePrize)
        {
            var gameExist = await _dbContext.Games.AnyAsync(x => x.Id == gamePrize.gameId);
            if (!gameExist)
            {
                return BadRequest("Game does not exist");
            }

            var prizeExist = await _dbContext.Prizes.AnyAsync(x => x.Id == gamePrize.prizeId);
            if (!prizeExist)
            {
                return BadRequest("Game does not exist");
            }

            var gamePrizeObj = await _dbContext.GamePrizes.FirstOrDefaultAsync(x => x.GameId == gamePrize.gameId && x.PrizeId == gamePrize.prizeId);
            if (gamePrizeObj == null)
            {
                return BadRequest("Game does not have the prize");
            }

            _dbContext.Remove(gamePrizeObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("run")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ItsAdmin")]
        public async Task<ActionResult<List<ParticipantPrize>>> Run([FromBody] RunGameDTO runGame)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == runGame.Id);
            var gamePrizes = await _dbContext.GamePrizes.Where(x => x.GameId == runGame.Id).ToListAsync();
            var winnerPrizes = new List<ParticipantPrize>();
            int[] winnerNumbers = { };

            if (game.GameStatus != "Ready" || gamePrizes.Count <= 0)
            {
                return BadRequest("Game is not ready");
            }

            for (int i = 0; i < gamePrizes.Count; i++)
            {
                int randN;
                do
                {
                    randN = (new Random()).Next(1, 55);
                } while (winnerNumbers.Contains(randN));
                winnerNumbers.Append(randN);
                var participantG = await _dbContext.ParticipantGames.FirstOrDefaultAsync(x => x.GameId == runGame.Id && x.GameNumber == randN);
                var participant = await _dbContext.Participants.FirstOrDefaultAsync(x => x.Id == participantG.ParticipantId);
                var prize = await _dbContext.Prizes.FirstOrDefaultAsync(x => x.Id == gamePrizes[i].PrizeId);
                var winner = new Winner();
                winner.GameId = runGame.Id;
                winner.ParticipantId = participantG.ParticipantId;
                winner.PrizeId = gamePrizes[i].PrizeId;
                _dbContext.Add(winner);

                var winParticipant = new ParticipantPrize();
                winParticipant.participantId = participantG.ParticipantId;
                winParticipant.participantEmail = participant.Email;
                winParticipant.prizeName = prize.Name;
                winnerPrizes.Add(winParticipant);
            }
            game.PlayedAt = new DateTime();
            game.GameStatus = "Finished";
            return winnerPrizes;
        }

    }
}
