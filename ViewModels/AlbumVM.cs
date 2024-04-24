using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseballUa.ViewModels
{
	public class AlbumVM
	{
		[Key]
		[DisplayName("Id")]
		public int Id { get; set; }
		[Required]
		[DisplayName("Спорт")]
		public SportType SportType { get; set; }
		[Required]
		[DisplayName("Глобальний")]
		public bool IsGeneral { get; set; }
		[StringLength(50)]
		[Required]
		[DisplayName("Назва")]
		public string Name { get; set; }
		[StringLength(200)]
		[DisplayName("Опис")]
		public string? Description { get; set; }
		[Required]
		[DisplayName("Дата публікації")]
		public DateTime PublishDate { get; set; }
		[DisplayName("Id новини")]
		public int? NewsId { get; set; }
		[DisplayName("Новина")]
		public NewsVM? News { get; set; }
		[DisplayName("Id катеогрії")]
		public int? CategoryId { get; set; }
		[DisplayName("Категорія")]
		public CategoryViewModel? Category { get; set; }
		[DisplayName("Id команди")]
		public int? TeamId { get; set; }
		[DisplayName("Команда")]
		public TeamViewModel? Team { get; set; }
		[DisplayName("Id гри")]
		public int? GameId { get; set; }
		[DisplayName("Гра")]
		public GameViewModel? Game { get; set; }

		[DisplayName("Фотографії")]
		public List<PhotoVM>? Photos { get; set; }
	}
}
