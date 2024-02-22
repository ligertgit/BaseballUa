using Microsoft.AspNetCore.Mvc.Rendering;


// delete
namespace BaseballUa.ViewModels.Custom
{
    public class AddPhotosToNews
    {
        public NewsVM News { get; set; }
        public List<int>? PhotoIds { get; set; }
        //public SelectList MyProperty1 { get; set; }
        public MultiSelectList? PhotosMSL { get; set; }
    }
}
