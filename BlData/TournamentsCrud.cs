using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

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
            return _dbContext.Tournaments.First(c => c.Id == itemId);
        }

        public IEnumerable<Tournament> GetAll()
        {
            return _dbContext.Tournaments;
        }

        public void Update(Tournament item)
        {
            _dbContext.Tournaments.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
