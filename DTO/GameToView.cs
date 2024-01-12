using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.DTO
{
    public class GameToView
    {
        public GameViewModel Convert(Game gameDAL, bool doSubConvert = true)
        {
            var gameView = new GameViewModel();
            if (gameDAL != null)
            {
                gameView.Name = gameDAL.Name;
                gameView.GameViewModelId = gameDAL.Id;
                gameView.StartDate = gameDAL.StartDate;
                gameView.AdditionalInfo = gameDAL.AdditionalInfo;
                gameView.RunsVisitor = gameDAL.RunsVisitor;
                gameView.RunsHome = gameDAL.RunsHome;
                gameView.PlacedAt = gameDAL.PlacedAt;
                gameView.HalfinningsPlayed = gameDAL.HalfinningsPlayed;
                gameView.GameStatus = gameDAL.GameStatus;
                gameView.PointsVisitor = gameDAL.PointsVisitor;
                gameView.PointsHome = gameDAL.PointsHome;
                gameView.Tour = gameDAL.Tour;
                gameView.ConditionVisitor = gameDAL.ConditionVisitor;
                gameView.ConditionHome = gameDAL.ConditionHome;
                gameView.SchemaGroupId = gameDAL.SchemaGroupId;
                if (gameDAL.SchemaGroup != null)
                {
                    gameView.SchemaGroup = new SchemaGroupToView().Convert(gameDAL.SchemaGroup, false);
                }
                gameView.HomeTeamId = gameDAL.HomeTeamId;
                gameView.VisitorTeamId = gameDAL.VisitorTeamId;
                if (gameDAL.HomeTeam != null)
                {
                    gameView.HomeTeam = new TeamToView().Convert(gameDAL.HomeTeam, false);
                }
                if (gameDAL.VisitorTeam != null)
                {
                    gameView.VisitorTeam = new TeamToView().Convert(gameDAL.VisitorTeam, false);
                }
                if (doSubConvert && gameDAL.Albums != null)
                {
                    gameView.Albums = new AlbumToView().ConvertAll(gameDAL.Albums.ToList(), false);
                }
                if (doSubConvert && gameDAL.Videos != null)
                {
                    gameView.Videos = new VideoToView().ConvertAll(gameDAL.Videos.ToList(), false);
                }
            }

            return gameView;
        }

        public Game ConvertBack(GameViewModel gameVL)
        {
            var gameDAL = new Game();
            gameDAL.Id = gameVL.GameViewModelId;
            gameDAL.Name = gameVL.Name;
            gameDAL.StartDate = gameVL.StartDate;
            gameDAL.AdditionalInfo = gameVL.AdditionalInfo;
            gameDAL.RunsVisitor = gameVL.RunsVisitor;
            gameDAL.RunsHome = gameVL.RunsHome;
            gameDAL.PlacedAt = gameVL.PlacedAt;
            gameDAL.HalfinningsPlayed = gameVL.HalfinningsPlayed;
            gameDAL.GameStatus = gameVL.GameStatus;
            gameDAL.PointsVisitor = gameVL.PointsVisitor;
            gameDAL.PointsHome = gameVL.PointsHome;
//!!!            gameDAL.EventSchemaItemId = gameVL.EventSchemaItemId;
            gameDAL.Tour = gameVL.Tour;
            gameDAL.ConditionVisitor = gameVL.ConditionVisitor;
            gameDAL.ConditionHome = gameVL.ConditionHome;
            gameDAL.SchemaGroupId = gameVL.SchemaGroupId;
            gameDAL.HomeTeamId = gameVL.HomeTeamId;
            gameDAL.VisitorTeamId = gameVL.VisitorTeamId;
            //gameDAL.EventId = gameVL.EventId;



            return gameDAL;
        }

        public GameViewModel CreateEmpty(int schemaGroupId)
        {
            var gameVL = new GameViewModel();
            gameVL.SchemaGroupId = schemaGroupId;
            //gameVL.EventSchemaItemItem = new EventSchemaItemsCrud(_dbContext).Get(eventSchemaItemId).SchemaItem;

            return gameVL;
        }

        public List<GameViewModel> ConvertAll(List<Game> gamesDAL, bool doSubConvert = true)
        { 
            var gamesVL = new List<GameViewModel>();
            foreach (var game in gamesDAL) 
            { 
                gamesVL.Add(Convert(game, doSubConvert));
            }

            return gamesVL;
        }

    }
}
