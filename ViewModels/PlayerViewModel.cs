using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseballUa.ViewModels
{
    public class PlayerViewModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        [Required]
        [DisplayName("Ім'я")]
        public string FirstName { get; set; }
        [StringLength(30)]
        [DisplayName("Фамілія")]
        public string? SecondName { get; set; }
        [StringLength(50)]
        [DisplayName("Аватар маленький")]
        public string? AvatarSmall { get; set; }
        [StringLength(50)]
        [DisplayName("Аватар великий")]
        public string? AvatarBig { get; set; }
        [Required]
        [DisplayName("Стать")]
        public Sex Sex { get; set; } = Sex.NotDefined;
        [DisplayName("Дата народження")]
        public DateTime? Birthdate { get; set; }
        [Required]
        [DisplayName("id команди")]
        public int TeamId { get; set; }
        [DisplayName("Команда")]
        public TeamViewModel? Team { get; set; }
    }
}
