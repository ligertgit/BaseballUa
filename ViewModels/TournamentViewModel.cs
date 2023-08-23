using BaseballUa.Models;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.ViewModels
{
    public class TournamentViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        [DisplayName("Назва змагань")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Спорт")]
        public SportType Sport { get; set; }

        [StringLength(250)]
        [DisplayName("Опис")]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Регулярний")]
        public bool IsAnual { get; set; }
        [Required]
        [DisplayName("Міжнародний")]
        public bool IsInternational { get; set; }
        [Required]
        [DisplayName("Офіційний")]
        public bool IsOfficial { get; set; }

        [Required]
        [DisplayName("Вікова категорія")]
        public string SelectedCategory { get; set; }
        //public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
