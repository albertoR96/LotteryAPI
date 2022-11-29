using Microsoft.AspNetCore.Mvc;
using LotteryAPI.Model; // change to dto
using Microsoft.EntityFrameworkCore;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("api/administrators")]
    public class AdministratorsController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public AdministratorsController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //use dtos
        [HttpGet]
        public async Task<List<Administrator>> Get()
        {
            return await _dbContext.Administrators.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Administrator admin)
        {
            var administrator = new Administrator();
            administrator.Name = admin.Name;
            administrator.Email = admin.Email;
            _dbContext.Add(administrator);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _dbContext.Administrators.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            _dbContext.Remove(new Administrator { Id = id });
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
