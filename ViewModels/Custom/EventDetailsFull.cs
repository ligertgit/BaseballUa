using BaseballUa.BlData;

namespace BaseballUa.ViewModels.Custom
{
    public class EventDetailsFull
    {
        public EventViewModel Event { get; set; }
        public List<NewsVM>? News { get; set; }
        public List<AlbumVM>? Albums { get; set; }
        public List<VideoVM>? Videos { get; set; }
        public List<GameViewModel>? CurrentGames { get; set; }
        public List<TeamViewModel>? Teams { get; set; }
    }
}
