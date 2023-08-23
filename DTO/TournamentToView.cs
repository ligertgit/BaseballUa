using BaseballUa.BlData;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            tournamentViewModel.SelectedCategory = "test";
            //tournamentViewModel.SelectedCategory = tournament.Category.ShortName;
            //tournamentViewModel.Categories = 
            tournamentViewModel.IsAnual = tournament.IsAnual;
            tournamentViewModel.IsInternational = tournament.IsInternational;
            tournamentViewModel.IsOfficial = tournament.IsOfficial;


            return tournamentViewModel;
        }
    }
}
