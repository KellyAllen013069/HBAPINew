using Microsoft.EntityFrameworkCore;
using HBAPI.Models;

namespace HBAPI.Data
{
    public class HbDbContext : DbContext
    {
        public HbDbContext(DbContextOptions<HbDbContext> options)
            : base(options)
        {
        }

        public DbSet<DanceClass> DanceClasses { get; set; }
        public DbSet<ClassDay> ClassDays { get; set; }
        public DbSet<ClassLevel> ClassLevels { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Translations> Translations { get; set; }
        public DbSet<UserClasses> UserClasses { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ClassesSessions> ClassesSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ClassDay>()
                .HasOne(cd => cd.DanceClass)
                .WithMany(dc => dc.ClassDays)
                .HasForeignKey(cd => cd.ClassId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<ClassDay>()
                .HasOne(cd => cd.Day)
                .WithMany(d => d.ClassDays)
                .HasForeignKey(cd => cd.DayId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Configure Classes_Levels relationship
            modelBuilder.Entity<ClassLevel>()
                .HasOne(cl => cl.DanceClass)
                .WithMany(dc => dc.ClassLevels)
                .HasForeignKey(cl => cl.ClassId)
                .OnDelete(DeleteBehavior.Cascade);  
            
            modelBuilder.Entity<ClassLevel>()
                .HasOne(cl => cl.Level)
                .WithMany(l => l.ClassLevels)
                .HasForeignKey(cl => cl.LevelId)
                .OnDelete(DeleteBehavior.Cascade);  
            
            modelBuilder.Entity<ClassesSessions>()
                .HasOne(cs => cs.DanceClass)
                .WithMany(dc => dc.ClassesSessions)
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<ClassesSessions>()
                .HasOne(cs => cs.Day)
                .WithMany(d => d.ClassesSessions)
                .HasForeignKey(cs => cs.DayId)
                .OnDelete(DeleteBehavior.Cascade);  
            
            modelBuilder.Entity<UserClasses>()
                .HasOne(uc => uc.ClassesSessions)       
                .WithMany(cs => cs.UserClasses)       
                .HasForeignKey(uc => uc.ClassesSessionsId) 
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}