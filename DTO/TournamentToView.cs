using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Text;

namespace BaseballUa.DTO
{
    public class TournamentToView
    {
        public TournamentViewModel Convert(Tournament tournament, BaseballUaDbContext _db)
        {
            TournamentViewModel tournamentViewModel = new TournamentViewModel();

            tournamentViewModel.Name = tournament.Name;
            tournamentViewModel.Description = tournament.Description;
            tournamentViewModel.Sport = tournament.Sport;
            tournamentViewModel.SelectedCategory = _db.Categories.First(a => a.Id == tournament.CategoryId).ShortName;
            tournamentViewModel.IsAnual = tournament.IsAnual;
            tournamentViewModel.IsInternational = tournament.IsInternational;
            tournamentViewModel.IsOfficial = tournament.IsOfficial;

            return tournamentViewModel;
        }

        public List<TournamentViewModel> ConvertList(List<Tournament> tournaments, BaseballUaDbContext _db)
        {
            var tournamentList = new List<TournamentViewModel>();
            foreach (var tournament in tournaments)
            {
                tournamentList.Add(Convert(tournament, _db));
            }

            return tournamentList;
        }
    }
}
