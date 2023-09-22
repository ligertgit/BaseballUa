using BaseballUa.Models;
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


        public Event? Event { get; set; }
        public Tournament? Tournament { get; set; }

        [NotMapped]
        public List<SchemaGroupViewModel>? Groups { get; set; }
    }
}
