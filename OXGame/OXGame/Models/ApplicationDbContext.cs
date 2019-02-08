using System.Data.Entity;


namespace OXGame.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<GamesData> GamesData { get; set; }
        public DbSet<MovesHistory> MovesHistory { get; set; }
    }
}