﻿namespace BaseballUa.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;
    }
}