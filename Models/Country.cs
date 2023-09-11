using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
    public class Country
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(3)]
        public string ShortName { get; set; }
        [StringLength(50)]
        public string FnameFlagSmall { get; set; }
        [StringLength(50)]
        public string FnameFlagBig { get; set; }

        public ICollection<Club> Clubs { get; } = new List<Club>();
    }
}
