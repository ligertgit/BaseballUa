using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;

namespace BaseballUa.DTO
{
    public static class CategoryToView
    {

        public static CategoryViewModel Convert(this Category category, BaseballUaDbContext dbcontext)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            var test = new CategoriesCrud(dbcontext).GetAll();
            categoryViewModel.Id = category.Id;
            categoryViewModel.Name = category.Name;
            categoryViewModel.ShortName = category.ShortName;

            return categoryViewModel;
        }

        public static List<CategoryViewModel> ConvertList(this List<Category> categoriesDTO, BaseballUaDbContext dbcontext)
        {
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            var test = new CategoriesCrud(dbcontext).GetAll();
            foreach (var categoryDTO in categoriesDTO)
            {
                categoryViewModels.Add(Convert(categoryDTO, dbcontext));
            }

            return categoryViewModels;
        }
    }
}
