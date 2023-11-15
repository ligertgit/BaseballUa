using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.BlData
{
	public class PhotosCrud : ICrud<Photo>
	{
		private readonly BaseballUaDbContext _dbContext;

        public PhotosCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(Photo item)
		{
			_dbContext.Photos.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(Photo item)
		{
			throw new NotImplementedException();
		}

		public Photo Get(int itemId)
		{
			return _dbContext.Photos.Where(p => p.Id == itemId).FirstOrDefault();
		}

		public IEnumerable<Photo> GetAll()
		{
			return _dbContext.Photos;
		}

        public IEnumerable<Photo> GetAll(int albumId)
        {
            return _dbContext.Photos.Where(p => p.AlbumId == albumId);
        }

        public void Update(Photo item)
		{
			throw new NotImplementedException();
		}

        public List<SelectListItem> GetSelectItemList()
        {
            var photosSL = _dbContext.Photos.OrderByDescending(a => a.Id).Take(Constants.DefaulSelectListAmount)
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList();

            return photosSL;
        }
    }
}
