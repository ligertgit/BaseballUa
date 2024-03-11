using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using BaseballUa.Migrations;

namespace BaseballUa.BlData
{
    public class GamesCrud : ICrud<Game>
    {
        private readonly BaseballUaDbContext _dbContext;

        public GamesCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Game item)
        {
            _dbContext.Games.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Game item)
        {
            _dbContext.Games.Remove(item);
            _dbContext.SaveChanges();
        }

        public Game Get(int itemId)
        {
            if (itemId == null) return null;

            return _dbContext.Games.Where(i => i.Id == itemId)
                                    .Include(g => g.SchemaGroup)
                                    .Include(g => g.HomeTeam)
                                    .Include(g => g.VisitorTeam)
                                    .FirstOrDefault();
        }

        public Game GetWithTeamsAndMedia(int itemId)
        {
            if (itemId < 1) return null;

            return _dbContext.Games.Where(i => i.Id == itemId)
                                    .Include(g => g.HomeTeam)
                                    .Include(g => g.VisitorTeam)
                                    .Include(g => g.Albums)
                                    .Include(g => g.Videos)
                                    .FirstOrDefault();
        }

        public IEnumerable<Game> GetAll() 
        {
            return _dbContext.Games.Include(g => g.SchemaGroup)
                                    .Include(g => g.HomeTeam)
                                    .Include(g => g.VisitorTeam);
        }

        public IEnumerable<Game> GetAll(int schemaGroupId)
        {
            if (schemaGroupId != 0) 
            {
                return _dbContext.Games.Include(g => g.SchemaGroup)
                                        .ThenInclude(g => g.EventSchemaItem)
                                            .ThenInclude(i => i.Event)
                                                .ThenInclude(e => e.Tournament)
                                                    .ThenInclude(t => t.Category)
                                    .Include(g => g.HomeTeam)
                                    .Include(g => g.VisitorTeam)
                                    .Where(g => g.SchemaGroupId == schemaGroupId);
            }
            return _dbContext.Games.Include(g => g.SchemaGroup)
                                        .ThenInclude(g => g.EventSchemaItem)
                                            .ThenInclude(i => i.Event)
                                                .ThenInclude(e => e.Tournament)
                                                    .ThenInclude(t => t.Category)
                                    .Include(g => g.HomeTeam)
                                    .Include(g => g.VisitorTeam);
        }

        public IEnumerable<Game> GetEventGames(int eventId, int amount = Constants.DefaultGameAmount)
        {
            var eventGames = new List<Game>();
            if (eventId > 0 && amount > 0)
            {
                var temp = (from game in _dbContext.Games
                              from eventGroup in _dbContext.SchemaGroups
                              where game.SchemaGroupId == eventGroup.Id
                              from eventItem in _dbContext.EventSchemaItems
                              where eventGroup.EventSchemaItemId == eventItem.Id
                              where eventItem.EventId == eventId
                                        //&& game.StartDate < DateTime.Now.AddDays(Constants.DefaulActiveGamesDaysRange)
                                        //&& game.StartDate > DateTime.Now.AddDays(-Constants.DefaulActiveGamesDaysRange)
                              select game)
                              .Include(g => g.HomeTeam)
                              .Include(g => g.VisitorTeam)
                              .ToList();
                eventGames = temp.OrderBy(g => g.StartDate.GetValueOrDefault().Subtract(DateTime.Now))
                                 .Take(amount)
                                 .OrderBy(g => g.StartDate)
                                 
                                 .ToList();
            }

            return eventGames;
        }

        public IEnumerable<Game> GetAllForClub(out int gamesCount, int clubId, int skip = 0, int amount = Constants.DefaultGameAmount)
        { 
            //var clubGames = new List<Game> ();

            //from game in _dbContext.Games
            //join subhteam in _dbContext.Teams on game.HomeTeamId equals subhteam.Id into ghteams
            //from hteam in ghteams.DefaultIfEmpty()
            //join subvteam in _dbContext.Teams on game.VisitorTeamId equals subvteam.Id into gvteams
            //from vteam in gvteams.DefaultIfEmpty()
            //where hteam.ClubId == clubId || vteam.ClubId == clubId
            var query = (from game in _dbContext.Games
                         join hteam in _dbContext.Teams on game.HomeTeamId equals hteam.Id
                         join vteam in _dbContext.Teams on game.VisitorTeamId equals vteam.Id
                         where hteam.ClubId == clubId || vteam.ClubId == clubId
                         select new Game
                         {
                             Id = game.Id,
                             Name = game.Name,
                             StartDate = game.StartDate,
                             AdditionalInfo = game.AdditionalInfo,
                             RunsVisitor = game.RunsVisitor,
                             RunsHome = game.RunsHome,
                             PlacedAt = game.PlacedAt,
                             HalfinningsPlayed = game.HalfinningsPlayed,
                             GameStatus = game.GameStatus,
                             PointsVisitor = game.PointsVisitor,
                             PointsHome = game.PointsHome,
                             Tour = game.Tour,
                             ConditionVisitor = game.ConditionVisitor,
                             ConditionHome = game.ConditionHome,
                             SchemaGroupId = game.SchemaGroupId,
                             HomeTeamId = game.HomeTeamId,
                             VisitorTeamId = game.VisitorTeamId,
                             HomeTeam = hteam,
                             VisitorTeam = vteam
                         }
                        ).Distinct();

            gamesCount = query.Count();
            var clubGames = query.OrderByDescending(g => g.StartDate).ThenByDescending(g => g.Id).Skip(skip).Take(amount);

            return clubGames;    
        }


