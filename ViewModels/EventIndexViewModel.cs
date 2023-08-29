using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static BaseballUa.Data.Enums;

namespace BaseballUa.ViewModels
{
    public class EventIndexViewModel
    {
        [Key]
        public int EventIndexViewModelId { get; set; }

        [Required]
        [Range(2000, 2050)]
        [DisplayName("Рік")]
        public int Year { get; set; }

        [Within2000To2050]
        [DisplayName("Початок")]
        public DateTime? StartDate { get; set; }

        [Within2000To2050]
        [DisplayName("Закінчення")]
        public DateTime? EndDate { get; set; }
        [Required]
        public int TournamentId { get; set; }

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
        [DisplayName("Фановий")]
        public bool IsFun { get; set; }

        [Required]
        [DisplayName("Вікова категорія ID")]
        public int CategoryId { get; set; }
        
        [DisplayName("Вікова категорія short")]
        public string? CategoryShortName { get; set; }
    }
}
