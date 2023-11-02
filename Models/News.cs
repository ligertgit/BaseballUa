using System.ComponentModel.DataAnnotations;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
	public class News
	{
        public int Id { get; set; }
        public SportType SportType { get; set; }
        public bool IsGeneral { get; set; }
        public DateTime PublishDate { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
		public int? EventId { get; set; }
        public Event Event { get; set; } = null!;
        public int? CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int? TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public ICollection<Album>? Albums { get; set; }
        public ICollection<NewsTitlePhoto>? NewsTitlePhotos { get; set; }
        public ICollection<Video>? Videos { get; set; }


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
