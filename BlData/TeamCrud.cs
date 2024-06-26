﻿using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
    public class TeamCrud : ICrud<Team>
    {
        private readonly BaseballUaDbContext _dbContext;

        public TeamCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public void Add(Team item)
        {
            _dbContext.Teams.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Team item)
        {
            _dbContext.Teams.Remove(item);
            _dbContext.SaveChanges();
        }

        public Team Get(int itemId)
        {
            if (itemId > 0)
            {
                return _dbContext.Teams.Where(t => t.Id == itemId)
                            .Include(t => t.Club)
                                .ThenInclude(c => c.Country)
                            .FirstOrDefault();
            }

            return null;
        }

        public IEnumerable<Team> GetAll()
        {
            return _dbContext.Teams.Include(t => t.Club)
                                            .ThenInclude(c => c.Country);
        }

        public IEnumerable<Team> GetAll(int clubId = 0)
        {
            if (clubId == 0)
            {
                return GetAll();
            }
            return _dbContext.Teams.Where(t => t.ClubId == clubId)
                            .Include(t => t.Club)
                                .ThenInclude(c => c.Country);
        }

        public IEnumerable<int>? GetIds(int clubId = 0)
        {
            if (clubId <= 0) return null;
            return _dbContext.Teams.Where(t => t.ClubId == clubId).Select(t => t.Id);
        }

        public IEnumerable<Team> GetEventTeams(int? eventId)
        {
            var eventTeams = new List<Team>();
            if (eventId != null)
            {
                eventTeams = (
                              from team in _dbContext.Teams
                              from game in _dbContext.Games where (game.HomeTeamId == team.Id || game.VisitorTeamId == team.Id)
                              from eventGroup in _dbContext.SchemaGroups where (game.SchemaGroupId == eventGroup.Id)
                              from eventItem in _dbContext.EventSchemaItems where (eventGroup.EventSchemaItemId == eventItem.Id)
                              where (eventItem.EventId == eventId)
                              select team
                              )
                              .Distinct()
                              .ToList();
            }

            return eventTeams;
        }

        public IEnumerable<Team> GetEventToTeam(int eventId)
        {
            var result = (from teams in _dbContext.Teams
                          from ett in _dbContext.EventToTeams where (teams.Id == ett.TeamId && ett.EventId == eventId)
                          select teams).Distinct().Include(t => t.Club).ThenInclude(c => c.Country);
            return result;
        }


        //public IEnumerable<Team> GetAllForClub(int clubId)
        //{
        //    return _dbContext.Teams.Where(t => t.ClubId == clubId)
        //                                .Include(t => t.Club)
        //                                    .ThenInclude(c => c.Country);
        //}

        public IEnumerable<Game> GetHomeGames(int teamId)
        {
            return _dbContext.Games.Where(g => g.HomeTeamId == teamId)
                                   .Where(g => (g.StartDate > DateTime.Now.AddMonths(-10) && g.StartDate < DateTime.Now.AddMonths(10)))
                                   .Include(g => g.HomeTeam)
                                   .Include(g => g.VisitorTeam);
        }

        public IEnumerable<Game> GetVisitorGames(int teamId)
        {
            return _dbContext.Games.Where(g => g.VisitorTeamId == teamId)
                                   .Where(g => (g.StartDate > DateTime.Now.AddMonths(-10) && g.StartDate < DateTime.Now.AddMonths(10)))
                                   .Include(g => g.HomeTeam)
                                   .Include(g => g.VisitorTeam);
        }

        public IEnumerable<Video> GetVideos(int teamId)
        {
            return _dbContext.Videos.Where(v => v.TeamId == teamId);
        }

        public IEnumerable<News> GetNews(int teamId) 
        { 
            return _dbContext.News.Where(n => n.TeamId == teamId);
        }

        public IEnumerable<Album> GetAlbums(int teamId) 
        { 
            return _dbContext.Albums.Where(a => a.TeamId == teamId);
        }

        public IEnumerable<Player> GetPlayers(int teamId) 
        {
            return _dbContext.Players.Where(p => p.TeamId == teamId);
        }

        //public TeamFullDetail GetFullDetails(int teamId)
        //{
        //var result = new TeamFullDetail();

        //result.Team = _dbContext.Teams.Include(t => t.Club).Where(t => t.Id == teamId).FirstOrDefault();
        //if (result.Team != null)
        //{

        //}

        //var temp = from ttt in _dbContext.Teams.Include(t => t.Club).Where(t => t.Id == teamId)
        //           join phv in ({ teamId, list(hgames), list(vgames), list(players) }) into g
        //           select new
        //           {
        //               team = ttt,
        //               players = g.players,
        //               homeG = g.homeG,
        //               visitorG = g.visitorG
        //           };

        //teamId, list(hgames), list(vgames), list(players)

        //var result = from team2 in
        //                 (from team in _dbContext.Teams
        //                  join homeGame in _dbContext.Games on team.Id equals homeGame.HomeTeamId into ght
        //                  select new
        //                  {
        //                      Id = team.Id,
        //                      Name = team.Name,
        //                      hgames = ght.ToList(),
        //                  }).ToList()
        //             join visitorGame in _dbContext.Games on team2.Id equals visitorGame.VisitorTeamId into gvt
        //             select new
        //             {
        //                 Id = team2.Id,
        //                 name = team2.Name,
        //                 hgames = team2.hgames,
        //                 vgames = gvt.ToList()
        //             };

        //var result = from team in _dbContext.Teams
        //             join homeGame in _dbContext.Games on team.Id equals homeGame.HomeTeamId into subhg
        //             from ghg in subhg.DefaultIfEmpty()
        //             join visitorGame in _dbContext.Games on team.Id equals visitorGame.VisitorTeamId into subvt
        //             from gvg in subvt.DefaultIfEmpty()
        //             group gvg by team into zzz1
        //             group ghg by team into zzz2
        //             select new
        //             {
        //                 Id = team.Id,
        //                 name = team.Name,
        //                 hgames = zzz2,
        //                 vgames = zzz1
        //             }



        ////join visitorGame in _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam) on team.Id equals visitorGame.VisitorTeamId into gvt
        //select new
        //{
        //    Team = team,
        //    TeamHomeGames = ght.ToList(),
        //    //Team.HomeGames = gvt

        //};

        //var result = from team in
        //                  (from team in _dbContext.Teams //.Include(t => t.Club)
        //                   join homeGame in _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam) on team.Id equals homeGame.HomeTeamId
        //                   //join visitorGame in _dbContext.Games on team.Id equals visitorGame.VisitorTeamId
        //                   where team.Id == teamId
        //                   group homeGame by team into g
        //                   select new Team
        //                   {
        //                       Id = g.Key.Id,
        //                       //Club = g.Key.Club,
        //                       ClubId = g.Key.ClubId,
        //                       Description = g.Key.Description,
        //                       FnameLogoBig = g.Key.FnameLogoBig,
        //                       FnameLogoSmall = g.Key.FnameLogoSmall,
        //                       HomeGames = g.ToList(),
        //                       IsTemp = g.Key.IsTemp,
        //                       Name = g.Key.Name,
        //                       SportType = g.Key.SportType
        //                   }
        //                  )
        //             join visitorTeam in _dbContext.Games.Include(g => g.HomeTeam).Include(g => g.VisitorTeam) on team.Id equals visitorTeam.VisitorTeamId
        //             where team.Id == teamId
        //             group visitorTeam by team into g
        //             select new Team
        //             {
        //                 Id = g.Key.Id,
        //                 //Club = g.Key.Club,
        //                 ClubId = g.Key.ClubId,
        //                 Description = g.Key.Description,
        //                 FnameLogoBig = g.Key.FnameLogoBig,
        //                 FnameLogoSmall = g.Key.FnameLogoSmall,
        //                 HomeGames = g.Key.HomeGames,
        //                 VisitorGames = g.ToList(),
        //                 IsTemp = g.Key.IsTemp,
        //                 Name = g.Key.Name,
        //                 SportType = g.Key.SportType
        //             };

        //    return new TeamFullDetail();
        //}

        public List<SelectListItem> GetSelectItemList(bool uaOnly = false)
        {
            //var teamsSL = new List<SelectListItem>();
            var teamsSL = _dbContext.Teams.Include(t => t.Club).Where(t => !uaOnly || t.Club.CountryId == Constants.UaCountryId).OrderBy(t => t.ClubId).ThenBy(t => t.SportType).ThenBy(t => t.Name).Select(c => new SelectListItem
                                    {
                                        Text = ((Enums.SportType)c.SportType).ToString() + " - " + c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList();
            return teamsSL;
        }

        //public List<SelectListItem> GetSelectItemList(bool uaOnly = false)
        //{
        //    var teamsSL = new List<SelectListItem>();
        //    teamsSL = _dbContext.Teams.Where(t => !uaOnly || Constants.UaClubIdList.Contains(t.ClubId)).OrderBy(t => t.ClubId).ThenBy(t => t.SportType).ThenBy(t => t.Name).Select(c => new SelectListItem
        //    {
        //        Text = ((Enums.SportType)c.SportType).ToString() + " - " + c.Name,
        //        Value = c.Id.ToString()
        //    }).ToList();
        //    return teamsSL;
        //}

        public void Update(Team item)
        {
            _dbContext.Teams.Update(item);
            _dbContext.SaveChanges();
        }

        //public IEnumerable<TeamWithClubCountry> GetAllWithClubCountry()
        //{
        //    var teamsVL = (from teams in _dbContext.Teams
        //                   join clubs in _dbContext.Clubs on teams.ClubId equals clubs.Id
        //                   join countries in _dbContext.Countries on clubs.CountryId equals countries.Id
        //                   select new TeamWithClubCountry
        //                   {
        //                       Team = new Team
        //                       {
        //                           Id = teams.Id,
        //                           ClubId = teams.ClubId,
        //                           Name = teams.Name,
        //                           Description = teams.Description,
        //                           SportType = teams.SportType,
        //                           FnameLogoSmall = teams.FnameLogoSmall,
        //                           FnameLogoBig = teams.FnameLogoBig,
        //                           IsTemp = teams.IsTemp
        //                       },
        //                       Club = new Club
        //                       {
        //                           Id = clubs.Id,
        //                           Name = clubs.Name,
        //                           Description = clubs.Description,
        //                           FnameLogoSmall = clubs.FnameLogoSmall,
        //                           FnameLogoBig = clubs.FnameLogoBig,
        //                           CountryId = clubs.CountryId
        //                       },
        //                       Country = new Country
        //                       { 
        //                           Id = countries.Id,
        //                           Name = countries.Name,
        //                           ShortName = countries.ShortName,
        //                           FnameFlagSmall = countries.FnameFlagSmall,
        //                           FnameFlagBig = countries.FnameFlagBig
        //                       }
        //                   }
        //                   );


        //    return teamsVL;
        //}
    }
}
