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

		public IEnumerable<Video> GetAll(SportType? sportType,
                                        bool? isGeneral,
                                        int? newsId,
                                        int? categoryId,
                                        int? teamId,
                                        int? gameId,
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
      //      return _dbContext.Videos
						//.Include(v => v.News)
						//.Include(v => v.Category)
						//.Include(v => v.Game)
						//	.ThenInclude(g => g.HomeTeam)
						//.Include(v => v.Game)
						//	.ThenInclude(g => g.VisitorTeam)
						//.Include(v => v.Game)
						//	.ThenInclude(g => g.SchemaGroup)
						//	.ThenInclude(g => g.EventSchemaItem)
						//	.ThenInclude(i => i.Event)
						//	.ThenInclude(equals => equals.Tournament);
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
                               select videos)
                                .Union(
                                from videos in _dbContext.Videos
                                join news in _dbContext.News on videos.NewsId equals news.Id
                                where news.EventId == eventId
                                select videos)
                                .Take(amount)
                                .ToList();

            }

            return eventVideos;
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
