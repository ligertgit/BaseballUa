using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.DTO
{
    public class TeamToView
    {
        //private readonly BaseballUaDbContext _dbContext;

        //public TeamToView(BaseballUaDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public TeamViewModel? Convert(Team teamDAL, bool doSubConvert = true)
        {
            
            if (teamDAL != null) 
            {
                var teamVL = new TeamViewModel();
                teamVL.Id = teamDAL.Id;
                teamVL.Name = teamDAL.Name;
                teamVL.Description = teamDAL.Description;
                teamVL.SportType = teamDAL.SportType;
                teamVL.FnameLogoSmall = teamDAL.FnameLogoSmall;
                teamVL.FnameLogoBig = teamDAL.FnameLogoBig;
                teamVL.IsTemp = teamDAL.IsTemp;
                teamVL.ClubId = teamDAL.ClubId;
                if (teamDAL.Club != null && doSubConvert)
                {
                    teamVL.Club = new ClubToView().Convert(teamDAL.Club, false);
                }
                //var ttt = new GameToView().ConvertAll(teamDAL.HomeGames.ToList());
                teamVL.Games = new List<GameViewModel>();
                if (teamDAL.HomeGames != null && doSubConvert) teamVL.Games.AddRange(new GameToView().ConvertAll(teamDAL.HomeGames.ToList(), false));
                if (teamDAL.VisitorGames != null && doSubConvert) teamVL.Games.AddRange(new GameToView().ConvertAll(teamDAL.VisitorGames.ToList(), false));
                if (teamDAL.Players != null && doSubConvert) teamVL.Players = new PlayerToView().ConvertAll(teamDAL.Players.ToList(), false);
                return teamVL;
            } 
            else
            {
                return null;
            }
        }

        public List<TeamViewModel> ConvertAll(List<Team> teamsDAL, bool doSubConvert = true)
        {
            var teamVL = new List<TeamViewModel>();
            foreach (var teamDAL in teamsDAL)
            {
                teamVL.Add(Convert(teamDAL, doSubConvert));
            }

            return teamVL;
        }

        public Team ConvertBack(TeamViewModel teamVL)
        {
            var teamDAL = new Team();
            teamDAL.Id = teamVL.Id;
            teamDAL.Name = teamVL.Name;
            teamDAL.Description = teamVL.Description;
            teamDAL.SportType = teamVL.SportType;
            teamDAL.ClubId = teamVL.ClubId;
            teamDAL.FnameLogoSmall = teamVL.FnameLogoSmall;
            teamDAL.FnameLogoBig = teamVL.FnameLogoBig;
            teamDAL.IsTemp = teamVL.IsTemp;


            return teamDAL;
        }

        public TeamViewModel CreateEmpty()
        {
            return new TeamViewModel();
        }

        public List<SelectListItem> GetFullSelestList(List<Team> teamsWithClubCountry)
        {
            var selectListItems = (from teams in teamsWithClubCountry
                                   select new SelectListItem
                                   {
                                        Text = $"{teams.Name} | {teams.Club.Name} | {teams.Club.Country.Name}",
                                        Value = teams.Id.ToString()
                                   }
                                   );

            return selectListItems.ToList();
        }

        //public List<SelectListItem> GetFullSelestList(List<TeamWithClubCountry> teamsWithClubCountry)
        //{
        //    var selectListItems = (from teams in teamsWithClubCountry
        //                           select new SelectListItem
        //                           {
        //                               Text = $"{teams.Team.Name} | {teams.Club.Name} | {teams.Country.Name}",
        //                               Value = teams.Team.Id.ToString()
        //                           }
        //                           );

        //    return selectListItems.ToList();
        //}
    }
}
