using BaseballUa.Models;

namespace BaseballUa.Data
{
	public interface ICrud<T>
	{
		void Add(T item);
		void Update(T item);
		void Delete(T item);
		T Get(int itemId);
		IEnumerable<T> GetAll();
	}
}
