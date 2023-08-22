using BaseballUa.Models;

namespace BaseballUa.Data
{
	public interface ICrud<T>
	{
		T Add(T category);
		T Update(T category);
		T Delete(T category);
		T Get(T category);
		IEnumerable<T> GetAll();
	}
}
