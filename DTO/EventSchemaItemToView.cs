using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;

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
            eventSchemaItemVL.SchemaItem = eventSchemaItemVL.SchemaItem;
            eventSchemaItemVL.EventId = eventSchemaItemDAL.EventId;
            eventSchemaItemVL.Event = new EventsCrud(_dbContext).Get(eventSchemaItemDAL.EventId);
            eventSchemaItemVL.Tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);

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
            eventSchemaItemVL.Event = new EventsCrud(_dbContext).Get(eventId);
            eventSchemaItemVL.Tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);

            return eventSchemaItemVL;
        }
    }
}
