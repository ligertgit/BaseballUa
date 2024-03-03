using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.BlData
{
    public class CountryCrud : ICrud<Country>
    {
        private readonly BaseballUaDbContext _dbContext;

        public CountryCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Add(Country item)
        {
            _dbContext.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Country item)
        {
            throw new NotImplementedException();
        }

        public Country? Get(int itemId)
        {
            return _dbContext.Countries.FirstOrDefault(c => c.Id == itemId);
        }

        public IEnumerable<Country> GetAll()
        {
            return _dbContext.Countries;
        }

        public void Update(Country item)
        {
            _dbContext.Countries.Update(item);
            _dbContext.SaveChanges();
        }


        public List<SelectListItem> GetSelectItemList()
        {
            var countriesSL = new List<SelectListItem>();
            countriesSL = _dbContext.Countries.Select(c => new SelectListItem
                                                        {
                                                            Text = c.Name,
                                                            Value = c.Id.ToString()
                                                        }).ToList();

            return countriesSL;
        }
    }
}
