﻿using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace BaseballUa.DTO
{
    public class EventToView
    {
        public EventViewModel Convert(Event eventDAL, BaseballUaDbContext _dbContext)
        {
            EventViewModel eventView = new EventViewModel();
            eventView.EventViewModelId = eventDAL.Id;
            eventView.Year = eventDAL.Year;
            eventView.StartDate = eventDAL.StartDate;
            eventView.EndDate = eventDAL.EndDate;
            eventView.TournamentId = eventDAL.TournamentId;
            var curTournamentDAL = _dbContext.Tournaments.First(a => a.Id == eventDAL.TournamentId);
            eventView.Tournament = new TournamentToView().Convert(curTournamentDAL, _dbContext);
            eventView.TournamentList = _dbContext.Tournaments.Select(a => new SelectListItem
            {
                Text = $"{a.Sport.ToString()} | {a.Category.ShortName} | {a.Name}",
                Value = a.Id.ToString()
            }).ToList();
            return eventView;
        }

        public List<EventViewModel> ConvertAll(List<Event> eventsDAL, BaseballUaDbContext _dbContext) 
        { 
            List<EventViewModel> eventsView = new List<EventViewModel>();
            foreach (var eventDAL in eventsDAL)
            {
                eventsView.Add(Convert(eventDAL, _dbContext));
            }

            return eventsView;
        }

        public EventViewModel CreateEmpty(BaseballUaDbContext _dbContext)
        {
            var eventView = new EventViewModel();
            eventView.Year = DateTime.Now.Year;
            eventView.StartDate = DateTime.Now;
            eventView.EndDate = DateTime.Now;
            eventView.TournamentId = default;
            eventView.Tournament = null;
            eventView.TournamentList = _dbContext.Tournaments.Select(a => new SelectListItem
                                                {
                                                    Text = $"{a.Sport.ToString()} | {a.Category.ShortName} | {a.Name}",
                                                    Value = a.Id.ToString()
                                                });
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