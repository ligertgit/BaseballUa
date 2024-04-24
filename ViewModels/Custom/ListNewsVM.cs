using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using static BaseballUa.Data.Enums;

namespace BaseballUa.ViewModels.Custom
{
    public class ListNewsVM
    {
        public List<NewsVM> News { get; set; } = new List<NewsVM> { };
        public SportType SportType { get; set; } = SportType.NotDefined;
        public SelectList SportTypeSL { get; set; } = Enums.SportType.NotDefined.ToSelectList();
        public bool isGeneral { get; set; } = false;
        public bool isFun { get; set; } = false;
        public bool isOfficial { get; set; } = false;
        public bool isAnnual { get; set; } = false;
        public EventViewModel? Event { get; set; } = null;
        public SelectList? EventSL { get; set; }
        public CategoryViewModel? Category { get; set; } = null;
        public SelectList? CategorySL { get; set; }
        public TeamViewModel? Team { get; set; } = null;
        public SelectList? TeamSL { get; set; }
        public DateTime? LastDate { get; set; } = null;
        public int SkipPrev { get; set; } = -1;
        public int SkipNext { get; set; } = -1;

    }
}
