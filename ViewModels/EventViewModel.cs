using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BaseballUa.Data.Within2000To2050Attribute;

namespace BaseballUa.ViewModels
{
    public class EventViewModel
    {
        [Key]
        public int EventViewModelId { get; set; }

        [Required]
        [Range(2000, 2050)]
        [DisplayName("Рік")]
        public int Year { get; set; }
        
        [Within2000To2050]
        [DisplayName("Початок")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Закінчення")]
        [Within2000To2050]
        public DateTime? EndDate { get; set;}
        [Required]
        public int TournamentId { get; set; }

        [NotMapped]
        [DisplayName("Пов'язаний тип змагань")]
        public TournamentViewModel? Tournament { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? TournamentList { get; set; }
    }


}
