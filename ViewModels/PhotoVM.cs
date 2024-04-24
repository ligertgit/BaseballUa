using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseballUa.ViewModels
{
	public class PhotoVM
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
		[StringLength(50)]
		[Required]
		[DisplayName("Ім'я файлу оригіналу")]
		public string FnameOrig { get; set; }
		[StringLength(50)]
		[DisplayName("Ім'я файлу зменшеного")]
		public string? FnameSmall { get; set; }
		[StringLength(50)]
		[DisplayName("Ім'я файлу збільшеного")]
		public string? FnameBig { get; set; }
		[Required]
		[DisplayName("Id альбому")]
		public int AlbumId { get; set; }
		[DisplayName("Альбом")]
		public AlbumVM? Album { get; set; }
		//[DisplayName("Новини")]
		//public List<NewsVM>? News { get; set; }
	}
}
