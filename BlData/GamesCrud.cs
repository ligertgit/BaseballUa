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
                var temp = (from game in _dbContext.Games
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
                return temp;


                //from a in objContext.FileProgresses
                //                    join pg in objContext.V01_PG on a.ProDocsId equals (int?)pg.ID into pgs
                //                    from m in pgs.DefaultIfEmpty()
                //                    join pr in objContext.V01_PR on m.ID equals pr.PAGE into prs
                //                    from p in prs.DefaultIfEmpty()
                //                    join ds in objContext.DOCSTATs on p.DOCSTAT equals ds.ID into docs
                //                    from docst in docs.DefaultIfEmpty()
                //                    where a.FullPath.Contains(txtSearchText.Text)
                //                    select new
                //                    {
                //                        a.Id,

                //from a in objContext.FileProgresses
                //                        join pg in objContext.V01_PG on a.ProDocsId equals (int?)pg.ID into pgs
                //                        from g in pgs.DefaultIfEmpty()
                //                        join pr in objContext.V01_PR on g.ID equals pr.PAGE into prs
                //                        from p in prs.DefaultIfEmpty()
                //                        where a.FullPath.Contains(extension)
                //                        select new
                //                        {
                //                            a.Id,

            }
            return (from game in _dbContext.Games
                    join homeTeam in _dbContext.Teams on game.HomeTeamId equals homeTeam.Id
                    join visitorTeam in _dbContext.Teams on game.VisitorTeamId equals visitorTeam.Id
                    select new GameWithTeams
                    {
                        Game = game,
                        HomeTeam = homeTeam,
                        VisitorTeam = visitorTeam
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
