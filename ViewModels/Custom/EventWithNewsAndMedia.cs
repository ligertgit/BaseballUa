namespace BaseballUa.ViewModels.Custom
{
    public class EventWithNewsAndMedia
    {
        public EventViewModel Event { get; set; }
        public List<NewsVM>? News { get; set; }
        public List<AlbumVM>? Albums { get; set; }
        public List<VideoVM>? Videos { get; set; }
        public List<GameViewModel>? Games { get; set; }
    }
}
