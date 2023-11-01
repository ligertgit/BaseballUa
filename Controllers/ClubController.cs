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
            var teamDAL = new TeamCrud(_db).Get(teamId);
            var teamHomeGamesDAL = new TeamCrud(_db).GetHomeGames(teamId).ToList();
            var teamVisitorGamesDAL = new TeamCrud(_db).GetVisitorGames(teamId).ToList();
            var teamPlayersDAL = new PlayersCrud(_db).GetAll(teamId).ToList();
            
            var teamVL = new TeamToView().Convert(teamDAL);
            teamVL.Games = new GameToView().ConvertAll(teamHomeGamesDAL).ToList();
            teamVL.Games.AddRange(new GameToView().ConvertAll(teamVisitorGamesDAL).ToList());
            teamVL.Players = new PlayerToView().ConvertAll(teamPlayersDAL).ToList();

            return View(teamVL);
        }

        public IActionResult DetailsClub(int clubId)
        {
            var clubDAL = new ClubCrud(_db).Get(clubId);
            clubDAL.Staffs = new StaffsCrud(_db).GetAll(clubId).ToList();
            clubDAL.Teams = new TeamCrud(_db).GetAll(clubId).ToList();
            var EventsDAL = new EventsCrud(_db).GetForClub(clubId).ToList();

            var clubFullVL = new ClubFullDetailVM();
            clubFullVL.Club = new ClubToView().Convert(clubDAL);
            clubFullVL.Events = new EventToView().ConvertAll(EventsDAL).ToList();

            return View(clubFullVL);

        }
    }
}
