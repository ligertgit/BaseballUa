using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.Models.Custom;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
            throw new NotImplementedException();
        }

        public Team Get(int itemId)
        {
            return _dbContext.Teams.Where(t => t.Id == itemId)
                                        .Include(t => t.Club)
                                            .ThenInclude(c => c.Country)
                                        .FirstOrDefault();
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

        //public IEnumerable<Team> GetAllForClub(int clubId)
        //{
        //    return _dbContext.Teams.Where(t => t.ClubId == clubId)
        //                                .Include(t => t.Club)
        //                                    .ThenInclude(c => c.Country);
        //}

        public void Update(Team item)
        {
            throw new NotImplementedException();
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
