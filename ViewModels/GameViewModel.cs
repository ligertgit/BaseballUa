using BaseballUa.Models;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.Identity.Client;

namespace BaseballUa.ViewModels
{
    public class GameViewModel
    {
        [Key]
        public int GameViewModelId { get; set; }
        
        [DisplayName("Дата та час гри")]
        public DateTime? StartDate { get; set; }
        
        [StringLength(250)]
        [DisplayName("Додаткова інформація")]
        public string? AdditionalInfo { get; set; }
        
        [DisplayName("Занесли гості")]
        [Range(0, 100)]
        public int? RunsVisitor { get; set; }
        
        [DisplayName("Занесли хозяєва")]
        [Range(0,100)]
        public int? RunsHome { get; set; }
        [StringLength(50)]
        [DisplayName("Місце проведення")]
        public string? PlacedAt { get; set; }

        [DisplayName("Зіграно полуіннінгів")]
        [Range(0, 100)]
        public int? HalfinningsPlayed { get; set; }

        [DisplayName("Статус гри")]
        public GameStatus GameStatus { get; set; }

        [DisplayName("Отримали очок гості")]
        [Range(0, 2)]
        public int? PointsVisitor { get; set; }

        [DisplayName("Отримали очок хозаєва")]
        [Range(0, 2)]
        public int? PointsHome { get; set; }
        
        [DisplayName("З якої частини турніру")]
        [Range(0, 100)]
        public GameType GameType { get; set; }

        [DisplayName("Тур")]
        public TourNumber? Tour { get; set; }
        
        [StringLength(50)]
        [DisplayName("Умова для команди-гостя")]
        public string? ConditionVisitor { get; set; }

        [StringLength(50)]
        [DisplayName("Умова для команди-володаря")]
        public string? ConditionHome { get; set; }
        
        public int EventId { get; set; }
        
        
        public Event? Event { get; set; }
        public Tournament? Tournament { get; set; }
    }
}
