using BaseballUa.Models.Custom;
using BaseballUa.ViewModels.Custom;

namespace BaseballUa.DTO.Custom
{
    public class GameWithTeamsToView
    {

        public GameWithTeamsViewModel Convert(GameWithTeams gameWithTeamsDAL)
        {
            var gameWithTeamsVL = new GameWithTeamsViewModel();
            gameWithTeamsVL.Game = new GameToView().Convert(gameWithTeamsDAL.Game);
            gameWithTeamsVL.HomeTeam = new TeamToView().Convert(gameWithTeamsDAL.HomeTeam);
            gameWithTeamsVL.VisitorTeam = new TeamToView().Convert(gameWithTeamsDAL.VisitorTeam);
            
            return gameWithTeamsVL;
        }

        public List<GameWithTeamsViewModel> ConvertAll(List<GameWithTeams> gamesWithTeamsDAL)
        {
            var gamesWithTeamsVL = new List<GameWithTeamsViewModel>();
            foreach (var gameWithTeamDAL in gamesWithTeamsDAL)
            {
                gamesWithTeamsVL.Add(Convert(gameWithTeamDAL));
            }

            return gamesWithTeamsVL;
        }
    }
}
