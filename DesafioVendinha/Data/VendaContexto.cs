using Microsoft.EntityFrameworkCore;
using DesafioVendinha.Models;


namespace DesafioVendinha.Data
{
    public class VendaContexto : DbContext
    {
        public VendaContexto(DbContextOptions<VendaContexto> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Venda> Vendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venda>().ToTable("Venda");
        }
    }
}
