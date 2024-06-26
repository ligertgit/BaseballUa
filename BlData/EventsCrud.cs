﻿using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static BaseballUa.Data.Enums;

namespace BaseballUa.BlData
{
    public class EventsCrud : ICrud<Event>
    {
        private readonly BaseballUaDbContext _dbContext;

        public EventsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Event item)
        {
            _dbContext.Events.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Event item)
        {
            throw new NotImplementedException();
        }

        public Event Get(int itemId)
        {
            var eventItem = _dbContext.Events.Where(e => e.Id == itemId)
                                                .OrderByDescending(e => e.Id)
                                                .Include(e => e.Tournament)
                                                    .ThenInclude(t => t.Category)
                                                .Include(e => e.EventToteams)
                                                //.Include(e => e.News)
                                                //    .ThenInclude(n => n.NewsTitlePhotos)
                                                //        .ThenInclude(t => t.Photo)
                                                .FirstOrDefault();

            return eventItem ?? new Event();
        }

        public IEnumerable<Event> GetAll()
        {
            var eventItem = _dbContext.Events.OrderByDescending(e => e.Id).Include(e => e.Tournament).ThenInclude(t => t.Category);
            //var eventItem = _dbContext.Events.Where().OrderBy(e => ((DateTime)e.StartDate - DateTime.Now).TotalMinutes).Include(e => e.Tournament).ThenInclude(t => t.Category);
            return eventItem;
        }

        public IEnumerable<Event> GetAll(SportType? sportType = null,
                                        int? categoryId = null,
                                        DateTime? firstDate = null,
                                        DateTime? lastDate = null,
                                        int? lastId = null,
                                        int? amount = null)
        {
            return _dbContext.Events.Where(e => (firstDate == null || lastDate == null
                                                    || (firstDate < e.StartDate && lastDate > e.StartDate)
                                                    || (lastDate > e.EndDate && firstDate < e.EndDate)
                                                    || (firstDate > e.StartDate && lastDate < e.EndDate)
                                                )
                                                && (lastId == null || e.Id < lastId)
                                                && (categoryId == null || e.Tournament.CategoryId == categoryId)
                                                && (sportType == null || e.Tournament.Sport == sportType)
                                          ).OrderByDescending(e => e.StartDate)
                                          .Take(amount == null ? Constants.DefaulEventAmount : (int)amount)
                                          .Include(e => e.Tournament)
                                                .ThenInclude(t => t.Category)
                                          .Include(e => e.News);
        }

        public IEnumerable<Event> GetAllFilteredActive(SportType sportType = SportType.NotDefined,
                                        bool includeAllFun = false,
                                        bool isOfficial = false,
                                        bool isInternational = false,
                                        bool isAnnual = false,
                                        IEnumerable<int>? categoryIds = null,
                                        DateTime? forDate = null,
                                        int amount = Constants.DefaulEventAmount)
        {
            var fixxedForDate = forDate ?? DateTime.Now.Date;

            var result = (from eventt in _dbContext.Events
                          join tour in _dbContext.Tournaments on eventt.TournamentId equals tour.Id
                          join cat in _dbContext.Categories on tour.CategoryId equals cat.Id
                          where (eventt.StartDate >= fixxedForDate.AddDays(-Constants.DefaulActiveEventDaysShift) && eventt.EndDate <= fixxedForDate.AddDays(Constants.DefaulActiveEventDaysShift))
                                && ((includeAllFun && tour.IsFun)
                                    || ((sportType == SportType.NotDefined
                                        || tour.Sport == sportType
                                        || tour.Sport == SportType.NotDefined)
                                       && (!isOfficial || tour.IsOfficial)
                                       && (!isInternational || tour.IsInternational)
                                       && (!isAnnual || tour.IsAnual)
                                       && (categoryIds.IsNullOrEmpty() || categoryIds.Any(c => c == tour.CategoryId))
                                       )
                                   )
                          select eventt)
                         .Distinct()
                         .OrderByDescending(n => n.StartDate)
                         .Take(amount)
                         .Include(n => n.Tournament)
                            .ThenInclude(t => t.Category);

            return result;
        }

