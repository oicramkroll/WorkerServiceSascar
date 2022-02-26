using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options)
        {
        }
        public DbSet<RelatorioPosicoes> RelatorioPosicoes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RelatorioPosicoes>().HasKey("Id");
        }
    }

}
