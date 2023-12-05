﻿using Azure.Core;
using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using BaseballUa.ViewModels.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using static BaseballUa.Data.Enums;

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
		public IActionResult Index(int skipNews = 0)
		{
            if(skipNews < 0) skipNews = 0;

			var pageDataVM = new MainIndexVM();

            Filters filters = Request.Cookies.GetFilters();
			pageDataVM.ApplyFilters = new ApplyFilters { Filters = filters, Controller = "Home", RedirectAction = "Index" };
			var selectedCategories = new CategoriesCrud(_db).GetIds(filters.GetSelectedCategories());


			int queryCount;
			int amount = Constants.DefaulNewsAmount;
            //var newsDAL = new NewsCrud(_db).GetAll(notForTeamOnly: true, lastDate: DateTime.Now).ToList();
            var newsDAL = new NewsCrud(_db).GetAllFiltered(out queryCount,
														   sportType: filters.GetSelectedSport(),
                                                           includeAllFun: filters.Fun,
														   includeAllGeneral: filters.General,
														   isOfficial: filters.Official,
														   isInternational: filters.International,
														   isAnnual: filters.Annual,
                                                           categoryIds: selectedCategories, 
														   newestDate: DateTime.Now,
														   skip: skipNews,
                                                           amount: amount
                                                            ).ToList();
			if (queryCount > skipNews + amount)
			{
				pageDataVM.skipNewsNext = skipNews + amount;
			}
			if (skipNews > 0)
			{
				pageDataVM.skipNewsPrev = skipNews - amount;
			}
			pageDataVM.News = new NewsToView().ConvertAll(newsDAL);


			var albumsDAL = new AlbumsCrud(_db).GetAllFiltered(sportType: filters.GetSelectedSport(),
                                                           includeAllFun: filters.Fun,
                                                           includeAllGeneral: filters.General,
                                                           isOfficial: filters.Official,
                                                           isInternational: filters.International,
                                                           isAnnual: filters.Annual,
                                                           categoryIds: selectedCategories,
                                                           newestDate: DateTime.Now).ToList();
            pageDataVM.LastAlbums = new AlbumToView().ConvertAll(albumsDAL);

			var eventsDAL = new EventsCrud(_db).GetAllFilteredActive(sportType: filters.GetSelectedSport(),
                                                               includeAllFun: filters.Fun,
															   isOfficial: filters.Official,
															   isInternational: filters.International,
															   isAnnual: filters.Annual,
                                                               categoryIds: selectedCategories,
                                                               forDate: DateTime.Now.AddDays(Constants.DefaulActiveEventDaysShift)).ToList();
			pageDataVM.ActiveEvents = new EventToView().ConvertAll(eventsDAL);

			var videosDAL = new VideosCrud(_db).GetAllFiltered(sportType: filters.GetSelectedSport(),
                                                               includeAllFun: filters.Fun,
                                                               includeAllGeneral: filters.General,
															   isOfficial: filters.Official,
															   isInternational: filters.International,
															   isAnnual: filters.Annual,
                                                               categoryIds: selectedCategories,
                                                               newestDate: DateTime.Now).ToList();
			pageDataVM.LastVideos = new VideoToView().ConvertAll(videosDAL);

            return View(pageDataVM);
        }

        [HttpPost]
        public IActionResult ApplyFilters(IFormCollection fc)
        {
            var filters = fc.GetFilters();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.AppendFilters(filters, option);

            return RedirectToAction(fc["redirectAction"]);
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

		public IActionResult ShowAlbums(int? eventId = null, int? categoryId = null, int? teamId = null, SportType? sportType = null)
		{
			var showAlbumsVM = new ShowAlbums();

            showAlbumsVM.Selections.CategorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text");
			showAlbumsVM.Selections.EvenSL = new SelectList(new EventsCrud(_db).GetSelectItemList(), "Value", "Text");
			showAlbumsVM.Selections.TeamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(), "Value", "Text");
			showAlbumsVM.Selections.SportTypeSL = Enums.SportType.NotDefined.ToSelectList();

            var albumsDAL = new List<Album>();

            if (eventId != null && eventId > 0)
			{
                albumsDAL = new AlbumsCrud(_db).GetAllEventAlbums(eventId, amount: Constants.DefaulListAlbumsAmount).ToList();
				showAlbumsVM.Selections.EvenSL.First(i => i.Value == eventId.ToString()).Selected = true;
            }
			else if (categoryId != null && categoryId > 0)
			{
                albumsDAL = new AlbumsCrud(_db).GetAllCategoryAlbums(categoryId, amount: Constants.DefaulListAlbumsAmount).ToList();
				showAlbumsVM.Selections.CategorySL.First(i => i.Value == categoryId.ToString()).Selected = true;
			}
			else if (teamId != null && teamId > 0)
			{
                albumsDAL = new AlbumsCrud(_db).GetAllTeamAlbums(teamId, amount: Constants.DefaulListAlbumsAmount).ToList();
				showAlbumsVM.Selections.TeamSL.First(i => i.Value == teamId.ToString()).Selected = true;
			}
			else if (sportType.HasValue)
			{
                albumsDAL = new AlbumsCrud(_db).GetAllSportTypeAlbums(sportType, amount: Constants.DefaulListAlbumsAmount).ToList();
				showAlbumsVM.Selections.SportTypeSL.First(i => i.Text == ((SportType)sportType).ToString()).Selected = true;
			}
			else
			{
                albumsDAL = new AlbumsCrud(_db).GetAll(amount: Constants.DefaulListAlbumsAmount).ToList();
            }

			showAlbumsVM.Albums = new AlbumToView().ConvertAll(albumsDAL);


			return View(showAlbumsVM);

        }

		public IActionResult ShowVideos(int? eventId = null, int? categoryId = null, int? teamId = null, SportType? sportType = null)
		{
			var showVideosVM = new ShowVideos();

			showVideosVM.Selections.CategorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text");
			showVideosVM.Selections.EvenSL = new SelectList(new EventsCrud(_db).GetSelectItemList(), "Value", "Text");
			showVideosVM.Selections.TeamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(), "Value", "Text");
			showVideosVM.Selections.SportTypeSL = Enums.SportType.NotDefined.ToSelectList();

			var videosDAL = new List<Video>();

			if (eventId != null && eventId > 0)
			{
				videosDAL = new VideosCrud(_db).GetAllEventVideos(eventId, amount: Constants.DefaulListVideosAmount).ToList();
				showVideosVM.Selections.EvenSL.First(i => i.Value == eventId.ToString()).Selected = true;
			}
			else if (categoryId != null && categoryId > 0)
			{
				videosDAL = new VideosCrud(_db).GetAllCategoryVideos(categoryId, amount: Constants.DefaulListVideosAmount).ToList();
				showVideosVM.Selections.CategorySL.First(i => i.Value == categoryId.ToString()).Selected = true;
			}
			else if (teamId != null && teamId > 0)
			{
				videosDAL = new VideosCrud(_db).GetAllTeamVideos(teamId, amount: Constants.DefaulListVideosAmount).ToList();
				showVideosVM.Selections.TeamSL.First(i => i.Value == teamId.ToString()).Selected = true;
			}
			else if (sportType.HasValue)
			{
				videosDAL = new VideosCrud(_db).GetAllSportTypeVideos(sportType, amount: Constants.DefaulListVideosAmount).ToList();
				showVideosVM.Selections.SportTypeSL.First(i => i.Text == ((SportType)sportType).ToString()).Selected = true;
			}
			else
			{
				videosDAL = new VideosCrud(_db).GetAll(amount: Constants.DefaulListVideosAmount).ToList();
			}

			showVideosVM.Videos = new VideoToView().ConvertAll(videosDAL);


			return View(showVideosVM);
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