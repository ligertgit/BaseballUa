using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using BaseballUa.Migrations;
using System.IO;

namespace BaseballUa.BlData
{
    public class EventSchemaItemsCrud : ICrud<EventSchemaItem>
    {
        private readonly BaseballUaDbContext _dbContext;

        public EventSchemaItemsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;            
        }
        public void Add(EventSchemaItem item)
        {
            _dbContext.EventSchemaItems.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(EventSchemaItem item)
        {
            throw new NotImplementedException();
        }

        public EventSchemaItem Get(int itemId)
        {
            return _dbContext.EventSchemaItems.Where(i => i.Id == itemId).Include(i => i.Event).ThenInclude(e => e.Tournament).ThenInclude(t => t.Category).FirstOrDefault();
            //return _dbContext.EventSchemaItems.First(i => i.Id == itemId);
        }

        public IEnumerable<EventSchemaItem> GetAll()
        {
            return _dbContext.EventSchemaItems.Include(i => i.Event).ThenInclude(e => e.Tournament).ThenInclude(t => t.Category);
        }

        public IEnumerable<EventSchemaItem> GetAll(int eventId)
        {
            var eventSchemaItems = _dbContext.EventSchemaItems.Where(i => i.EventId == eventId).Include(i => i.Event).ThenInclude(e => e.Tournament).ThenInclude(t => t.Category);
            return eventSchemaItems;
        }
        public void Update(EventSchemaItem item)
        {
            _dbContext.EventSchemaItems.Update(item);
            _dbContext.SaveChanges();
        }

        //custom methods

        public IEnumerable<EventSchemaItem> GetAllWithGames_test(int eventId)
        {
            var schemaItems = _dbContext.EventSchemaItems.Where(s => s.EventId == eventId)
                                        .Include(i => i.SchemaGroups)
                                            .ThenInclude(g => g.Games)
                                                .ThenInclude(g => g.HomeTeam)
                                        .Include(i => i.SchemaGroups)
                                            .ThenInclude(g => g.Games)
                                                .ThenInclude(g => g.VisitorTeam)
                                        .Include(i => i.SchemaGroups)
                                            .ThenInclude(g => g.Games)
                                                .ThenInclude(g => g.Albums)
                                        .Include(i => i.SchemaGroups)
                                            .ThenInclude(g => g.Games)
                                                .ThenInclude(g => g.Videos);
            return schemaItems;
        }


        public IEnumerable<EventSchemaItem> GetAllWithGames(int eventId)
        {
            
            //var schemaItems = _dbContext.EventSchemaItems.Where(s => s.EventId == eventId)
            //                            .Include(i => i.SchemaGroups)
            //                                .ThenInclude(g => g.Games)
            //                                    .ThenInclude(g => g.HomeTeam)
            //                            .Include(i => i.SchemaGroups)
            //                                .ThenInclude(g => g.Games)
            //                                    .ThenInclude(g => g.VisitorTeam);

            var temp3 = (from esi in _dbContext.EventSchemaItems
                         where esi.EventId == eventId
                         join sg in _dbContext.SchemaGroups on esi.Id equals sg.EventSchemaItemId into gsg
                         from subsg in gsg.DefaultIfEmpty()
                         join game in _dbContext.Games on subsg.Id equals game.SchemaGroupId into ggame
                         from subgame in ggame.DefaultIfEmpty()
                         join hteam in _dbContext.Teams on subgame.HomeTeamId equals hteam.Id into ghteam
                         from subhteam in ghteam.DefaultIfEmpty()
                         join vteam in _dbContext.Teams on subgame.VisitorTeamId equals vteam.Id into gvteam
                         from subvteam in gvteam.DefaultIfEmpty()
                         select new
                         {
                             esi,subsg,subgame,subhteam,subvteam
                         }
                         );

            var temp4 = temp3.GroupBy(i => i.esi)
                            .Select(esi => new EventSchemaItem
                            {
                                Id = esi.Key.Id,
                                Order = esi.Key.Order,
                                EventId = esi.Key.EventId,
                                SchemaItem = esi.Key.SchemaItem,
                                SchemaGroups = esi.GroupBy(esg => esg.subsg )
                                                .Select(esgs => new SchemaGroup
                                                {
                                                    Id = esgs.Key.Id,
                                                    GroupName = esgs.Key.GroupName,
                                                    EventSchemaItemId = esgs.Key.EventSchemaItemId,
                                                    Games = esgs.Select(game => new Game
                                                    {
                                                        Id = game.subgame.Id,
                                                        Name = game.subgame.Name,
                                                        StartDate = game.subgame.StartDate,
                                                        AdditionalInfo = game.subgame.AdditionalInfo,
                                                        RunsVisitor = game.subgame.RunsVisitor,
                                                        RunsHome = game.subgame.RunsHome,
                                                        PlacedAt = game.subgame.PlacedAt,
                                                        HalfinningsPlayed = game.subgame.HalfinningsPlayed,
                                                        GameStatus = game.subgame.GameStatus,
                                                        PointsVisitor = game.subgame.PointsVisitor,
                                                        PointsHome = game.subgame.PointsHome,
                                                        Tour = game.subgame.Tour,
                                                        ConditionVisitor = game.subgame.ConditionVisitor,
                                                        ConditionHome = game.subgame.ConditionHome,
                                                        SchemaGroupId = game.subgame.SchemaGroupId,
                                                        HomeTeamId = game.subgame.HomeTeamId,
                                                        VisitorTeamId = game.subgame.VisitorTeamId,
                                                        HomeTeam = game.subhteam,
                                                        VisitorTeam = game.subvteam
                                                    }).ToList()
                                                }).ToList()
                            });
            
            return temp4;
        }

    }
}
