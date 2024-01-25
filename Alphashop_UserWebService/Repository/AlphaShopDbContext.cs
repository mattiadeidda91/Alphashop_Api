using Alphashop_UserWebService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alphashop_UserWebService.Repository
{
    public class AlphaShopDbContext : DbContext
    {
        protected readonly IConfiguration configuration;
        public AlphaShopDbContext(DbContextOptions<AlphaShopDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
            //Database.Migrate();
        }

        public virtual DbSet<Utenti> Utenti { get; set; }
        public virtual DbSet<Profili> Profili { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("AlphashopDbConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utenti>()
                .HasKey(a => new { a.CodFidelity });

            modelBuilder.Entity<Profili>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Profili>()
                .HasOne<Utenti>(s => s.Utente)
                .WithMany(g => g.Profili)
                .HasForeignKey(g => g.CodFidelity);

        }
    }
}
