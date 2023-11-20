using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
	public class Album
	{
        public int Id { get; set; }
        public SportType SportType { get; set; }
        public bool IsGeneral { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
		public DateTime PublishDate { get; set; }
		public int? NewsId { get; set; }
        public News News { get; set; } = null!;
        public int? CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int? TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public int? GameId { get; set; }
        public Game Game { get; set; } = null!;
        [NotMapped]
        public ICollection<Photo>? Photos { get; set; }

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
