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
        public DbSet<EgitimModulu> EgitimModulleri { get; set; }
        public DbSet<ModulIcerik> ModulIcerikleri { get; set; }
        public DbSet<Cocuk> Cocuklar { get; set; }
        public DbSet<CocukDurumu> CocukDurumlari { get; set; }
        public DbSet<VeliBilgilendirme> VeliBilgilendirmeler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanıcı tipi için kısıtlama
            modelBuilder.Entity<Kullanici>()
                .Property(k => k.KullaniciTipi)
                .HasMaxLength(20)
                .IsRequired();

            // Çocuk-Veli ilişkisi
            modelBuilder.Entity<Cocuk>()
                .HasOne(c => c.Veli)
                .WithMany()
                .HasForeignKey(c => c.VeliId)
                .OnDelete(DeleteBehavior.Restrict);

            // Admin kullanıcısını seed data olarak ekle
            modelBuilder.Entity<Kullanici>().HasData(
                new Kullanici
                {
                    Id = 1,
                    Ad = "Admin",
                    Soyad = "Admin",
                    Email = "admin@gmail.com",
                    Sifre = "admin1234",
                    KullaniciTipi = "Admin"
                }
            );

            // EgitimModulu - ModulIcerik ilişkisi
            modelBuilder.Entity<ModulIcerik>()
                .HasOne(mi => mi.EgitimModulu)
                .WithMany(em => em.Icerikler)
                .HasForeignKey(mi => mi.EgitimModuluId);
        }
    }
} 