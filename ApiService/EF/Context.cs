using ApiService.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiService.EF
{
    public class Context:DbContext
    {
        // Bu kısımda tablolarımızı set ediyoruz
        public DbSet<Baller> Baller { get; set; }
        public DbSet<Team> Team { get; set; }

        public DbSet<League> League { get; set; }

        

        public DbSet<Country> Country { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<BallerTeam> BallerTeam { get; set; }
        

        public DbSet<BallerPerformance> BallerPerformance { get; set; }

        public DbSet<TeamLeague> TeamLeague { get; set; }

        public DbSet<TeamLeagueWinner> teamLeagueWinners { get; set; }

        public DbSet<Member> Member { get; set; }


        //Bu kısımda ise veri tabanı bağlantısını kuruyoruz.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Fimarkt;User Id=MyLogin;Password = 123;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
