using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
    public class PlayersCrud : ICrud<Player>
    {
        private readonly BaseballUaDbContext _dbContext;

        public PlayersCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Player item)
        {
            _dbContext.Players.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Player item)
        {
            throw new NotImplementedException();
        }

        public Player Get(int itemId)
        {
            return _dbContext.Players.Where(p => p.Id == itemId).Include(p => p.Team).FirstOrDefault();
        }

        public IEnumerable<Player> GetAll()
        {
            return _dbContext.Players.Include(p => p.Team);
        }

        public IEnumerable<Player> GetAll(int teamId)
        {
            return _dbContext.Players.Where(p => p.TeamId == teamId).Include(p => p.Team);
        }

        public void Update(Player item)
        {
            throw new NotImplementedException();
        }
    }
}
