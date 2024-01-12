using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.ViewModels.Custom
{
    public class EditGameVM
    {
        public GameViewModel Game { get; set; }
        public SelectList HomeTeamSL { get; set; }
        public SelectList VisitorTeamSL { get; set; }
        public SelectList StatusSL { get; set; }
        public SelectList TourSL { get; set; }
    }
}
