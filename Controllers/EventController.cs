using Azure;
using BaseballUa.BlData;
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
            var eventIndexView = new EventIndexVM();

            Filters filters = Request.Cookies.GetFilters();
            eventIndexView.ApplyFilters = new ApplyFilters { Filters = filters, Controller = "Event", RedirectAction = "Index" };

            eventIndexView.Events = new EventIndex(_db).GetMonthFilters(monthShift, filters);

            var routeItem = new RouteItem { Name = "monthShift", Value = monthShift };
            eventIndexView.ApplyFilters.RouteItems.Add(routeItem);

            eventIndexView.MonthShift = monthShift;

            return View(eventIndexView);
        }

        
        [HttpPost]
        public IActionResult ApplyFilters(IFormCollection fc) 
        {
            var filters = fc.GetFilters();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.AppendFilters(filters, option);

            var monthShift = (fc["monthShift"].IsNullOrEmpty()) ? 0 : Convert.ToInt32(fc["monthShift"]);

            return RedirectToAction(fc["redirectAction"], new { monthShift = monthShift });
        }

        public IActionResult DetailsEvent(int id, string ShowMenu = "schema", int monthShift = 0)
        {
            var eventDetailsFull = new EventDetailsFull();
            var eventDAL = new EventsCrud(_db).Get(id);
            eventDetailsFull.Event = new EventToView().Convert(eventDAL);
            eventDetailsFull.News = new NewsToView().ConvertAll(eventDAL.News.ToList());
            var eventALbumsDAL = new AlbumsCrud(_db).GetAllEventAlbums(id);
            eventDetailsFull.Albums = new AlbumToView().ConvertAll(eventALbumsDAL.ToList());
            var eventVideosDAL = new VideosCrud(_db).GetAllEventVideos(id);
            eventDetailsFull.Videos = new VideoToView().ConvertAll(eventVideosDAL.ToList());
            var eventTeamsDAL = new TeamCrud(_db).GetEventTeams(id);
            eventDetailsFull.Teams = new TeamToView().ConvertAll(eventTeamsDAL.ToList());
            var currentGamesDAL = new GamesCrud(_db).GetEventGames(id);
            eventDetailsFull.CurrentGames = new GameToView().ConvertAll(currentGamesDAL.ToList());

            //// probably should use EventIndexViewModel
            //var eventDAL = new EventsCrud(_db).Get(id);
            ////eventDAL.Tournament.Category = new CategoriesCrud(_db).Get(eventDAL.Tournament.CategoryId);
            //var eventView = new EventToView().Convert(eventDAL);

            //ViewData["monthShift"] = monthShift;
            ViewData["ShowMenu"] = ShowMenu;
            return View(eventDetailsFull);
        }

        public IActionResult Schema(int id)
        {
            var eventSchemaFullVL = new EventSchemaFull();
            
            var eventDAL = new EventsCrud(_db).Get(id);
            eventSchemaFullVL.Event = new EventToView().Convert(eventDAL);
            eventSchemaFullVL.News = new NewsToView().ConvertAll(eventDAL?.News?.ToList(), false);

            var schemaItemsFullDAL = new EventSchemaItemsCrud(_db).GetAllWithGames_test(id);
            eventSchemaFullVL.SchemaItems = new List<EventSchemaItemViewModel>();
            foreach (var schemaItemDAL in schemaItemsFullDAL)
            {
                var schemaItemVL = new EventSchemaItemToView().Convert(schemaItemDAL, false);
                schemaItemVL.Groups = new List<SchemaGroupViewModel>();
                if (schemaItemDAL.SchemaGroups != null)
                {
                    foreach (var groupDAL in schemaItemDAL.SchemaGroups)
                    {
                        var groupVL = new SchemaGroupToView().Convert(groupDAL, true);
                        if (groupDAL.Games != null)
                        {
                            groupVL.Games = new GameToView().ConvertAll(groupDAL.Games.ToList(), true);
                        }
                        schemaItemVL.Groups.Add(groupVL);
                    }
                    eventSchemaFullVL.SchemaItems.Add(schemaItemVL);
                }
            }

            eventSchemaFullVL.Albums = new AlbumToView().ConvertAll(schemaItemsFullDAL?.SelectMany(i => i.SchemaGroups ?? Enumerable.Empty<SchemaGroup>())
                                                                        .SelectMany(g => g.Games ?? Enumerable.Empty<Game>())
                                                                        .SelectMany(g => g.Albums ?? Enumerable.Empty<Album>())
                                                                        .ToList(), false);
            eventSchemaFullVL.Videos = new VideoToView().ConvertAll(schemaItemsFullDAL?.SelectMany(i => i.SchemaGroups ?? Enumerable.Empty<SchemaGroup>())
                                                                        .SelectMany(g => g.Games ?? Enumerable.Empty<Game>())
                                                                        .SelectMany(g => g.Videos ?? Enumerable.Empty<Video>())
                                                                        .ToList(), false);

            return  View(eventSchemaFullVL);
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
            var eventStanding = new EventStandingFull();
            var eventDAL = new EventsCrud(_db).Get(id);
            eventStanding.Event = new EventToView().Convert(eventDAL);
            eventStanding.News = new NewsToView().ConvertAll(eventDAL?.News?.ToList(), false);
            var schemaItemsFullDAL = new EventSchemaItemsCrud(_db).GetAllWithGames_test(id);
            eventStanding.EventItemsStanding = new EventSchemaItemToView().ConvertAllToStanding(schemaItemsFullDAL.ToList());
            eventStanding.Albums = new AlbumToView().ConvertAll(schemaItemsFullDAL?.SelectMany(i => i.SchemaGroups ?? Enumerable.Empty<SchemaGroup>())
                                                            .SelectMany(g => g.Games ?? Enumerable.Empty<Game>())
                                                            .SelectMany(g => g.Albums ?? Enumerable.Empty<Album>())
                                                            .ToList(), false);
            eventStanding.Videos = new VideoToView().ConvertAll(schemaItemsFullDAL?.SelectMany(i => i.SchemaGroups ?? Enumerable.Empty<SchemaGroup>())
                                                                        .SelectMany(g => g.Games ?? Enumerable.Empty<Game>())
                                                                        .SelectMany(g => g.Videos ?? Enumerable.Empty<Video>())
                                                                        .ToList(), false);

            return View(eventStanding);
        }
    }
}
