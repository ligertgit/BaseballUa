namespace BaseballUa.ViewModels.Custom
{
    public class TeamFullDetailVM
    {
        public TeamViewModel Team { get; set; } = new TeamViewModel();
        public List<GameViewModel> Games { get; set; } = new List<GameViewModel>();
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
        public List<VideoVM> Videos { get; set; } = new List<VideoVM>();
        public List<NewsVM> News { get; set; } = new List<NewsVM>();
        public List<AlbumVM> Albums { get; set; } = new List<AlbumVM>();
        public List<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
