using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO.Custom;
using BaseballUa.Migrations;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BaseballUa.DTO
{
    public class EventSchemaItemToView
    {
        //private readonly BaseballUaDbContext _dbContext;

        //public EventSchemaItemToView(BaseballUaDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public EventSchemaItemViewModel Convert(EventSchemaItem eventSchemaItemDAL)
        {
            var eventSchemaItemVL = new EventSchemaItemViewModel();
            eventSchemaItemVL.EventSchemaItemViewModelId = eventSchemaItemDAL.Id;
            eventSchemaItemVL.Order = eventSchemaItemDAL.Order;
            eventSchemaItemVL.SchemaItem = eventSchemaItemDAL.SchemaItem;
            eventSchemaItemVL.EventId = eventSchemaItemDAL.EventId;
            if (eventSchemaItemDAL.Event != null)
            {
                eventSchemaItemVL.Event = new EventToView().Convert(eventSchemaItemDAL.Event);
            }
            //fix -dbaccess. and get this navigation data from crud directrly
            //var eventt = new EventsCrud(_dbContext).Get(eventSchemaItemDAL.EventId);
            //eventSchemaItemVL.Event = new EventToView().Convert(eventt, _dbContext);

            //fix -dbaccess. and get this navigation data from crud directrly
            //should be passed through viewBag
            //var tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);
            //eventSchemaItemVL.Tournament = new TournamentToView().Convert(tournament, _dbContext);
            if (eventSchemaItemDAL.SchemaGroups != null) 
            { 
                eventSchemaItemVL.Groups = new SchemaGroupToView().ConvertAll(eventSchemaItemDAL.SchemaGroups.ToList());
            }
            //eventSchemaItemVL.Groups = new SchemaGroupToView(_dbContext).ConvertAll(eventSchemaItemDAL.SchemaGroups.ToList());

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
            //var eventt = new EventsCrud(_dbContext).Get(eventId);
            //eventSchemaItemVL.Event = new EventToView().Convert(eventt, _dbContext);

            //fix -dbaccess. and get this navigation data from crud directrly
            //var tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);
            //eventSchemaItemVL.Tournament = new TournamentToView().Convert(tournament, _dbContext);

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


        public List<DayGames> ConvertAllToGamesByDay(List<EventSchemaItem> schemaItemsFullDAL)
        {
            var allGamesByDayVL = new List<DayGames>();
            //List<(DateTime, List<GameViewModel>)> gamesByDay = new List<(DateTime, List<GameViewModel>)> ();
            //  Array gamesByDay = new Array() 
            //List<GameViewModel> allGames = new List<GameViewModel> ();
            if (schemaItemsFullDAL != null)
            {
                var allGames = schemaItemsFullDAL.SelectMany(si => si.SchemaGroups).SelectMany(sg => sg.Games).ToList();
                allGamesByDayVL = allGames.Where(d => d.StartDate != null).GroupBy(
                                                game => game.StartDate?.Date,
                                                game => game,
                                                (gamesDate, GroupedGames) => new DayGames
                                                {
                                                    GamesDate = gamesDate,
                                                    Games = new GameToView().ConvertAll(GroupedGames.ToList()),
                                                }).ToList();
                //allGamesByDayVL = allGamesByDayDAL;
            }
            

            return allGamesByDayVL;
        //    if (schemaItemsFullDAL != null)
        //    {
        //        foreach (var schemaItem )
        //    }
        }
    }
}
