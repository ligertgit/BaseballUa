namespace BaseballUa.ViewModels.Custom
{
    public class GameInfo
    {
        
        public EventViewModel Event { get; set; }
        public List<AlbumVM>? Albums { get; set; }
        public List<VideoVM>? Videos { get; set; }
        public List<NewsVM>? News { get; set; }
        public List<GameViewModel>? CurrentGames { get; set; }
        public GameViewModel Game { get; set; } = new GameViewModel();
    }
}
