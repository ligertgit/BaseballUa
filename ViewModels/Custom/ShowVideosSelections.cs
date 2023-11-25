using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BaseballUa.ViewModels.Custom
{
	public class ShowVideosSelections
	{
		[DisplayName("Відео команди")]
		public SelectList? TeamSL { get; set; }
		public SelectList? CategorySL { get; set; }
		public SelectList? EvenSL { get; set; }
		public SelectList? SportTypeSL { get; set; }

        public int TeamId { get; set; }
        public int CategoryId { get; set; }
        public int EventId { get; set; }
        public int SportType { get; set; }
    }
}
