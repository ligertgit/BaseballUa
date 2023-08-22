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

        public AdminController(ICrud<Category> categoryCrud)
        {
            _categoryCrud = categoryCrud;
        }
        public IActionResult Index()
        {
            return View();
        }

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
    }
}
