using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
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
    }
}
