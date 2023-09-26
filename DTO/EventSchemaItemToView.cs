using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Migrations;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BaseballUa.DTO
{
    public class EventSchemaItemToView
    {
        private readonly BaseballUaDbContext _dbContext;

        public EventSchemaItemToView(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EventSchemaItemViewModel Convert(EventSchemaItem eventSchemaItemDAL)
        {
            var eventSchemaItemVL = new EventSchemaItemViewModel();
            eventSchemaItemVL.EventSchemaItemViewModelId = eventSchemaItemDAL.Id;
            eventSchemaItemVL.Order = eventSchemaItemDAL.Order;
            eventSchemaItemVL.SchemaItem = eventSchemaItemDAL.SchemaItem;
            eventSchemaItemVL.EventId = eventSchemaItemDAL.EventId;

            //fix -dbaccess. and get this navigation data from crud directrly
            var eventt = new EventsCrud(_dbContext).Get(eventSchemaItemDAL.EventId);
            eventSchemaItemVL.Event = new EventToView().Convert(eventt, _dbContext);

            //fix -dbaccess. and get this navigation data from crud directrly
            var tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);
            eventSchemaItemVL.Tournament = new TournamentToView().Convert(tournament, _dbContext);

            eventSchemaItemVL.Groups = new SchemaGroupToView(_dbContext).ConvertAll(eventSchemaItemDAL.SchemaGroups.ToList());

            return eventSchemaItemVL;
        }

        public EventSchemaItem ConvertBack(EventSchemaItemViewModel eventSchemaItemVL)
        { 
            var eventSchemaItemDAL = new EventSchemaItem();
            eventSchemaItemDAL.Id = eventSchemaItemVL.EventSchemaItemViewModelId;
            eventSchemaItemDAL.Order = eventSchemaItemVL.Order;
            eventSchemaItemDAL.SchemaItem = eventSchemaItemVL.SchemaItem;
            eventSchemaItemDAL.EventId = eventSchemaItemVL.EventId;

            return eventSchemaItemDAL;
        }

        public EventSchemaItemViewModel CreateEmpty(int eventId)
        {
            var eventSchemaItemVL = new EventSchemaItemViewModel();
            eventSchemaItemVL.EventId = eventId;

            //fix -dbaccess. and get this navigation data from crud directrly
            var eventt = new EventsCrud(_dbContext).Get(eventId);
            eventSchemaItemVL.Event = new EventToView().Convert(eventt, _dbContext);

            //fix -dbaccess. and get this navigation data from crud directrly
            var tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);
            eventSchemaItemVL.Tournament = new TournamentToView().Convert(tournament, _dbContext);

            return eventSchemaItemVL;
        }

        public List<EventSchemaItemViewModel> ConvertAll(List<EventSchemaItem>? eventSchemaItemsDAL)
        {
            var eventSchemaItemsVL = new List<EventSchemaItemViewModel>();
            if (eventSchemaItemsDAL != null) 
            {
                foreach (var item in eventSchemaItemsDAL)
                {
                    eventSchemaItemsVL.Add(Convert(item));
                }
            }
            
            return eventSchemaItemsVL;
        }

        //public List<(DateTime, List<GameViewModel>)> ConvertALLToGamesByDay(List<EventSchemaItem> schemaItemsFullDAL)
        //{
        //    List<(DateTime, List<GameViewModel>)> gamesByDay = new List<(DateTime, List<GameViewModel>)> ();

        //    if (schemaItemsFullDAL != null)
        //    {
        //        foreach (var schemaItem )
        //    }
        //}
    }
}
