using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Models;

namespace AutismEducationPlatform.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<EgitimModulu> EgitimModulleri { get; set; }
        public DbSet<ModulIcerik> ModulIcerikleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanıcı tipi için kısıtlama
            modelBuilder.Entity<Kullanici>()
                .Property(k => k.KullaniciTipi)
                .HasMaxLength(20)
                .IsRequired();

            // Öğrenci-Veli ilişkisi
            modelBuilder.Entity<Ogrenci>()
                .HasOne(o => o.Veli)
                .WithMany()
                .HasForeignKey(o => o.VeliId)
                .OnDelete(DeleteBehavior.Restrict);

            // Modül içerikleri ilişkisi
            modelBuilder.Entity<ModulIcerik>()
                .HasOne(mi => mi.EgitimModulu)
                .WithMany(em => em.Icerikler)
                .HasForeignKey(mi => mi.EgitimModuluId);

            // Admin için seed data
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    KullaniciAdi = "admin",
                    Sifre = "admin123" // Gerçek uygulamada şifre hash'lenmelidir
                }
            );
        }
    }
} 