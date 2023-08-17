using BaseballUa.Models;

namespace BaseballUa.Data
{
	public interface ICrud
	{
		Category Add(Category category);
		Category Update(Category category);
		Category Delete(Category category);
		Category Get(Category category);
		IEnumerable<Category> GetAll();
	}
}
