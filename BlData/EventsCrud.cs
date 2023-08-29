using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Drawing;
using System.Linq;

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


        //remove
        public IEnumerable<Event> GetMonth(int monthShift)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(monthShift);
            //var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + monthShift, 1);
            var endDate = startDate.AddMonths(1);
            return _dbContext.Events.Where(i =>
                                ((i.StartDate >= startDate && i.StartDate <= endDate)
                                || (i.EndDate >= startDate && i.EndDate <= endDate))
                                || (i.StartDate <= startDate && i.EndDate >= endDate)
                                );
        }

        
    }
}
