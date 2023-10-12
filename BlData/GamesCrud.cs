using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;

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
            throw new NotImplementedException();
        }

        public Game Get(int itemId)
        {
            return _dbContext.Games.Where(i => i.Id == itemId)
                                    .Include(g => g.SchemaGroup)
                                    .Include(g => g.HomeTeam)
                                    .Include(g => g.VisitorTeam)
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
    }
}
