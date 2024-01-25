using Alphashop_ArticoliWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace Alphashop_ArticoliWebService.Repository
{
    public class AlphaShopDbContext : DbContext
    {
        protected readonly IConfiguration configuration;
        public AlphaShopDbContext(DbContextOptions<AlphaShopDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public virtual DbSet<Articoli> Articoli { get; set; }
        public virtual DbSet<BarCode> BarCode { get; set; }
        public virtual DbSet<Ingredienti> Ingredienti { get; set; }
        public virtual DbSet<FamAssort> FamAssort { get; set; }
        public virtual DbSet<Iva> Iva { get; set; }

        //NON SERVE CON L'USO DEL JWT TOKEN
        //public virtual DbSet<Utenti> Utenti { get; set; }
        //public virtual DbSet<Profili> Profili { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("AlphashopDbConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articoli>()
                .HasKey(a => new { a.CodArt });

            //One to Many relationship Articoli-Barcode
            modelBuilder.Entity<BarCode>() 
                .HasOne<Articoli>(b => b.Articolo)      // ad un Articolo
                .WithMany(a => a.BarCodes)              //corrispondono molti Barcode
                .HasForeignKey(b => b.CodArt);          //chiave esterna Barcode

            //One to One Articolo-Ingredienti
            modelBuilder.Entity<Articoli>()
                .HasOne(a => a.Ingrediente)                 // ad un Articolo
                .WithOne(i => i.Articolo)                   // corrisponde un Ingrediente
                .HasForeignKey<Ingredienti>(i => i.CodArt); //ForeignKey di Ingredienti

            //One to Many Iva-Articolo
            modelBuilder.Entity<Articoli>()
                .HasOne<Iva>(a => a.Iva)
                .WithMany(i => i.Articoli)
                .HasForeignKey(a => a.IdIva);

            //One to Many FamAssort-Articolo
            modelBuilder.Entity<Articoli>()
                .HasOne<FamAssort>(a => a.FamAssort)
                .WithMany(i => i.Articoli)
                .HasForeignKey(a => a.IdFamAss);

            //NON SERVE CON L'USO DEL JWT TOKEN
            ////Utenti e Profili
            //modelBuilder.Entity<Profili>()
            //    .HasOne<Utenti>(s => s.Utente)
            //    .WithMany(g => g.Profili)
            //    .HasForeignKey(g => g.CodFidelity);
        }
    }
}
