using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
	public class Category
	{
		public int Id { get; set; }

		[StringLength(50)]
		public string Name { get; set; }
	
		[StringLength(50)]
		public string ShortName { get; set; }

	}
}
