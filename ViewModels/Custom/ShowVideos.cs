using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.ViewModels.Custom
{
    public class ShowVideos
    {
        public List<VideoVM> Videos { get; set; } = new List<VideoVM>();

        public ShowVideosSelections Selections { get; set; } = new ShowVideosSelections();
    }
}
