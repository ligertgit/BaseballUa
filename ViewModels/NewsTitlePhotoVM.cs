using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseballUa.ViewModels
{
	public class NewsTitlePhotoVM
	{
		[Key]
		[DisplayName("Id")]
		public int Id { get; set; }
		[StringLength(50)]
		[Required]
		[DisplayName("Назва")]
		public string Name { get; set; }
		[StringLength(200)]
		[DisplayName("Опис")]
		public string? Description { get; set; }
		[Required]
		[DisplayName("Id новини")]
		public int NewsId { get; set; }
		//[DisplayName("Новина")]
		//public NewsVM? News { get; set; }
		[Required]
		[DisplayName("Id фото")]
		public int PhotoId { get; set; }
		//[DisplayName("Фото")]
		//public PhotoVM? Photo { get; set; }
	}
}
