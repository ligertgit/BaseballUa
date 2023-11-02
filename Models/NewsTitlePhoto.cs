using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
	public class NewsTitlePhoto
	{
        public int Id { get; set; }
		[StringLength(50)]
		public string Name { get; set; }
		[StringLength(200)]
		public string? Description { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; } = null!;
        public int PhotoId { get; set; }
        public Photo Photo { get; set; } = null!;


		//+fill as collection in all classes that should contain news.
		//+Check cruds for that classes.
		//create basic crud and toVIew for this class

		//Check toView for that classes
		//make admin pages
		//add to main menu
		//add to pages in controller
		//add to views
	}
}
