namespace BaseballUa.Models
{
    public class EventToTeams
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
