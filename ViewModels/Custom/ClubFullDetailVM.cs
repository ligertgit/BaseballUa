﻿namespace BaseballUa.ViewModels.Custom
{
    public class ClubFullDetailVM
    {
        public ClubViewModel Club { get; set; }
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
        public List<VideoVM> Videos { get; set; } = new List<VideoVM>();
        public List<NewsVM> News { get; set; } = new List<NewsVM>();
        public List<AlbumVM> Albums { get; set; } = new List<AlbumVM>();
    }
}
