using BaseballUa.Data;
using BaseballUa.Models;

namespace BaseballUa.BlData
{
	public class CategoriesCrud : ICrud
	{
		private readonly BaseballUaDbContext _dbcontext;

        public CategoriesCrud(BaseballUaDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Category Add(Category category)
		{
			throw new NotImplementedException();
		}

		public Category Delete(Category category)
		{
			throw new NotImplementedException();
		}

		public Category Get(Category category)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Category> GetAll()
		{
			return _dbcontext.Categories;
		}

		public Category Update(Category category)
		{
			throw new NotImplementedException();
		}
	}
}
