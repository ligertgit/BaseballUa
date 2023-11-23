using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseballUa.Models
{
    public class SchemaGroup
    {
        public int Id { get; set; }
        [StringLength(3)]
        public string GroupName { get; set; }
        public int EventSchemaItemId { get; set; }
        public EventSchemaItem EventSchemaItem { get; set; } = null!;

        //[NotMapped]
        public ICollection<Game>? Games { get; set; }
    }
}
