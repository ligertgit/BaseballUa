using System.ComponentModel.DataAnnotations;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        [StringLength(250)]
        public string? AdditionalInfo { get; set; }
        public int? RunsVisitor { get; set; }
        public int? RunsHome { get; set; }
        [StringLength(50)]
        public string? PlacedAt { get; set; }
        public int? HalfinningsPlayed { get; set; }
        public GameStatus GameStatus { get; set; }
        public int? PointsVisitor { get; set; }
        public int? PointsHome { get; set;}
        //public GameType GameType { get; set; }
        public TourNumber? Tour { get; set; }
        [StringLength(50)]
        public string? ConditionVisitor { get; set; }
        [StringLength(50)]
        public string? ConditionHome { get; set; }
        
        public int SchemaGroupId { get; set; }
        
        public SchemaGroup SchemaGroup { get; set; } = null!;

    }
}
