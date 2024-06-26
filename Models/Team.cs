﻿using System.ComponentModel.DataAnnotations;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Models
{
    public class Team
    {

        public int Id { get; set; }
        public SportType SportType { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(2500)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string FnameLogoSmall { get; set; }
        [StringLength(50)]
        public string FnameLogoBig { get; set; }
        public bool IsTemp { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; } = null!;

        //[NotMapped]
        public ICollection<Game>? HomeGames { get; set; }
        //[NotMapped]
        public ICollection<Game>? VisitorGames { get; set; }
        //[NotMapped]
        public ICollection<Player>? Players { get; set; }

        //[NotMapped]
        public ICollection<News>? News { get; set; }
        //[NotMapped]
        public ICollection<Album>? Albums { get; set; }
        //[NotMapped]
        public ICollection<Video>? Videos { get; set; }
        public ICollection<EventToTeams>? EventToTeams { get; set; } = null!;
    }
}
