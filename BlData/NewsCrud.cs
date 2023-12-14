using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                                    .Include(n => n.Event)
                                        .ThenInclude(e => e.Tournament)
											.ThenInclude(t => t.Category)
									.Include(t => t.Category)
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
											&& (notForTeamOnly == false || (n.IsGeneral != false || n.EventId != null || n.CategoryId != null))
										)
										.OrderByDescending(n => n.PublishDate).ThenByDescending(n => n.Id)
										.Take(amount == null ? Constants.DefaulNewsAmount : (int)amount)
										.Include(n => n.Event)
											.ThenInclude(e => e.Tournament)
												.ThenInclude(n => n.Category)
										.Include(n => n.Category)
										.Include(n => n.Albums)
											.ThenInclude(a => a.Photos)
										.Include(n => n.Videos)
										.Include(n => n.NewsTitlePhotos)
											.ThenInclude(ntp => ntp.Photo);

		}
        public IEnumerable<News> GetAllFiltered(out int countt,
										SportType sportType = SportType.NotDefined,
                                        bool includeAllGeneral = false,
										bool includeAllFun = false,
										bool isOfficial = false,
										bool isInternational = false,
										bool isAnnual = false,
                                        int? eventId = null,
                                        IEnumerable<int>? categoryIds = null,
                                        int? teamId = null,
                                        DateTime? newestDate = null,
                                        //int? lastId = null,
										int skip = 0,
                                        int amount = Constants.DefaulNewsAmount
										)
		{
            var fixxedNewestDate = newestDate ?? DateTime.Now.Date;
			//var fixxedLastId = lastId ?? int.MaxValue;

			var result = (from news in _dbContext.News
						  join eventt in _dbContext.Events on news.EventId equals eventt.Id into gEventt
						  from subEvent in gEventt.DefaultIfEmpty()
						  join tour in _dbContext.Tournaments on subEvent.TournamentId equals tour.Id into gTour
						  from subTour in gTour.DefaultIfEmpty()
						  join category in _dbContext.Categories on subTour.CategoryId equals category.Id into gCategory
						  from subCategory in gCategory.DefaultIfEmpty()
							  //where (news.PublishDate < fixxedNewestDate || (news.PublishDate == fixxedNewestDate && news.Id < fixxedLastId))
						  where (news.PublishDate <= fixxedNewestDate)
								 && ((includeAllGeneral && news.IsGeneral)
									  || (includeAllFun && subTour.IsFun)
									  || ((sportType == SportType.NotDefined
												|| news.SportType == sportType
												|| subTour.Sport == sportType
													//|| (news.SportType == SportType.NotDefined
													//	&& (news.EventId == null || subTour.Sport == SportType.NotDefined))
													)
											&& (!isOfficial || subTour.IsOfficial)
											&& (!isInternational || subTour.IsInternational)
											&& (!isAnnual || subTour.IsAnual)
											&& (eventId == null || news.EventId == eventId)
											&& (categoryIds.IsNullOrEmpty() || categoryIds.Any(c => c == news.CategoryId)
																		|| categoryIds.Any(c => c == subTour.CategoryId))
												&& (teamId == null || news.TeamId == teamId)
										)
									)
						  select news)
						 .Distinct()
						 .OrderByDescending(n => n.PublishDate)
							.ThenByDescending(n => n.Id);

			countt = result.Count();
			
			return result.Skip(skip)
						 .Take(amount)
						 .Include(n => n.Category)
						 .Include(n => n.Event)
							.ThenInclude(e => e.Tournament)
								.ThenInclude(t => t.Category)
						.Include(n => n.NewsTitlePhotos)
								.ThenInclude(tf => tf.Photo);	   
		}


        public IEnumerable<News> GetAllClubNews(int? clubId, int amount = Constants.DefaulNewsAmount)
		{
			var newsForClub = new List<News>();
			if (clubId != null && amount > 0)
			{
				var teamIds = _dbContext.Teams.Where(t => t.ClubId == clubId).Select(t => t.Id).DefaultIfEmpty();
				newsForClub = (from news in _dbContext.News
							   where teamIds.Any(t => t == news.TeamId)
							   select news).DefaultIfEmpty()
							   .Distinct()
							   .OrderBy(n => n.PublishDate)
							   .Take(amount)
							   .Include(n => n.NewsTitlePhotos)
									.ThenInclude(tp => tp.Photo)
							   .ToList();
			}

			return newsForClub;
		}

        public IEnumerable<News> GetAllTeamNews(out int countt, int? teamId, int skip = 0, int amount = Constants.DefaulNewsAmount )
        {
            var newsForClub = new List<News>();
            countt = 0;

            if (teamId != null && amount > 0)
            {
				//var teamIds = _dbContext.Teams.Where(t => t.ClubId == clubId).Select(t => t.Id).DefaultIfEmpty();
				var result = (from news in _dbContext.News
							   where news.TeamId == teamId
							   select news).DefaultIfEmpty()
							   .Distinct()
							   .OrderBy(n => n.PublishDate)
							   .ThenBy(n => n.Id);

				countt = result.Count();

				newsForClub = result.Skip(skip)
									.Take(amount)
									.Include(n => n.NewsTitlePhotos)
										.ThenInclude(tp => tp.Photo)
									.ToList();
            }
            return newsForClub;
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
