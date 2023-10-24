using BaseballUa.DTO.Custom;

namespace BaseballUa.ViewModels.Custom
{
    public class EventGamesByDayVM
    {
        public EventViewModel Event { get; set; }
        public List<DayGames> GamesByDay { get; set; }
    }
}
