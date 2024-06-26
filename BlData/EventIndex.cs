﻿using BaseballUa.Data;
using BaseballUa.ViewModels.Custom;

namespace BaseballUa.BlData
{
    public class EventIndex
    {
        private readonly BaseballUaDbContext _dbContext;

        public EventIndex(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<EventForIndexVM> GetMonthFilters(int monthShift, Filters? filters)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(monthShift);
            var endDate = startDate.AddMonths(1);
            var fixxedFilters = new Filters().FixForSelect(filters);

            var result = (from eventt in _dbContext.Events
                          join tournament in _dbContext.Tournaments on eventt.TournamentId equals tournament.Id
                          join category in _dbContext.Categories on tournament.CategoryId equals category.Id
                          where (
                                    (
                                        (eventt.StartDate >= startDate && eventt.StartDate <= endDate)
                                        || (eventt.EndDate >= startDate && eventt.EndDate <= endDate)
                                    )
                                    || (eventt.StartDate <= startDate && eventt.EndDate >= endDate)
                                 )
                                 &&
                                 (
                                    (
                                        (
                                            category.ShortName == fixxedFilters.chkU10()
                                            || category.ShortName == fixxedFilters.chkU12()
                                            || category.ShortName == fixxedFilters.chkU15()
                                            || category.ShortName == fixxedFilters.chkU18()
                                            || category.ShortName == fixxedFilters.chkAdult()
                                            || category.ShortName == fixxedFilters.chkVeteran()
                                        )
                                        &&
                                        (
                                            tournament.Sport == fixxedFilters.chkBaseball()
                                            || tournament.Sport == fixxedFilters.chkSoftball()
                                        )
                                    )
                                    || ( tournament.IsFun == fixxedFilters.Fun && tournament.IsFun )
                                 )
                          select new EventForIndexVM
                          {
                              EventIndexViewModelId = eventt.Id,
                              Year = eventt.Year,
                              StartDate = eventt.StartDate,
                              EndDate = eventt.EndDate,
                              TournamentId = eventt.TournamentId,
                              Name = tournament.Name,
                              Sport = tournament.Sport,
                              Description = tournament.Description,
                              IsAnual = tournament.IsAnual,
                              IsInternational = tournament.IsInternational,
                              IsOfficial = tournament.IsOfficial,
                              IsFun = tournament.IsFun,
                              CategoryId = tournament.CategoryId,
                              CategoryShortName = category.ShortName
                          }).ToList();

            return result;
        }
    }
}
