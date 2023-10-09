using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BaseballUa.DTO
{
    public class CategoryToView
    {
        public CategoryViewModel Convert(Category category)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Id = category.Id;
            categoryViewModel.Name = category.Name;
            categoryViewModel.ShortName = category.ShortName;

            if (category.Tournaments != null) 
            { 
                categoryViewModel.Tournaments = new TournamentToView().ConvertList(category.Tournaments.ToList());
                categoryViewModel.SelectTournaments = categoryViewModel.Tournaments.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name });
            }
            return categoryViewModel;
        }

        public List<CategoryViewModel> ConvertList(List<Category> categoriesDTO)
        {
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            foreach (var categoryDTO in categoriesDTO)
            {
                categoryViewModels.Add(Convert(categoryDTO));
            }

            return categoryViewModels;
        }

        public Category ConvertBack(CategoryViewModel category) 
        { 
            var categoryDAL = new Category();
            categoryDAL.Id = category.Id;
            categoryDAL.Name = category.Name;
            categoryDAL.ShortName = category.ShortName;


            return categoryDAL;
        }

        public List<SelectListItem> GetSelect(List<Category> categoriesListDAL)
        {
            return categoriesListDAL.Select(c => new SelectListItem { Value = c.Id.ToString(), Text =  c.Name }).ToList();
        }
    }
}
