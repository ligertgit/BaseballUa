using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;
using static BaseballUa.Data.Enums;
using BaseballUa.ViewModels;

namespace BaseballUa.Data
{
    public class BaseballUaDbContext : DbContext
    {
		public BaseballUaDbContext(DbContextOptions<BaseballUaDbContext> options) : base(options)
		{ 
		
		}

        public DbSet<Category> Categories { get; set; }
		public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //tournament defaults
            modelBuilder.Entity<Tournament>()
                .Property(b => b.IsInternational)
                .HasDefaultValue(false);

            modelBuilder.Entity<Tournament>()
                .Property(b => b.IsAnual)
                .HasDefaultValue(false);

            modelBuilder.Entity<Tournament>()
                .Property(b => b.IsOfficial)
                .HasDefaultValue(false);

            modelBuilder.Entity<Tournament>()
                .Property(b => b.IsFun)
                .HasDefaultValue(false);

            modelBuilder.Entity<Tournament>()
                .Property(b => b.Sport)
                .HasDefaultValue(SportType.NotDefined);
        }

        //public DbSet<BaseballUa.ViewModels.TournamentViewModel>? TournamentViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.CategoryViewModel>? CategoryViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.EventViewModel>? EventViewModel { get; set; }

    }
}
