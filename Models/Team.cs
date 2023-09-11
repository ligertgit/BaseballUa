using BaseballUa.Migrations;
using System.ComponentModel.DataAnnotations;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class Team
    {

        public int Id { get; set; }
        public SportType SportType { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string FnameLogoSmall { get; set; }
        [StringLength(50)]
        public string FnameLogoBig { get; set; }
        public bool IsTemp { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; } = null!;

        public ICollection<Game> HomeGames { get; } = new List<Game>();
        public ICollection<Game> VisitorGames { get; } = new List<Game>();


    }
}
