using BaseballUa.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace BaseballUa.Data
{
    public class GCalendar
    {
        //private readonly string _jsonFile = "uaball-201a5e222d38.json";
        //string calendarId = @"b2e7bc05bea02abcc81252a192487781b878cab7a0c9315e6135873c79545d69@group.calendar.google.com";
        //CalendarService service = new CalendarService();
        //private readonly string _calendarId;
        private CalendarService service { get; set; }
        //public Google.Apis.Calendar.v3.Data.Calendar calendar { get; set; }

        public GCalendar(string jsonFile)
        {
            string[] Scopes = { CalendarService.Scope.Calendar };
            ServiceAccountCredential credential;

            using (var stream =
                new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
            {
                var confg = Google.Apis.Json.NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
                credential = new ServiceAccountCredential(
                               new ServiceAccountCredential.Initializer(confg.ClientEmail)
                               {
                                   Scopes = Scopes
                               }.FromPrivateKey(confg.PrivateKey));
            }

            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "UA-BALL.COM",
            });
        }

        public void ClearAll(List<string> calendarIds)
        {
            if(calendarIds.IsNullOrEmpty())
            {
                foreach(var Id in calendarIds) 
                { 
                    service.Calendars.Clear(Id).Execute();
                }
            }

            //service.Events.Delete(calendarId, eventToDelete.Id).Execute();
            //CalendarService myService = new CalendarService("your calendar name");
            //myService.setUserCredentials(username, password);

            //CalendarEntry calendar;

            //try
            //{
            //    calendar = (CalendarEntry)myService.Get(http://www.google.com/calendar/feeds/default/owncalendars/full/45kk8jl9nodfri1qgepsb65fnc%40group.calendar.google.com);
            //    foreach (AtomEntry item in calendar.Feed.Entries)
            //    {
            //        item.Delete();
            //    }
            //}
            //catch (GDataRequestException)
            //{
            //}
        }

        public void Insert(Google.Apis.Calendar.v3.Data.Event myEvent, string calendarId)
        {
            var InsertRequest = service.Events.Insert(myEvent, calendarId);

            try
            {
                InsertRequest.Execute();
                Console.WriteLine("executed!!!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}
