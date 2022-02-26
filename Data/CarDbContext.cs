using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options)
        {
        }
        public DbSet<PosicaoVeiculo> posicaoVeiculos { get; set; }
        
    }

}
