using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BaseballUa.DTO
{
    public class EventToView
    {
        public EventViewModel Convert(Event eventDAL, bool doSubConvert = true)
        {
            EventViewModel eventView = new EventViewModel();
            eventView.EventViewModelId = eventDAL.Id;
            eventView.Year = eventDAL.Year;
            eventView.StartDate = eventDAL.StartDate;
            eventView.EndDate = eventDAL.EndDate;
            eventView.TournamentId = eventDAL.TournamentId;
            //var curTournamentDAL = _dbContext.Tournaments.First(a => a.Id == eventDAL.TournamentId);
            if (eventDAL.Tournament != null) 
            {
				eventView.Tournament = new TournamentToView().Convert(eventDAL.Tournament, false);
			}
            
            //eventView.TournamentList = _dbContext.Tournaments.Select(a => new SelectListItem
            //{
            //    Text = $"{a.Sport.ToString()} | {a.Category.ShortName} | {a.Name}",
            //    Value = a.Id.ToString()
            //}).ToList();

            // convert schema
            if (doSubConvert && eventDAL.SchemaItems != null) 
            {
                eventView.SchemaItems = new EventSchemaItemToView().ConvertAll(eventDAL.SchemaItems.ToList(), false);
            }
            
            if (doSubConvert && eventDAL.News != null) 
            { 
                eventView.News = new NewsToView().ConvertAll(eventDAL.News.ToList(), false);
            }


            return eventView;
        }

        public List<EventViewModel> ConvertAll(List<Event> eventsDAL, bool doSubConvert = true) 
        { 
            List<EventViewModel> eventsView = new List<EventViewModel>();
            foreach (var eventDAL in eventsDAL)
            {
                eventsView.Add(Convert(eventDAL, doSubConvert));
            }

            return eventsView;
        }

        public EventViewModel CreateEmpty()
        {
            var eventView = new EventViewModel();
            eventView.Year = DateTime.Now.Year;
            eventView.StartDate = DateTime.Now;
            eventView.EndDate = DateTime.Now;
            eventView.TournamentId = default;
            eventView.Tournament = null;
            //eventView.TournamentList = _dbContext.Tournaments.Select(a => new SelectListItem
            //                                    {
            //                                        Text = $"{a.Sport.ToString()} | {a.Category.ShortName} | {a.Name}",
            //                                        Value = a.Id.ToString()
            //                                    });
            return eventView;
        }

        public Event ConvertBack(EventViewModel eventView) 
        {
            var eventDAL = new Event();
            eventDAL.Id = eventView.EventViewModelId;
            eventDAL.Year = eventView.Year;
            eventDAL.StartDate = eventView.StartDate;
            eventDAL.EndDate = eventView.EndDate;
            eventDAL.TournamentId = eventView.TournamentId;

            return eventDAL;
        } 


    }
}
