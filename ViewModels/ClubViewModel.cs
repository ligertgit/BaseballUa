using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseballUa.ViewModels
{
    public class ClubViewModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Назва клубу")]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }


        [DisplayName("Опис")]
        [StringLength(100)]
        public string? Description { get; set; }
        [DisplayName("Як записатися")]
        [StringLength(100)]
        public string? Invitation { get; set; }

        [DisplayName("Маленьке лого")]
        [StringLength(50)]
        public string FnameLogoSmall { get; set; }
        
        [DisplayName("Велике лого")]        
        [StringLength(50)]
        public string FnameLogoBig { get; set; }


        public int CountryId { get; set; }

        [DisplayName("Країна")]
        public Country? Country { get; set; }
        //public IEnumerable<SelectListItem>? CountriesList { get; set; }

        [NotMapped]
        public List<TeamViewModel>? Teams { get; set; }
        [NotMapped]
        public List<StaffViewModel>? Staffs { get; set; }
    }
}
