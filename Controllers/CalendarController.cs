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

        //static List<Event> DB =
        //     new List<Event>() {
        //        new Event(){
        //            Id = "eventid" + 1,
        //            Summary = "Google I/O 2015",
        //            Location = "800 Howard St., San Francisco, CA 94103",
        //            Description = "A chance to hear more about Google's developer products.",
        //            Start = new EventDateTime()
        //            {
        //                DateTime = new DateTime(2024, 03, 15, 15, 30, 0),
        //                TimeZone = "America/Los_Angeles",
        //            },
        //            End = new EventDateTime()
        //            {
        //                DateTime = new DateTime(2024, 04, 16, 15, 30, 0),
        //                TimeZone = "America/Los_Angeles",
        //            },
        //            //Recurrence = new List<string> { "RRULE:FREQ=DAILY;COUNT=2" },
        //            //Attendees = new List<EventAttendee>
        //            //{
        //            //    new EventAttendee() { Email = "lpage@example.com"},
        //            //    new EventAttendee() { Email = "sbrin@example.com"}
        //            //}
        //        }
        //     };

        public CalendarController(BaseballUaDbContext dbcontext)
        {
            _db = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Regenerate(int year)
        { 
            var gCalendar = new GCalendar("uaball-201a5e222d38.json");
            gCalendar.ClearAll(Constants.Calendars.Select(c => c.CalendarId).ToList());

            //var categories = new CategoriesCrud(_db).GetAll();
            //foreach (var calendarData in Constants.Calendars)
            //{
            //    int count;
            //    var events = new EventsCrud(_db).GetAllFiltered(out count,
            //                                                        sportType: calendarData.SportType,
            //                                                        categoryIds: categories.Where(c => c.ShortName == calendarData.AgeShortName).Select(c => c.Id),
            //                                                        newestDate: new DateTime(year + 1, 1, 1),
            //                                                        eldestDate: new DateTime(year, 1, 1),
            //                                                        amount: Constants.UnlimitedAmount).ToList();
            //    foreach (var eventt in events)
            //    {
            //        if(eventt.StartDate != null && eventt.EndDate != null)
            //        {
            //            if (((DateTime)eventt.EndDate - (DateTime)eventt.StartDate).Days <= Constants.CalendarEventSplitInterval)
            //            {
            //                var eventData = new Google.Apis.Calendar.v3.Data.Event()
            //                {
            //                    Id = "eventid" + eventt.Id,
            //                    Summary = String.Format("{0} {1} {2} {3}", eventt.Tournament.Sport, eventt.Tournament.Category.ShortName, eventt.Tournament.Name, eventt.Year),
            //                    Start = new EventDateTime()
            //                    {
            //                        DateTime = eventt.StartDate,
            //                    },
            //                    End = new EventDateTime()
            //                    {
            //                        DateTime = eventt.EndDate,
            //                    }
            //                };
            //                gCalendar.Insert(eventData, calendarData.CalendarId);
            //                //Console.WriteLine("Insert. {0} {1}-{2}", eventData.Id, eventData.Start.DateTime, eventData.End.DateTime);
            //                //Console.WriteLine("CalendarSport: {0} CalendarAge: {1}", calendarData.SportType, calendarData.AgeShortName);
            //            }
            //            else
            //            {
            //                var eventGames = new EventsCrud(_db).GetGames(eventt.Id);
            //                var gamesDistDates = eventGames.Where(g => g.StartDate != null).Select(g => ((DateTime)g.StartDate).Date).Distinct().OrderBy(d => d);

            //                if(!gamesDistDates.IsNullOrEmpty())
            //                {
            //                    var curTourStartDate = gamesDistDates.First();
            //                    var curTourEndDate = gamesDistDates.First();
            //                    var curTourId = 1;
            //                    foreach(var curDate in gamesDistDates)
            //                    {
            //                        if(curDate <= curTourEndDate.AddDays(3))
            //                        {
            //                            curTourEndDate = curDate;
            //                        }
            //                        else
            //                        {
            //                            var eventData = new Google.Apis.Calendar.v3.Data.Event()
            //                            {
            //                                Id = "eventid" + eventt.Id + "tour" + curTourId,
            //                                Summary = String.Format("{0} {1} {2} {3} PART{4}", eventt.Tournament.Sport, eventt.Tournament.Category.ShortName, eventt.Tournament.Name, eventt.Year, curTourId),
            //                                Start = new EventDateTime()
            //                                {
            //                                    DateTime = curTourStartDate,
            //                                },
            //                                End = new EventDateTime()
            //                                {
            //                                    DateTime = curTourEndDate,
            //                                }
            //                            };
            //                            gCalendar.Insert(eventData, calendarData.CalendarId);
            //                            //Console.WriteLine("Insert. {0} {1}-{2}", eventData.Id, eventData.Start.DateTime, eventData.End.DateTime);
            //                            //Console.WriteLine("CalendarSport: {0} CalendarAge: {1}", calendarData.SportType, calendarData.AgeShortName);
            //                            curTourId++;
            //                            curTourStartDate = curDate;
            //                            curTourEndDate = curDate;
            //                        }
            //                    }
            //                    var lastEventData = new Google.Apis.Calendar.v3.Data.Event()
            //                    {
            //                        Id = "eventid" + eventt.Id + "tour" + curTourId,
            //                        Summary = String.Format("{0} {1} {2} {3} PART{4}", eventt.Tournament.Sport, eventt.Tournament.Category.ShortName, eventt.Tournament.Name, eventt.Year, curTourId),
            //                        Start = new EventDateTime()
            //                        {
            //                            DateTime = curTourStartDate,
            //                        },
            //                        End = new EventDateTime()
            //                        {
            //                            DateTime = curTourEndDate,
            //                        }
            //                    };
            //                    gCalendar.Insert(lastEventData, calendarData.CalendarId);
            //                    //Console.WriteLine("Insert. {0} {1}-{2}", lastEventData.Id, lastEventData.Start.DateTime, lastEventData.End.DateTime);
            //                    //Console.WriteLine("CalendarSport: {0} CalendarAge: {1}", calendarData.SportType, calendarData.AgeShortName);
            //                }

            //            }
            //        }
            //    }
            //}
            
            return RedirectToAction("Index");
        }





        public IActionResult TestAddEvent()
        {
            //var gCalendar = new GCalendar("uaball-201a5e222d38.json", @"b2e7bc05bea02abcc81252a192487781b878cab7a0c9315e6135873c79545d69@group.calendar.google.com");

            //string jsonFile = "uaball-201a5e222d38.json";
            //string calendarId = @"b2e7bc05bea02abcc81252a192487781b878cab7a0c9315e6135873c79545d69@group.calendar.google.com";

            //string[] Scopes = { CalendarService.Scope.Calendar };

            //ServiceAccountCredential credential;

            //using (var stream =
            //    new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
            //{
            //    var confg = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
            //    credential = new ServiceAccountCredential(
            //       new ServiceAccountCredential.Initializer(confg.ClientEmail)
            //       {
            //           Scopes = Scopes
            //       }.FromPrivateKey(confg.PrivateKey));
            //}

            //var service = new CalendarService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "Calendar API Sample",
            //});

            //var calendar = service.Calendars.Get(calendarId).Execute();
            //Console.WriteLine("Calendar Name :");
            //Console.WriteLine(calendar.Summary);

            //Console.WriteLine("click for more .. ");
            ////Console.Read();


            //// Define parameters of request.
            //EventsResource.ListRequest listRequest = service.Events.List(calendarId);
            //listRequest.TimeMin = DateTime.Now;
            //listRequest.ShowDeleted = false;
            //listRequest.SingleEvents = true;
            //listRequest.MaxResults = 10;
            //listRequest.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            //// List events.
            //Events events = listRequest.Execute();
            //Console.WriteLine("Upcoming events:");
            //if (events.Items != null && events.Items.Count > 0)
            //{
            //    foreach (var eventItem in events.Items)
            //    {
            //        string when = eventItem.Start.DateTime.ToString();
            //        if (String.IsNullOrEmpty(when))
            //        {
            //            when = eventItem.Start.Date;
            //        }
            //        Console.WriteLine("{0} ({1})", eventItem.Summary, when);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No upcoming events found.");
            //}
            //Console.WriteLine("click for more .. ");
            ////Console.Read();

            //var myevent = DB.Find(x => x.Id == "eventid" + 1);

            //var InsertRequest = service.Events.Insert(myevent, calendarId);
            ////var ClearRequest = service.Calendars.Clear(calendarId);


            //try
            //{
            //    InsertRequest.Execute();
            //    Console.WriteLine("executed!!!!!");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    try
            //    {
            //        service.Events.Update(myevent, calendarId, myevent.Id).Execute();
            //        Console.WriteLine("Insert/Update new Event ");
            //        //Console.Read();

            //    }
            //    catch (Exception ex2)
            //    {
            //        Console.WriteLine(ex2.Message);
            //        Console.WriteLine("can't Insert/Update new Event ");

            //    }
            //}


            //var calendar = service.Calendars.Get(calendarId).Execute();
            //Console.WriteLine("Calendar Name :");
            //Console.WriteLine(calendar.Summary);
            //Console.WriteLine("Clearing Calendar");
            //var ClearRequest = service.Calendars.Clear(calendarId);

            //try
            //{
            //    ClearRequest.Execute();
            //    Console.WriteLine("executed!!!!!");
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("Clearing fail");
            //}

            //Console.Read();
            return RedirectToAction("Index", "Home");
        }
    }
}
