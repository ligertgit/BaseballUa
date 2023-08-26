using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BaseballUa.Controllers
{
    public class EventController : Controller
    {
        private BaseballUaDbContext _db;

        public EventController(BaseballUaDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int monthShift = 0)
        {
            var eventsDAL = new EventsCrud(_db).GetMonth(monthShift).ToList();
            var eventsView = new EventToView().ConvertAll(eventsDAL, _db);
            ViewBag.monthShift = monthShift;
            return View(eventsView);
        }

        public IActionResult DetailsEvent(int id)
        {
            var eventDAL = new EventsCrud(_db).Get(id);
            var eventView = new EventToView().Convert(eventDAL, _db);

            return View(eventView);
        }
    }
}
