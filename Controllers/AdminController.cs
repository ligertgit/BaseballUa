using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

#region Games


        // need refactor with FK key to schema only
        public IActionResult AddGame(int eventSchemaItemId)
        {
            var gameView = new GameToView(_db).CreateEmpty(eventSchemaItemId);
            return View(gameView);
        }


        // need refactor with FK key to schema only
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGame(GameViewModel gameVL)
        {
            if (ModelState.IsValid)
            {
                var gameDAL = new GameToView(_db).ConvertBack(gameVL);
                new GamesCrud(_db).Add(gameDAL);
            }
            
            return RedirectToAction("ListEvents");
        }

        public IActionResult ListGames(int eventSchemaItemId)
        {
            var gamesDAL = new GamesCrud(_db).GetForEventSchema(eventSchemaItemId);
            var gamesVL = new GameToView(_db).ConvertAll(gamesDAL);

            ViewData["eventSchemaItemId"] = eventSchemaItemId;

            return View(gamesVL);
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


    }
}
