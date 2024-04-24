using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;
using static BaseballUa.Data.Enums;
using BaseballUa.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BaseballUa.Data
{
    public class BaseballUaDbContext : IdentityDbContext<BaseballUaUser>
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
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<NewsTitlePhoto> NewsTitlePhotos { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<EventToTeams> EventToTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //tournament defaults
            builder.Entity<Tournament>()
                .Property(b => b.IsInternational)
                .HasDefaultValue(false);

            builder.Entity<Tournament>()
                .Property(b => b.IsAnual)
                .HasDefaultValue(false);

            builder.Entity<Tournament>()
                .Property(b => b.IsOfficial)
                .HasDefaultValue(false);

            builder.Entity<Tournament>()
                .Property(b => b.IsFun)
                .HasDefaultValue(false);

            builder.Entity<Tournament>()
                .Property(b => b.Sport)
                .HasDefaultValue(SportType.NotDefined);


            builder.Entity<NewsTitlePhoto>()
                .HasOne(t => t.News)
                .WithMany(n => n.NewsTitlePhotos)
                .HasForeignKey(t => t.NewsId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<NewsTitlePhoto>()
                .HasOne(t => t.Photo)
                .WithMany(n => n.NewsTitlePhotos)
                .HasForeignKey(t => t.PhotoId)
                .OnDelete(DeleteBehavior.NoAction);

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

            builder.Entity<Team>()
                            .HasMany(t => t.HomeGames)
                            .WithOne(g => g.HomeTeam)
                            .HasForeignKey(g => g.HomeTeamId)
                            .OnDelete(DeleteBehavior.Restrict); // ondelete not working due to lines with ignore. looks like can be removed

            builder.Entity<Team>()
                            .HasMany(t => t.VisitorGames)
                            .WithOne(g => g.VisitorTeam)
                            .HasForeignKey(g => g.VisitorTeamId)
                            .OnDelete(DeleteBehavior.Restrict); // ondelete not working due to lines with ignore. looks like can be removed

            builder.Entity<Team>().Ignore(t => t.VisitorGames);
            builder.Entity<Team>().Ignore(t => t.HomeGames);

            builder.Entity<EventToTeams>()
                            .HasOne(et => et.Event)
                            .WithMany(e => e.EventToteams)
                            .HasForeignKey(et => et.EventId)
                            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<EventToTeams>()
                            .HasOne(et => et.Team)
                            .WithMany(t => t.EventToTeams)
                            .HasForeignKey(et => et.TeamId)
                            .OnDelete(DeleteBehavior.NoAction);

        }

        //public DbSet<BaseballUa.ViewModels.TeamViewModel>? TeamViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.PlayerViewModel>? PlayerViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.TeamViewModel>? TeamViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.ClubViewModel>? ClubViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.StaffViewModel>? StaffViewModel { get; set; }

        //public DbSet<BaseballUa.ViewModels.GameViewModel>? GameViewModel { get; set; }

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
