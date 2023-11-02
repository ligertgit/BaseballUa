using BaseballUa.Data;
using BaseballUa.Models;

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
			return _dbContext.Videos.Where(v => v.Id == itemId).FirstOrDefault();
		}

		public IEnumerable<Video> GetAll()
		{
			return _dbContext.Videos;
		}

		public void Update(Video item)
		{
			throw new NotImplementedException();
		}
	}
}
