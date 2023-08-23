using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
	public class CategoriesCrud : ICrud<Category>
	{
		private readonly BaseballUaDbContext _dbContext;

        public CategoriesCrud(BaseballUaDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public void Add(Category item)
		{
            _dbContext.Categories.Add(item);
            _dbContext.SaveChanges();
		}

		public void Delete(Category item)
		{
			throw new NotImplementedException();
		}

		public Category Get(int itemId)
		{
			var category = _dbContext.Categories.First(c => c.Id == itemId);
			return category;
		}

		public IEnumerable<Category> GetAll()
		{
			return _dbContext.Categories;
		}

		public void Update(Category item)
		{
			_dbContext.Categories.Update(item);
			_dbContext.SaveChanges();
		}
	}
}
