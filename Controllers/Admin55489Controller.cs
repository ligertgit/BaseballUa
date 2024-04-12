using BaseballUa.BlData;
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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc.Html;
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
            var eventsDAL = new EventsCrud(_db).GetAll().Where(i => i.StartDate != null).OrderBy(i => Math.Abs(((DateTime)i.StartDate - DateTime.Now).TotalMinutes));
            var eventsView = new EventToView().ConvertAll(eventsDAL.ToList());

            return View(eventsView); 
        }

        public IActionResult CreateEvent() 
        {
            var eventsView = new EventToView().CreateEmpty();
            var tournamentList = new TournamentsCrud(_db).GetAll().ToList();
            var teamsList = new TeamCrud(_db).GetAll().ToList();
            eventsView.SelectTournament = new TournamentToView().GetSelect(tournamentList);
            eventsView.EventTeamsSL = new SelectList(new TeamToView().GetFullSelestList(teamsList), "Value", "Text");
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
                if(!eventView.EventTeamsIds.IsNullOrEmpty() && eventDAL.Id > 0)
                {
                    foreach(var teamId in eventView.EventTeamsIds)
                    {
                        new EventToTeamsCrud(_db).Add(eventDAL.Id, teamId);
                    }
                }
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult EditEvent(int id)
        {
            var eventDAL = new EventsCrud(_db).Get(id);
            var eventView = new EventToView().Convert(eventDAL);

            var tournamentsDTO = new TournamentsCrud(_db).GetAll().ToList();
            var teamsList = new TeamCrud(_db).GetAll().ToList();
            eventView.SelectTournament = new TournamentToView().GetSelect(tournamentsDTO);
            eventView.EventTeamsSL = new SelectList(new TeamToView().GetFullSelestList(teamsList), "Value", "Text");

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
                new EventToTeamsCrud(_db).DeleteList(eventDAL.Id);
                if (!eventView.EventTeamsIds.IsNullOrEmpty() && eventDAL.Id > 0)
                {
                    foreach (var teamId in eventView.EventTeamsIds)
                    {
                        new EventToTeamsCrud(_db).Add(eventDAL.Id, teamId);
                    }
                }
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

        #region EventTeams
        public IActionResult ListEventTeams(int eventId)
        {
            var eventTeamsDAL = new TeamCrud(_db).GetEventToTeam(eventId).ToList();
            var eventTeamsVL = new TeamToView().ConvertAll(eventTeamsDAL);
            ViewBag.EventId = eventId;

            return View(eventTeamsVL);
        }

        public IActionResult AddEventTeam(int eventId)
        {
            var eventDAL = new EventsCrud(_db).Get(eventId);
            var eventVL = new EventViewModel();
            if (eventDAL.Id > 0)
            {
                eventVL = new EventToView().Convert(eventDAL);
                var teamsList = new TeamCrud(_db).GetAll().ToList();
                eventVL.EventTeamsSL = new SelectList(new TeamToView().GetFullSelestList(teamsList), "Value", "Text");
            }
            
            return View(eventVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEventTeam(IFormCollection fc)
        {
            int eventId;
            var eventTeamIdsList = new List<int>();
            if(fc != null 
                && fc.ContainsKey("EventViewModelId")
                && Int32.TryParse(fc["EventViewModelId"], out eventId)
                && fc.ContainsKey("EventTeamsIds")
                && fc["EventTeamsIds"].Count > 0)
            {
                foreach(var eventTeamId in fc["EventTeamsIds"])
                {
                    int i;
                    if(Int32.TryParse(eventTeamId, out i) && i > 0)
                    {
                        eventTeamIdsList.Add(i);
                    }
                }
                var eventTeamIdsListDB = new EventToTeamsCrud(_db).GetForEvent(eventId).Select(ett => ett.TeamId).ToList();
                var cleanedTeamList = eventTeamIdsList.Except(eventTeamIdsListDB);
                foreach(var  teamId in cleanedTeamList)
                {
                    new EventToTeamsCrud(_db).Add(eventId, teamId);
                }
                return RedirectToAction("ListEventTeams", new { eventId = eventId });
            }
            return RedirectToAction("ListEvents");
        }

        public IActionResult DeleteEventTeam(int teamId, int eventId)
        {
            if(teamId > 0 && eventId > 0)
            {
                new EventToTeamsCrud(_db).Delete(eventId, teamId);
                return RedirectToAction("ListEventTeams", new { eventId = eventId });
            }

            return RedirectToAction("ListEvents");
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
                return RedirectToAction("ListSchemaItems", new { eventId = eventSchemaItemDAL.EventId});
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

        public IActionResult EditSchemaItem(int eventSchemaItemId)
        {
            var eventSchemaItemDAL = new EventSchemaItemsCrud(_db).Get(eventSchemaItemId);
            if(eventSchemaItemDAL != null)
            {
                var eventSchemaItemVL = new EventSchemaItemToView().Convert(eventSchemaItemDAL);
                var schemaItemSL = Enums.GameType.Group1.ToSelectList();
                if(schemaItemSL.Any(s => s.Text == eventSchemaItemVL.SchemaItem.ToString()))
                {
                    schemaItemSL.First(s => s.Text == eventSchemaItemVL.SchemaItem.ToString()).Selected = true;
                    ViewBag.SchemaItemSL = schemaItemSL;
                }
                return View(eventSchemaItemVL);
            }

            return RedirectToAction("ListEvents");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSchemaItem(EventSchemaItemViewModel eventSchemaItem)
        {
            if(ModelState.IsValid)
            {
                var eventSchemaItemDAL = new EventSchemaItemToView().ConvertBack(eventSchemaItem);
                new EventSchemaItemsCrud(_db).Update(eventSchemaItemDAL);
                return RedirectToAction("ListSchemaItems", new { eventId = eventSchemaItemDAL.EventId });
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult DeleteSchemaItem(int eventSchemaItemId)
        {
            var schemaItemDAL = new EventSchemaItemsCrud(_db).Get(eventSchemaItemId);
            if(schemaItemDAL != null)
            {
                var eventGames = new EventSchemaItemsCrud(_db).GetGames(eventSchemaItemId);
                if(eventGames.IsNullOrEmpty())
                {
                    new EventSchemaItemsCrud(_db).Delete(schemaItemDAL);
                }
                return RedirectToAction("ListSchemaItems", new { eventId = schemaItemDAL.EventId });
            }

            return RedirectToAction("ListEvents");
        }
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

        public IActionResult DeleteSchemaGroup(int schemaGroupId)
        {
            var schemaGroupDAL = new SchemaGroupCrud(_db).Get(schemaGroupId);
            if(schemaGroupDAL != null)
            {
                if (new GamesCrud(_db).GetAll(schemaGroupId).IsNullOrEmpty())
                {
                    new SchemaGroupCrud(_db).Delete(schemaGroupDAL);
                }

                return RedirectToAction("ListSchemaGroups", new { EventSchemaItemId = schemaGroupDAL.EventSchemaItemId });
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult EditSchemaGroup(int schemaGroupId)
        {
            var schemaGroupDAL = new SchemaGroupCrud(_db).Get(schemaGroupId);
            if(schemaGroupDAL != null)
            {
                var schemaGroupVL = new SchemaGroupToView().Convert(schemaGroupDAL);
                return View(schemaGroupVL);
            }

            return RedirectToAction("ListEvents");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSchemaGroup(SchemaGroupViewModel schemaGroupVL)
        {
            if(ModelState.IsValid)
            {
                var schemaGroupDAL = new SchemaGroupToView().ConvertBack(schemaGroupVL);
                new SchemaGroupCrud(_db).Update(schemaGroupDAL);

                return RedirectToAction("ListSchemaGroups", new { eventSchemaItemId = schemaGroupDAL.EventSchemaItemId });
            }

            return RedirectToAction("ListEvents");
        }
        #endregion

        #region Games

        public IActionResult ListGames(int schemaGroupId)
        {
            //var gamesWithTeamsDAL = new GamesCrud(_db).GetAllForGroupWithTeams(schemaGroupId).ToList();
            //var gamesWithTeamsVL = new GameWithTeamsToView().ConvertAll(gamesWithTeamsDAL);
            
            var gamesWithTeamsDAL = new GamesCrud(_db).GetAll(schemaGroupId).ToList();
            var gamesWithTeamsVL = new GameToView().ConvertAll(gamesWithTeamsDAL).ToList();
            var schemaGroup = new SchemaGroupCrud(_db).Get(schemaGroupId);

            ViewData["schemaGroupId"] = schemaGroupId;
            ViewBag.schemaGroup = new SchemaGroupToView().Convert(schemaGroup, doSubConvert : false);
            
            return View(gamesWithTeamsVL);
        }

        public IActionResult AddGame(int schemaGroupId)
        {
            var gameView = new GameToView().CreateEmpty(schemaGroupId);
            var eventId = new SchemaGroupCrud(_db).Get(schemaGroupId)?.EventSchemaItem?.EventId;
            if(eventId != null)
            {
                var teamsWithClubCountry = new TeamCrud(_db).GetEventToTeam((int)eventId).ToList();
                ViewBag.teamsList = new TeamToView().GetFullSelestList(teamsWithClubCountry);
                return View(gameView);
            }
            //var teamsWithClubCountry = new TeamCrud(_db).GetAllWithClubCountry().ToList();

            return RedirectToAction("ListEvents");
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

                    var eventId = new SchemaGroupCrud(_db).Get(gameDAL.SchemaGroupId)?.EventSchemaItem?.EventId;
                    if (eventId != null)
                    {
                        var teamsWithClubCountry = new TeamCrud(_db).GetEventToTeam((int)eventId).ToList();
                        editGameVL.HomeTeamSL = new SelectList(new TeamToView().GetFullSelestList(teamsWithClubCountry), "Value", "Text", editGameVL.Game?.HomeTeam?.Id.ToString());
                        editGameVL.VisitorTeamSL = new SelectList(new TeamToView().GetFullSelestList(teamsWithClubCountry), "Value", "Text", editGameVL.Game?.VisitorTeam?.Id.ToString());
                        
                        editGameVL.TourSL = Enums.TourNumber.Tour1.ToSelectList();
                        if (editGameVL.TourSL != null && editGameVL.TourSL.FirstOrDefault(i => i.Text == editGameVL.Game?.Tour.ToString()) != null)
                        {
                            editGameVL.TourSL.First(i => i.Text == editGameVL.Game.Tour.ToString()).Selected = true;
                        }

                        editGameVL.StatusSL = Enums.GameStatus.Canceled.ToSelectList();
                        if (editGameVL.StatusSL != null && editGameVL.StatusSL.FirstOrDefault(i => i.Text == editGameVL.Game.GameStatus.ToString()) != null)
                        {
                            editGameVL.StatusSL.First(i => i.Text == editGameVL.Game.GameStatus.ToString()).Selected = true;
                        }

                        ViewBag.homeTeamsList = editGameVL.HomeTeamSL;
                        ViewBag.visitorTeamsList = editGameVL.VisitorTeamSL;
                        ViewBag.StatusList = editGameVL.StatusSL;
                        ViewBag.TourList = editGameVL.TourSL;

                        return View(editGameVL.Game);
                    }
                }
            }
            return RedirectToAction("ListEvents");
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

            if(gameVL.SchemaGroupId > 0)
            {
                return RedirectToAction("ListGames", new { schemaGroupId = gameVL.SchemaGroupId });
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult DeleteGame(int gameId)
        {
            var gameDAL = new GamesCrud(_db).Get(gameId);
            if(gameDAL != null)
            {
                new VideosCrud(_db).UnlinkFromGames(gameId);
                new AlbumsCrud(_db).UnlinkFromGames(gameId);
                new GamesCrud(_db).Delete(gameDAL);
                return RedirectToAction("ListGames", new { schemaGroupId = gameDAL.SchemaGroupId });
            }

            return RedirectToAction("ListEvents");
        }

        public IActionResult DeleteGameAlbum(int albumId, int gameId)
        {
            var gameDAL = new GamesCrud(_db).Get(gameId);
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            if (gameDAL != null && albumDAL != null && albumDAL.GameId == gameId)
            {
                albumDAL.GameId = null;
                new AlbumsCrud(_db).Update(albumDAL);
            }

            return RedirectToAction("ListAlbums", new { gameId = gameId });
        }

        public IActionResult DeleteGameVideo(int videoId, int gameId)
        {
            var gameDAL = new GamesCrud(_db).Get(gameId);
            var videoDAL = new VideosCrud(_db).Get(videoId);
            if (gameDAL != null && videoDAL != null && videoDAL.GameId == gameId)
            {
                videoDAL.GameId = null;
                new VideosCrud(_db).Update(videoDAL);
            }

            return RedirectToAction("ListVideos", new { gameId = gameId });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateCountry(IFormCollection fc)
        {
            var country = new Country();
            var countryToUpdate = new Country();

            if (fc != null
                && fc.ContainsKey("Name")
                && fc["Name"].ToString().Length > 0
                && fc["Name"].ToString().Length <= typeof(Country).GetProperty("Name").GetCustomAttribute<StringLengthAttribute>().MaximumLength
                && fc.ContainsKey("ShortName")
                && fc["ShortName"].ToString().Length > 0
                && fc["ShortName"].ToString().Length <= typeof(Country).GetProperty("ShortName").GetCustomAttribute<StringLengthAttribute>().MaximumLength)
            {
                int formCountryId;
                string newFileName;

                if (fc.ContainsKey("Id") && fc["id"].ToString() != null && Int32.TryParse(fc["id"],out formCountryId))
                {
                    countryToUpdate = new CountryCrud(_db).Get(formCountryId);
                }
                if(countryToUpdate != null)
                {
                    country = countryToUpdate;
                    newFileName = country.FnameFlagBig == Constants.DefaultCountryBigImage ? Path.ChangeExtension(Path.GetRandomFileName(), ".jpg") : country.FnameFlagBig;
                }
                else
                {
                    country.FnameFlagSmall = Constants.DefaultCountrySmallImage;
                    country.FnameFlagBig = Constants.DefaultCountryBigImage;
                    newFileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg");
                }


                
                if(fc.Files != null && fc.Files.Count == 1)
                {
                    // загрузить фотки, сресайзить, засунуть в папку, получить имя файла
                    var checkedFile = FileTools.GetValidated(fc.Files.ToList(), isIcon: true).FirstOrDefault();

                    if (checkedFile != null && await FileTools.ResizeAndSave(checkedFile, 0, _rootPath, newFileName, ImageType.Flag))
                    {
                        country.FnameFlagBig = newFileName;
                        country.FnameFlagSmall = newFileName;
                    }
                }
                country.Name = fc["Name"].ToString();
                country.ShortName = fc["ShortName"].ToString();
                if(country.Id > 0)
                {
                    new CountryCrud(_db).Update(country);
                }
                else 
                {
                    new CountryCrud(_db).Add(country);
                }
            }

            return RedirectToAction("ListCountries");
        }

        public IActionResult EditCountry(int countryId)
        {
            var countryDAL = new CountryCrud(_db).Get(countryId);
            var countryVL = new CountryViewModel();
            if (countryDAL != null)
            {
                countryVL = new CountryToView().Convert(countryDAL, false);
            }

            return View(countryVL);

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
        public IActionResult AddClub(ClubViewModel clubVL)
        {
            if (ModelState.IsValid)
            {
                var clubDAL = new ClubToView().ConvertBack(clubVL);
                if(clubDAL.Id > 0)
                {
                    new ClubCrud(_db).Update(clubDAL);
                }
                else
                {
                    new ClubCrud(_db).Add(clubDAL);
                }
            }

            return RedirectToAction("ListClubs");
        }

        public IActionResult EditClub(int clubId)
        {
            var clubDAL = new ClubCrud(_db).Get(clubId);
            if(clubDAL != null)
            {
                var countrySL = new SelectList(new CountryCrud(_db).GetSelectItemList(), "Value", "Text");
                if(countrySL.IsNullOrEmpty() && countrySL.Any(c => c.Value == clubDAL.Id.ToString()))
                {
                    var selectedItem = countrySL.First(c => c.Value == clubDAL.Id.ToString());
                    selectedItem.Selected = true;
                }
                ViewBag.CountriesSL = countrySL;
                return View(new ClubToView().Convert(clubDAL));
            }

            return RedirectToAction("ListClubs");
        }

        public IActionResult UpdateClubLogo(int clubId)
        {
            var clubDAL = new ClubCrud(_db).Get(clubId);
            if(clubDAL != null && clubDAL.Id > 0)
            {
                return View(new ClubToView().Convert(clubDAL));
            }

            return RedirectToAction("ListClubs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClubLogo(IFormCollection fc)
        {
            var club = new Club();
            int clubId;

            if (fc != null
                && fc.ContainsKey("Id")
                && fc["Id"].ToString() != null && Int32.TryParse(fc["Id"], out clubId)
                && fc.Files != null && fc.Files.Count == 1)
            {
                club = new ClubCrud(_db).Get(clubId);
                if(club != null && club.Id >0)
                {
                    var newFileName = club.FnameLogoBig == Constants.DefaultClubBigImage ?
                                                            Path.ChangeExtension(Path.GetRandomFileName(), ".jpg")
                                                            : club.FnameLogoBig;
                    var checkedFile = FileTools.GetValidated(fc.Files.ToList(), isIcon: true).FirstOrDefault();
                    
                    if(checkedFile != null 
                        && await FileTools.ResizeAndSave(checkedFile, 0, _rootPath, newFileName, ImageType.Club)
                        && club.FnameLogoBig == Constants.DefaultClubBigImage)
                    {
                        club.FnameLogoSmall = newFileName;
                        club.FnameLogoBig = newFileName;
                        new ClubCrud(_db).Update(club);
                    }
                }
            }

            return RedirectToAction("ListClubs");
        }

        #endregion

        #region Team

        public IActionResult ListTeams(int clubId) 
        { 
            var teamsDAL = new TeamCrud(_db).GetAll(clubId).ToList();
            var teamsVL = new TeamToView().ConvertAll(teamsDAL, doSubConvert: false); 
            
            var clubDAL = new ClubCrud(_db).Get(clubId);
            ViewBag.Club = new ClubToView().Convert(clubDAL, doSubConvert: false);
            
            return View(teamsVL);
        }

        public IActionResult AddTeam(int clubId)
        {
            var teamVL = new TeamToView().CreateEmpty();
            teamVL.ClubId = clubId;
            teamVL.FnameLogoBig = Constants.DefaultTeamBigImage;
            teamVL.FnameLogoSmall = Constants.DefaultTeamSmallImage;

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

        public IActionResult UpdateTeamLogo(int teamId)
        {
            var teamDAL = new TeamCrud(_db).Get(teamId);
            if(teamDAL != null)
            {
                return View(new TeamToView().Convert(teamDAL));
            }

            return RedirectToAction("ListClubs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTeamLogo(IFormCollection fc)
        {
            var teamDAL = new Team();
            int teamId;
            string newFileName;
            if(fc != null
                && fc.ContainsKey("Id")
                && Int32.TryParse(fc["Id"], out teamId)
                && fc.Files != null
                && fc.Files.Count() == 1)
            {
                teamDAL = new TeamCrud(_db).Get(teamId);
                if(teamDAL != null)
                {
                    newFileName = teamDAL.FnameLogoBig == Constants.DefaultTeamBigImage
                                ? newFileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg")
                                : teamDAL.FnameLogoBig;

                    var checkedFile = FileTools.GetValidated(fc.Files.ToList(), isIcon: true).FirstOrDefault();
                    if (checkedFile != null
                        && await FileTools.ResizeAndSave(checkedFile, 0, _rootPath, newFileName, ImageType.Team)
                        && teamDAL.FnameLogoBig == Constants.DefaultTeamBigImage)
                    {
                        teamDAL.FnameLogoBig = newFileName;
                        teamDAL.FnameLogoSmall = newFileName;
                        new TeamCrud(_db).Update(teamDAL);
                    }
                    return RedirectToAction("ListTeams", new { clubId = teamDAL.ClubId });
                }
            }
            return RedirectToAction("ListClubs");
        }

        public IActionResult EditTeam(int teamId)
        {
            var teamDAL = new TeamCrud(_db).Get(teamId);
            if(teamDAL != null)
            {
                //var sportTypeSL = new SelectList(new TeamCrud(_db).GetSelectItemList(), "Value", "Text", teamDAL.SportType);
                var sportTypeSL = Enums.SportType.NotDefined.ToSelectList();
                sportTypeSL.SetSelected(((int)teamDAL.SportType).ToString());
                var clubSL = new SelectList(new ClubCrud(_db).GetSelectItemList(), "Value", "Text", teamDAL.ClubId);
                ViewBag.SportTypeSL = sportTypeSL;
                ViewBag.ClubSL = clubSL;
                return View(new TeamToView().Convert(teamDAL, doSubConvert : false));
            }

            return RedirectToAction("ListClubs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTeam(TeamViewModel teamVL)
        {
            Team teamDAL;
            if(teamVL != null)
            {
                teamDAL = new TeamToView().ConvertBack(teamVL);
                new TeamCrud(_db).Update(teamDAL);
                return RedirectToAction("ListTeams", new { clubId = teamVL.ClubId});
            }

            return RedirectToAction("ListClubs");
        }

        public IActionResult DeleteTeam(int teamId)
        {
            var teamDAL = new TeamCrud(_db).Get(teamId);

            if (teamDAL != null
                && !new TeamCrud(_db).GetHomeGames(teamId).Any()
                && !new TeamCrud(_db).GetVisitorGames(teamId).Any()
                && !new TeamCrud(_db).GetVideos(teamId).Any()
                && !new TeamCrud(_db).GetAlbums(teamId).Any()
                && !new TeamCrud(_db).GetNews(teamId).Any()
                && !new TeamCrud(_db).GetPlayers(teamId).Any()) 
            {
                FileTools.RemoveTeamLogo(teamDAL);
                new TeamCrud(_db).Delete(teamDAL);
                return RedirectToAction("ListTeams", new { clubId = teamDAL.ClubId });
            }
            return RedirectToAction("ListClubs");
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

        public IActionResult UpdateStaffAvatar(int staffId)
        {
            var staffDAL = new StaffsCrud(_db).Get(staffId);
            if(staffDAL != null)
            {
                return View(new StaffToView().Convert(staffDAL));
            }

            return RedirectToAction("ListClubs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStaffAvatar(IFormCollection fc)
        {
            int staffId;
            var staffDAL = new Staff();
            string newFileName;

            if(fc != null
                && fc.ContainsKey("Id")
                && Int32.TryParse(fc["Id"].ToString(), out staffId)
                && !fc.Files.IsNullOrEmpty()
                && fc.Files.Count() == 1)
            {
                staffDAL = new StaffsCrud(_db).Get(staffId);
                if(staffDAL != null)
                {
                    newFileName = staffDAL.AvatarLarge == Constants.DefaultStaffBigImage
                                    ? Path.ChangeExtension(Path.GetRandomFileName(), ".jpg")
                                    : staffDAL.AvatarLarge;

                    var checkedFile = FileTools.GetValidated(fc.Files.ToList(), isIcon: true).FirstOrDefault();
                    if (checkedFile != null
                        && await FileTools.ResizeAndSave(checkedFile, 0, _rootPath, newFileName, ImageType.Staff)
                        && staffDAL.AvatarLarge == Constants.DefaultStaffBigImage)
                    {
                        staffDAL.AvatarLarge = newFileName;
                        staffDAL.AvatarSmall = newFileName;
                        new StaffsCrud(_db).Update(staffDAL);
                    }
                }
            }
            return RedirectToAction("ListClubs");
        }

        public IActionResult DeleteStaff(int staffId)
        {
            var staffDAL = new StaffsCrud(_db).Get(staffId);
            if(staffDAL != null)
            {
                new StaffsCrud(_db).Delete(staffDAL);
                if(staffDAL.AvatarLarge != Constants.DefaultStaffBigImage)
                {
                    FileTools.RemoveStaffAvatar(staffDAL);
                }
                return RedirectToAction("ListStaffs", new { clubId = staffDAL.ClubId });
            }

            return RedirectToAction("ListClubs");
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
            var sexSL = new SelectList(EnumHelper.GetSelectList(typeof(Enums.Sex)), "Value", "Text");
            ViewBag.sexSL = sexSL;
            return View(playerVL);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddPlayer(PlayerViewModel playerVL)
        {
            if (ModelState.IsValid)
            {
                var playerDAL = new PlayerToView().ConvertBack(playerVL);
                playerDAL.AvatarSmall = Constants.DefaultPlayerSmallImage;
                playerDAL.AvatarBig = Constants.DefaultPlayerBigImage;
                new PlayersCrud(_db).Add(playerDAL);
            }

            return RedirectToAction("ListPlayers", new { teamId = playerVL.TeamId });
        }

        public IActionResult DeletePlayer(int playerId)
        {
            var playerDAL = new PlayersCrud(_db).Get(playerId);
            if (playerDAL != null)
            {
                new PlayersCrud(_db).Delete(playerDAL);
                FileTools.RemovePlayerAvatar(playerDAL);
                return RedirectToAction("ListPlayers", new { teamId = playerDAL.TeamId });
            }

            return RedirectToAction("ListClubs");
        }

        public IActionResult UpdatePlayerAvatar(int playerId)
        {
            var playerDAL = new PlayersCrud(_db).Get(playerId);
            if(playerDAL != null)
            {
                var playerVL = new PlayerToView().Convert(playerDAL);
                return View(playerVL);
            }

            return RedirectToAction("ListClubs");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePlayerAvatar(IFormCollection fc)
        {
            int playerId;
            if(fc != null
                && fc.ContainsKey("Id")
                && Int32.TryParse(fc["Id"], out playerId)
                && fc.Files != null
                && fc.Files.Count == 1)
            {
                var playerDAL = new PlayersCrud(_db).Get(playerId);
                if(playerDAL != null)
                {
                    var newFileName = playerDAL.AvatarBig == Constants.DefaultPlayerBigImage ?
                                                            Path.ChangeExtension(Path.GetRandomFileName(), ".jpg")
                                                            : playerDAL.AvatarBig;
                    var checkedFile = FileTools.GetValidated(fc.Files.ToList(), isIcon: true).FirstOrDefault();

                    if (checkedFile != null
                        && await FileTools.ResizeAndSave(checkedFile, 0, _rootPath, newFileName, ImageType.Player)
                        && playerDAL.AvatarBig == Constants.DefaultPlayerBigImage)
                    {
                        playerDAL.AvatarBig = newFileName;
                        playerDAL.AvatarSmall = newFileName;
                        new PlayersCrud(_db).Update(playerDAL);
                    }
                    return RedirectToAction("ListPlayers", new { teamId = playerDAL.TeamId });
                }
            }
            return RedirectToAction("ListClubs");
        }

        #endregion

        #region Videos
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

        //public IActionResult AddVideo(int? newsId)
        //{
        //    var VideoVL = new VideoToView().CreateEmpty();
        //    if (newsId != null)
        //    {
        //        var newsDAL = new NewsCrud(_db).Get((int)newsId);
        //        if (newsDAL != null && newsDAL.Id == newsId)
        //        {
        //            ViewBag.News = new NewsToView().Convert(newsDAL);
        //        }
        //    }

        //    ViewBag.NewsSL = new NewsCrud(_db).GetSelectItemList();
        //    ViewBag.CategorySL = new CategoriesCrud(_db).GetSelectItemList();
        //    ViewBag.TeamSL = new TeamCrud(_db).GetSelectItemList(uaOnly : true);
        //    ViewBag.GamesSL = new GamesCrud(_db).GetSelectItemList();

        //    return View(VideoVL);
        //}

        public IActionResult AddVideo(int categoryId = 0, int newsId = 0, int teamId = 0, int gameId = 0, SportType sportType = SportType.NotDefined)
        {
            var VideoVL = new VideoToView().CreateEmpty();
            if (newsId > 0)
            {
                var newsDAL = new NewsCrud(_db).Get((int)newsId);
                if (newsDAL != null && newsDAL.Id == newsId)
                {
                    ViewBag.News = new NewsToView().Convert(newsDAL);
                }
            }

            var categorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text");
            categorySL.SetSelected(categoryId.ToString());
            var newsSL = new SelectList(new NewsCrud(_db).GetSelectItemList(), "Value", "Text");
            newsSL.SetSelected(newsId.ToString());
            var teamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly: true), "Value", "Text");
            teamSL.SetSelected(teamId.ToString());
            var gameSL = new SelectList(new GamesCrud(_db).GetSelectItemList(gameId: gameId == 0 ? null : gameId), "Value", "Text");
            gameSL.SetSelected(gameId.ToString());
            var sportSL = Enums.SportType.NotDefined.ToSelectList();
            sportSL.SetSelected(Convert.ToInt32(sportType).ToString());

            ViewBag.CategorySL = categorySL;
            ViewBag.NewsSL = newsSL;
            ViewBag.TeamSL = teamSL;
            ViewBag.GameSL = gameSL;
            ViewBag.SportSL = sportSL;

            ViewBag.newsId = newsId;
            ViewBag.gameId = gameId;
            ViewBag.teamId = teamId;

            return View(VideoVL);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVideo(VideoVM videoVM, int navNewsId = 0, int navTeamId = 0, int navGameId = 0)
        {
            if (ModelState.IsValid)
            {
                new VideosCrud(_db).Add(new VideoToView().ConvertBack(videoVM));
            }

            if (navGameId > 0)
            {
                var gameDAL = new GamesCrud(_db).Get(navGameId);
                if (gameDAL != null)
                {
                    return RedirectToAction("ListGames", new { schemaGroupId = gameDAL.SchemaGroupId });
                }
            }
            else if (navTeamId > 0)
            {
                var teamDAL = new TeamCrud(_db).Get(navTeamId);
                if (teamDAL != null)
                {
                    return RedirectToAction("ListTeams", new { clubId = teamDAL.ClubId });
                }
            }
            else if (navNewsId > 0)
            {
                return RedirectToAction("ListNews");
            }

            return RedirectToAction("ListVideos");
        }


        public IActionResult DeleteVideo(int videoId)
        {
            var videoDAL = new VideosCrud(_db).Get(videoId);
            if (videoDAL != null)
            {
                new VideosCrud(_db).Delete(videoDAL);
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

                    if (editVideo.SportTypeSL != null && editVideo.SportTypeSL.FirstOrDefault(s => s.Text == editVideo.Video.SportType.ToString()) != null)
                    {
                        editVideo.SportTypeSL.FirstOrDefault(s => s.Text == editVideo.Video.SportType.ToString()).Selected = true;
                    }

                    editVideo.CategorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.CategoryId.ToString());
                    editVideo.TeamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly : true), "Value", "Text", editVideo.Video.TeamId.ToString());
                    editVideo.GameSl = new SelectList(new GamesCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.GameId.ToString());
                    editVideo.NewsSL = new SelectList(new NewsCrud(_db).GetSelectItemList(), "Value", "Text", editVideo.Video.NewsId.ToString());
                }
            }

            return View(editVideo);
        }

        [HttpPost]
        public IActionResult UpdateVideo(EditVideoVM editVideoVM)
        {
            if (ModelState.IsValid && editVideoVM?.Video != null)
            {
                var videoDAL = new VideoToView().ConvertBack(editVideoVM.Video);
                new VideosCrud(_db).Update(videoDAL);
            }

            return RedirectToAction("ListVideos");
        }
        #endregion

        #region Albums
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

            return View(albumsVL);
        }

        public IActionResult AddAlbum(int categoryId = 0, int newsId = 0, int teamId = 0, int gameId = 0, SportType sportType = SportType.NotDefined)
        {
            var albumVL = new AlbumToView().CreateEmpty();


            var categorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text");
            categorySL.SetSelected(categoryId.ToString());
            var newsSL = new SelectList(new NewsCrud(_db).GetSelectItemList(), "Value", "Text");
            newsSL.SetSelected(newsId.ToString());
            var teamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly: true), "Value", "Text");
            teamSL.SetSelected(teamId.ToString());
            var gameSL = new SelectList(new GamesCrud(_db).GetSelectItemList(gameId: gameId == 0 ? null : gameId), "Value", "Text");
            gameSL.SetSelected(gameId.ToString());
            var sportSL = Enums.SportType.NotDefined.ToSelectList();
            sportSL.SetSelected(Convert.ToInt32(sportType).ToString());

            ViewBag.CategorySL = categorySL;
            ViewBag.NewsSL = newsSL;
            ViewBag.TeamSL = teamSL;
            ViewBag.GameSL = gameSL;
            ViewBag.SportSL = sportSL;

            ViewBag.newsId = newsId;
            ViewBag.gameId = gameId;
            ViewBag.teamId = teamId;

            return View(albumVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAlbum(AlbumVM albumVL, int navNewsId = 0, int navTeamId = 0, int navGameId = 0)
        {
            if (ModelState.IsValid)
            {
                var albumDL = new AlbumToView().ConvertBack(albumVL);
                new AlbumsCrud(_db).Add(albumDL);
                return RedirectToAction("AddPhoto", new { albumId = albumDL.Id, navNewsId = navNewsId, navTeamId = navTeamId, navGameId = navGameId });
            }

            return RedirectToAction("ListAlbums");
        }

        public IActionResult DeleteAlbum(int albumId)
        {
            var albumDAL = new AlbumsCrud(_db).GetWithTitlePhotos(albumId);
            //var ttt = albumDAL.Photos.Any(p => !p.NewsTitlePhotos.IsNullOrEmpty());
            //var ttt2 = albumDAL.Photos.First().NewsTitlePhotos;
            //var ttt3 = albumDAL.Photos.Where(p => !p.NewsTitlePhotos.IsNullOrEmpty());
            if (albumDAL != null && (albumDAL.Photos == null || !albumDAL.Photos.Any(p => !p.NewsTitlePhotos.IsNullOrEmpty())))
            {
                if (albumDAL.Photos != null)
                {
                    foreach (var photo in albumDAL.Photos)
                    {
                        FileTools.RemoveAlbumPhoto(photo);
                        //new PhotosCrud(_db).Delete(photo);
                    }
                }
                new AlbumsCrud(_db).Delete(albumDAL);
            }

            return RedirectToAction("ListAlbums");
        }

        public IActionResult ListPhotos(int albumId)
        {
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            var albumVL = new AlbumToView().Convert(albumDAL);

            return View(albumVL);
        }

        public IActionResult RemoveAlbumPhoto(int albumId, int id)
        {
            var photoDAL = new PhotosCrud(_db).Get(id);
            if (photoDAL != null && photoDAL.NewsTitlePhotos != null && photoDAL.NewsTitlePhotos.Count == 0 && photoDAL.AlbumId == albumId)
            {
                FileTools.RemoveAlbumPhoto(photoDAL);
                new PhotosCrud(_db).Delete(photoDAL);
            }
            return RedirectToAction("ListPhotos", new { albumId = albumId });
        }

        public IActionResult AddPhoto(int albumId, int navGameId = 0, int navTeamId = 0, int navNewsId = 0)
        {
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            if (albumDAL == null)
            {
                return NotFound();
            }

            var photoVL = new PhotoToView().CreateEmpty(albumId);
            var albumVL = new AlbumToView().Convert(albumDAL);

            ViewBag.album = albumVL;
            ViewBag.navGameId = navGameId;
            ViewBag.navTeamId = navTeamId;
            ViewBag.navNewsId = navNewsId;

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
                        if (await FileTools.ResizeAndSave(file, albumId, _rootPath, newfileName, ImageType.Photo))
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

        public IActionResult NavigationAddPhoto(int navGameId, int navTeamId, int navNewsId, int navAlbumId)
        {
            if(navGameId > 0)
            {
                var gameDAL = new GamesCrud(_db).Get(navGameId);
                if(gameDAL != null)
                {
                    return RedirectToAction("ListGames", new { schemaGroupId = gameDAL.SchemaGroupId });
                }
            }
            else if(navTeamId > 0)
            {
                var teamDAL = new TeamCrud(_db).Get(navTeamId);
                if (teamDAL != null)
                {
                    return RedirectToAction("ListTeams", new { clubId = teamDAL.ClubId });
                }
            }
            else if(navNewsId > 0)
            {
                return RedirectToAction("ListNews");
            }

            return RedirectToAction("ListAlbums");
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddPhoto(IFormCollection fc)
        //{

        //    if (!fc.Keys.Contains("fnames") || !fc.Keys.Contains("albumId"))
        //    {
        //        return RedirectToAction("ListAlbums");
        //    }
        //    int albumId;
        //    if (!Int32.TryParse(fc["albumId"].FirstOrDefault(), out albumId))
        //    {
        //        return RedirectToAction("ListAlbums");
        //    }
        //    if (albumId <= 0)
        //    {
        //        return RedirectToAction("ListAlbums");
        //    }
        //    if (fc["fnames"].IsNullOrEmpty())
        //    {
        //        return RedirectToAction("ListAlbums");
        //    }
        //    var fileList = (fc["fnames"].FirstOrDefault()).Split("\r\n").ToList();
        //    if (fileList.Count <= 0)
        //    {
        //        return RedirectToAction("ListAlbums");
        //    }

        //    foreach (var fileName in fileList)
        //    {
        //        if (!fileName.IsNullOrEmpty())
        //        {
        //            var photoDAL = new Photo();
        //            photoDAL.AlbumId = albumId;
        //            photoDAL.Name = albumId + "_" + fileName;
        //            photoDAL.Description = fileName;
        //            photoDAL.FnameOrig = fileName;
        //            photoDAL.FnameBig = fileName;
        //            photoDAL.FnameSmall = fileName;
        //            new PhotosCrud(_db).Add(photoDAL);
        //        }
        //    }

        //    return RedirectToAction("ListAlbums");
        //}

        public IActionResult EditAlbum(int albumId)
        {
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            var albumVL = new AlbumVM();
            if(albumDAL != null)
            {
                albumVL = new AlbumToView().Convert(albumDAL, doSubConvert : false);
                var categorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text", albumVL.CategoryId?.ToString());
                var teamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly : true), "Value", "Text", albumVL.TeamId?.ToString());
                var gameSL = new SelectList(new GamesCrud(_db).GetSelectItemList(), "Value", "Text", albumVL.GameId?.ToString());
                var newsSL = new SelectList(new NewsCrud(_db).GetSelectItemList(), "Value", "Text", albumVL.NewsId?.ToString());
                var sportTypeSL = Enums.SportType.NotDefined.ToSelectList();
                if(sportTypeSL.Any(st => st.Value == albumVL.SportType.ToString()))
                {
                    sportTypeSL.First(st => st.Value == albumVL.SportType.ToString()).Selected = true;
                }

                ViewBag.CategorySL = categorySL;
                ViewBag.TeamSL = teamSL;
                ViewBag.GameSL = gameSL;
                ViewBag.NewsSL = newsSL;
                ViewBag.SportTypeSL = sportTypeSL;
                return View(albumVL);
            }

            return RedirectToAction("ListAlbums");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAlbum(AlbumVM albumVL)
        {
            if(ModelState.IsValid)
            {
                var albumDAL = new AlbumToView().ConvertBack(albumVL);
                new AlbumsCrud(_db).Update(albumDAL);
            }
         
            return RedirectToAction("ListAlbums");
        }
        #endregion

        #region News

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

        public IActionResult DeleteNewsAlbum(int albumId, int newsId)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            if(newsDAL != null && albumDAL != null && albumDAL.NewsId == newsId)
            {
                albumDAL.NewsId = null;
                new AlbumsCrud(_db).Update(albumDAL);
            }

            return RedirectToAction("ListAlbums", new { newsId = newsId });
        }

        public IActionResult DeleteTeamAlbum(int albumId, int teamId)
        {
            var teamDAL = new NewsCrud(_db).Get(teamId);
            var albumDAL = new AlbumsCrud(_db).Get(albumId);
            if (teamDAL != null && albumDAL != null && albumDAL.TeamId == teamId)
            {
                albumDAL.TeamId = null;
                new AlbumsCrud(_db).Update(albumDAL);
            }

            return RedirectToAction("ListAlbums", new { teamId = teamId });
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

            pageDataVM.EventSL = new SelectList(new EventsCrud(_db).GetSelectItemList(amount: 1000), "Value", "Text");
            pageDataVM.EventSL.SetSelected(pageDataVM.Event?.EventViewModelId.ToString() ?? "novalue");
            //if (pageDataVM.Event != null && pageDataVM.EventSL.First(i => i.Value == pageDataVM.Event.EventViewModelId.ToString()) != null)
            //{
            //    pageDataVM.EventSL.First(i => i.Value == pageDataVM.Event.EventViewModelId.ToString()).Selected = true;
            //}

            return View(pageDataVM);
        }

        public IActionResult AddNews(int categoryId = 0, int eventId = 0, int teamId = 0, SportType sportType = SportType.NotDefined)
        {
            var newsVL = new NewsToView().CreateEmpty();

            var eventSL = new SelectList(new EventsCrud(_db).GetSelectItemList(amount: Constants.HugeSelectListAmount), "Value", "Text");
            eventSL.SetSelected(eventId.ToString());
            var categorySL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text");
            categorySL.SetSelected(categoryId.ToString());
            var teamSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly: true), "Value", "Text");
            teamSL.SetSelected(teamId.ToString());
            var sportSL = Enums.SportType.NotDefined.ToSelectList();
            sportSL.SetSelected(Convert.ToInt32(sportType).ToString());

            ViewBag.CategorySL = categorySL;
            ViewBag.TeamSL = teamSL;
            ViewBag.SportSL = sportSL;
            ViewBag.EventSL = eventSL;

            ViewBag.eventId = eventId;
            ViewBag.teamId = teamId;

            return View(newsVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNews(NewsVM newsVL, int navTeamId = 0, int navEventId = 0)
        {
            if(ModelState.IsValid)
            {
                var newsDAL = new NewsToView().ConvertBack(newsVL);
				new NewsCrud(_db).Add(newsDAL);
            }

            if (navTeamId > 0)
            {
                var teamDAL = new TeamCrud(_db).Get(navTeamId);
                if (teamDAL != null)
                {
                    return RedirectToAction("ListTeams", new { clubId = teamDAL.ClubId });
                }
            }
            else if (navEventId > 0)
            {
                return RedirectToAction("ListEvents");
            }


            return RedirectToAction("ListNews");
        }

        public IActionResult  EditNews(int newsId)
        {
            var newsDAL = new NewsCrud(_db).Get(newsId);
            if(newsDAL != null)
            {
                var newsVL = new NewsToView().Convert(newsDAL);
                var categoriesSL = new SelectList(new CategoriesCrud(_db).GetSelectItemList(), "Value", "Text", newsVL.CategoryId);
                var teamsSL = new SelectList(new TeamCrud(_db).GetSelectItemList(uaOnly: true), "Value", "Text", newsVL.TeamId);
                var eventsSL = new SelectList(new EventsCrud(_db).GetSelectItemList(amount : Constants.HugeSelectListAmount), "Value", "Text", newsVL.EventId);
                var sportTypesSL = Enums.SportType.NotDefined.ToSelectList();
                if(sportTypesSL.Any(st => st.Text == newsVL.SportType.ToString()))
                {
                    sportTypesSL.First(st => st.Text == newsVL.SportType.ToString()).Selected = true;
                }

                ViewBag.categoriesSL = categoriesSL;
                ViewBag.teamsSL = teamsSL;
                ViewBag.eventsSL = eventsSL;
                ViewBag.sportTypesSL = sportTypesSL;

                return View(newsVL);
            }

            return RedirectToAction("ListNews");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNews(NewsVM newsVL)
        {
            if(ModelState.IsValid)
            {
                var newsDAL = new NewsToView().ConvertBack(newsVL);
                new NewsCrud(_db).Update(newsDAL);
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
            var newsDAL = new NewsCrud(_db).Get(newsId);
            var newsVL = new NewsToView().Convert(newsDAL);

            return View(newsVL);
        }

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
                    if (await FileTools.ResizeAndSave(file, Constants.TitleAlbumsId, _rootPath, newfileName, ImageType.Photo))
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
