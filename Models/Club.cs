﻿using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Models
{
    public class Club
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(2500)]
        public string? Description { get; set; }
        [StringLength(150)]
        public string? Invitation { get; set; }
        [StringLength(50)]
        public string FnameLogoSmall { get; set; }
        [StringLength(50)]
        public string FnameLogoBig { get; set; }
        public int CountryId { get; set; }

        
        public Country Country { get; set; } = null!;
        //[NotMapped]
        public IEnumerable<Team>? Teams { get; set; }
        //[NotMapped]
        public ICollection<Staff>? Staffs { get; set; }
    }
}
