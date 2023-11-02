using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

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
										.Include(a => a.Photos)
									.FirstOrDefault();
		}

		public IEnumerable<Album> GetAll()
		{
			return _dbContext.Albums.Include(a => a.Photos);
		}

		public void Update(Album item)
		{
			throw new NotImplementedException();
		}
	}
}
