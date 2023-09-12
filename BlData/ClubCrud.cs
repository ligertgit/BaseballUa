using BaseballUa.Data;
using BaseballUa.Models;

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
            var clubWithCountry = _dbContext.Clubs.First(c => c.Id == itemId);
            clubWithCountry.Country = _dbContext.Countries.First(c => c.Id == clubWithCountry.CountryId);

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

        public void Update(Club item)
        {
            throw new NotImplementedException();
        }
    }
}
