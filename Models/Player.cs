using System.ComponentModel.DataAnnotations;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string? SecondName { get; set; }
        [StringLength(50)]
        public string? AvatarSmall { get; set; }
        [StringLength(50)]
        public string? AvatarBig { get; set; }
        public Sex Sex { get; set; } = Sex.NotDefined;
        public DateTime? Birthdate { get; set; }
        public int TeamId { get; set; }

        public Team Team { get; set; } = null!;
    }
}
