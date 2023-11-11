using BaseballUa.Data;
using BaseballUa.Models;
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

        public IEnumerable<Album> GetAll(SportType? sportType,
                                        bool? isGeneral,
                                        int? newsId,
                                        int? categoryId,
                                        int? teamId,
                                        int? gameId,
                                        DateTime? lastDate = null,
                                        int? lastId = null,
                                        int? amount = null)
		{
			return _dbContext.Albums.Where(a => (sportType == null || a.SportType == sportType)
											&& (isGeneral == null || a.IsGeneral == isGeneral)
											&& (categoryId == null || a.CategoryId == categoryId)
											&& (teamId == null || a.TeamId == teamId)
											&& (gameId == null || a.GameId == gameId)
											&& (lastDate == null || (lastId == null ? a.PublishDate < lastDate : a.PublishDate <= lastDate && a.Id < lastId))
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
	}
}
