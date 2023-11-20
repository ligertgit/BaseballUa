﻿using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.DTO.Custom;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using BaseballUa.ViewModels.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace BaseballUa.Controllers
{
    public class EventController : Controller
    {
        private readonly BaseballUaDbContext _db;

        public EventController(BaseballUaDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int monthShift = 0)
        {

            Filters filters = new Filters();
            filters.Baseball = Convert.ToBoolean(Request.Cookies["filterBaseball"]);
            filters.Softball = Convert.ToBoolean(Request.Cookies["filterSoftball"]);
            filters.U10 = Convert.ToBoolean(Request.Cookies["filterU10"]);
            filters.U12 = Convert.ToBoolean(Request.Cookies["filterU12"]);
            filters.U15 = Convert.ToBoolean(Request.Cookies["filterU15"]);
            filters.U18 = Convert.ToBoolean(Request.Cookies["filterU18"]);
            filters.Adult = Convert.ToBoolean(Request.Cookies["filterAdult"]);
            filters.Veteran = Convert.ToBoolean(Request.Cookies["filterVeteran"]);
            filters.Fun = Convert.ToBoolean(Request.Cookies["filterFun"]);

            var eventsView = new EventIndex(_db).GetMonthFilters(monthShift, filters);
            //var eventsView = new EventIndexToView().ConvertAll(eventsData);

            ViewBag.monthShift = monthShift;
            ViewBag.filters = filters;

            return View(eventsView);
        }

        public IActionResult DetailsEvent(int id, string ShowMenu = "schema", int monthShift = 0)
        {
            // probably should use EventIndexViewModel
            var eventDAL = new EventsCrud(_db).Get(id);
            //eventDAL.Tournament.Category = new CategoriesCrud(_db).Get(eventDAL.Tournament.CategoryId);
            var eventView = new EventToView().Convert(eventDAL);
            
            ViewData["monthShift"] = monthShift;
            ViewData["ShowMenu"] = ShowMenu;
            return View(eventView);
        }
        
        [HttpPost]
        public IActionResult ApplyFilters(IFormCollection fc) 
        {
            //make as structure and move to config
            var filters = new Filters();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);

            //use cookie
            if (fc["chkBaseball"] == "on") filters.Baseball = true;
            if (fc["chkSoftball"] == "on") filters.Softball = true;
            if (fc["chkU10"] == "on") filters.U10 = true;
            if (fc["chkU12"] == "on") filters.U12 = true;
            if (fc["chkU15"] == "on") filters.U15 = true;
            if (fc["chkU18"] == "on") filters.U18 = true;
            if (fc["chkU23"] == "on") filters.U23 = true;
            if (fc["chkAdult"] == "on") filters.Adult = true;
            if (fc["chkVeteran"] == "on") filters.Veteran = true;
            if (fc["chkFun"] == "on") filters.Fun = true;

            Response.Cookies.Append("filterBaseball", filters.Baseball.ToString(), option);
            Response.Cookies.Append("filterSoftball", filters.Softball.ToString(), option);
            Response.Cookies.Append("filterU10", filters.U10.ToString(), option);
            Response.Cookies.Append("filterU12", filters.U12.ToString(), option);
            Response.Cookies.Append("filterU15", filters.U15.ToString(), option);
            Response.Cookies.Append("filterU18", filters.U18.ToString(), option);
            Response.Cookies.Append("filterAdult", filters.Adult.ToString(), option);
            Response.Cookies.Append("filterVeteran", filters.Veteran.ToString(), option);
            Response.Cookies.Append("filterFun", filters.Fun.ToString(), option);

            var monthShift = (fc["monthShift"].IsNullOrEmpty()) ? 0 : Convert.ToInt32(fc["monthShift"]);
            //TempData.Put("filters", filters);

            return RedirectToAction("Index", new { monthShift = monthShift });
        }

        public IActionResult Schema(int id)
        {
            var eventDAL = new EventsCrud(_db).Get(id);
			var eventVL = new EventToView().Convert(eventDAL, false);

			var schemaItemsFullDAL = new EventSchemaItemsCrud(_db).GetAllWithGames(id);
			eventVL.SchemaItems = new List<EventSchemaItemViewModel>();
            foreach(var schemaItemDAL in schemaItemsFullDAL.ToList())
            {
                var schemaItemVL = new EventSchemaItemToView().Convert(schemaItemDAL, false);
                schemaItemVL.Groups = new SchemaGroupToView().ConvertAll(schemaItemDAL.SchemaGroups.ToList(), true);
                eventVL.SchemaItems.Add(schemaItemVL);
			}

            return  View(eventVL);
        }

        public IActionResult Schedule(int id, int dateIndex = -1)
        {
            int showIndex;
            var gamesByDay = new EventGamesByDayVM();
            
            var eventDAL = new EventsCrud(_db).Get(id);
            gamesByDay.Event = new EventToView().Convert(eventDAL);

            //var schemaItemsFullDAL = new EventSchemaItemsCrud(_db).GetAllWithGames(id);
            var schemaItemsFullDAL = new EventSchemaItemsCrud(_db).GetAllWithGames_test(id);
            if (schemaItemsFullDAL == null)
            {
                gamesByDay.GamesByDay = new List<DayGames>
                {
                    new DayGames { GamesDate = DateTime.Now.Date, Games = new List<GameViewModel>() }
                };
                showIndex = 0;
            }
            else 
            {
                gamesByDay.GamesByDay = new EventSchemaItemToView().ConvertAllToGamesByDay(schemaItemsFullDAL.ToList()).OrderBy(gd => gd.GamesDate).ToList();
                showIndex = (dateIndex >= 0) && (dateIndex < gamesByDay.GamesByDay.Count) ? dateIndex : gamesByDay.GamesByDay.GetShowIndex();
                    //showIndex = gamesByDay.GamesByDay.GetShowIndex();
            }
            
            ViewBag.ShowIndex = showIndex;

            return View(gamesByDay);
        }

        public IActionResult Standing(int id)
        {
            var EventDAL = new EventsCrud(_db).Get(id);

            var schemaItemsFullDAL = new EventSchemaItemsCrud(_db).GetAllWithGames(id);
            var standingsByEventItemVM = new EventSchemaItemToView().ConvertAllToStanding(schemaItemsFullDAL.ToList());
            //var t1 = new GamesCrud(_db).G;
            //var t1v = new GameToView().CreateEmpty(5);
            //var x = new TeamStandingVM();
            //x.TotalGames = 10;
            //t1v.HomeTeam = x;

            ViewBag.Eventt = new EventToView().Convert(EventDAL);

            //var z = (t1v.HomeTeam as TeamStandingVM).TotalGames;
            return View(standingsByEventItemVM);
        }
    }
}
