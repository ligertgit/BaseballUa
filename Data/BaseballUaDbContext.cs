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
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Category> Categories { get; set; }
		public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventSchemaItem> EventSchemaItems { get; set; }
        public DbSet<SchemaGroup> SchemaGroups { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Team> Teams { get; set; }

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

            //modelBuilder.Entity<Game>()
            //    .HasOne(b => b.EventSchemaItem)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.ClientNoAction);

            /////////////////////
            //modelBuilder.Entity<Game>()
            //    .HasOne(b => b.EventSchemaItem)
            //    .WithMany(e => e.Games)
            //    .OnDelete(DeleteBehavior.ClientNoAction);

            ////modelBuilder.Entity<Event>()
            ////    .HasMany(e => e.Games)
            ////    .WithOne(g => g.Event)
            ////    .OnDelete(DeleteBehavior.NoAction);

            //entity.HasOne(d => d.Team1)
            //        .WithMany(p => p.GameTeam1)
            //        .HasForeignKey(d => d.Team1Id)
            //        .OnDelete(DeleteBehavior.ClientNoAction)
            //        .HasConstraintName("FK_Games_Teams_Team1ID");

            //modelBuilder.Entity<Game>()
            //            .HasOne<Team>(g => g.HomeTeam)
            //            .WithMany(t => t.Games)
            //            .HasForeignKey(g => g.HomeTeamId)
            //            .OnDelete(DeleteBehavior.ClientNoAction);

            //modelBuilder.Entity<Game>()
            //            .HasOne<Team>(g => g.VisitorTeam)
            //            .WithMany(t => t.Games)
            //            .HasForeignKey(g => g.VisitorTeamId)
            //            .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<Team>()
                            .HasMany(t => t.HomeGames)
                            .WithOne(g => g.HomeTeam)
                            .HasForeignKey(g => g.HomeTeamId)
                            .OnDelete(DeleteBehavior.Restrict); // ondelete not working due to lines with ignore. looks like can be removed

            modelBuilder.Entity<Team>()
                            .HasMany(t => t.VisitorGames)
                            .WithOne(g => g.VisitorTeam)
                            .HasForeignKey(g => g.VisitorTeamId)
                            .OnDelete(DeleteBehavior.Restrict); // ondelete not working due to lines with ignore. looks like can be removed

            modelBuilder.Entity<Team>().Ignore(t => t.VisitorGames);
            modelBuilder.Entity<Team>().Ignore(t => t.HomeGames);
        }

        //public DbSet<BaseballUa.ViewModels.EventViewModel>? EventViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.CountryViewModel>? CountryViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.ClubViewModel>? ClubViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.TeamViewModel>? TeamViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.SchemaGroupViewModel>? SchemaGroupViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.EventSchemaItemViewModel>? EventSchemaItemViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.GameViewModel>? GameViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.GameViewModel>? GameViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.EventSchemaItemViewModel>? EventSchemaItemViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.TournamentViewModel>? TournamentViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.CategoryViewModel>? CategoryViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.EventViewModel>? EventViewModel { get; set; }

    }
}
