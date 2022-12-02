using Microsoft.AspNetCore.Mvc;
using LotteryAPI.Model;
using LotteryAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/administrators")]
    //[Authorize]
    public class AdministratorsController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AdministratorsController> _logger;

        public AdministratorsController(AppDBContext dbContext, IMapper mapper, ILogger<AdministratorsController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<AdministratorDTO>> Get()
        {
            _logger.LogInformation("Loading administrators");
            return _mapper.Map<List<AdministratorDTO>>(await _dbContext.Administrators.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdministratorDTO>> Get(int id)
        {
            _logger.LogInformation("Loading administrator");
            var administrator = await _dbContext.Administrators.FirstOrDefaultAsync(x => x.Id == id);
            if (administrator == null)
            {
                return NotFound();
            }
            return _mapper.Map<AdministratorDTO>(administrator);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] AdministratorDTO administrator, int id)
        {
            var exists = await _dbContext.Administrators.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            var adminObj = _mapper.Map<Administrator>(administrator);
            adminObj.Id = id;
            _dbContext.Update(adminObj);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AdministratorDTO admin)
        {
            var existsAdministrator = await _dbContext.Administrators.AnyAsync(x => x.Email == admin.Email);
            if (existsAdministrator)
            {
                _logger.LogWarning("Duplicate administrator");
                return BadRequest($"Theres already an administrator with the email {admin.Email}");
            }
            var administrator = _mapper.Map<Administrator>(admin);
            _dbContext.Add(administrator);
            _logger.LogInformation("Adding administrator");
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        /*[HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _dbContext.Administrators.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                _logger.LogWarning("Administrator not found");
                return NotFound();
            }

            _dbContext.Remove(new Administrator { Id = id });
            _logger.LogInformation("Removing administrator");
            await _dbContext.SaveChangesAsync();
            return Ok();
        }*/

    }
}
