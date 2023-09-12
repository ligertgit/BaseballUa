using BaseballUa.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseballUa.ViewModels
{
    public class CountryViewModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Країна")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Назва коротко")]
        [StringLength(3)]
        public string ShortName { get; set; }

        [DisplayName("Файл для маленького флагу")]
        [StringLength(50)]
        public string FnameFlagSmall { get; set; }

        [DisplayName("Файл для великого флагу")]
        [StringLength(50)]
        public string FnameFlagBig { get; set; }

        [NotMapped]
        public IEnumerable<Club>? Clubs { get; set; } = new List<Club>();
    }
}
