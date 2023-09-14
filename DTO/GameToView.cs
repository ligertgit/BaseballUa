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
        //private readonly BaseballUaDbContext _dbContext;

        //public GameToView(BaseballUaDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public GameViewModel Convert(Game gameDAL)
        {
            var gameView = new GameViewModel();
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
            //gameView.GameType = gameDAL.GameType;
//!            gameView.EventSchemaItemId = gameDAL.EventSchemaItemId;
//!            gameView.EventSchemaItemItem = new EventSchemaItemsCrud(_dbContext).Get(gameDAL.EventSchemaItemId).SchemaItem;
            //gameView.EventSchemaItem = new EventSchemaItemsCrud(_dbContext).Get(gameDAL.EventSchemaItemId).SchemaItem;
            //gameView.EventSchemaItems = new EventSchemaItemsCrud(_dbContext).GetEventSchemaItems(gameDAL.EventId);
            //gameView.EventSchemaItems = new EventSchemaItemsCrud(_dbContext).GetEventSchemaItems(gameDAL.EventId).
            //                                    Select(i => new SelectListItem 
            //                                                    { 
            //                                                        Text = i.SchemaItem.ToString(), 
            //                                                        Value = i.Id.ToString()
            //                                                    }
            //                                          ).ToList();
            gameView.Tour = gameDAL.Tour;
            gameView.ConditionVisitor = gameDAL.ConditionVisitor;
            gameView.ConditionHome = gameDAL.ConditionHome;
            gameView.SchemaGroupId = gameDAL.SchemaGroupId;
            gameView.HomeTeamId = gameDAL.HomeTeamId;
            gameView.VisitorTeamId = gameDAL.VisitorTeamId;
            //gameView.EventId = gameDAL.EventId;
            //gameView.Event = new EventsCrud(_dbContext).Get(gameDAL.EventId);
            //gameView.Tournament = new TournamentsCrud(_dbContext).Get(gameView.Event.TournamentId);

            return gameView;
        }

        public Game ConvertBack(GameViewModel gameVL)
        {
            var gameDAL = new Game();
            //gameView.GameViewModelId = gameDAL.Id;
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

        public List<GameViewModel> ConvertAll(List<Game> gamesDAL)
        { 
            var gamesVL = new List<GameViewModel>();
            foreach (var game in gamesDAL) 
            { 
                gamesVL.Add(Convert(game));
            }

            return gamesVL;
        }

        //public GameWithTeamsViewModel ConvertWithTeams(GameWithTeams)
        //public List<GameWithTeamsViewModel> ConvertAllWithTeams(List<GameWithTeams> gamesDAL)
        //{
        //    var gamesVL = new List<GameWithTeams>();
        //    foreach (var game in gamesDAL)
        //    {
        //        gamesVL.Add(ConvertWithTeams(game));
        //    }

        //    return gamesVL;
        //}
    }
}
