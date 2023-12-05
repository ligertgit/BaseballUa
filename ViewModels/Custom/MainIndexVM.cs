using BaseballUa.Data;

namespace BaseballUa.ViewModels.Custom
{
    public class MainIndexVM
    {
        public List<NewsVM> News { get; set; } = new List<NewsVM>();
        public int skipNewsPrev { get; set; } = -1;
        public int skipNewsNext { get; set; } = -1;
        public List<EventViewModel> ActiveEvents { get; set; } = new List<EventViewModel>();
        public List<AlbumVM> LastAlbums { get; set; } = new List<AlbumVM>();
        public List<VideoVM> LastVideos { get; set; } = new List<VideoVM>();
        public ApplyFilters ApplyFilters { get; set; } = new ApplyFilters();
    }
}
