using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Rules;
using System.Collections.Generic;
using System.Drawing.Text;

namespace BaseballUa.DTO
{
    public class TournamentToView
    {
        public TournamentViewModel Convert(Tournament tournament, BaseballUaDbContext _db)
        {
            TournamentViewModel tournamentViewModel = new TournamentViewModel();

            List<SelectListItem> categories = _db.Categories.Select(
                                    a => new SelectListItem
                                    {
                                        Value = a.Id.ToString(),
                                        Text = a.Name
                                    }).ToList();
            var categoryDefault = new SelectListItem()
            {
                Value = null,
                Text = "--- select category ---"
            };
            categories.Insert(0, categoryDefault);

            tournamentViewModel.Id = tournament.Id;
            tournamentViewModel.Name = tournament.Name;
            tournamentViewModel.Description = tournament.Description;
            tournamentViewModel.Sport = tournament.Sport;
            tournamentViewModel.IsAnual = tournament.IsAnual;
            tournamentViewModel.IsInternational = tournament.IsInternational;
            tournamentViewModel.IsOfficial = tournament.IsOfficial;
            tournamentViewModel.IsFun = tournament.IsFun;
            tournamentViewModel.CategoryId = _db.Categories.First(a => a.Id == tournament.CategoryId).Id;
            tournamentViewModel.CategoryShortName = _db.Categories.First(a => a.Id == tournament.CategoryId).ShortName;

            //tournamentViewModel.CategoryIdText = _db.Categories.First(a => a.Id == tournament.CategoryId).Id.ToString();
            //tournamentViewModel.CategoriesNames = new SelectList(categories, "Value", "Text");
            tournamentViewModel.CategoriesNames = categories;

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

        public TournamentViewModel GetEmpty(BaseballUaDbContext _db)
        {
            var tournamentViewModel = new TournamentViewModel();

            List<SelectListItem> categories = _db.Categories.Select(
                        a => new SelectListItem
                        {
                            Value = a.Id.ToString(),
                            Text = a.Name
                        }).ToList();
            tournamentViewModel.CategoriesNames = categories;

            return tournamentViewModel;
        }

        public Tournament ConvertBack(TournamentViewModel tournamentView)
        {
            var tournamentDAL = new Tournament();
            tournamentDAL.Id = tournamentView.Id;
            tournamentDAL.Name = tournamentView.Name;
            tournamentDAL.Description = tournamentView.Description;
            tournamentDAL.Sport = tournamentView.Sport;
            tournamentDAL.IsOfficial = tournamentView.IsOfficial;
            tournamentDAL.IsInternational = tournamentView.IsInternational;
            tournamentDAL.IsAnual = tournamentView.IsAnual;
            tournamentDAL.IsFun = tournamentView.IsFun;
            tournamentDAL.CategoryId = tournamentView.CategoryId;

            return tournamentDAL;
        }
    }
}
