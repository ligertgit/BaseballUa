using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class EventSchemaItem
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public GameType SchemaItem { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public ICollection<Game>? Games { get; set; } = new List<Game>();
    }
}
