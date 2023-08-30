using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;

namespace BaseballUa.DTO
{
    public class GameToView
    {
        private readonly BaseballUaDbContext _dbContext;

        public GameToView(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GameViewModel Convert(Game gameDAL)
        {
            var gameView = new GameViewModel();
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
            gameView.GameType = gameDAL.GameType;
            gameView.Tour = gameDAL.Tour;
            gameView.ConditionVisitor = gameDAL.ConditionVisitor;
            gameView.ConditionHome = gameDAL.ConditionHome;
            gameView.EventId = gameDAL.EventId;
            //gameView.Event = _dbContext.Events.First(i => i.Id == gameDAL.EventId);
            gameView.Event = new EventsCrud(_dbContext).Get(gameDAL.EventId);
            //gameView.Tournament = _dbContext.Tournaments.First(i => i.Id == gameView.Event.TournamentId);
            gameView.Tournament = new TournamentsCrud(_dbContext).Get(gameView.Event.TournamentId);

            return gameView;
        }

        public Game ConvertBack(GameViewModel gameVL)
        {
            var gameDAL = new Game();
            //gameView.GameViewModelId = gameDAL.Id;
            gameDAL.StartDate = gameVL.StartDate;
            gameDAL.AdditionalInfo = gameVL.AdditionalInfo;
            gameDAL.RunsVisitor = gameVL.RunsVisitor;
            gameDAL.RunsHome = gameVL.RunsHome;
            gameDAL.PlacedAt = gameVL.PlacedAt;
            gameDAL.HalfinningsPlayed = gameVL.HalfinningsPlayed;
            gameDAL.GameStatus = gameVL.GameStatus;
            gameDAL.PointsVisitor = gameVL.PointsVisitor;
            gameDAL.PointsHome = gameVL.PointsHome;
            gameDAL.GameType = gameVL.GameType;
            gameDAL.Tour = gameVL.Tour;
            gameDAL.ConditionVisitor = gameVL.ConditionVisitor;
            gameDAL.ConditionHome = gameVL.ConditionHome;
            gameDAL.EventId = gameVL.EventId;

            return gameDAL;
        }

        public GameViewModel CreateEmpty(int eventId)
        {
            var gameVL = new GameViewModel();
            gameVL.Event = _dbContext.Events.First(i => i.Id == eventId);
            gameVL.Tournament = _dbContext.Tournaments.First(i => i.Id == gameVL.Event.TournamentId);

            return gameVL;
        }
    }
}
