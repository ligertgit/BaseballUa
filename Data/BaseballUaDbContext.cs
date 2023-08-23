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
                .Property(b => b.Sport)
                .HasDefaultValue(SportType.Both);
        }

        public DbSet<BaseballUa.ViewModels.CategoryViewModel>? CategoryViewModel { get; set; }

        public DbSet<BaseballUa.ViewModels.TournamentViewModel>? TournamentViewModel { get; set; }
    }
}
