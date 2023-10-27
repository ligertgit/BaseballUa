using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
    public class StaffsCrud : ICrud<Staff>
    {
        private readonly BaseballUaDbContext _dbContext;

        public StaffsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Staff item)
        {
            _dbContext.Staffs.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(Staff item)
        {
            throw new NotImplementedException();
        }

        public Staff Get(int itemId)
        {
            return _dbContext.Staffs.Where(s => s.Id == itemId).Include(x => x.Club).FirstOrDefault();
        }

        public IEnumerable<Staff> GetAll()
        {
            return _dbContext.Staffs.Include(s => s.Club);
        }

        public IEnumerable<Staff> GetAll(int clubId)
        {
            return _dbContext.Staffs.Where(s => s.ClubId == clubId).Include(s => s.Club);
        }

        public void Update(Staff item)
        {
            _dbContext.Staffs.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
