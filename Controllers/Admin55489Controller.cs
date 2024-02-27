﻿using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.DTO.Custom;
using BaseballUa.Migrations;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using BaseballUa.ViewModels;
using BaseballUa.ViewModels.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using System.Drawing;
using System.Linq;
using System.Net;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Controllers
{
    public class Admin55489Controller : Controller
    {
        private readonly BaseballUaDbContext _db;
        private readonly string _rootPath;

        public Admin55489Controller(BaseballUaDbContext dbcontext, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _db = dbcontext;
            _rootPath = env.WebRootPath;
        }

        public IActionResult Index()
        {
            return View();
        }
#region Category


        public IActionResult ListCategories() 
        {
            //fix fromDTO to VIEW model and fix method _db
            var allCategoriesDTO = new CategoriesCrud(_db).GetAll().ToList();
            var allCategoriesView = new CategoryToView().ConvertList(allCategoriesDTO);
            return View(allCategoriesView);
        }

        public IActionResult CreateCategory() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(CategoryViewModel categoryView)
        {
            if (ModelState.IsValid) 
            {
                Category categoryDAL = new CategoryToView().ConvertBack(categoryView);
                new CategoriesCrud(_db).Add(categoryDAL);
            }
            
            return RedirectToAction("ListCategories");
        }

        public IActionResult EditCategory(int id)
        {
            var categoryDTO = new CategoriesCrud(_db).Get(id);
            var categoryView = new CategoryToView().Convert(categoryDTO);
            return View(categoryView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(CategoryViewModel category)
        {
            if (ModelState.IsValid) 
            {
                Category categoryDAL = new CategoryToView().ConvertBack(category);
                //categoryDAL.Id = category.Id;
                //categoryDAL.Name = category.Name;
                //categoryDAL.ShortName = category.ShortName;
                new CategoriesCrud(_db).Update(categoryDAL);
            }

            return RedirectToAction("ListCategories");
        }
#endregion

#region Tournaments
        public IActionResult ListTournaments()
        {
            var tournamentsDTO = new TournamentsCrud(_db).GetAll().ToList();
            var tournamentsView = new TournamentToView().ConvertList(tournamentsDTO);
            return View(tournamentsView);
        }

        public IActionResult CreateTournament()
        {
            //var tournamentDTO = new TournamentsCrud(_db).Get(1);
            //var tournamentDTO = new TournamentsCrud(_db).GetEmpty();
            var tournamentView = new TournamentToView().GetEmpty();
            var categoriesList = new CategoriesCrud(_db).GetAll().ToList();
            tournamentView.SelectCategories = new CategoryToView().GetSelect(categoriesList);
            return View(tournamentView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTournament(TournamentViewModel tournament)
        {
            if (ModelState.IsValid)
            {
                var tournamentDAL = new TournamentToView().ConvertBack(tournament);
                new TournamentsCrud(_db).Add(tournamentDAL);
            }
            return RedirectToAction("ListTournaments");
        }

        public IActionResult EditTournament(int id)
        {
            var tournamentDAL = new TournamentsCrud(_db).Get(id);
            var tournamentView = new TournamentToView().Convert(tournamentDAL);

            var categoriesList = new CategoriesCrud(_db).GetAll().ToList();
            tournamentView.SelectCategories = new CategoryToView().GetSelect(categoriesList);

            return View(tournamentView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTournament(TournamentViewModel tournamentView)
        {
            if (ModelState.IsValid)
            {
                var tournamentDAL = new TournamentToView().ConvertBack(tournamentView);
                new TournamentsCrud(_db).Update(tournamentDAL);
            }
            return RedirectToAction("ListTournaments");
        }
#endregion

#region Events
        public IActionResult ListEvents()
        { 
            var eventsDAL = new EventsCrud(_db).GetAll();
            var eventsView = new EventToView().ConvertAll(eventsDAL.ToList());

            return View(eventsView); 
        }

        public IActionResult CreateEvent() 
        {
            var eventsView = new EventToView().CreateEmpty();
            var tournamentList = new TournamentsCrud(_db).GetAll().ToList();
            eventsView.SelectTournament = new TournamentToView().GetSelect(tournamentList);
            return View(eventsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEvent(EventViewModel eventView) 
        { 
            if (ModelState.IsValid) 
            {
                var eventDAL = new EventToView().ConvertBack(eventView);
                new EventsCrud(_db).Add(eventDAL);
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult EditEvent(int id)
        {
            var eventDAL = new EventsCrud(_db).Get(id);
            var eventView = new EventToView().Convert(eventDAL);

            var tournamentsDTO = new TournamentsCrud(_db).GetAll().ToList();
            eventView.SelectTournament = new TournamentToView().GetSelect(tournamentsDTO);

            return View(eventView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEvent(EventViewModel eventView) 
        {
            if (ModelState.IsValid) 
            {
                var eventDAL = new EventToView().ConvertBack(eventView);
                new EventsCrud(_db).Update(eventDAL);
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult DetailsEvent(int id)
        { 
            var eventDAL = new EventsCrud(_db).Get(id);
            var eventView = new EventToView().Convert(eventDAL);

            return View(eventView);
        }
        #endregion

#region EventSchema

        public IActionResult AddSchemaItem(int eventId)
        {
            var eventSchemaItemVL = new EventSchemaItemToView().CreateEmpty(eventId);
            return View(eventSchemaItemVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchemaItem(EventSchemaItemViewModel eventSchemaItemVL)
        {
            if (ModelState.IsValid)
            { 
                var eventSchemaItemDAL = new EventSchemaItemToView().ConvertBack(eventSchemaItemVL);
                new EventSchemaItemsCrud(_db).Add(eventSchemaItemDAL);

            }
            return RedirectToAction("ListEvents");
        }

        public IActionResult ListSchemaItems(int eventId) 
        { 
            var eventSchemaItemsDAL = new EventSchemaItemsCrud(_db).GetAll(eventId);
            //var eventSchemaItemsDAL = new EventSchemaItemsCrud(_db).GetEventSchemaItems(eventId);
            var eventSchemaItemsVL = new EventSchemaItemToView().ConvertAll(eventSchemaItemsDAL.ToList());

            var eventt = new EventsCrud(_db).Get(eventId);
            ViewData["eventId"] = eventId;
            //ViewData["tournamentName"] = new TournamentsCrud(_db).Get(eventt.TournamentId).Name;
            ViewData["tournamentName"] = eventt.Tournament.Name;

            return View(eventSchemaItemsVL);
        }

        //public IActionResult DetailsEventSchemaItem(int eventSchemaItemId)
        //{
        //    var eventSchemaItemDAL = new EventSchemaItemsCrud(_db).Get(eventSchemaItemId);
        //    var eventSchemaItemVL = new EventSchemaItemToView(_db).Convert(eventSchemaItemDAL);

        //    return View(eventSchemaItemVL);
        //}
        #endregion

#region EventSchemaGroups
        public IActionResult ListSchemaGroups(int EventSchemaItemId)
        {
            var groupsDAL = new SchemaGroupCrud(_db).GetAll(EventSchemaItemId).ToList();
            var groupsVL = new SchemaGroupToView().ConvertAll(groupsDAL);

            var eventSchemaItemDAL = new EventSchemaItemsCrud(_db).Get(EventSchemaItemId);
            var eventSchemaItemVL = new EventSchemaItemToView().Convert(eventSchemaItemDAL);
            // tournament removed from viewmodel. Now it can be reached via eventSchemaItemVL.Event.Tournament + (.category)

            ViewBag.EventSchemaItem = eventSchemaItemVL;

            return View(groupsVL);
        }

        public IActionResult AddSchemaGroup(int eventSchemaItemId)
        {
            var groupVL = new SchemaGroupToView().CreateEmpty(eventSchemaItemId);

            var eventSchemaItemDAL = new EventSchemaItemsCrud(_db).Get(eventSchemaItemId);
            groupVL.EventSchemaItem = new EventSchemaItemToView().Convert(eventSchemaItemDAL);

            return View(groupVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchemaGroup(SchemaGroupViewModel schemaGroupVL)
        {
            if (ModelState.IsValid)
            {
                var schemaGroupDAL = new SchemaGroupToView().ConvertBack(schemaGroupVL);
                new SchemaGroupCrud(_db).Add(schemaGroupDAL);
            }

            return RedirectToAction("ListSchemaGroups", new { EventSchemaItemId = schemaGroupVL.EventSchemaItemId });
        }

        #endregion

#region Games

        public IActionResult ListGames(int schemaGroupId)
        {
            //var gamesWithTeamsDAL = new GamesCrud(_db).GetAllForGroupWithTeams(schemaGroupId).ToList();
            //var gamesWithTeamsVL = new GameWithTeamsToView().ConvertAll(gamesWithTeamsDAL);
            
            var gamesWithTeamsDAL = new GamesCrud(_db).GetAll(schemaGroupId).ToList();
            var gamesWithTeamsVL = new GameToView().ConvertAll(gamesWithTeamsDAL).ToList();

            ViewData["schemaGroupId"] = schemaGroupId;

            return View(gamesWithTeamsVL);
        }

        public IActionResult AddGame(int schemaGroupId)
        {
            var gameView = new GameToView().CreateEmpty(schemaGroupId);
            var teamsWithClubCountry = new TeamCrud(_db).GetAll().ToList();
            //var teamsWithClubCountry = new TeamCrud(_db).GetAllWithClubCountry().ToList();

            ViewBag.teamsList = new TeamToView().GetFullSelestList(teamsWithClubCountry);

            return View(gameView);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGame(GameViewModel gameVL)
        {
            if (ModelState.IsValid)
            {
                var gameDAL = new GameToView().ConvertBack(gameVL);
                new GamesCrud(_db).Add(gameDAL);
            }

            return RedirectToAction("ListGames", new { schemaGroupId = gameVL.SchemaGroupId});
        }

        public IActionResult EditGame(int gameId)
        {
            var editGameVL = new EditGameVM();
            if (gameId > 0)
            {
                var gameDAL = new GamesCrud(_db).Get(gameId);
                if (gameDAL != null)
                {
                    editGameVL.Game = new GameToView().Convert(gameDAL);

                    var teamsWithClubCountry = new TeamCrud(_db).GetAll().ToList();
                    editGameVL.HomeTeamSL = new SelectList(new TeamToView().GetFullSelestList(teamsWithClubCountry), "Value", "Text");
                    if (editGameVL.HomeTeamSL != null && editGameVL.HomeTeamSL.FirstOrDefault(i => i.Value == editGameVL.Game.HomeTeamId.ToString()) != null)
                    {
                        editGameVL.HomeTeamSL.First(i => i.Value == editGameVL.Game.HomeTeamId.ToString()).Selected = true;
                    }

                    editGameVL.VisitorTeamSL = new SelectList(new TeamToView().GetFullSelestList(teamsWithClubCountry), "Value", "Text");
                    if (editGameVL.VisitorTeamSL.FirstOrDefault(i => i.Value == editGameVL.Game.VisitorTeamId.ToString()) != null)
                    {
                        editGameVL.VisitorTeamSL.First(i => i.Value == editGameVL.Game.VisitorTeamId.ToString()).Selected = true;
                    }

                    editGameVL.TourSL = Enums.TourNumber.Tour1.ToSelectList();
                    if (editGameVL.TourSL != null && editGameVL.TourSL.FirstOrDefault(i => i.Text == editGameVL.Game.Tour.ToString()) != null)
                    {
                        editGameVL.TourSL.First(i => i.Text == editGameVL.Game.Tour.ToString()).Selected = true;
                    }

                    editGameVL.StatusSL = Enums.GameStatus.Canceled.ToSelectList();
                    //var ttt = editGameVL.StatusSL.FirstOrDefault().Text;
                    //var ttt2 = editGameVL.Game.GameStatus.ToString();
                    if (editGameVL.StatusSL != null && editGameVL.StatusSL.FirstOrDefault(i => i.Text == editGameVL.Game.GameStatus.ToString()) != null)
                    {
                        editGameVL.StatusSL.First(i => i.Text == editGameVL.Game.GameStatus.ToString()).Selected = true;
                        var ttt2 = editGameVL.StatusSL.First(i => i.Text == editGameVL.Game.GameStatus.ToString());
                        var ttt = editGameVL.StatusSL;
                    }
                }
            }

            ViewBag.homeTeamsList = editGameVL.HomeTeamSL;
            ViewBag.visitorTeamsList = editGameVL.VisitorTeamSL;
            ViewBag.StatusList = editGameVL.StatusSL;
            ViewBag.TourList = editGameVL.TourSL;

            return View(editGameVL.Game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGame(GameViewModel gameVL)
        {
            if (ModelState.IsValid)
            {
                var gameDAL = new GameToView().ConvertBack(gameVL);
                new GamesCrud(_db).Update(gameDAL);
            }

            return RedirectToAction("ListGames", new { schemaGroupId = gameVL.SchemaGroupId });
        }

        #endregion

#region Country

        public IActionResult ListCountries()
        {
            var countriesDAL = new CountryCrud(_db).GetAll().ToList();
            var countriesVL = new CountryToView().ConvertAll(countriesDAL);

            return View(countriesVL);
        }

        public IActionResult AddCountry()
        {
            var CountryVL = new CountryToView().CreateEmpty();

            return View(CountryVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCountry(CountryViewModel countryVL)
        {
            if (ModelState.IsValid)
            {
                var countryDAL = new CountryToView().ConvertBack(countryVL);
                new CountryCrud(_db).Add(countryDAL);
            }

            return RedirectToAction("ListCountries");
        }

#endregion

#region Club
        public IActionResult ListClubs()
        {
            var clubsDAL = new ClubCrud(_db).GetAll().ToList();
            var clubsVL = new ClubToView().ConvertAll(clubsDAL);

            //ViewBag.CountriesSL = new CountryCrud(_db).GetSelectItemList();

            return View(clubsVL);
        }

        public IActionResult AddClub()
        {
            var clubVl = new ClubToView().CreateEmpty();

            ViewBag.CountriesSL = new CountryCrud(_db).GetSelectItemList();

            return View(clubVl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddClub(ClubViewModel clubVl)
        {
            if (ModelState.IsValid)
            {
                var clubDAL = new ClubToView().ConvertBack(clubVl);
                new ClubCrud(_db).Add(clubDAL);
            }

            return RedirectToAction("ListClubs");
        }



#endregion

#region Team

        public IActionResult ListTeams(int clubId) 
        { 
            var teamsDAL = new TeamCrud(_db).GetAll(clubId).ToList();
            var teamsVL = new TeamToView().ConvertAll(teamsDAL); 
            
            var clubDAL = new ClubCrud(_db).Get(clubId);
            ViewBag.Club = new ClubToView().Convert(clubDAL);
            
            return View(teamsVL);
        }

        public IActionResult AddTeam(int clubId)
        {
            var teamVL = new TeamToView().CreateEmpty();
            teamVL.ClubId = clubId;

            return View(teamVL);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddTeam(TeamViewModel teamVL)
        {
            if (ModelState.IsValid)
            {
                var teamDAL = new TeamToView().ConvertBack(teamVL);
                new TeamCrud(_db).Add(teamDAL);
            }

            return RedirectToAction("ListTeams", new { clubId = teamVL.ClubId });
        }



        #endregion

#region Staff

        public IActionResult ListStaffs(int clubId) 
        {
            var staffsDAL = new StaffsCrud(_db).GetAll(clubId).ToList();
            var staffsVL = new StaffToView().ConvertAll(staffsDAL);

            var clubDAL = new ClubCrud(_db).Get(clubId);
            ViewBag.club = new ClubToView().Convert(clubDAL);

            return View(staffsVL);
        }

        public IActionResult AddStaff(int clubId)
        {
            var emptyStaff = new StaffToView().CreateEmpty(clubId);

            return View(emptyStaff);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddStaff(StaffViewModel staffVL)
        {
            if (ModelState.IsValid)
            {
                new StaffsCrud(_db).Add(new StaffToView().ConvertBack(staffVL));
            }

            return RedirectToAction("ListStaffs", new { clubId = staffVL.ClubId } );
        }

        #endregion

#region Players
        public IActionResult ListPlayers(int teamId)
        {
            var playersDAL = new PlayersCrud(_db).GetAll(teamId).ToList();
            var playersVL = new PlayerToView().ConvertAll(playersDAL);

            var teamDAL = new TeamCrud(_db).Get(teamId);
            ViewBag.team = new TeamToView().Convert(teamDAL);

            return View(playersVL);
        }

        public IActionResult AddPlayer(int teamId)
        {
            var playerVL = new PlayerToView().CreateEmpty(teamId);

            return View(playerVL);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddPlayer(PlayerViewModel playerVL)
        {
            if (ModelState.IsValid)
            {
                var playerDAL = new PlayerToView().ConvertBack(playerVL);
                new PlayersCrud(_db).Add(playerDAL);
            }

            return RedirectToAction("ListPlayers", new { teamId = playerVL.TeamId });
        }


		#endregion

#region News
        public IActionResult ListVideos(SportType? sportType,
                                        bool? isGeneral,
                                        int? newsId,
                                        int? categoryId,
                                        int? teamId,
                                        int? gameId,
                                        DateTime? lastDate = null,
                                        int? lastId = null,
                                        int? skip = null
                                        )
        {

            var count = 0;
            var skipFixxed = 0;
            var amount = Constants.DefaulVideosAmount;
            if (skip != null && skip > 0)
            {
                skipFixxed = (int)skip;
            }

            var videosDAL = new VideosCrud(_db).GetAllHard(out count, sportType, isGeneral, newsId, categoryId, teamId, gameId, lastDate, skipFixxed, amount);
            var videosVL = new VideoToView().ConvertAll(videosDAL.ToList());

            if (count > skipFixxed + amount)
            {
                ViewBag.SkipNext = skipFixxed + amount;
            }
            if (skipFixxed > 0)
            {
                ViewBag.SkipPrev = skip - amount <= 0 ? 0 : skip - amount;
            }

            ViewBag.sportType = sportType;
            ViewBag.isGeneral = isGeneral;
            ViewBag.news = newsId == null ? null : new NewsToView().Convert(new NewsCrud(_db).Get((int)newsId));
            ViewBag.category = categoryId == null ? null : new CategoryToView().Convert(new CategoriesCrud(_db).Get((int)categoryId));
            ViewBag.team = teamId == null ? null : new TeamToView().Convert(new TeamCrud(_db).Get((int)teamId));
            ViewBag.game = gameId == null ? null : new GameToView().Convert(new GamesCrud(_db).Get((int)gameId));
            ViewBag.lastDate = lastDate;
            ViewBag.lastId = lastId;

            return View(videosVL);
        }

        public IActionResult AddVideo(int? newsId)
        {
            var VideoVL = new VideoToView().CreateEmpty();
            if(newsId != null)
            {
                var newsDAL = new NewsCrud(_db).Get((int)newsId);
                if(newsDAL != null && newsDAL.Id == newsId)
                {
                    ViewBag.News = new NewsToView().Convert(newsDAL);
                }
                else
                {
                    ViewBag.NewsSL = new NewsCrud(_db).GetSelectItemList();
                }
            }

            ViewBag.CategorySL = new CategoriesCrud(_db).GetSelectItemList();
            ViewBag.TeamSL = new TeamCrud(_db).GetSelectItemList();
            ViewBag.GamesSL = new GamesCrud(_db).GetSelectItemList();

            return View(VideoVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVideo(VideoVM videoVM)
        {
            if (ModelState.IsValid)
            {
                new VideosCrud(_db).Add(new VideoToView().ConvertBack(videoVM));
            }

            return RedirectToAction("ListVideos");
        }

        public IActionResult EditVideo(int videoId)
        {
            var editVideo = new EditVideoVM();
            var videoDAL = new Video();
            //var videoVL = new VideoVM();
            if (videoId > 0)
            {
                videoDAL = new VideosCrud(_db).Get(videoId);
                if (videoDAL != null)
                {
                    editVideo.Video = new VideoToView().Convert(videoDAL, false);

                    if(editVideo.SportTypeSL != null && editVideo.SportTypeSL.FirstOrDefault(s => s.Text == editVideo.Video.SportType.ToString()) != null)
                    {
                        editVideo.SportTypeSL.FirstOrDefault(s => s.Text == editVideo.Video.SportType.ToString()).Selected = true;
                    }

                    editVideo.CategorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.CategoryId.ToString());
                    editVideo.TeamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.TeamId.ToString());
                    editVideo.GameSl = new SelectList(new GamesCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.GameId.ToString());
                    editVideo.NewsSL = new SelectList(new NewsCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.NewsId.ToString());
                }
            }

            return View(editVideo);
        }

        [HttpPost]
        public IActionResult UpdateVideo(EditVideoVM editVideoVM)
        {
            if(ModelState.IsValid && editVideoVM?.Video != null)
            {
                var videoDAL = new VideoToView().ConvertBack(editVideoVM.Video);
                new VideosCrud(_db).Update(videoDAL);
            }

            return RedirectToAction("ListVideos");
        }

        public IActionResult DeleteNewsVideo(int newsId, int videoId)
        {
            var videoDAL = new VideosCrud(_db).Get(videoId);
            if(videoDAL != null && videoDAL.NewsId == newsId)
            {
                videoDAL.NewsId = null;
                new VideosCrud(_db).Update(videoDAL);
            }

            return RedirectToAction("ListVideos", new { newsId = newsId });
        }

        public IActionResult ListAlbums(SportType? sportType,
                                        bool? isGeneral,
                                        int? newsId,
                                        int? categoryId,
                                        int? teamId,
                                        int? gameId,
                                        DateTime? lastDate = null,
                                        int? lastId = null,
                                        int? skip = null
                                        )
        {
            var count = 0;
            var skipFixxed = 0;
            var amount = Constants.DefaulAlbumsAmount;
            if (skip != null && skip > 0)
            {
                skipFixxed = (int)skip;
            }

            var albumsDAL = new AlbumsCrud(_db).GetAllHard(out count, sportType, isGeneral, newsId, categoryId, teamId, gameId, lastDate, skipFixxed, amount).ToList();
            var albumsVL = new AlbumToView().ConvertAll(albumsDAL, false);

            if(count > skipFixxed + amount)
            {
                ViewBag.SkipNext = skipFixxed + amount;
            }
            if(skipFixxed > 0)
            {
                ViewBag.SkipPrev = skip - amount <= 0 ? 0 : skip - amount;
            }


            ViewBag.sportType = sportType;
            ViewBag.isGeneral = isGeneral;
            ViewBag.news = newsId == null ? null : new NewsToView().Convert(new NewsCrud(_db).Get((int)newsId));
            ViewBag.category = categoryId == null ? null : new CategoryToView().Convert(new CategoriesCrud(_db).Get((int)categoryId));
            ViewBag.team = teamId == null ? null : new TeamToView().Convert(new TeamCrud(_db).Get((int)teamId));
            ViewBag.game = gameId == null ? null : new GameToView().Convert(new GamesCrud(_db).Get((int)gameId));
            ViewBag.lastDate = lastDate;
            ViewBag.lastId = lastId;

            return View(albumsVL);
        }

        public IActionResult DeleteNewsAlbum(int albumId, int newsId)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            if(newsDAL != null && albumDAL.NewsId != null && albumDAL.NewsId == newsId)
            {
                albumDAL.NewsId = null;
                new AlbumsCrud(_db).Update(albumDAL);
            }

            return RedirectToAction("ListAlbums", new { newsId = newsId });
        }

        public IActionResult AddAlbum(int categoryId = 0, int newsId = 0, int teamId = 0, int gameId =0, SportType sportType = SportType.NotDefined)
        {
            var albumVL = new AlbumToView().CreateEmpty();

            var categorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text");
            categorySL.SetSelected(categoryId.ToString());
            var newsSL = new SelectList(new NewsCrud(_db).GetSelectItemList(), "Value", "Text");
            newsSL.SetSelected(newsId.ToString());
            var teamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly: true), "Value", "Text");
            teamSL.SetSelected(teamId.ToString());
            var gameSL = new SelectList(new GamesCrud(_db).GetSelectItemList(), "Value", "Text");
            gameSL.SetSelected(gameId.ToString());
            var sportSL = Enums.SportType.NotDefined.ToSelectList();
            var t1 = sportType;
            var t2 = (int)sportType;
            var t3 = Convert.ToInt32(sportType);

            sportSL.SetSelected(Convert.ToInt32(sportType).ToString());

            ViewBag.CategorySL = categorySL;
            ViewBag.NewsSL = newsSL;
            ViewBag.TeamSL = teamSL;
            ViewBag.GameSL = gameSL;
            ViewBag.SportSL = sportSL;

            return View(albumVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAlbum(AlbumVM albumVL)
        {
            if (ModelState.IsValid)
            {
                var albumDL = new AlbumToView().ConvertBack(albumVL);
                new AlbumsCrud(_db).Add(albumDL);
                return RedirectToAction("AddPhoto", new { albumId = albumDL.Id});
            }

            return RedirectToAction("ListAlbums");
        }



        public IActionResult ListPhotos(int albumId)
        {
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            var albumVL = new AlbumToView().Convert(albumDAL);

            return View(albumVL);
        }

        public IActionResult ListTitlePhotos(int newsId)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            var newsVL = new NewsToView().Convert(newsDAL);

            return View(newsVL);
        }

        public IActionResult DeleteTPAlbum(int newsId, int tPId)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            if (newsDAL != null && newsDAL.NewsTitlePhotos != null)
            {
                var newsTitlePhoto = newsDAL.NewsTitlePhotos.FirstOrDefault(i => i.PhotoId == tPId);
                if(newsTitlePhoto != null && newsTitlePhoto.PhotoId == tPId)
                {
                    new NewsTitlePhotosCrud(_db).Delete(newsTitlePhoto);
                }
            }

            return RedirectToAction("ListTitlePhotos", new { newsId = newsId });
        }

        public IActionResult AddPhoto(int albumId)
        {
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            if (albumDAL == null)
            {
                return NotFound();
            }

            var photoVL = new PhotoToView().CreateEmpty(albumId);
            var albumVL = new AlbumToView().Convert(albumDAL);

            ViewBag.album = albumVL;

            return View(photoVL);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormCollection fc)
        {
            int albumId;
            int successfulySaves = 0;

            if (fc != null 
               && fc.ContainsKey("AlbumId") 
               && Int32.TryParse(fc["AlbumId"], out albumId) 
               && fc.Files != null 
               && fc.Files.Count > 0)
            {
                var album = new AlbumsCrud(_db).Get(albumId);
                if (album != null)
                {
                    var checkedFiles = FileTools.GetValidated(fc.Files.ToList());
                    foreach (var file in checkedFiles)
                    {
                        var newfileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg");
                        if (await FileTools.ResizeAndSave(file, albumId, _rootPath, newfileName))
                        {
                            var photoDAL = new Photo();
                            photoDAL.AlbumId = albumId;
                            photoDAL.Name = "Noname";
                            photoDAL.FnameOrig = newfileName;
                            photoDAL.FnameBig = newfileName;
                            photoDAL.FnameSmall = newfileName;
                            new PhotosCrud(_db).Add(photoDAL);
                            successfulySaves++;
                        }
                    }
                }
            }

            return Ok(new { count = successfulySaves });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPhoto(IFormCollection fc)
        {

            if(!fc.Keys.Contains("fnames") || !fc.Keys.Contains("albumId"))
            {
                return RedirectToAction("ListAlbums");
            }
            int albumId;
            if(!Int32.TryParse(fc["albumId"].FirstOrDefault(), out albumId))
            {
                return RedirectToAction("ListAlbums");
            }
            if(albumId <=0)
            {
                return RedirectToAction("ListAlbums");
            }
            if (fc["fnames"].IsNullOrEmpty())
            {
                return RedirectToAction("ListAlbums");
            }
            var fileList = (fc["fnames"].FirstOrDefault()).Split("\r\n").ToList();
            if(fileList.Count <= 0)
            {
                return RedirectToAction("ListAlbums");
            }

            foreach(var fileName in fileList)
            {
                if(!fileName.IsNullOrEmpty())
                {
                    var photoDAL = new Photo();
                    photoDAL.AlbumId = albumId;
                    photoDAL.Name = albumId + "_" + fileName;
                    photoDAL.Description = fileName;
                    photoDAL.FnameOrig = fileName;
                    photoDAL.FnameBig = fileName;
                    photoDAL.FnameSmall = fileName;
                    new PhotosCrud(_db).Add(photoDAL);
                }
            }

            return RedirectToAction("ListAlbums");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ListNews(IFormCollection fc)
        {
            if(ModelState.IsValid && fc != null && fc.Keys.Contains("eventId"))
            {
                int eventId;
                if (Int32.TryParse(fc["eventId"].FirstOrDefault(), out eventId) && eventId > 0)
                {
                    return RedirectToAction("ListNews", new { eventId = eventId });
                }
            }

            return RedirectToAction("ListNews");
        }


        public IActionResult ListNews(SportType sportType = SportType.NotDefined,
                                        bool isGeneral = false,
                                        bool isFun = false,
                                        bool isOfficial = false,
                                        bool isInternational = false,
                                        bool isAnnual = false,
                                        int? eventId = null,
                                        int? categoryId = null,
                                        int? teamId = null,
                                        DateTime? lastDate = null,
                                        int skip = 0,
                                        int? newsId = null)
        {
            var pageDataVM = new ListNewsVM();
            var newestDate = lastDate ?? DateTime.MaxValue;
            // if (newsId != null ) {get news, return view}

            var countt = 0;
            if(skip < 0)
            {
                skip = 0;
            }
            List<int>? categoryIds = null;
            if(categoryId != null)
            {
                categoryIds = new List<int>() { (int)categoryId };
            }
            List<int>? teamIds = null;
            if(teamId != null)
            {
                teamIds = new List<int>() { (int)teamId };
            }

            //var newsDAL = new NewsCrud(_db).GetAll(sportType, isGeneral, eventId, categoryId, teamId, lastDate, lastId, amount).ToList();
            var newsDAL = new NewsCrud(_db).GetAllFiltered(out countt, 
                                                           sportType: sportType, 
                                                           includeAllGeneral: isGeneral,
                                                           includeAllFun: isFun,
                                                           isOfficial: isOfficial,
                                                           isInternational: isInternational,
                                                           isAnnual: isAnnual,
                                                           eventId: eventId,
                                                           categoryIds: categoryIds,
                                                           teamIds: teamIds,
                                                           newestDate: newestDate,
                                                           skip: skip).ToList();
            pageDataVM.News = new NewsToView().ConvertAll(newsDAL);
            if(countt > skip + Constants.DefaulNewsAmount)
            {
                pageDataVM.SkipNext = skip + Constants.DefaulNewsAmount;
            }
            if(skip > 0)
            {
                pageDataVM.SkipPrev = skip - Constants.DefaulNewsAmount <= 0 ? 0 : skip - Constants.DefaulNewsAmount;
            }
            pageDataVM.SportType = sportType;
            pageDataVM.isGeneral = isGeneral;
            pageDataVM.isOfficial = isOfficial;
            pageDataVM.isAnnual = isAnnual;
            pageDataVM.isFun = isFun;
            pageDataVM.Event = eventId == null ? null : new EventToView().Convert(new EventsCrud(_db).Get((int)eventId));
            pageDataVM.Category = categoryId == null ? null : new CategoryToView().Convert(new CategoriesCrud(_db).Get((int)categoryId));
            pageDataVM.Team = teamId == null ? null : new TeamToView().Convert(new TeamCrud(_db).Get((int)teamId));
            pageDataVM.LastDate = lastDate;


            // at the moment we let only event selection
            //pageDataVM.TeamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(), "Value", "Text");
            //if(pageDataVM.Team != null && pageDataVM.TeamSL.First(i => i.Value == pageDataVM.Team.Id.ToString()) != null)
            //{
            //    pageDataVM.TeamSL.First(i => i.Value == pageDataVM.Team.Id.ToString()).Selected = true;
            //}
            pageDataVM.EventSL = new SelectList(new EventsCrud(_db).GetSelectItemList(), "Value", "Text");
            if (pageDataVM.Event != null && pageDataVM.EventSL.First(i => i.Value == pageDataVM.Event.EventViewModelId.ToString()) != null)
            {
                pageDataVM.EventSL.First(i => i.Value == pageDataVM.Event.EventViewModelId.ToString()).Selected = true;
            }

            //ViewBag.sportType = sportType;
            //ViewBag.isGeneral = isGeneral;
            //ViewBag.eventt = eventId == null ? null : new EventToView().Convert(new EventsCrud(_db).Get((int)eventId));
            //ViewBag.category = categoryId == null ? null : new CategoryToView().Convert(new CategoriesCrud(_db).Get((int)categoryId));
            //ViewBag.team = teamId == null ? null : new TeamToView().Convert(new TeamCrud(_db).Get((int)teamId));
            //ViewBag.lastDate = lastDate;
            //ViewBag.lastId = lastId;
            //ViewBag.amount = amount; // limit to const.defaout maximum in crud


            return View(pageDataVM);
        }

        public IActionResult AddNews()
        {
            var newsVL = new NewsToView().CreateEmpty();

            ViewBag.eventsSL = new EventsCrud(_db).GetSelectItemList();
            ViewBag.categoriesSL = new CategoriesCrud(_db).GetSelectItemList();
            ViewBag.teamsSL = new TeamCrud(_db).GetSelectItemList();

            return View(newsVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNews(NewsVM newsVL)
        {
            if(ModelState.IsValid)
            {
                var newsDAL = new NewsToView().ConvertBack(newsVL);
                new NewsCrud(_db).Add(newsDAL);
            }

            return RedirectToAction("ListNews");
        }

        public IActionResult PublishNews(int newsId, int? fordate)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            if (newsDAL != null)
            {
                newsDAL.PublishDate = DateTime.Now;
                if (fordate == 1)
                {
                    return View(newsId);
                }
                else
                {
                    new NewsCrud(_db).Update(newsDAL);
                }
            }
            return RedirectToAction("ListNews");
        }

        [HttpPost]
        public IActionResult PublishNews(int newsId, DateTime publishDate)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            if(newsDAL != null)
            {
                newsDAL.PublishDate = publishDate;
                new NewsCrud(_db).Update(newsDAL);
            }

            return RedirectToAction("ListNews");
        }

        public IActionResult DeleteNews(int newsId)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            if(newsDAL != null && newsDAL.Id == newsId)
            {
                new AlbumsCrud(_db).UnlinkFromNews(newsId);
                new NewsTitlePhotosCrud(_db).UnlinkFromNews(newsId);
                new VideosCrud(_db).UnlinkFromNews(newsId);

                new NewsCrud(_db).Delete(newsDAL);
            }

            return RedirectToAction("ListNews");
        }

        public IActionResult AddAlbumToNews(int newsId)
        {
            var albumVL = new AlbumToView().CreateEmpty();
            
            var newsDAL = new NewsCrud(_db).Get(newsId);
            var newsVL = new NewsToView().Convert(newsDAL);
            
            var albumsSL = new AlbumsCrud(_db).GetSelectItemList(isNewsEmpty: true);

            ViewBag.albumsSL = albumsSL;
            ViewBag.newsVL = newsVL;

            return View(albumVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAlbumToNews(int NewsId, int Id)
        {
            if (NewsId > 0 && Id >0)
            {
                var albumDAL = new AlbumsCrud(_db).Get(Id);
                if(albumDAL != null && albumDAL.NewsId == null)
                {
                    new AlbumsCrud(_db).Update(id: Id, newsId: NewsId);
                }
                
            }

            return RedirectToAction("ListNews");
        }

        public IActionResult AddVideoToNews(int newsId)
        {
            var videoVL = new VideoToView().CreateEmpty();

            var newsDAL = new NewsCrud(_db).Get(newsId);
            var newsVL = new NewsToView().Convert(newsDAL);

            var videosSL = new VideosCrud(_db).GetSelectItemList(isEmptyNews: true);

            ViewBag.videosSL = videosSL;
            ViewBag.newsVL = newsVL;

            return View(videoVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVideoToNews(int NewsId, int Id)
        {
            if (NewsId > 0 && Id > 0)
            {
                var videoDAL = new VideosCrud(_db).Get(Id);
                if (videoDAL != null && videoDAL.NewsId == null)
                {
                    new VideosCrud(_db).Update(id: Id, newsId: NewsId);
                }
            }

            return RedirectToAction("ListNews");
        }

        public IActionResult AddPhotosToNews(int newsId)
        {

            //var newsWithPhotosVL = new AddPhotosToNews();

            //var newsDAL = new NewsCrud(_db).Get(newsId);
            //newsWithPhotosVL.News = new NewsToView().Convert(newsDAL);
            //newsWithPhotosVL.PhotosMSL = new MultiSelectList(new PhotosCrud(_db).GetSelectItemList(100), "Value", "Text");

            //return View(newsWithPhotosVL);
            var newsDAL = new NewsCrud(_db).Get(newsId);
            var newsVL = new NewsToView().Convert(newsDAL);

            return View(newsVL);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddPhotosToNews(AddPhotosToNews newsWithPhotosVL)
        //{
        //    if (newsWithPhotosVL.News.Id > 0 && !newsWithPhotosVL.PhotoIds.IsNullOrEmpty())
        //    {
        //        foreach (var photoId in newsWithPhotosVL.PhotoIds)
        //        {
        //            var newsTitlePhoto = new NewsTitlePhoto();
        //            newsTitlePhoto.NewsId = newsWithPhotosVL.News.Id;
        //            newsTitlePhoto.PhotoId = photoId;
        //            newsTitlePhoto.Name = photoId.ToString();
        //            new NewsTitlePhotosCrud(_db).Add(newsTitlePhoto);
        //        }
        //    }

        //    return RedirectToAction("ListNews");
        //}

        [HttpPost]
        public async Task<IActionResult> AddPhotosToNews(IFormCollection fc)
        {
            int newsId;
            if (fc != null
               && fc.ContainsKey("NewsId")
               && Int32.TryParse(fc["NewsId"], out newsId)
               && fc.Files != null
               && fc.Files.Count > 0
               && new NewsCrud(_db).Get(newsId) != null)
            {
                var checkedFiles = FileTools.GetValidated(fc.Files.ToList());
                foreach (var file in checkedFiles)
                {
                    var newfileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg");
                    if (await FileTools.ResizeAndSave(file, Constants.TitleAlbumsId, _rootPath, newfileName))
                    {
                        var photoDAL = new Photo();
                        photoDAL.AlbumId = Constants.TitleAlbumsId;
                        photoDAL.Name = "Noname";
                        photoDAL.FnameOrig = newfileName;
                        photoDAL.FnameBig = newfileName;
                        photoDAL.FnameSmall = newfileName;
                        new PhotosCrud(_db).Add(photoDAL);

                        var newsTitlePhotoDAL = new NewsTitlePhoto();
                        newsTitlePhotoDAL.NewsId = newsId;
                        newsTitlePhotoDAL.PhotoId = photoDAL.Id;
                        newsTitlePhotoDAL.Name = "Title photo";

                        new NewsTitlePhotosCrud(_db).Add(newsTitlePhotoDAL);
                    }
                }
            }

            return Ok(new { count = 1 });
        }

        #endregion

    }
}
