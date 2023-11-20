using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static BaseballUa.Data.Enums;

namespace BaseballUa.BlData
{
	public class AlbumsCrud : ICrud<Album>
	{
		private readonly BaseballUaDbContext _dbContext;

        public AlbumsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(Album item)
		{
			_dbContext.Albums.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(Album item)
		{
			throw new NotImplementedException();
		}

		public Album Get(int itemId)
		{
			return _dbContext.Albums.Where(a => a.Id == itemId)
										.Include(a => a.News)
										.Include(a => a.Category)
										.Include(a => a.Team)
										.Include(a => a.Game)
											.ThenInclude(g => g.SchemaGroup)
												.ThenInclude(g => g.EventSchemaItem)
													.ThenInclude(i => i.Event)
														.ThenInclude(e => e.Tournament)
															.ThenInclude(t => t.Category)
										.Include(a => a.Photos)
									.FirstOrDefault();
		}

        public IEnumerable<Album> GetAll()
		{
            throw new NotImplementedException();
        }

        public IEnumerable<Album> GetAll(SportType? sportType = null,
                                        bool? isGeneral = null,
                                        int? newsId = null,
                                        int? categoryId = null,
                                        int? teamId = null,
                                        int? gameId = null,
                                        DateTime? lastDate = null,
                                        int? lastId = null,
                                        int? amount = null,
										bool notForTeamOnly = false)
		{
			return _dbContext.Albums.Where(a => (sportType == null || a.SportType == sportType)
											&& (isGeneral == null || a.IsGeneral == isGeneral)
                                            && (newsId == null || a.NewsId == newsId)
                                            && (categoryId == null || a.CategoryId == categoryId)
											&& (teamId == null || a.TeamId == teamId)
											&& (gameId == null || a.GameId == gameId)
											&& (lastDate == null || (lastId == null ? a.PublishDate < lastDate : a.PublishDate <= lastDate && a.Id < lastId))
											&& (!notForTeamOnly || (a.IsGeneral || a.News.EventId != null || a.CategoryId != null || a.GameId != null))
											)
											.OrderByDescending(a => a.PublishDate).ThenByDescending(a => a.Id)
											.Take(amount == null ? Constants.DefaulAlbumsAmount : (int)amount)
											.Include(a => a.News)
											.Include(a => a.Category)
											.Include(a => a.Team)
											.Include(a => a.Game)
												.ThenInclude(g => g.SchemaGroup)
													.ThenInclude(g => g.EventSchemaItem)
														.ThenInclude(i => i.Event)
															.ThenInclude(e => e.Tournament)
																.ThenInclude(t => t.Category)
											.Include(a => a.Photos);
		}

		public void Update(Album item)
		{
			throw new NotImplementedException();
		}

		public void Update(int id, 
							SportType? sportType = null, 
							bool? isGeneral = null, 
							string? name = null, 
							string? description = null, 
							DateTime? publishDate = null, 
							int? newsId = null, 
							int? categoryId = null,
							int? teamId = null,
							int? gameId = null)
		{
            if (sportType != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.SportType, sportType));
            if (isGeneral != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.IsGeneral, isGeneral));
			if (name != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.Name, name));
			if (description != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.Description, description));
			if (publishDate != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.PublishDate, publishDate));
			if (newsId != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.NewsId, newsId));
			if (categoryId != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.CategoryId, categoryId));
			if (teamId != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.TeamId, teamId));
			if (gameId != null) _dbContext.Albums.Where(a => a.Id == id).ExecuteUpdate(a => a.SetProperty(i => i.GameId, gameId));

		}

        public List<SelectListItem> GetSelectItemList()
        {
            var albumsSL = _dbContext.Albums.OrderByDescending(a => a.Id).Take(Constants.DefaulSelectListAmount)
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList();

            return albumsSL;
        }

    }
}
