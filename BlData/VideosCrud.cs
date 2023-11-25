using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static BaseballUa.Data.Enums;

namespace BaseballUa.BlData
{
	public class VideosCrud : ICrud<Video>
	{
		private readonly BaseballUaDbContext _dbContext;

        public VideosCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(Video item)
		{
			_dbContext.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(Video item)
		{
			throw new NotImplementedException();
		}

		public Video Get(int itemId)
		{
			return _dbContext.Videos.Where(v => v.Id == itemId)
						.Include(v => v.News)
						.Include(v => v.Category)
						.Include(v => v.Game)
							.ThenInclude(g => g.SchemaGroup)
							.ThenInclude(g => g.EventSchemaItem)
							.ThenInclude(i => i.Event)
							.ThenInclude(e => e.Tournament)
					.FirstOrDefault();
		}

		public IEnumerable<Video> GetAll(SportType? sportType = null,
                                        bool? isGeneral = null,
                                        int? newsId = null,
                                        int? categoryId = null,
                                        int? teamId	= null,
                                        int? gameId = null,
                                        DateTime? lastDate = null,
                                        int? lastId = null,
                                        int? amount = null)
		{

			return _dbContext.Videos.Where(a => (sportType == null || a.SportType == sportType)
											&& (isGeneral == null || a.IsGeneral == isGeneral)
											&& (newsId == null || a.NewsId == newsId)
											&& (categoryId == null || a.CategoryId == categoryId)
											&& (teamId == null || a.TeamId == teamId)
											&& (gameId == null || a.GameId == gameId)
											&& (lastDate == null || (lastId == null ? a.PublishDate < lastDate : a.PublishDate <= lastDate && a.Id < lastId))
											)
											.OrderByDescending(a => a.PublishDate).ThenByDescending(a => a.Id)
											.Take(amount == null ? Constants.DefaulVideosAmount : (int)amount);
		}

		public IEnumerable<Video> GetAllHard(SportType? sportType = null,
										bool? isGeneral = null,
										int? newsId = null,
										int? categoryId = null,
										int? teamId = null,
										int? gameId = null,
										DateTime? lastDate = null,
										int? lastId = null,
										int? amount = null)
		{

			return _dbContext.Videos.Where(a => (sportType == null || a.SportType == sportType)
											&& (isGeneral == null || a.IsGeneral == isGeneral)
											&& (newsId == null || a.NewsId == newsId)
											&& (categoryId == null || a.CategoryId == categoryId)
											&& (teamId == null || a.TeamId == teamId)
											&& (gameId == null || a.GameId == gameId)
											&& (lastDate == null || (lastId == null ? a.PublishDate < lastDate : a.PublishDate <= lastDate && a.Id < lastId))
											)
											.OrderByDescending(a => a.PublishDate).ThenByDescending(a => a.Id)
											.Take(amount == null ? Constants.DefaulVideosAmount : (int)amount)
											.Include(a => a.News)
											.Include(a => a.Category)
											.Include(a => a.Team)
											.Include(v => v.Game)
												.ThenInclude(g => g.HomeTeam)
											.Include(v => v.Game)
												.ThenInclude(g => g.VisitorTeam)
											.Include(a => a.Game)
												.ThenInclude(g => g.SchemaGroup)
													.ThenInclude(g => g.EventSchemaItem)
														.ThenInclude(i => i.Event)
															.ThenInclude(e => e.Tournament)
																.ThenInclude(t => t.Category);
		}

		public IEnumerable<Video> GetAll()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Video> GetAllEventVideos(int? eventId,int amount = Constants.DefaulVideosAmount)
        {
            var eventVideos = new List<Video>();
            if (eventId != null && amount >0)
            {
                eventVideos = (from videos in _dbContext.Videos
                               join games in _dbContext.Games on videos.GameId equals games.Id
                               join eventGroups in _dbContext.SchemaGroups on games.SchemaGroupId equals eventGroups.Id
                               join eventSchemas in _dbContext.EventSchemaItems on eventGroups.EventSchemaItemId equals eventSchemas.Id
                               where eventSchemas.EventId == eventId
                               select videos).Take(amount)
								.Union(
                                from videos in _dbContext.Videos
                                join news in _dbContext.News on videos.NewsId equals news.Id
                                where news.EventId == eventId
                                select videos).Take(amount)
								.Take(amount)
                                .ToList();

            }
			return eventVideos;
        }

		public IEnumerable<Video> GetAllCategoryVideos(int? categoryId, int amount = Constants.DefaulVideosAmount)
		{
			var categoryVideos = new List<Video>();
			if (categoryId != null && amount > 0)
			{
                categoryVideos = (from videos in _dbContext.Videos
							   join games in _dbContext.Games on videos.GameId equals games.Id
							   join eventGroups in _dbContext.SchemaGroups on games.SchemaGroupId equals eventGroups.Id
							   join eventSchemas in _dbContext.EventSchemaItems on eventGroups.EventSchemaItemId equals eventSchemas.Id
                               join events in _dbContext.Events on eventSchemas.EventId equals events.Id
                               join tournament in _dbContext.Tournaments on events.TournamentId equals tournament.Id
							   where tournament.CategoryId == categoryId
							   select videos).Take(amount)
								.Union(
								from videos in _dbContext.Videos
								join news in _dbContext.News on videos.NewsId equals news.Id
								where news.CategoryId == categoryId
								select videos).Take(amount)
								.Union(
                                from videos in _dbContext.Videos
                                join news in _dbContext.News on videos.NewsId equals news.Id
                                join events in _dbContext.Events on news.EventId equals events.Id
                                join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
                                where tournaments.CategoryId == categoryId
                                select videos).Take(amount)
								.Union(
								from videos in _dbContext.Videos
								where videos.CategoryId == categoryId
								select videos).Take(amount)
								.Distinct()
                                .OrderByDescending(v => v.PublishDate)
								.Take(amount)
								.ToList();
			}
			return categoryVideos;
		}

		public IEnumerable<Video> GetAllTeamVideos(int? teamId, int amount = Constants.DefaulVideosAmount)
		{
			var teamVideos = new List<Video>();
			if (teamId != null && amount > 0)
			{
                teamVideos = (from videos in _dbContext.Videos
							   join games in _dbContext.Games on videos.GameId equals games.Id
							   where games.HomeTeamId == teamId || games.VisitorTeamId == teamId
							   select videos).Take(amount)
								.Union(
								from videos in _dbContext.Videos
								where videos.TeamId == teamId
								select videos).Take(amount)
								.Distinct()
								.OrderByDescending(v => v.PublishDate)
								.Take(amount)
								.ToList();
			}
			return teamVideos;
		}

        public IEnumerable<Video> GetAllClubVideos(int? clubId, int amount = Constants.DefaulVideosAmount)
        {
            var clubVideos = new List<Video>();
			
            if (clubId != null && amount > 0)
            {
                var teamIds = new TeamCrud(_dbContext).GetAll((int)clubId).Select(t => t.Id).DefaultIfEmpty().ToList();
                clubVideos = (from videos in _dbContext.Videos
                              join games in _dbContext.Games on videos.GameId equals games.Id
                              where teamIds.Contains(games.HomeTeamId ?? 0) 
									|| teamIds.Contains(games.VisitorTeamId ?? 0)
									|| teamIds.Contains(videos.TeamId ?? 0)
							  select videos).DefaultIfEmpty()
                                .Distinct()
                                .OrderByDescending(v => v.PublishDate)
                                .Take(amount)
                                .ToList();
				//clubVideos = (from videos in _dbContext.Videos
				//			  join games in _dbContext.Games on videos.GameId equals games.Id
				//			  where teamIds.Any(t => t == games.HomeTeamId)
				//					|| teamIds.Any(t => t == games.VisitorTeamId)
				//					|| teamIds.Any(t => t == videos.TeamId)
				//			  select videos).DefaultIfEmpty()
				//.Distinct()
				//.OrderByDescending(v => v.PublishDate)
				//.Take(amount)
				//.ToList();
			}
            return clubVideos;
        }

        public IEnumerable<Video> GetAllSportTypeVideos(SportType? sportType, int amount = Constants.DefaulVideosAmount)
		{
			var sportTypeVideos = new List<Video>();
			if (sportType != null && amount > 0)
			{
                sportTypeVideos = (from videos in _dbContext.Videos
							   join games in _dbContext.Games on videos.GameId equals games.Id
							   join eventGroups in _dbContext.SchemaGroups on games.SchemaGroupId equals eventGroups.Id
							   join eventSchemas in _dbContext.EventSchemaItems on eventGroups.EventSchemaItemId equals eventSchemas.Id
							   join events in _dbContext.Events on eventSchemas.EventId equals events.Id
							   join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
							   where tournaments.Sport == sportType
							   select videos).Take(amount)
								.Union(
								from videos in _dbContext.Videos
								where videos.SportType == sportType
								select videos).Take(amount)
								.Union(
								from videos in _dbContext.Videos
								join teams in _dbContext.Teams on videos.TeamId equals teams.Id
								where teams.SportType == sportType
								select videos).Take(amount)
								.Union(
								from videos in _dbContext.Videos
								join news in _dbContext.News on videos.NewsId equals news.Id
								join events in _dbContext.Events on news.EventId equals events.Id
								join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
								where tournaments.Sport == sportType
								select videos).Take(amount)

								.Distinct()
								.OrderByDescending(v => v.PublishDate)
								.Take(amount)
								.ToList();
			}
			return sportTypeVideos;
		}


		public void Update(Video item)
		{
			throw new NotImplementedException();
		}

        public void Update(int id,
                            SportType? sportType = null,
                            bool? isGeneral = null,
                            string? name = null,
                            string? description = null,
                            string? fname = null,
                            DateTime? publishDate = null,
                            int? newsId = null,
                            int? categoryId = null,
                            int? teamId = null,
                            int? gameId = null)
        {
            if (sportType != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.SportType, sportType));
            if (isGeneral != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.IsGeneral, isGeneral));
            if (name != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.Name, name));
            if (description != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.Description, description));
            if (fname != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.Fname, fname));
            if (publishDate != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.PublishDate, publishDate));
            if (newsId != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.NewsId, newsId));
            if (categoryId != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.CategoryId, categoryId));
            if (teamId != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.TeamId, teamId));
            if (gameId != null) _dbContext.Videos.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.GameId, gameId));

        }

        public List<SelectListItem> GetSelectItemList()
        {
            var videosSL = _dbContext.Videos.OrderByDescending(a => a.Id).Take(Constants.DefaulSelectListAmount)
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList();

            return videosSL;
        }
    }
}
