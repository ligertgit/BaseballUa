using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;

namespace BaseballUa.DTO
{
    public class CategoryToView
    {
        private readonly BaseballUaDbContext _dbContext;

        public CategoryToView(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;        
        }
        public CategoryViewModel Convert(Category category)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Id = category.Id;
            categoryViewModel.Name = category.Name;
            categoryViewModel.ShortName = category.ShortName;

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
    }
}
