using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using BaseballUa.ViewModels.Custom;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BaseballUa.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly BaseballUaDbContext _db;
		 

		public HomeController(ILogger<HomeController> logger, BaseballUaDbContext dbcontext)
		{
			_logger = logger;
			_db = dbcontext;
		}
#region news
		public IActionResult Index()
		{
			var pageDataVM = new MainIndexVM();
			var newsDAL = new NewsCrud(_db).GetAll(notForTeamOnly: true, lastDate: DateTime.Now).ToList();
			pageDataVM.News = new NewsToView().ConvertAll(newsDAL);
			var albumsDAL = new AlbumsCrud(_db).GetAll(notForTeamOnly: true, lastDate: DateTime.Now).ToList();
			pageDataVM.LastAlbums = new AlbumToView().ConvertAll(albumsDAL);
			var eventsDAL = new EventsCrud(_db).GetAll( firstDate: DateTime.Now.AddDays( Constants.DefaulActiveEventDaysShift ),
													    lastDate: DateTime.Now.AddDays( -Constants.DefaulActiveEventDaysShift ))
														.ToList();
			pageDataVM.ActiveEvents = new EventToView().ConvertAll(eventsDAL);

			return View(pageDataVM);
		}
#endregion


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult History()
		{
			return View();
		}
	}
}