using BaseballUa.Data;
using BaseballUa.Models;

namespace BaseballUa.BlData
{
    public class EventsCrud : ICrud<Event>
    {
        private readonly BaseballUaDbContext _dbContext;

        public EventsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;   
        }
        public void Add(Event item)
        {
            _dbContext.Events.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Event item)
        {
            throw new NotImplementedException();
        }

        public Event Get(int itemId)
        {
            var eventItem = _dbContext.Events.First(a => a.Id == itemId);
            return eventItem;
        }

        public IEnumerable<Event> GetAll()
        {
            return _dbContext.Events;
        }

        public void Update(Event item)
        {
            _dbContext.Events.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
