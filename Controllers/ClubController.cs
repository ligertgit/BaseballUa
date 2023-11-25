using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.ViewModels.Custom;
using Microsoft.AspNetCore.Mvc;

namespace BaseballUa.Controllers
{
    public class ClubController : Controller
    {
        private readonly BaseballUaDbContext _db;

        public ClubController(BaseballUaDbContext dbContext)
        {
            _db = dbContext;
        }
        public IActionResult Index()
        {
            var ClubsDL = new ClubCrud(_db).GetAllWithTeams();
            var ClubsVL = new ClubToView().ConvertAll(ClubsDL.ToList());
            
            return View(ClubsVL);
        }

        public IActionResult DetailsTeam(int teamId)
        {
            var teamFullVL = new TeamFullDetailVM();
            
            if (teamId <= 0)
            {
                teamFullVL = null;
            }
            else
            {
                var teamDAL = new TeamCrud(_db).Get(teamId);
                if (teamDAL == null)
                {
                    teamFullVL = null;
                }
                else
                {
                    teamFullVL.Team = new TeamToView().Convert(teamDAL);
                    
                    var teamGamesDAL = new TeamCrud(_db).GetHomeGames(teamId).ToList();
                    teamGamesDAL.AddRange(new TeamCrud(_db).GetVisitorGames(teamId).ToList());
                    teamFullVL.Games = new GameToView().ConvertAll(teamGamesDAL);

                    var teamEventsDAL = new EventsCrud(_db).GetAllForTeam(teamId).ToList();
                    teamFullVL.Events = new EventToView().ConvertAll(teamEventsDAL);

                    var teamPlayersDAL = new PlayersCrud(_db).GetAll(teamId).ToList();
                    teamFullVL.Players = new PlayerToView().ConvertAll(teamPlayersDAL);

                    var teamAlbumsDAL = new AlbumsCrud(_db).GetAllTeamAlbums(teamId).ToList();
                    teamFullVL.Albums = new AlbumToView().ConvertAll(teamAlbumsDAL);

                    var teamVideosDAL = new VideosCrud(_db).GetAllTeamVideos(teamId).ToList();
                    teamFullVL.Videos = new VideoToView().ConvertAll(teamVideosDAL);

                    var teamNewsDAL = new NewsCrud(_db).GetAllTeamNews(teamId).ToList();
                    teamFullVL.News = new NewsToView().ConvertAll(teamNewsDAL);
                }
            }
            //var teamDAL = new TeamCrud(_db).Get(teamId);
            //var teamHomeGamesDAL = new TeamCrud(_db).GetHomeGames(teamId).ToList();
            //var teamVisitorGamesDAL = new TeamCrud(_db).GetVisitorGames(teamId).ToList();
            //var teamPlayersDAL = new PlayersCrud(_db).GetAll(teamId).ToList();
            
            //var teamVL = new TeamToView().Convert(teamDAL);
            //teamVL.Games = new GameToView().ConvertAll(teamHomeGamesDAL).ToList();
            //teamVL.Games.AddRange(new GameToView().ConvertAll(teamVisitorGamesDAL).ToList());
            //teamVL.Players = new PlayerToView().ConvertAll(teamPlayersDAL).ToList();

            return View(teamFullVL);
        }

        public IActionResult DetailsClub(int clubId)
        {
            var clubFullVL = new ClubFullDetailVM();

            var clubDAL = new ClubCrud(_db).Get(clubId);
            clubDAL.Staffs = new StaffsCrud(_db).GetAll(clubId).ToList();
            clubDAL.Teams = new TeamCrud(_db).GetAll(clubId).ToList();
            clubFullVL.Club = new ClubToView().Convert(clubDAL);

            var EventsDAL = new EventsCrud(_db).GetAllForClub(clubId).ToList();
            clubFullVL.Events = new EventToView().ConvertAll(EventsDAL).ToList();

            var clubVideos = new VideosCrud(_db).GetAllClubVideos(clubId, amount : Constants.DefaulVideosAmount).ToList();
            clubFullVL.Videos = new VideoToView().ConvertAll(clubVideos);

            var clubAlbums = new AlbumsCrud(_db).GetAllClubAlbums(clubId, amount: Constants.DefaulAlbumsAmount).ToList();
            clubFullVL.Albums = new AlbumToView().ConvertAll(clubAlbums);

            var clubNews = new NewsCrud(_db).GetAllClubNews(clubId, amount: Constants.DefaulNewsAmount).ToList();
            clubFullVL.News = new NewsToView().ConvertAll(clubNews);

            return View(clubFullVL);

        }
    }
}
