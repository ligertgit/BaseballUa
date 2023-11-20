using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using BaseballUa.ViewModels.Custom;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Drawing;


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
        public IActionResult ShowNews(int newsId)
        {
            var newsVL = new NewsVM();
            var newsDAL = new News();
            if (newsId > 0)
            {
				newsDAL = new NewsCrud(_db).Get(newsId);
			}
			if (newsDAL != null) 
			{ 
				newsVL = new NewsToView().Convert(newsDAL);
				newsVL.Albums = new AlbumToView().ConvertAll(newsDAL.Albums.ToList());
			}

			return View(newsVL);
		}

		public IActionResult ShowAlbum(int Id)
		{
			Album? albumDAL = null;
			AlbumVM albumVL = new AlbumVM();
			if (Id > 0)
			{
				albumDAL = new AlbumsCrud(_db).Get(Id);
			}
			if (albumDAL != null)
			{
				albumVL = new AlbumToView().Convert(albumDAL);
				if (albumDAL.Game != null && albumDAL.Game.HomeTeamId != null && albumDAL.Game.VisitorTeamId != null) 
				{
					var homeTeamDAL = new TeamCrud(_db).Get((int)albumDAL.Game.HomeTeamId);
					var visitorTeamDAL = new TeamCrud(_db).Get((int)albumDAL.Game.VisitorTeamId);
					albumVL.Game.HomeTeam = new TeamToView().Convert(homeTeamDAL, false);
					albumVL.Game.VisitorTeam = new TeamToView().Convert(visitorTeamDAL, false);
				}
			}

			return View(albumVL);
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