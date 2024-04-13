using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using static BaseballUa.Data.Enums;

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
