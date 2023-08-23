using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BaseballUa.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICrud<Category> _categoryCrud;
        private readonly ICrud<Tournament> _tournamentCrud;

        public AdminController(ICrud<Category> categoryCrud, ICrud<Tournament> tournamentCrud)
        {
            _categoryCrud = categoryCrud;
            _tournamentCrud = tournamentCrud;
        }
        public IActionResult Index()
        {
            return View();
        }
#region Category


        public IActionResult ListCategories() 
        {
            var allCategories = _categoryCrud.GetAll().Select(a => CategoryToView.Convert(a)).ToList();
            return View(allCategories);
        }

        public IActionResult CreateCategory() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(CategoryViewModel category)
        {
            if (ModelState.IsValid) 
            {
                Category categoryDAL = new Category();
                categoryDAL.Name = category.Name;
                categoryDAL.ShortName = category.ShortName;
                _categoryCrud.Add(categoryDAL);
            }
            
            return RedirectToAction("ListCategories");
        }

        public IActionResult EditCategory(int id)
        {
            var categoryItem = _categoryCrud.Get(id).Convert();
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
                _categoryCrud.Update(categoryDAL);
            }

            return RedirectToAction("ListCategories");
        }
#endregion
        public IActionResult ListTournaments()
        {
        //    // List<Tournament> tournamentsDAL = new TournamentCrud().GetAll();
        //    // List<TournamentViewModel> tournamentsView = new TournamentToView().ConvertList(tournamentsDAL);
        //    //return View(tournamentsView);
            var allTournaments = _tournamentCrud.GetAll().Select(a => TournamentToView.Convert(a)).ToList();
            return View(allTournaments);
        }
    }
}
