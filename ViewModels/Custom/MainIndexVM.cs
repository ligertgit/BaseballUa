namespace BaseballUa.ViewModels.Custom
{
    public class MainIndexVM
    {
        public List<NewsVM> News { get; set; } = new List<NewsVM>();
        public List<EventViewModel> ActiveEvents { get; set; } = new List<EventViewModel>();
        public List<AlbumVM> LastAlbums { get; set; } = new List<AlbumVM>();
        public List<VideoVM> LastVideos { get; set; } = new List<VideoVM>();
    }
}