        public IEnumerable<Event> GetAllFiltered(
                                out int countt,
                                SportType sportType = SportType.NotDefined,
                                bool includeAllFun = false,
                                bool isOfficial = false,
                                bool isInternational = false,
                                bool isAnnual = false,
                                IEnumerable<int>? teamIds = null,
                                IEnumerable<int>? categoryIds = null,
                                DateTime? newestDate = null,
                                DateTime? eldestDate = null,
                                int skip = 0,
                                int amount = Constants.DefaulEventAmount)
        {
            var fixxedNDate = newestDate ?? DateTime.Now.Date;
            var fixxedEDate = eldestDate ?? DateTime.MinValue;

            var result = (from eventt in _dbContext.Events
                          join tour in _dbContext.Tournaments on eventt.TournamentId equals tour.Id
                          join cat in _dbContext.Categories on tour.CategoryId equals cat.Id
                          join subEventItem in _dbContext.EventSchemaItems on eventt.Id equals subEventItem.EventId into gEventItems
                          from eventItem in gEventItems.DefaultIfEmpty()
                          join subItemGroup in _dbContext.SchemaGroups on eventItem.Id equals subItemGroup.EventSchemaItemId into gItemGroups
                          from itemGroup in gItemGroups.DefaultIfEmpty()
                          join subGame in _dbContext.Games on itemGroup.Id equals subGame.SchemaGroupId into gGames
                          from game in gGames.DefaultIfEmpty()
                          //where (eventt.StartDate >= fixxedForDate.AddDays(-Constants.DefaulActiveEventDaysShift) && eventt.EndDate <= fixxedForDate.AddDays(Constants.DefaulActiveEventDaysShift))
                          where (eventt.StartDate <= fixxedNDate && eventt.EndDate >= fixxedEDate)
                                && ((includeAllFun && tour.IsFun)
                                    || ((sportType == SportType.NotDefined
                                        || tour.Sport == sportType
                                        || tour.Sport == SportType.NotDefined)
                                       && (!isOfficial || tour.IsOfficial)
                                       && (!isInternational || tour.IsInternational)
                                       && (!isAnnual || tour.IsAnual)
                                       && (categoryIds.IsNullOrEmpty() || categoryIds.Any(c => c == tour.CategoryId))
                                       && (teamIds.IsNullOrEmpty()
                                            || teamIds.Any(t => t == game.HomeTeamId.GetValueOrDefault())
                                            || teamIds.Any(t => t == game.VisitorTeamId.GetValueOrDefault())

                                           )
                                       )
                                   )
                                
                          select eventt)
                         .Distinct()
                         .OrderByDescending(n => n.StartDate);
            countt = result.Count();

            return result.Skip(skip)
                         .Take(amount)
                         .Include(n => n.Tournament)
                            .ThenInclude(t => t.Category);
        }


        public IEnumerable<Event> GetAllForClub(int clubId)
        {
            var result = (from eventt in _dbContext.Events.Include(e => e.Tournament).ThenInclude(t => t.Category)
                         join eventItem in _dbContext.EventSchemaItems on eventt.Id equals eventItem.EventId
                         join schemaGroup in _dbContext.SchemaGroups on eventItem.Id equals schemaGroup.EventSchemaItemId
                         join game in _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam) on schemaGroup.Id equals game.SchemaGroupId
                         where (    (eventt.StartDate > DateTime.Now.AddMonths(-3) && eventt.StartDate < DateTime.Now.AddMonths(1)) &&
                                    (game.HomeTeam.ClubId == clubId || game.VisitorTeam.ClubId == clubId)
                               )
                         select eventt).Distinct();

            return result;
        }

        public IEnumerable<Event> GetAllForTeam(int teamId)
        {
            var result = (from eventt in _dbContext.Events.Include(e => e.Tournament).ThenInclude(t => t.Category)
                          join eventItem in _dbContext.EventSchemaItems on eventt.Id equals eventItem.EventId
                          join schemaGroup in _dbContext.SchemaGroups on eventItem.Id equals schemaGroup.EventSchemaItemId
                          join game in _dbContext.Games on schemaGroup.Id equals game.SchemaGroupId
                          where ((eventt.StartDate > DateTime.Now.AddMonths(-3) && eventt.StartDate < DateTime.Now.AddMonths(1)) &&
                                     (game.HomeTeamId == teamId || game.VisitorTeamId == teamId)
                                )
                          select eventt).Distinct();

            return result;
        }

        public int GetIdForGame(int gameId)
        {
            if (gameId < 1) return -1;
            var result = (from eventt in _dbContext.Events.Include(e => e.Tournament).ThenInclude(t => t.Category)
                          join eventItem in _dbContext.EventSchemaItems on eventt.Id equals eventItem.EventId
                          join schemaGroup in _dbContext.SchemaGroups on eventItem.Id equals schemaGroup.EventSchemaItemId
                          join game in _dbContext.Games on schemaGroup.Id equals game.SchemaGroupId
                          where game.Id == gameId
                          select eventt.Id).FirstOrDefault();

            return result;
        }

        public void Update(Event item)
        {
            _dbContext.Events.Update(item);
            _dbContext.SaveChanges();
        }


        //remove??
        public IEnumerable<Event> GetMonth(int monthShift)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(monthShift);
            var endDate = startDate.AddMonths(1);
            return _dbContext.Events.Where(i =>
                                ((i.StartDate >= startDate && i.StartDate <= endDate)
                                || (i.EndDate >= startDate && i.EndDate <= endDate))
                                || (i.StartDate <= startDate && i.EndDate >= endDate)
                                ).Include(e => e.Tournament).ThenInclude(t => t.Category);
        }

        public List<SelectListItem> GetSelectItemList(int amount = Constants.DefaulSelectListAmount)
        {
            if (amount < 1) 
            { 
                amount = Constants.DefaulSelectListAmount;
            }

            var eventSL = new List<SelectListItem>();
            eventSL = _dbContext.Events.OrderByDescending(e => e.StartDate).Take(amount).Include(e => e.Tournament)
                                    .Select(e => new SelectListItem
                                    {
                                        Text = e.Tournament.Name + " - " + e.Year.ToString(),
                                        Value = e.Id.ToString()
                                    }).ToList();

            return eventSL;
        }

        public List<Game> GetGames(int eventId)
        {
            var result = from eventSchema in _dbContext.EventSchemaItems
                         join eventGroup in _dbContext.SchemaGroups on eventSchema.Id equals eventGroup.EventSchemaItemId
                         join game in _dbContext.Games on eventGroup.Id equals game.SchemaGroupId
                         where eventSchema.EventId == eventId
                         select game;
            return result.Distinct().OrderBy(g => g.StartDate).ToList();
        }


    }
}
