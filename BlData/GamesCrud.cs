using BaseballUa.Data;
using BaseballUa.Models;

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
