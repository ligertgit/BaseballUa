using BaseballUa.DTO.Custom;

namespace BaseballUa.ViewModels.Custom
{
    public class EventGamesByDayVM
    {
        public EventViewModel Event { get; set; }
        public List<NewsVM>? News { get; set; }
        public List<AlbumVM>? Albums { get; set; }
        public List<VideoVM>? Videos { get; set; }
        public List<GameViewModel>? CurrentGames { get; set; }
        public List<DayGames> GamesByDay { get; set; }
        public int ShowIndex { get; set; } = 0;
    }
}
