using LotteryAPI.Model;
using Microsoft.EntityFrameworkCore;
namespace LotteryAPI
{
    public class AppDBContext : DbContext
    {
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Prize> Prizes { get; set; }
    }
}
