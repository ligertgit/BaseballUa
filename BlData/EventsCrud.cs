using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Drawing;
using System.Linq;
using static BaseballUa.Data.Enums;

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
            //check for null here
            //var eventItem = _dbContext.Events.First(a => a.Id == itemId);
            //_dbContext.ChangeTracker.LazyLoadingEnabled = false;
            var eventItem = _dbContext.Events.Where(e => e.Id == itemId)
                                                .Include(e => e.Tournament)
                                                    .ThenInclude(t => t.Category)
                                                .Include(e => e.News) 
                                                    .ThenInclude(n => n.NewsTitlePhotos)
                                                        .ThenInclude(t => t.Photo)
                                                .FirstOrDefault();
            
            return eventItem ?? new Event();
        }

        public IEnumerable<Event> GetAll()
        {
            var eventItem = _dbContext.Events;
            //throw new NotImplementedException();
            //return _dbContext.Events.Include(e => e.Tournament).ThenInclude(t => t.Category);
            return eventItem;
		}

		public IEnumerable<Event> GetAll(SportType? sportType = null,
										int? categoryId = null,
										DateTime? firstDate = null,
										DateTime? lastDate = null,
										int? lastId = null,
										int? amount = null)
		{
			return _dbContext.Events.Where(e => ( firstDate == null || lastDate == null 
                                                    || ( firstDate < e.StartDate && lastDate > e.StartDate )
                                                    || ( lastDate > e.EndDate && firstDate < e.EndDate )
                                                    || ( firstDate > e.StartDate && lastDate < e.EndDate )
                                                ) 
                                                && ( lastId == null || e.Id < lastId)
                                                && ( categoryId == null || e.Tournament.CategoryId  == categoryId )
                                                && ( sportType == null || e.Tournament.Sport == sportType)
                                          ).OrderByDescending( e => e.StartDate )
                                          .Take( amount == null ? Constants.DefaulEventAmount : (int)amount )
                                          .Include( e => e.Tournament )
                                                .ThenInclude( t => t.Category )
                                          .Include(e => e.News);
		}

		public IEnumerable<Event> GetAllForClub(int clubId)
        {
            var result = (from eventt in _dbContext.Events.Include(e => e.Tournament).ThenInclude(t => t.Category)
                         join eventItem in _dbContext.EventSchemaItems on eventt.Id equals eventItem.EventId
                         join schemaGroup in _dbContext.SchemaGroups on eventItem.Id equals schemaGroup.EventSchemaItemId
                         join game in _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam) on schemaGroup.Id equals game.SchemaGroupId
                         where (    (eventt.StartDate > DateTime.Now.AddMonths(-3) && eventt.StartDate < DateTime.Now.AddMonths(1)) &&
                                    (game.HomeTeam.ClubId == clubId || game.VisitorTeam.ClubId == clubId)
                               )
                         select eventt).Distinct();

            return result;
        }

        public IEnumerable<Event> GetAllForTeam(int teamId)
        {
            var result = (from eventt in _dbContext.Events.Include(e => e.Tournament).ThenInclude(t => t.Category)
                          join eventItem in _dbContext.EventSchemaItems on eventt.Id equals eventItem.EventId
                          join schemaGroup in _dbContext.SchemaGroups on eventItem.Id equals schemaGroup.EventSchemaItemId
                          join game in _dbContext.Games on schemaGroup.Id equals game.SchemaGroupId
                          where ((eventt.StartDate > DateTime.Now.AddMonths(-3) && eventt.StartDate < DateTime.Now.AddMonths(1)) &&
                                     (game.HomeTeamId == teamId || game.VisitorTeamId == teamId)
                                )
                          select eventt).Distinct();

            return result;
        }

        public void Update(Event item)
        {
            _dbContext.Events.Update(item);
            _dbContext.SaveChanges();
        }


        //remove??
        public IEnumerable<Event> GetMonth(int monthShift)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(monthShift);
            var endDate = startDate.AddMonths(1);
            return _dbContext.Events.Where(i =>
                                ((i.StartDate >= startDate && i.StartDate <= endDate)
                                || (i.EndDate >= startDate && i.EndDate <= endDate))
                                || (i.StartDate <= startDate && i.EndDate >= endDate)
                                ).Include(e => e.Tournament).ThenInclude(t => t.Category);
        }

        public List<SelectListItem> GetSelectItemList(int amount = Constants.DefaulSelectListAmount)
        {
            if (amount < 1) 
            { 
                amount = Constants.DefaulSelectListAmount;
            }

            var eventSL = new List<SelectListItem>();
            eventSL = _dbContext.Events.OrderByDescending(e => e.StartDate).Take(amount).Include(e => e.Tournament)
                                    .Select(e => new SelectListItem
                                    {
                                        Text = e.Tournament.Name + " - " + e.Year.ToString(),
                                        Value = e.Id.ToString()
                                    }).ToList();

            return eventSL;
        }


    }
}
