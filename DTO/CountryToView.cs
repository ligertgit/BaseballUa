using BaseballUa.Models;
using BaseballUa.ViewModels;

namespace BaseballUa.DTO
{
    public class CountryToView
    {
        //private readonly BaseballUaDbContext _dbContext;

        //public CountryToView(BaseballUaDbContext dbContext)
        //{
        //    _dbContext = dbContext;   
        //}


        public CountryViewModel Convert(Country countryDAL, bool doSubConvert = true)
        {
            var countryVL = new CountryViewModel();
            countryVL.Id = countryDAL.Id;
            countryVL.Name = countryDAL.Name;
            countryVL.ShortName = countryDAL.ShortName;
            countryVL.FnameFlagSmall = countryDAL.FnameFlagSmall;
            countryVL.FnameFlagBig = countryDAL.FnameFlagBig;
            if ( doSubConvert && countryDAL.Clubs != null) 
            { 
                countryVL.Clubs = new ClubToView().ConvertAll(countryDAL.Clubs.ToList(), false);
			}

			return countryVL;
        }

        public Country ConvertBack(CountryViewModel countryVL) 
        {
            var countryDAL = new Country();
            countryDAL.Id = countryVL.Id;
            countryDAL.Name = countryVL.Name;
            countryDAL.ShortName = countryVL.ShortName;
            countryDAL.FnameFlagSmall = countryVL.FnameFlagSmall;
            countryDAL.FnameFlagBig = countryVL.FnameFlagBig;

            return countryDAL;
        }

        public List<CountryViewModel> ConvertAll(List<Country> countriesDAL, bool doSubConvert = true)
        {
            var countriesVL = new List<CountryViewModel>();

            foreach (var countryDAL in countriesDAL) 
            {
                countriesVL.Add(Convert(countryDAL, doSubConvert));
            }

            return countriesVL;

        }

        public CountryViewModel CreateEmpty()
        {
            var countryVL = new CountryViewModel();
            countryVL.Id = 0;
            countryVL.Name = string.Empty;
            countryVL.ShortName = string.Empty;
            countryVL.FnameFlagSmall = string.Empty;
            countryVL.FnameFlagBig = string.Empty;

            return countryVL;
        }
    }
}
