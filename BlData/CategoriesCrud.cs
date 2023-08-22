using BaseballUa.Data;
using BaseballUa.Models;

namespace BaseballUa.BlData
{
	public class CategoriesCrud : ICrud<Category>
	{
		private readonly BaseballUaDbContext _dbcontext;

        public CategoriesCrud(BaseballUaDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void Add(Category category)
		{
			_dbcontext.Categories.Add(category);
			_dbcontext.SaveChanges();
		}

		public void Delete(Category category)
		{
			throw new NotImplementedException();
		}

		public Category Get(int id)
		{
			var category = _dbcontext.Categories.First(c => c.Id == id);
			return category;
		}

		public IEnumerable<Category> GetAll()
		{
			return _dbcontext.Categories;
		}

		public void Update(Category category)
		{
			_dbcontext.Categories.Update(category);
			_dbcontext.SaveChanges();
		}
	}
}
