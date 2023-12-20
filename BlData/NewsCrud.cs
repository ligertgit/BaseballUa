using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
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


        public IEnumerable<News> TESTGetAllFiltered(out int countt,
                                        SportType sportType = SportType.NotDefined,
                                        bool includeAllGeneral = false,
                                        bool includeAllFun = false,
                                        bool isOfficial = false,
                                        bool isInternational = false,
                                        bool isAnnual = false,
                                        int? eventId = null,
                                        IEnumerable<int>? categoryIds = null,
                                        IEnumerable<int>? teamIds = null,
                                        DateTime? newestDate = null,
                                        int skip = 0,
                                        int amount = Constants.DefaulNewsAmount
                                        )
        {
            var fixxedNewestDate = newestDate ?? DateTime.Now.Date;

            var result = (from news in _dbContext.News.Include(n => n.NewsTitlePhotos).ThenInclude(tf => tf.Photo)
                          join eventt in _dbContext.Events on news.EventId equals eventt.Id into gEventt
                          from subEvent in gEventt.DefaultIfEmpty()
							  join tour in _dbContext.Tournaments on subEvent.TournamentId equals tour.Id into gTour
							  from subTour in gTour.DefaultIfEmpty()
								  join category in _dbContext.Categories on subTour.CategoryId equals category.Id into gCategory
								  from subCategory in gCategory.DefaultIfEmpty()
                          join ncategory in _dbContext.Categories on news.CategoryId equals ncategory.Id into gncategory
						  from subncategory in gncategory.DefaultIfEmpty()
                          join team in _dbContext.Teams on news.TeamId equals team.Id into gteam
						  from subteam in gteam.DefaultIfEmpty()
                          join titlephoto in _dbContext.NewsTitlePhotos on news.Id equals titlephoto.NewsId into gtp
						  from subtitlephoto in gtp.DefaultIfEmpty()
							//join photo in _dbContext.Photos on subtitlephoto.PhotoId equals photo.Id into gp
							//from subphoto in gtp.DefaultIfEmpty()

                          where (news.PublishDate <= fixxedNewestDate)
                                 && ((includeAllGeneral && news.IsGeneral)
                                      || (includeAllFun && subTour.IsFun)
                                      || ((sportType == SportType.NotDefined
                                                || news.SportType == sportType
                                                || subTour.Sport == sportType
                                          )
                                            && (!isOfficial || subTour.IsOfficial)
                                            && (!isInternational || subTour.IsInternational)
                                            && (!isAnnual || subTour.IsAnual)
                                            && (eventId == null || news.EventId == eventId)
                                            && (categoryIds.IsNullOrEmpty()
                                                || categoryIds.Any(c => c == news.CategoryId)
                                                || categoryIds.Any(c => c == subTour.CategoryId))
                                            && (teamIds.IsNullOrEmpty() || teamIds.Any(t => t == news.TeamId))
                                        )
                                    )
                          select new News 
						  { 
							Id = news.Id,
                            SportType = news.SportType,
                            IsGeneral = news.IsGeneral,
                            PublishDate = news.PublishDate,
							Title = news.Title,
							Description = news.Description,
							EventId  = news.EventId,
							CategoryId = news.CategoryId,
							TeamId = news.TeamId,
							Event = new Event
										{
											Id = subEvent.Id,
											Year = subEvent.Year,
											StartDate = subEvent.StartDate,
											EndDate = subEvent.EndDate,
											TournamentId = subEvent.TournamentId,
											Tournament = new Tournament
															{ 
																Id = subTour.Id,
																Name = subTour.Name,
																Sport = subTour.Sport,
																Description = subTour.Description,
                                                                IsAnual = subTour.IsAnual,
																IsInternational = subTour.IsInternational,
																IsOfficial = subTour.IsOfficial,
																IsFun = subTour.IsFun,
																CategoryId = subTour.CategoryId,
																Category = new Category
																			{
																				Id = subCategory.Id,
																				Name = subCategory.Name,
																				ShortName = subCategory.ShortName,
																			}
															}
										},
							Category = subncategory,
							Team = subteam,
                            //NewsTitlePhotos = (ICollection<NewsTitlePhoto>)gtp
                          }
						 ).Distinct()
                          .OrderByDescending(n => n.PublishDate)
                          .ThenByDescending(n => n.Id);

            //countt = result.Count();
            countt = 0;

			return result.Skip(skip)
						 .Take(amount);
        //                 .Include(n => n.NewsTitlePhotos)
								//.ThenInclude(tf => tf.Photo);
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
                                        IEnumerable<int>? teamIds = null,
                                        //int? teamId = null,
                                        DateTime? newestDate = null,
										int skip = 0,
                                        int amount = Constants.DefaulNewsAmount
										)
		{
            var fixxedNewestDate = newestDate ?? DateTime.Now.Date;
            //List<int> fixxedTeamIds = new List<int> { 1 };

			var result = (from news in _dbContext.News
						  join eventt in _dbContext.Events on news.EventId equals eventt.Id into gEventt
						  from subEvent in gEventt.DefaultIfEmpty()
							  join tour in _dbContext.Tournaments on subEvent.TournamentId equals tour.Id into gTour
							  from subTour in gTour.DefaultIfEmpty()
								  join category in _dbContext.Categories on subTour.CategoryId equals category.Id into gCategory
								  from subCategory in gCategory.DefaultIfEmpty()
						  where (news.PublishDate <= fixxedNewestDate)
								 && ((includeAllGeneral && news.IsGeneral)
									  || (includeAllFun && subTour.IsFun)
									  || ((sportType == SportType.NotDefined
												|| news.SportType == sportType
												|| subTour.Sport == sportType
										  )
											&& (!isOfficial || subTour.IsOfficial)
											&& (!isInternational || subTour.IsInternational)
											&& (!isAnnual || subTour.IsAnual)
											&& (eventId == null || news.EventId == eventId)
											&& (categoryIds.IsNullOrEmpty() 
												|| categoryIds.Any(c => c == news.CategoryId.GetValueOrDefault())
												|| categoryIds.Any(c => c == subTour.CategoryId))
                                            && (teamIds.IsNullOrEmpty() || teamIds.Any(t => t == news.TeamId.GetValueOrDefault()))
                                            //&& (fixxedTeamIds.IsNullOrEmpty() || fixxedTeamIds.Any(t => t == news.TeamId.GetValueOrDefault()))
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


        public IEnumerable<News> GetAllClubNews(out int newsCount,int? clubId, int skip = 0, int amount = Constants.DefaulNewsAmount)
		{
			var newsForClub = new List<News>();
			newsCount = 0;

            if (clubId != null && amount > 0)
			{
				var teamIds = _dbContext.Teams.Where(t => t.ClubId == clubId).Select(t => t.Id).DefaultIfEmpty();
				var query = (from news in _dbContext.News
							   where teamIds.Any(t => t == news.TeamId)
							   select news).DefaultIfEmpty()
							   .Distinct()
							   .OrderBy(n => n.PublishDate);
				newsCount = query.Count();

                newsForClub = query.Skip(skip).Take(amount)
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
