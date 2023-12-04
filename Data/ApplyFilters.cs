namespace BaseballUa.Data
{
    public class ApplyFilters
    {

        public Filters Filters { get; set; } = new Filters();
        public string Controller { get; set; }
        public string RedirectAction { get; set; }
        public List<RouteItem> RouteItems { get; set; } = new List<RouteItem>();

    }
}
