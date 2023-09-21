using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

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
            return _dbContext.Games.First(i => i.Id == itemId);
        }

        public IEnumerable<Game> GetAll() 
        {
            return _dbContext.Games;
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
                            where game.SchemaGroupId == schemaGroupId
                            select new GameWithTeams
                            {
                                Game = game,
                                HomeTeam = ght,
                                VisitorTeam = gvt
                            }
                            );
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
