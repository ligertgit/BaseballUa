using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseballUa.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        [DisplayName("Назва категорії")]
        public string Name { get; set; }

        [StringLength(15)]
        [Required]
        [DisplayName("Скорочена назва")]
        public string ShortName { get; set; }

        [NotMapped]
        public List<TournamentViewModel>? Tournaments { get; set;}

        [NotMapped]
        public IEnumerable<SelectListItem>? SelectTournaments { get; set; }

    }
}
