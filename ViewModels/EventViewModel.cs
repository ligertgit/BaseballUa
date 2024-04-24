using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DisplayName("Пов'язаний вид змагань")]
        public TournamentViewModel? Tournament { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? SelectTournament { get; set; }

        [NotMapped]
        public List<EventSchemaItemViewModel>? SchemaItems { get; set; }

        public List<NewsVM>? News { get; set;}

        public List<int>? EventTeamsIds { get; set; }
        [DisplayName("Список команд")]
        public SelectList? EventTeamsSL { get; set; }
    }


}
