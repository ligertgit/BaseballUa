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

        public TeamViewModel? Convert(Team teamDAL)
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
                teamVL.Club = teamDAL.Club;
                teamVL.Games = new List<Game>();
                if ( teamDAL.HomeGames != null ) teamVL.Games.AddRange(teamDAL.HomeGames);
                if ( teamDAL.VisitorGames != null ) teamVL.Games.AddRange(teamDAL.VisitorGames);
                //teamVL.Games = teamDAL.HomeGames.Concat(teamDAL.VisitorGames).ToList();
                return teamVL;
            } 
            else
            {
                return null;
            }
        }

        public List<TeamViewModel> ConvertAll(List<Team> teamsDAL)
        {
            var teamVL = new List<TeamViewModel>();
            foreach (var teamDAL in teamsDAL)
            {
                teamVL.Add(Convert(teamDAL));
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

        public List<SelectListItem> GetFullSelestList(List<TeamWithClubCountry> teamsWithClubCountry)
        {
            var selectListItems = (from teams in teamsWithClubCountry
                                   select new SelectListItem
                                   {
                                        Text = $"{teams.Team.Name} | {teams.Club.Name} | {teams.Country.Name}",
                                        Value = teams.Team.Id.ToString()
                                   }
                                   );

            return selectListItems.ToList();
        }
    }
}
