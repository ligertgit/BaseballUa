using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BaseballUa.BlData
{
    public class TournamentsCrud : ICrud<Tournament>
    {
        private readonly BaseballUaDbContext _dbContext;

        public TournamentsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Tournament item)
        {
            _dbContext.Tournaments.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Tournament item)
        {
            _dbContext.Tournaments.Remove(item);
            _dbContext.SaveChanges();
        }

        public Tournament Get(int itemId)
        {
            //return _dbContext.Tournaments.First(c => c.Id == itemId);
            //var tournament = (from tournamentt in _dbContext.Tournaments
            //                  join category in _dbContext.Categories on tournamentt.CategoryId equals category.Id
            //                  where tournamentt.Id == itemId
            //                  select new Tournament
            //                  {
            //                      Id = tournamentt.Id,
            //                      //...
            //                  }
            //                  );

            // fix. result may be null
            var temp = _dbContext.Tournaments.Where(t => t.Id == itemId).Include(c => c.Category).FirstOrDefault();
            return temp;
        }

        public IEnumerable<Tournament> GetAll()
        {
            var temp = _dbContext.Tournaments.Include(c => c.Category);
            return temp;
        }

        public void Update(Tournament item)
        {
            _dbContext.Tournaments.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
