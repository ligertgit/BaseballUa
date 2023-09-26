namespace BaseballUa.ViewModels.Custom
{
    public class EventGamesByDayVM
    {
        public EventViewModel Event { get; set; }
        public List<(DateTime, List<GameViewModel>)> GamesByDay { get; set; }
    }
}
