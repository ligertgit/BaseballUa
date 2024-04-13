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
        private CalendarService service { get; set; }

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
            if(!calendarIds.IsNullOrEmpty())
            {
                foreach(var Id in calendarIds) 
                {
                    var calendarEvents = service.Events.List(Id).Execute();
                    if(calendarEvents != null && !calendarEvents.Items.IsNullOrEmpty())
                    {
                        foreach(var eventt in calendarEvents.Items)
                        {
                            service.Events.Delete(Id, eventt.Id).Execute();
                        }
                    }
                }
            }
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
