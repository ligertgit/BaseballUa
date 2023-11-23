using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO.Custom;
using BaseballUa.Migrations;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using BaseballUa.ViewModels.Custom;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static BaseballUa.Data.Enums;

namespace BaseballUa.DTO
{
    public class EventSchemaItemToView
    {
        //private readonly BaseballUaDbContext _dbContext;

        //public EventSchemaItemToView(BaseballUaDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public EventSchemaItemViewModel Convert(EventSchemaItem eventSchemaItemDAL, bool doSubConvert = true)
        {
            var eventSchemaItemVL = new EventSchemaItemViewModel();
            eventSchemaItemVL.EventSchemaItemViewModelId = eventSchemaItemDAL.Id;
            eventSchemaItemVL.Order = eventSchemaItemDAL.Order;
            eventSchemaItemVL.SchemaItem = eventSchemaItemDAL.SchemaItem;
            eventSchemaItemVL.EventId = eventSchemaItemDAL.EventId;
            if (eventSchemaItemDAL.Event != null)
            {
                eventSchemaItemVL.Event = new EventToView().Convert(eventSchemaItemDAL.Event, false);
            }
            //fix -dbaccess. and get this navigation data from crud directrly
            //var eventt = new EventsCrud(_dbContext).Get(eventSchemaItemDAL.EventId);
            //eventSchemaItemVL.Event = new EventToView().Convert(eventt, _dbContext);

            //fix -dbaccess. and get this navigation data from crud directrly
            //should be passed through viewBag
            //var tournament = new TournamentsCrud(_dbContext).Get(eventSchemaItemVL.Event.TournamentId);
            //eventSchemaItemVL.Tournament = new TournamentToView().Convert(tournament, _dbContext);
            if (doSubConvert && eventSchemaItemDAL.SchemaGroups != null) 
            { 
                eventSchemaItemVL.Groups = new SchemaGroupToView().ConvertAll(eventSchemaItemDAL.SchemaGroups.ToList(), false);
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

        public List<EventSchemaItemViewModel> ConvertAll(List<EventSchemaItem>? eventSchemaItemsDAL, bool doSubConvert = true)
        {
            var eventSchemaItemsVL = new List<EventSchemaItemViewModel>();
            if (eventSchemaItemsDAL != null) 
            {
                foreach (var item in eventSchemaItemsDAL)
                {
                    eventSchemaItemsVL.Add(Convert(item, doSubConvert));
                }
            }
            
            return eventSchemaItemsVL;
        }

        public List<EventItemStandingVM> ConvertAllToStanding(List<EventSchemaItem> schemaItemsFullDAL)
        {
            var eventItemsStandinfVM = schemaItemsFullDAL.Select(li => new EventItemStandingVM
            {
                EventItem = new EventSchemaItemToView().Convert(li),
                GroupStandings = li.SchemaGroups.Select(sg => new GroupStandingVM
                {
                    SchemaGroup = new SchemaGroupToView().Convert(sg),
                    TeamStandings = sg.Games.Where(i => i.HomeTeam != null && i.VisitorTeam != null)
                                                        .SelectMany(g => new[]
                                                        {
                                                            new {
                                                                    Team = new TeamToView().Convert(g.HomeTeam),
                                                                    RunsHome = g.RunsHome,
                                                                    RunsVisitor = g.RunsVisitor,
                                                                    GameStatus = g.GameStatus
                                                                },
                                                            new {
                                                                    Team = new TeamToView().Convert(g.VisitorTeam),
                                                                    RunsHome = g.RunsVisitor,
                                                                    RunsVisitor = g.RunsHome,
                                                                    GameStatus = g.GameStatus
                                                                }
                                                        })
                                                        .Select(i => new
                                                        {
                                                            TeamId = i.Team.Id,
                                                            Team = i.Team,
                                                            RunsHome = i.RunsHome,
                                                            RunsVisitor = i.RunsVisitor,
                                                            GameStatus = i.GameStatus
                                                        })
                                                        .GroupBy(hg => hg.TeamId, hg => hg, (gTeamId, groupedGames) => new TeamStandingVM
                                                        {
                                                            Team = groupedGames.First().Team,
                                                            TotalGames = groupedGames.Count(i => i.GameStatus == GameStatus.Finished),
                                                            WinGames = groupedGames.Count(i => i.RunsHome > i.RunsVisitor && i.GameStatus == GameStatus.Finished),
                                                            LooseGames = groupedGames.Count(i => i.RunsVisitor > i.RunsHome && i.GameStatus == GameStatus.Finished),
                                                            PCT = groupedGames.Any(i => i.GameStatus == GameStatus.Finished) 
                                                                    ? groupedGames.Count(i => i.RunsHome > i.RunsVisitor && i.GameStatus == GameStatus.Finished) / groupedGames.Count(i => i.GameStatus == GameStatus.Finished) 
                                                                    : 0
                                                        }
                                                       )
                                                       .ToList()
                }).ToList()
            }).ToList();

            return eventItemsStandinfVM;
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
