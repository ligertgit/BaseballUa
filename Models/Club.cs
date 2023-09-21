using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
    public class Club
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string FnameLogoSmall { get; set; }
        [StringLength(50)]
        public string FnameLogoBig { get; set; }
        public int CountryId { get; set; }

        
        public Country Country { get; set; } = null!;

        public ICollection<Team>? Teams { get; set; }
    }
}
