using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
    public class ClubCrud : ICrud<Club>
    {
        private readonly BaseballUaDbContext _dbContext;

        public ClubCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Club item)
        {
            _dbContext.Clubs.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Club item)
        {
            throw new NotImplementedException();
        }

        public Club Get(int itemId)
        {
            var clubWithCountry = _dbContext.Clubs.Where(c => c.Id == itemId).Include(c => c.Country).FirstOrDefault();
            //clubWithCountry.Country = _dbContext.Countries.First(c => c.Id == clubWithCountry.CountryId);

            return clubWithCountry;
            //return _dbContext.Clubs.First(c => c.Id == itemId);
        }

        public IEnumerable<Club> GetAll()
        {
            var clubsWithCountry = (from club in _dbContext.Clubs
                                    join country in _dbContext.Countries on club.CountryId equals country.Id
                                    select new Club
                                    {
                                        Id = club.Id,
                                        Name = club.Name,
                                        Description = club.Description,
                                        Invitation = club.Invitation,
                                        FnameLogoSmall = club.FnameLogoSmall,
                                        FnameLogoBig = club.FnameLogoBig,
                                        CountryId = club.Id,
                                        Country = new Country
                                        {
                                            Id = country.Id,
                                            Name = country.Name,
                                            ShortName = country.ShortName,
                                            FnameFlagBig = country.FnameFlagBig,
                                            FnameFlagSmall = country.FnameFlagSmall,
                                        }
                                    });

            return clubsWithCountry;
            //return _dbContext.Clubs;
        }

        public IEnumerable<Club> GetAllWithTeams()
        {
            //var result = (from club in _dbContext.Clubs.Include(c => c.Staffs)
            //              join team in _dbContext.Teams on club.Id equals team.ClubId into subTeams
            //              from gTeams in subTeams.DefaultIfEmpty()
            //              group gTeams by club into g
            //              select new Club
            //              {
            //                  Id = g.Key.Id,
            //                  Name = g.Key.Name,
            //                  Description = g.Key.Description,
            //                  Invitation = g.Key.Invitation,
            //                  FnameLogoSmall = g.Key.FnameLogoSmall,
            //                  FnameLogoBig = g.Key.FnameLogoBig,
            //                  CountryId = g.Key.CountryId,
            //                  Staffs = g.Key.Staffs.ToList(),
            //                  Teams = g.ToList()
            //              }
            //              );
            var result = _dbContext.Clubs.Where(c => c.Id != Constants.EuroClubId).Include(c => c.Teams).Include(c => c.Staffs);

            return result;
        }


        public void Update(Club item)
        {
            _dbContext.Clubs.Update(item);
            _dbContext.SaveChanges();
        }

        public List<SelectListItem> GetSelectItemList()
        {
            var sList = _dbContext.Clubs.OrderBy(c => c.CountryId).ThenBy(c => c.Name).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return sList;
        }
    }
}
