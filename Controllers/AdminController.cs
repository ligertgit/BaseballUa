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
            var allCategoriesView = new CategoriesCrud(_db).GetAll().Select(a => CategoryToView.Convert(a, _db)).ToList();
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
                Category categoryDAL = new Category();
                categoryDAL.Name = categoryView.Name;
                categoryDAL.ShortName = categoryView.ShortName;
                new CategoriesCrud(_db).Add(categoryDAL);
            }
            
            return RedirectToAction("ListCategories");
        }

        public IActionResult EditCategory(int id)
        {
            var categoryItem = new CategoriesCrud(_db).Get(id).Convert(_db);
            return View(categoryItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(CategoryViewModel category)
        {
            if (ModelState.IsValid) 
            {
                Category categoryDAL = new Category();
                categoryDAL.Id = category.Id;
                categoryDAL.Name = category.Name;
                categoryDAL.ShortName = category.ShortName;
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
    }
}
