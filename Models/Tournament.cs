using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class Tournament
    {


        public int Id { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }
        public SportType Sport { get; set; }
        
        [StringLength(250)]
        public string? Description { get; set; }
        public bool IsAnual { get; set; }
        public bool IsInternational { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsFun { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        //[NotMapped]
        public ICollection<Event>? Events { get; set; }
    }

}
