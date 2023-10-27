using System.ComponentModel.DataAnnotations;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class Staff
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string FirsName { get; set; }
        [StringLength(30)]
        public string? SecondName { get; set; }
        public ClubRole Role { get; set; }
        [StringLength(100)]
        public string? RoleDescription { get; set; }
        [StringLength(50)]
        public string? AvatarSmall { get; set; }
        [StringLength(50)]
        public string? AvatarLarge { get; set;}
        public int ClubId { get; set; }
        public Club Club { get; set; } = null!;
    }
}
