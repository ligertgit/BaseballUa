using BaseballUa.Data;
using Microsoft.AspNetCore.Mvc;

namespace BaseballUa.Controllers
{
    public class CalendarController : Controller
    {

        private readonly BaseballUaDbContext _db;

        public CalendarController(BaseballUaDbContext dbcontext)
        {
            _db = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
