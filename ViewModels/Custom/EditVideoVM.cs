using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.ViewModels.Custom
{
    public class EditVideoVM
    {
        public VideoVM Video { get; set; } = new VideoVM();
        public SelectList SportTypeSL { get; set; } = Enums.SportType.NotDefined.ToSelectList();
        public SelectList? CategorySL { get; set; }
        public SelectList? TeamSL { get; set; }
        public SelectList? GameSl { get; set; }
        public SelectList? NewsSL { get; set; }

    }
}
