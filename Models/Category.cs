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

		//[NotMapped]
		public ICollection<Tournament>? Tournaments { get; set; }
        //[NotMapped]
        public ICollection<News>? News { get; set; }
        //[NotMapped]
        public ICollection<Album>? Albums { get; set; }
        //[NotMapped]
        public ICollection<Video>? Videos { get; set; }
    }
}
