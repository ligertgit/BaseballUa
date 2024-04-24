using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseballUa.ViewModels
{
	public class NewsVM
	{
		[Key]
		[DisplayName("Id новини")]
		public int Id { get; set; }
		[DisplayName("Спорт")]
		[Required]
		public SportType SportType { get; set; }
		[Required]
		[DisplayName("Глобальна")]
		public bool IsGeneral { get; set; }
		[Required]
		[DisplayName("Дата публікації")]
		public DateTime PublishDate { get; set; }
		[StringLength(50)]
		[Required]
		[DisplayName("Заголовок")]
		public string Title { get; set; }
		[StringLength(2500)]
		[Required]
		[DisplayName("Текст")]
		public string Description { get; set; }
		[DisplayName("Id турніра")]
		public int? EventId { get; set; }
		[DisplayName("Турнір")]
		public EventViewModel? Event { get; set; }
		[DisplayName("Id категрії")]
		public int? CategoryId { get; set; }
		[DisplayName("Категорія")]
		public CategoryViewModel? Category { get; set; }
		[DisplayName("Id команди")]
		public int? TeamId { get; set; }
		[DisplayName("Команда")]
		public TeamViewModel? Team { get; set; }

		[DisplayName("Альбоми")]
		public List<AlbumVM>? Albums { get; set; }
		[DisplayName("Фотки")]
		public List<PhotoVM>? Photos { get; set; }
		[DisplayName("Відео")]
		public List<VideoVM>? Videos { get; set; }
	}
}
