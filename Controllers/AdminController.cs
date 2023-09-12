using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Drawing;

namespace BaseballUa.Controllers
{
    public class AdminController : Controller
    {
        private readonly BaseballUaDbContext _db;

        public AdminController(BaseballUaDbContext dbcontext)
        {
            _db = dbcontext;
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
            var allCategoriesView = new CategoryToView(_db).ConvertList(allCategoriesDTO);
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
                Category categoryDAL = new CategoryToView(_db).ConvertBack(categoryView);
                //categoryDAL.Name = categoryView.Name;
                //categoryDAL.ShortName = categoryView.ShortName;
                new CategoriesCrud(_db).Add(categoryDAL);
            }
            
            return RedirectToAction("ListCategories");
        }

        public IActionResult EditCategory(int id)
        {
            var categoryDTO = new CategoriesCrud(_db).Get(id);
            var categoryView = new CategoryToView(_db).Convert(categoryDTO);
            return View(categoryView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(CategoryViewModel category)
        {
            if (ModelState.IsValid) 
            {
                Category categoryDAL = new CategoryToView(_db).ConvertBack(category);
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
            var tournamentsView = new TournamentToView().ConvertList(tournamentsDTO, _db);
            return View(tournamentsView);
        }

        public IActionResult CreateTournament()
        {
            //var tournamentDTO = new TournamentsCrud(_db).Get(1);
            //var tournamentDTO = new TournamentsCrud(_db).GetEmpty();
            var tournamentView = new TournamentToView().GetEmpty(_db);
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
            var tournamentDAL = new TournamentsCrud(_db).Get(id);//.Convert(_db);
            var tournamentView = new TournamentToView().Convert(tournamentDAL, _db);
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
            var eventsDAL = new EventsCrud(_db).GetAll().ToList();
            var eventsView = new EventToView().ConvertAll(eventsDAL, _db);
            return View(eventsView); 
        }

        public IActionResult CreateEvent() 
        {
            var eventsView = new EventToView().CreateEmpty(_db);
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
            var eventView = new EventToView().Convert(eventDAL, _db);

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
            var eventView = new EventToView().Convert(eventDAL, _db);

            return View(eventView);
        }
        #endregion

#region EventSchema

        public IActionResult AddSchemaItem(int eventId)
        {
            var eventSchemaItemVL = new EventSchemaItemToView(_db).CreateEmpty(eventId);
            return View(eventSchemaItemVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchemaItem(EventSchemaItemViewModel eventSchemaItemVL)
        {
            if (ModelState.IsValid)
            { 
                var eventSchemaItemDAL = new EventSchemaItemToView(_db).ConvertBack(eventSchemaItemVL);
                new EventSchemaItemsCrud(_db).Add(eventSchemaItemDAL);

            }
            return RedirectToAction("ListEvents");
        }

        public IActionResult ListSchemaItems(int eventId) 
        { 
            var eventSchemaItemsDAL = new EventSchemaItemsCrud(_db).GetEventSchemaItems(eventId);
            var eventSchemaItemsVL = new EventSchemaItemToView(_db).ConvertAll(eventSchemaItemsDAL);

            var eventt = new EventsCrud(_db).Get(eventId);
            ViewData["eventId"] = eventId;
            ViewData["tournamentName"] = new TournamentsCrud(_db).Get(eventt.TournamentId).Name;

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
            var groupsDAL = new SchemaGroupCrud(_db).GetAllForSchema(EventSchemaItemId).ToList();
            var groupsVL = new SchemaGroupToView(_db).ConvertAll(groupsDAL);

            var eventSchemaItemDAL = new EventSchemaItemsCrud(_db).Get(EventSchemaItemId);
            var eventSchemaItemVL = new EventSchemaItemToView(_db).Convert(eventSchemaItemDAL);

            ViewBag.EventSchemaItem = eventSchemaItemVL;

            return View(groupsVL);
        }

        public IActionResult AddSchemaGroup(int eventSchemaItemId)
        {
            var groupVL = new SchemaGroupToView(_db).CreateEmpty(eventSchemaItemId);

            return View(groupVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSchemaGroup(SchemaGroupViewModel schemaGroupVL)
        {
            if (ModelState.IsValid)
            {
                var schemaGroupDAL = new SchemaGroupToView(_db).ConvertBack(schemaGroupVL);
                new SchemaGroupCrud(_db).Add(schemaGroupDAL);
            }

            return RedirectToAction("ListSchemaGroups", new { EventSchemaItemId = schemaGroupVL.EventSchemaItemId });
        }

        #endregion

#region Games

        public IActionResult ListGames(int schemaGroupId)
        {
            var gamesDAL = new GamesCrud(_db).GetAllForGroup(schemaGroupId).ToList();
            var gamesVL = new GameToView(_db).ConvertAll(gamesDAL);

            ViewData["schemaGroupId"] = schemaGroupId;

            return View(gamesVL);
        }

        public IActionResult AddGame(int schemaGroupId)
        {
            var gameView = new GameToView(_db).CreateEmpty(schemaGroupId);
            return View(gameView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGame(GameViewModel gameVL)
        {
            if (ModelState.IsValid)
            {
                var gameDAL = new GameToView(_db).ConvertBack(gameVL);
                new GamesCrud(_db).Add(gameDAL);
            }

            return RedirectToAction("ListGames", new { schemaGroupId = gameVL.SchemaGroupId});
        }


        #endregion

#region Country

        public IActionResult ListCountries()
        {
            var countriesDAL = new CountryCrud(_db).GetAll().ToList();
            var countriesVL = new CountryToView(_db).ConvertAll(countriesDAL);

            return View(countriesVL);
        }

        public IActionResult AddCountry()
        {
            var CountryVL = new CountryToView(_db).CreateEmpty();

            return View(CountryVL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCountry(CountryViewModel countryVL)
        {
            if (ModelState.IsValid)
            {
                var countryDAL = new CountryToView(_db).ConvertBack(countryVL);
                new CountryCrud(_db).Add(countryDAL);
            }

            return RedirectToAction("ListCountries");
        }

#endregion

        #region Club
        public IActionResult ListClubs()
        {
            var clubsDAL = new ClubCrud(_db).GetAll().ToList();
            var clubsVL = new ClubToView(_db).ConvertAll(clubsDAL);

            //ViewBag.CountriesSL = new CountryCrud(_db).GetSelectItemList();

            return View(clubsVL);
        }

        public IActionResult AddClub()
        {
            var clubVl = new ClubToView(_db).CreateEmpty();

            ViewBag.CountriesSL = new CountryCrud(_db).GetSelectItemList();

            return View(clubVl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddClub(ClubViewModel clubVl)
        {
            if (ModelState.IsValid)
            {
                var clubDAL = new ClubToView(_db).ConvertBack(clubVl);
                new ClubCrud(_db).Add(clubDAL);
            }

            return RedirectToAction("ListClubs");
        }



        #endregion

        #region Team
        #endregion

    }
}
