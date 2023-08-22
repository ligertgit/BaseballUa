using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;

namespace BaseballUa.DTO
{
    public static class CategoryToView
    {

        public static CategoryViewModel Convert(this Category category)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Id = category.Id;
            categoryViewModel.Name = category.Name;
            categoryViewModel.ShortName = category.ShortName;

            return categoryViewModel;
        }
    }
}
