using BaseballUa.Models;
using BaseballUa.ViewModels;

namespace BaseballUa.DTO
{
    public class PlayerToView
    {

        public PlayerViewModel Convert(Player playerDAL, bool doSubConvert = true)
        {
            var playerVL = new PlayerViewModel();
            playerVL.Id = playerDAL.Id;
            playerVL.FirstName = playerDAL.FirstName;
            playerVL.SecondName = playerDAL.SecondName;
            playerVL.Birthdate = playerDAL.Birthdate;
            playerVL.AvatarSmall = playerDAL.AvatarSmall;
            playerVL.AvatarBig = playerDAL.AvatarBig;
            playerVL.Sex = playerDAL.Sex;
            playerVL.TeamId = playerDAL.TeamId;
            if (playerDAL.Team != null) 
            { 
                playerVL.Team = new TeamToView().Convert(playerDAL.Team, false);
            }

            return playerVL;
        }

        public List<PlayerViewModel> ConvertAll(List<Player> playersDAL, bool doSubConvert = true)
        {
            var playersVL = new List<PlayerViewModel>();
            foreach (var player in playersDAL) 
            { 
                playersVL.Add(Convert(player, doSubConvert));
            }

            return playersVL;
        }

        public Player ConvertBack(PlayerViewModel playerVL)
        {
            var playerDAL = new Player();
            playerDAL.Id = playerVL.Id;
            playerDAL.FirstName = playerVL.FirstName;
            playerDAL.SecondName = playerVL.SecondName;
            playerDAL.Birthdate = playerVL.Birthdate;
            playerDAL.AvatarSmall = playerVL.AvatarSmall;
            playerDAL.AvatarBig = playerVL.AvatarBig;
            playerDAL.Sex = playerVL.Sex;
            playerDAL.TeamId = playerVL.TeamId;

            return playerDAL;
        }

        public PlayerViewModel CreateEmpty(int teamId)
        {
            var playerVL = new PlayerViewModel();
            playerVL.TeamId = teamId;

            return playerVL;
        }


    }
}
