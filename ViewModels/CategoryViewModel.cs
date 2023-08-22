using BaseballUa.Models;
using System.ComponentModel.DataAnnotations;

namespace BaseballUa.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(15)]
        public string ShortName { get; set; }

    }
}
