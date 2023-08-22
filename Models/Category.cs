using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
	public class Category
	{
		public int Id { get; set; }

		[StringLength(50)]
		public string Name { get; set; }
	
		[StringLength(15)]
		public string ShortName { get; set; }

		public ICollection<Tournament> Tournaments { get; } = new List<Tournament>();

    }
}
