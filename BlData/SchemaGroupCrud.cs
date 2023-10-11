using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
    public class SchemaGroupCrud : ICrud<SchemaGroup>
    {
        private readonly BaseballUaDbContext _dbContext;

        public SchemaGroupCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(SchemaGroup item)
        {
            _dbContext.Add(item);
            _dbContext.SaveChanges();
        }

        public void Delete(SchemaGroup item)
        {
            throw new NotImplementedException();
        }

        public SchemaGroup Get(int itemId)
        {
            return _dbContext.SchemaGroups.Where(g => g.Id == itemId).Include(g => g.EventSchemaItem).FirstOrDefault();
        }

        public IEnumerable<SchemaGroup> GetAll()
        {
            return _dbContext.SchemaGroups.Include(g => g.EventSchemaItem);
        }

        public IEnumerable<SchemaGroup> GetAll(int eventSchemaItemId)
        {
            
            return _dbContext.SchemaGroups.Where(s => s.EventSchemaItemId == eventSchemaItemId).Include(g => g.EventSchemaItem);
        }

        public void Update(SchemaGroup item)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<SchemaGroup> GetAllForSchema(int eventSchemaItemId)
        //{

        //    return _dbContext.SchemaGroups.Where(s => s.EventSchemaItemId == eventSchemaItemId).Include(g => g.EventSchemaItem);
        //}
    }
}
