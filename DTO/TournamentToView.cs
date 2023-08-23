using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Text;

namespace BaseballUa.DTO
{
    public static class TournamentToView
    {
        public static TournamentViewModel Convert(this Tournament tournament)
        {
            
            TournamentViewModel tournamentViewModel = new TournamentViewModel();
        


            tournamentViewModel.Name = tournament.Name;
            tournamentViewModel.Description = tournament.Description;
            tournamentViewModel.Sport = tournament.Sport;
            //tournamentViewModel.SelectedCategory = categoryCrud.Get(tournament.CategoryId).ShortName;
            //tournamentViewModel.SelectedCategory = new CategoriesCrud().Get(tournament.CategoryId).ShortName;
            //tournamentViewModel.Categories = 
            tournamentViewModel.SelectedCategory = "asdf";
            tournamentViewModel.IsAnual = tournament.IsAnual;
            tournamentViewModel.IsInternational = tournament.IsInternational;
            tournamentViewModel.IsOfficial = tournament.IsOfficial;


            return tournamentViewModel;
        }
    }
}