        public IEnumerable<GameWithTeams> GetAllForGroupWithTeams(int schemaGroupId = 0)
        {
            if (schemaGroupId != 0) 
            {
                return (from game in _dbContext.Games
                            join homeTeam in _dbContext.Teams on game.HomeTeamId equals homeTeam.Id into subght
                            from ght in subght.DefaultIfEmpty()
                            join visitorTeam in _dbContext.Teams on game.VisitorTeamId equals visitorTeam.Id into subgvt
                            from gvt in subgvt.DefaultIfEmpty()
                            join schemaGroup in _dbContext.SchemaGroups on game.SchemaGroupId equals schemaGroup.Id
                            where game.SchemaGroupId == schemaGroupId
                            select new GameWithTeams
                            {
                                Game = new Game
                                        {
                                            Id = game.Id,
                                            Name = game.Name,
                                            StartDate = game.StartDate,
                                            AdditionalInfo = game.AdditionalInfo,
                                            RunsVisitor = game.RunsVisitor,
                                            RunsHome = game.RunsHome,
                                            PlacedAt = game.PlacedAt,
                                            HalfinningsPlayed = game.HalfinningsPlayed,
                                            GameStatus = game.GameStatus,
                                            PointsVisitor = game.PointsVisitor,
                                            PointsHome = game.PointsHome,
                                            Tour = game.Tour,
                                            ConditionVisitor = game.ConditionVisitor,
                                            ConditionHome = game.ConditionHome,
                                            SchemaGroupId = game.SchemaGroupId,
                                            HomeTeamId = game.HomeTeamId,
                                            VisitorTeamId = game.VisitorTeamId,
                                            HomeTeam = game.HomeTeam,
                                            VisitorTeam = game.VisitorTeam,
                                            SchemaGroup = schemaGroup
                                        },
                                HomeTeam = ght,
                                VisitorTeam = gvt
                            }
                            );
                //return (from game in _dbContext.Games
                //        join homeTeam in _dbContext.Teams on game.HomeTeamId equals homeTeam.Id into subght
                //        from ght in subght.DefaultIfEmpty()
                //        join visitorTeam in _dbContext.Teams on game.VisitorTeamId equals visitorTeam.Id into subgvt
                //        from gvt in subgvt.DefaultIfEmpty()
                //        where game.SchemaGroupId == schemaGroupId
                //        select new GameWithTeams
                //        {
                //            Game = game,
                //            HomeTeam = ght,
                //            VisitorTeam = gvt
                //        }
                //        );
            }
            return (from game in _dbContext.Games
                    join homeTeam in _dbContext.Teams on game.HomeTeamId equals homeTeam.Id into subght
                    from ght in subght.DefaultIfEmpty()
                    join visitorTeam in _dbContext.Teams on game.VisitorTeamId equals visitorTeam.Id into subgvt
                    from gvt in subgvt.DefaultIfEmpty()
                    select new GameWithTeams
                    {
                        Game = game,
                        HomeTeam = ght,
                        VisitorTeam = gvt
                    }
                    );
        }

        public void Update(Game item)
        {
            _dbContext.Games.Update(item);
            _dbContext.SaveChanges();
        }

        //!!!!!!!!!!!!! fix
        //public List<Game> GetForEventSchema(int eventSchemaId)
        //{
        //    var gamesForEventSchema = _dbContext.Games.Where(g => g.EventSchemaItemId == eventSchemaId).ToList();
        //    return gamesForEventSchema;
        //}

        public List<SelectListItem> GetSelectItemList()
        {
            var gamesSL = _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam)
                                            .OrderByDescending(g => g.Id)
                                            .Take(Constants.DefaulSelectListAmount)
                                            .OrderByDescending(g => g.StartDate)
                                            .Select(c => new SelectListItem
                                            {
                                                Text = c.StartDate == null ? "--.--" : ((DateTime)c.StartDate).ToString("MM.dd") + " " + c.Name + " " + (c.VisitorTeam == null ? " - " : c.VisitorTeam.Name) + " - " + (c.HomeTeam == null ? " - " : c.HomeTeam.Name),
                                                Value = c.Id.ToString()
                                            }).ToList();

            return gamesSL;
        }

        //public List<SelectListItem> GetSelectItemList()
        //{
        //    var gamesSL = _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam).Where(g => (g.StartDate > DateTime.Now.AddDays(-Constants.GamesSelectDaysShift)
        //                                        && (g.StartDate < DateTime.Now.AddDays(Constants.GamesSelectDaysShift)))
        //                                    ).Select(c => new SelectListItem
        //                                    {
        //                                        Text = c.StartDate == null ? "--.--" : ((DateTime)c.StartDate).ToString("MM.dd") + " " + c.Name + " " + (c.VisitorTeam == null ? " - " : c.VisitorTeam.Name) + " - " + (c.HomeTeam == null ? " - " : c.HomeTeam.Name),
        //                                        Value = c.Id.ToString()
        //                                    }).ToList();

        //    return gamesSL;
        //}
    }
}
