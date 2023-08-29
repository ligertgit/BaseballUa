using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.DTO
{
    public class EventIndexToView
    {
        public EventIndexViewModel Convert(EventIndexModel eventData)
        {
            EventIndexViewModel eventView = new EventIndexViewModel();
            eventView.EventIndexViewModelId = eventData.Id;
            eventView.Year = eventData.Year;
            eventView.StartDate = eventData.StartDate;
            eventView.EndDate = eventData.EndDate;
            eventView.TournamentId = eventData.TournamentId;
            eventView.Name = eventData.Name;
            eventView.Sport = eventData.Sport;
            eventView.Description = eventData.Description;
            eventView.IsAnual = eventData.IsAnual;
            eventView.IsInternational = eventData.IsInternational;
            eventView.IsOfficial = eventData.IsOfficial;
            eventView.CategoryId = eventData.CategoryId;
            eventView.CategoryShortName = eventData.CategoryShortName;

            return eventView;
        }

        public List<EventIndexViewModel> ConvertAll(List<EventIndexModel> eventsData)
        {
            List<EventIndexViewModel> eventsView = new List<EventIndexViewModel>();
            foreach (var eventData in eventsData)
            {
                eventsView.Add(Convert(eventData));
            }

            return eventsView;
        }
    }
}
