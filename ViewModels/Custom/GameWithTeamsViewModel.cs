namespace BaseballUa.ViewModels.Custom
{
    public class GameWithTeamsViewModel
    {
        public GameViewModel Game { get; set; }
        public TeamViewModel? HomeTeam { get; set; }
        public TeamViewModel? VisitorTeam { get; set; }
    }
}
