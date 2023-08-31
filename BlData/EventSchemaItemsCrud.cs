using BaseballUa.Data;
using BaseballUa.Models;

namespace BaseballUa.BlData
{
    public class EventSchemaItemsCrud : ICrud<EventSchemaItem>
    {
        private readonly BaseballUaDbContext _dbContext;

        public EventSchemaItemsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;            
        }
        public void Add(EventSchemaItem item)
        {
            _dbContext.EventSchemaItems.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(EventSchemaItem item)
        {
            throw new NotImplementedException();
        }

        public EventSchemaItem Get(int itemId)
        {
            return _dbContext.EventSchemaItems.First(i => i.Id == itemId);
        }

        public IEnumerable<EventSchemaItem> GetAll()
        {
            return _dbContext.EventSchemaItems;
        }

        public void Update(EventSchemaItem item)
        {
            _dbContext.EventSchemaItems.Update(item);
            _dbContext.SaveChanges();
        }

        //=====================
        public List<EventSchemaItem> GetEventSchemaItems(int eventId)
        { 
            var eventSchemaItems = _dbContext.EventSchemaItems.Where(i => i.EventId == eventId).ToList();
            return eventSchemaItems;
        }

    }
}
