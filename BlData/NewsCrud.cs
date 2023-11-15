using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static BaseballUa.Data.Enums;

namespace BaseballUa.BlData
{
	public class NewsCrud : ICrud<News>
	{
		private readonly BaseballUaDbContext _dbContext;

		public NewsCrud(BaseballUaDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Add(News item)
		{
			_dbContext.News.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(News item)
		{
			throw new NotImplementedException();
		}

		public News Get(int itemId)
		{
			if (itemId == null) return null;

			return _dbContext.News.Where(n => n.Id == itemId)
									.Include(n => n.Albums)
										.ThenInclude(a => a.Photos)
									.Include(n => n.Videos)
									.Include(n => n.NewsTitlePhotos)
										.ThenInclude(ntp => ntp.Photo)
								.FirstOrDefault();
		}

		public IEnumerable<News> GetAll()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<News> GetAll(SportType? sportType = null,
										bool? isGeneral = null,
										int? eventId = null,
										int? categoryId = null,
										int? teamId = null,
										DateTime? lastDate = null,
										int? lastId = null,
										int? amount = null,
										bool? notForTeamOnly = false)
		{
			return _dbContext.News.Where(n => (sportType == null || n.SportType == sportType)
											&& (isGeneral == null || n.IsGeneral == isGeneral)
											&& (eventId == null || n.EventId == eventId)
											&& (categoryId == null || n.CategoryId == categoryId)
											&& (teamId == null || n.TeamId == teamId)
											&& (lastDate == null || (lastId == null ? n.PublishDate < lastDate : (n.PublishDate <= lastDate && n.Id < lastId)))
											&& (notForTeamOnly == false || (n.IsGeneral != false || n.EventId != null || n.CategoryId!= null))
										)
										.OrderByDescending(n => n.PublishDate).ThenByDescending(n => n.Id)
										.Take(amount == null ? Constants.DefaulNewsAmount : (int)amount)
										.Include(n => n.Event)
											.ThenInclude(e => e.Tournament)
										.Include(n => n.Category)
										.Include(n => n.Albums)
											.ThenInclude(a => a.Photos)
										.Include(n => n.Videos)
										.Include(n => n.NewsTitlePhotos)
											.ThenInclude(ntp => ntp.Photo);
										
		}

		public void Update(News item)
		{
			throw new NotImplementedException();
		}

        public List<SelectListItem> GetSelectItemList()
        {
            //var newsSL = new List<SelectListItem>();
            var newsSL = _dbContext.News.Where(n => n.PublishDate > DateTime.Now.AddDays(-1 * Constants.NewsSelectDaysShift))
									.Select(c => new SelectListItem
										{
											Text = c.Title,
											Value = c.Id.ToString()
										}).ToList();

            return newsSL;
        }
    }
}
