using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BaseballUa.Data.Enums;

namespace BaseballUa.ViewModels
{
    public class EventSchemaItemViewModel
    {
        [Key]
        public int EventSchemaItemViewModelId { get; set; }
        
        [DisplayName("Порядковий номер блоку схеми турніру")]
        [Range(1,100)]
        public int Order { get; set; }
        
        [DisplayName("Тип блоку схеми турніру")]
        public GameType SchemaItem { get; set; }
        
        public int EventId { get; set; }

        [NotMapped]
        public EventViewModel? Event { get; set; }

        [NotMapped]
        public List<SelectListItem>? selectEvent { get; set; }

        // its not coorect to have it here. Probably custom view model OR get it from Event
        //public TournamentViewModel? Tournament { get; set; }

        [NotMapped]
        public List<SchemaGroupViewModel>? Groups { get; set; }
    }
}
