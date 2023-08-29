using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
    public class EventIndexModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public SportType Sport { get; set; }
        public string? Description { get; set; }
        public bool IsAnual { get; set; }
        public bool IsInternational { get; set; }
        public bool IsOfficial { get; set; }
        public int CategoryId { get; set; }
        public string CategoryShortName { get; set; }
    }
}
