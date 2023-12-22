using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using static BaseballUa.Data.Enums;

namespace BaseballUa.ViewModels.Custom
{
    public class HomeShowListVM
    {
        public List<NewsVM> News { get; set; } = new List<NewsVM>();
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
        public List<AlbumVM> Albums { get; set; } = new List<AlbumVM>();
        public List<VideoVM> Videos { get; set; } = new List<VideoVM>();

        //public ApplyFilters ApplyFilters { get; set; } = new ApplyFilters();
        public Filters Filters { get; set; } = new Filters();

        public int TeamId { get; set; } = 0;
        public SelectList? TeamSL { get; set; }
        public int ClubId { get; set; } = 0;
        public SelectList? ClubSL { get; set; }
        public int EventId { get; set; } = 0;
        public SelectList? EventSL { get; set; }

        public ListToShow listToShow { get; set; } = ListToShow.News;

        public int skipPrev { get; set; } = -1;
        public int skipNext { get; set; } = -1;

        
    }
}
