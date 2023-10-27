namespace BaseballUa.ViewModels.Custom
{
    public class TeamStandingVM
    {
        public TeamViewModel Team { get; set; }
        public int TotalGames { get; set; }
        public int WinGames { get; set; }
        public int LooseGames { get; set; }
        //public int Points { get; set; }
        public double PCT { get; set; }

    }

    public class GroupStandingVM
    {
        public SchemaGroupViewModel SchemaGroup { get; set; }
        public List<TeamStandingVM> TeamStandings { get; set; } = new List<TeamStandingVM>();
    }

    public class EventItemStandingVM
    {
        public EventSchemaItemViewModel EventItem { get; set; }
        public List<GroupStandingVM> GroupStandings { get; set; } = new List<GroupStandingVM>();
    }
}
