using Alphashop_PriceWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace Alphashop_PriceWebService.Repository
{
    public class AlphaShopDbContext : DbContext
    {
        protected readonly IConfiguration configuration;
        public AlphaShopDbContext(DbContextOptions<AlphaShopDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Listino> Listini { get; set; }
        public DbSet<DettListino> DettListini { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("AlphashopDbConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Listino>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(e => e.Descrizione).IsRequired();
            });
                

            modelBuilder.Entity<DettListino>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CodArt).IsRequired();
                entity.Property(e => e.IdList).IsRequired();
                entity.Property(e => e.Prezzo).IsRequired();

                // Relazione uno-a-molti tra DettListino e Listino
                entity.HasOne(e => e.Listino)
                    .WithMany(d => d.DettListini)
                    .HasForeignKey(e => e.IdList);

                // Aggiunta dell'indice per la colonna IdList
                entity.HasIndex(e => e.IdList);
            });
        }
    }
}
