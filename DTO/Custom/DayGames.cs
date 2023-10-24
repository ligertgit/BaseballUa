using BaseballUa.ViewModels;

namespace BaseballUa.DTO.Custom
{
    public class DayGames
    {
        public DateTime? GamesDate { get; set; }
        public List<GameViewModel> Games { get; set; }
    }
}
