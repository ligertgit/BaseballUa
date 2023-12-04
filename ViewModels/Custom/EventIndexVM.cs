using BaseballUa.Data;

namespace BaseballUa.ViewModels.Custom
{
    public class EventIndexVM
    {
        public List<EventForIndexVM> Events { get; set; } = new List<EventForIndexVM>();
        public ApplyFilters ApplyFilters { get; set; }
        public int MonthShift { get; set; }
    }
}
