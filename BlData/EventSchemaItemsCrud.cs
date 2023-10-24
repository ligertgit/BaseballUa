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
        public IEnumerable<EventSchemaItem> GetAllWithGames(int eventId)
        {
            //var schemaItems = _dbContext.EventSchemaItems.Where(s => s.EventId == eventId)
            //                            .Include(i => i.SchemaGroups)
            //                                .ThenInclude(g => g.Games)
            //                                    .ThenInclude(g => g.HomeTeam)
            //                            .Include(i => i.SchemaGroups)
            //                                .ThenInclude(g => g.Games)
            //                                    .ThenInclude(g => g.VisitorTeam);

            //var temp = (from esi in _dbContext.EventSchemaItems
            //            where esi.EventId == eventId
            //            join sg in _dbContext.SchemaGroups on esi.Id equals sg.EventSchemaItemId
            //            join game in _dbContext.Games on sg.Id equals game.SchemaGroupId
            //            join hteam in _dbContext.Teams on game.HomeTeamId equals hteam.Id
            //            join vteam in _dbContext.Teams on game.VisitorTeamId equals vteam.Id
            //            select new
            //            {
            //                Id = esi.Id,
            //                Order = esi.Order,
            //                EventId = esi.EventId,
            //                SchemaItem = esi.SchemaItem,
            //                sgId = sg.Id,
            //                sgGroupName = sg.GroupName,
            //                sgEventSchemaItemId = sg.EventSchemaItemId,
            //                //sgEventSchemaItem = sg.EventSchemaItem,
            //                gameId = game.Id,
            //                gameName = game.Name,
            //                gameStartDate = game.StartDate,
            //                gameAdditionalInfo = game.AdditionalInfo,
            //                gameRunsVisitor = game.RunsVisitor,
            //                gameRunsHome = game.RunsHome,
            //                gamePlacedAt = game.PlacedAt,
            //                gameHalfinningsPlayed = game.HalfinningsPlayed,
            //                gameGameStatus = game.GameStatus,
            //                gamePointsVisitor = game.PointsVisitor,
            //                gamePointsHome = game.PointsHome,
            //                gameTour = game.Tour,
            //                gameConditionVisitor = game.ConditionVisitor,
            //                gameConditionHome = game.ConditionHome,
            //                gameSchemaGroupId = game.SchemaGroupId,
            //                gameHomeTeamId = game.HomeTeamId,
            //                gameVisitorTeamId = game.VisitorTeamId,
            //                hteamId = hteam.Id,
            //                hteamSportType = hteam.SportType,
            //                hteamName = hteam.Name,
            //                hteamDescription = hteam.Description,
            //                hteamFnameLogoSmall = hteam.FnameLogoSmall,
            //                hteamFnameLogoBig = hteam.FnameLogoBig,
            //                hteamIsTemp = hteam.IsTemp,
            //                hteamClubId = hteam.ClubId,
            //                vteamId = vteam.Id,
            //                vteamSportType = vteam.SportType,
            //                vteamName = vteam.Name,
            //                vteamDescription = vteam.Description,
            //                vteamFnameLogoSmall = vteam.FnameLogoSmall,
            //                vteamFnameLogoBig = vteam.FnameLogoBig,
            //                vteamIsTemp = vteam.IsTemp,
            //                vteamClubId = vteam.ClubId
            //            }
            //            );


            //var temp2 = temp.GroupBy(i => new { i.Id, i.Order, i.EventId, i.SchemaItem })
            //                .Select(esi => new EventSchemaItem
            //                {
            //                    Id = esi.Key.Id,
            //                    Order = esi.Key.Order,
            //                    EventId = esi.Key.EventId,
            //                    SchemaItem = esi.Key.SchemaItem,
            //                    SchemaGroups = esi.GroupBy(esg => new { esg.sgId, esg.sgGroupName, esg.sgEventSchemaItemId })
            //                                    .Select(esgs => new SchemaGroup
            //                                    {
            //                                        Id = esgs.Key.sgId,
            //                                        GroupName = esgs.Key.sgGroupName,
            //                                        EventSchemaItemId = esgs.Key.sgEventSchemaItemId,
            //                                        Games = esgs.Select(game => new Game 
            //                                                    {
            //                                                        Id = game.gameId,
            //                                                        Name = game.gameName,
            //                                                        StartDate = game.gameStartDate,
            //                                                        AdditionalInfo = game.gameAdditionalInfo,
            //                                                        RunsVisitor = game.gameRunsVisitor,
            //                                                        RunsHome = game.gameRunsHome,
            //                                                        PlacedAt = game.gamePlacedAt,
            //                                                        HalfinningsPlayed = game.gameHalfinningsPlayed,
            //                                                        GameStatus = game.gameGameStatus,
            //                                                        PointsVisitor = game.gamePointsVisitor,
            //                                                        PointsHome = game.gamePointsHome,
            //                                                        Tour = game.gameTour,
            //                                                        ConditionVisitor = game.gameConditionVisitor,
            //                                                        ConditionHome = game.gameConditionHome,
            //                                                        SchemaGroupId = game.gameSchemaGroupId,
            //                                                        HomeTeamId = game.gameHomeTeamId,
            //                                                        VisitorTeamId = game.gameVisitorTeamId,
            //                                                        HomeTeam = new Team 
            //                                                                    {
            //                                                                        Id = game.hteamId,
            //                                                                        SportType = game.hteamSportType,
            //                                                                        Name = game.hteamName,
            //                                                                        Description = game.hteamDescription,
            //                                                                        FnameLogoSmall = game.hteamFnameLogoSmall,
            //                                                                        FnameLogoBig = game.hteamFnameLogoBig,
            //                                                                        IsTemp = game.hteamIsTemp,
            //                                                                        ClubId = game.hteamClubId,
            //                                                                    },
            //                                                        VisitorTeam = new Team
            //                                                        {
            //                                                            Id = game.vteamId,
            //                                                            SportType = game.vteamSportType,
            //                                                            Name = game.vteamName,
            //                                                            Description = game.vteamDescription,
            //                                                            FnameLogoSmall = game.vteamFnameLogoSmall,
            //                                                            FnameLogoBig = game.vteamFnameLogoBig,
            //                                                            IsTemp = game.vteamIsTemp,
            //                                                            ClubId = game.vteamClubId,
            //                                                        }
            //                                        }).ToList()
            //                                    }).ToList()
            //                });


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
                             //Id = esi.Id,
                             //Order = esi.Order,
                             //EventId = esi.EventId,
                             //SchemaItem = esi.SchemaItem,
                             //sgId = subsg.Id,
                             //sgGroupName = subsg.GroupName,
                             //sgEventSchemaItemId = subsg.EventSchemaItemId,
                             ////sgEventSchemaItem = sg.EventSchemaItem,
                             //gameId = subgame.Id,
                             //gameName = subgame.Name,
                             //gameStartDate = subgame.StartDate,
                             //gameAdditionalInfo = subgame.AdditionalInfo,
                             //gameRunsVisitor = subgame.RunsVisitor,
                             //gameRunsHome = subgame.RunsHome,
                             //gamePlacedAt = subgame.PlacedAt,
                             //gameHalfinningsPlayed = subgame.HalfinningsPlayed,
                             //gameGameStatus = subgame.GameStatus,
                             //gamePointsVisitor = subgame.PointsVisitor,
                             //gamePointsHome = subgame.PointsHome,
                             //gameTour = subgame.Tour,
                             //gameConditionVisitor = subgame.ConditionVisitor,
                             //gameConditionHome = subgame.ConditionHome,
                             //gameSchemaGroupId = subgame.SchemaGroupId,
                             //gameHomeTeamId = subgame.HomeTeamId,
                             //gameVisitorTeamId = subgame.VisitorTeamId,
                             //hteamId = subhteam.Id,
                             //hteamSportType = subhteam.SportType,
                             //hteamName = subhteam.Name,
                             //hteamDescription = subhteam.Description,
                             //hteamFnameLogoSmall = subhteam.FnameLogoSmall,
                             //hteamFnameLogoBig = subhteam.FnameLogoBig,
                             //hteamIsTemp = subhteam.IsTemp,
                             //hteamClubId = subhteam.ClubId,
                             //vteamId = subvteam.Id,
                             //vteamSportType = subvteam.SportType,
                             //vteamName = subvteam.Name,
                             //vteamDescription = subvteam.Description,
                             //vteamFnameLogoSmall = subvteam.FnameLogoSmall,
                             //vteamFnameLogoBig = subvteam.FnameLogoBig,
                             //vteamIsTemp = subvteam.IsTemp,
                             //vteamClubId = subvteam.ClubId
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
                                                        //HomeTeam = new Team
                                                        //{
                                                        //    Id = game.subhteam.Id,
                                                        //    SportType = game.subhteam.SportType,
                                                        //    Name = game.subhteam.Name,
                                                        //    Description = game.subhteam.Description,
                                                        //    FnameLogoSmall = game.subhteam.FnameLogoSmall,
                                                        //    FnameLogoBig = game.subhteam.FnameLogoBig,
                                                        //    IsTemp = game.subhteam.IsTemp,
                                                        //    ClubId = game.subhteam.ClubId,
                                                        //},
                                                        //VisitorTeam = new Team
                                                        //{
                                                        //    Id = game.subvteam.Id,
                                                        //    SportType = game.subvteam.SportType,
                                                        //    Name = game.subvteam.Name,
                                                        //    Description = game.subvteam.Description,
                                                        //    FnameLogoSmall = game.subvteam.FnameLogoSmall,
                                                        //    FnameLogoBig = game.subvteam.FnameLogoBig,
                                                        //    IsTemp = game.subvteam.IsTemp,
                                                        //    ClubId = game.subvteam.ClubId,
                                                        //}
                                                    }).ToList()
                                                }).ToList()
                            });
            //var temp2 = temp.Select(i => new EventSchemaItem
            //{
            //    Id = i.Id,
            //    Order = i.Order,
            //    EventId = i.EventId,
            //    SchemaGroups = i.GroupBy(g => new { g.schemaGroup.Id, g.GroupName, g.EventSchemaItemId, g.EventSchemaItem })

            //});

            //List<Continent> List = MyRepository.GetList<GetAllCountriesAndCities>("EXEC sp_GetAllCountriesAndCities")

            //Result.GroupBy(x => x.ContinentName).Select(g => g.GroupBy(x => x.CountryName))

            //.GroupBy(x => x.ContinentName)
            //.Select(g => new Continent
            //{
            //    ContinentName = g.Key,
            //    Countries = g.GroupBy(x => x.CountryName)
            //                 .Select(cg => new Country
            //                 {
            //                     CountryName = cg.Key,
            //                     Cities = cg.GroupBy(x => x.CityName)
            //                                .Select(cityG => new City { CityName = cityG.Key })
            //                                .ToList()
            //                 })
            //                 .ToList()
            //})
            //.ToList();


            //var temp = _dbContext.Categories.Include(c => c.Tournaments);
            //var temp = (from category in _dbContext.Categories
            //            join tournament in _dbContext.Tournaments on category.Id equals tournament.CategoryId into ct
            //            //select new Category
            //            //{
            //            //    Id = category.Id,
            //            //    Name = category.Name,
            //            //    ShortName = category.ShortName,
            //            //    Tournaments = ct.ToList(),
            //            //}
            //    );


            //return schemaItems;
            return temp4;
        }

    }
}
