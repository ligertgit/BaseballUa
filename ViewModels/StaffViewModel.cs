using BaseballUa.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BaseballUa.Data.Enums;

namespace BaseballUa.ViewModels
{
    public class StaffViewModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        [DisplayName("Ім'я")]
        [Required]
        public string FirsName { get; set; }
        [StringLength(30)]
        [DisplayName("Фамілія")]
        public string? SecondName { get; set; }
        [DisplayName("Роль")]
        [Required]
        public ClubRole Role { get; set; }
        [StringLength(100)]
        [DisplayName("Задачі")]
        public string? RoleDescription { get; set; }
        [StringLength(50)]
        [DisplayName("Аватар малий")]
        public string? AvatarSmall { get; set; }
        [StringLength(50)]
        [DisplayName("Аватар великий")]
        public string? AvatarLarge { get; set; }
        public int ClubId { get; set; }
        [NotMapped]
        public ClubViewModel? Club { get; set; }
    }
}
