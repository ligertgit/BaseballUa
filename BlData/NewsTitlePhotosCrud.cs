using BaseballUa.Data;
using BaseballUa.Models;

namespace BaseballUa.BlData
{
	public class NewsTitlePhotosCrud : ICrud<NewsTitlePhoto>
	{
		private readonly BaseballUaDbContext _dbContext;

        public NewsTitlePhotosCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(NewsTitlePhoto item)
		{
			_dbContext.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(NewsTitlePhoto item)
		{
			throw new NotImplementedException();
		}

		public NewsTitlePhoto Get(int itemId)
		{
			return _dbContext.NewsTitlePhotos.Where(ntp => ntp.Id == itemId).FirstOrDefault();
		}

		public IEnumerable<NewsTitlePhoto> GetAll()
		{
			return _dbContext.NewsTitlePhotos;
		}

		public void Update(NewsTitlePhoto item)
		{
			throw new NotImplementedException();
		}
	}
}
