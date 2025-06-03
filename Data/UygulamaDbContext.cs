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

        public DbSet<User> Users { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<LearningModule> LearningModules { get; set; }
        public DbSet<ModuleContent> ModuleContents { get; set; }
        public DbSet<LearningProgress> LearningProgress { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User email unique index
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // User - Child ilişkisi
            modelBuilder.Entity<User>()
                .HasMany(u => u.Children)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Child - LearningProgress ilişkisi
            modelBuilder.Entity<Child>()
                .HasMany(c => c.LearningProgress)
                .WithOne(lp => lp.Child)
                .HasForeignKey(lp => lp.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            // LearningModule - ModuleContent ilişkisi
            modelBuilder.Entity<LearningModule>()
                .HasMany(m => m.Contents)
                .WithOne(c => c.Module)
                .HasForeignKey(c => c.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // LearningModule - LearningProgress ilişkisi
            modelBuilder.Entity<LearningModule>()
                .HasMany(m => m.LearningProgress)
                .WithOne(lp => lp.Module)
                .HasForeignKey(lp => lp.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 