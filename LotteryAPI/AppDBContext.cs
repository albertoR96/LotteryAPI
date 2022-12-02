using LotteryAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LotteryAPI
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions optopns) : base(optopns)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity
        }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<ParticipantGame> ParticipantGames { get; set; }
        public DbSet<GamePrize> GamePrizes { get; set; }
        public DbSet<Winner> Winners { get; set; }
    }
}
