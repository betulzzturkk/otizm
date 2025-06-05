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
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EducationModule> EducationModules { get; set; }
        public DbSet<ModuleContent> ModuleContents { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ProgressReport> ProgressReports { get; set; }
        public DbSet<ChildStatus> ChildStatuses { get; set; }
        public DbSet<ParentInformation> ParentInformations { get; set; }
        public DbSet<LearningProgress> LearningProgresses { get; set; }
        public DbSet<LearningModule> LearningModules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Admin ilişkisi
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Parent ilişkisi
            modelBuilder.Entity<Parent>()
                .HasOne(p => p.User)
                .WithOne(u => u.Parent)
                .HasForeignKey<Parent>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Instructor ilişkisi
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.User)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Parent - Child ilişkisi
            modelBuilder.Entity<Child>()
                .HasOne(c => c.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Child - Education ilişkisi
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Child)
                .WithMany(c => c.Educations)
                .HasForeignKey(e => e.ChildId)
                .OnDelete(DeleteBehavior.Restrict);

            // Education - Instructor ilişkisi
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Instructor)
                .WithMany(i => i.Educations)
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Education - EducationModule ilişkisi
            modelBuilder.Entity<EducationModule>()
                .HasOne(m => m.Education)
                .WithMany(e => e.Modules)
                .HasForeignKey(m => m.EducationId)
                .OnDelete(DeleteBehavior.Cascade);

            // EducationModule - ModuleContent ilişkisi
            modelBuilder.Entity<ModuleContent>()
                .HasOne(c => c.Module)
                .WithMany(m => m.Contents)
                .HasForeignKey(c => c.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Child - ProgressReport ilişkisi
            modelBuilder.Entity<ProgressReport>()
                .HasOne(r => r.Child)
                .WithMany(c => c.ProgressReports)
                .HasForeignKey(r => r.ChildId)
                .OnDelete(DeleteBehavior.Restrict);

            // Child - ChildStatus ilişkisi
            modelBuilder.Entity<ChildStatus>()
                .HasOne(s => s.Child)
                .WithMany(c => c.StatusHistory)
                .HasForeignKey(s => s.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            // Child - ParentInformation ilişkisi
            modelBuilder.Entity<ParentInformation>()
                .HasOne(i => i.Child)
                .WithMany(c => c.ParentInformations)
                .HasForeignKey(i => i.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            // Child - LearningProgress ilişkisi
            modelBuilder.Entity<LearningProgress>()
                .HasOne(l => l.Child)
                .WithMany(c => c.LearningProgresses)
                .HasForeignKey(l => l.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            // LearningModule - LearningProgress ilişkisi
            modelBuilder.Entity<LearningProgress>()
                .HasOne(l => l.Module)
                .WithMany()
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 