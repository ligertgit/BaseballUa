using BaseballUa.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    }
}
