using BaseballUa.Models;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseballUa.ViewModels
{
    public class TeamViewModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Вид спорту")]
        public SportType SportType { get; set; }

        [DisplayName("Назва команди")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Опис")]
        [StringLength(100)]
        public string? Description { get; set; }

        [DisplayName("Лого")]
        [StringLength(50)]
        public string FnameLogoSmall { get; set; }
        
        [DisplayName("Велике лого")]        
        [StringLength(50)]
        public string FnameLogoBig { get; set; }
        

        public bool IsTemp { get; set; }

        [DisplayName("Клуб")]
        public int ClubId { get; set; }

        [DisplayName("Клуб")]
        public Club? Club { get; set; }

        [NotMapped]
        public List<Game>? Games { get; set; }

    }
}
