using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseballUa.ViewModels
{
    public class SchemaGroupViewModel
    {
        [Key]
        public int Id { get; set; }
        
        
        [StringLength(3)]
        [DisplayName("Назва групи")]
        public string GroupName { get; set; }
        
        
        [DisplayName("Частина схеми турніру")]
        public int EventSchemaItemId { get; set; }

        [NotMapped]
        public EventSchemaItem? EventSchemaItem { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? EventSchemaItems { get; set; } // for select
        [NotMapped]
        public List<GameViewModel>? Games { get; set; } // to show if neccesary

        [NotMapped]
        public List<TeamViewModel>? VirtualTeams { get; set; }
    }
}
