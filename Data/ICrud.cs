using BaseballUa.Models;

namespace BaseballUa.Data
{
	public interface ICrud<T>
	{
		void Add(T category);
		void Update(T category);
		void Delete(T category);
		T Get(int id);
		IEnumerable<T> GetAll();
	}
}
