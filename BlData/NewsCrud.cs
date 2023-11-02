using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

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
			return _dbContext.News.Include(n => n.Albums)
										.ThenInclude(a => a.Photos)
								  .Include(n => n.Videos)
								  .Include(n => n.NewsTitlePhotos)
										.ThenInclude(ntp => ntp.Photo);
		}

		public void Update(News item)
		{
			throw new NotImplementedException();
		}
	}
}
