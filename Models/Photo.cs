using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
	public class Photo
	{
        public int Id { get; set; }
		[StringLength(50)]
		public string Name { get; set; }
		[StringLength(200)]
		public string? Description { get; set; }
		[StringLength(50)]
		public string FnameOrig { get; set; }
		[StringLength(50)]
		public string FnameSmall { get; set; }
		[StringLength(50)]
		public string FnameBig { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;
        public List<NewsTitlePhoto>? NewsTitlePhotos { get; set; }

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
