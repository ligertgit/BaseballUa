using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
	public class VideosCrud : ICrud<Video>
	{
		private readonly BaseballUaDbContext _dbContext;

        public VideosCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(Video item)
		{
			_dbContext.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(Video item)
		{
			throw new NotImplementedException();
		}

		public Video Get(int itemId)
		{
			return _dbContext.Videos.Where(v => v.Id == itemId)
						.Include(v => v.News)
						.Include(v => v.Category)
						.Include(v => v.Game)
							.ThenInclude(g => g.SchemaGroup)
							.ThenInclude(g => g.EventSchemaItem)
							.ThenInclude(i => i.Event)
							.ThenInclude(e => e.Tournament)
					.FirstOrDefault();
		}

		public IEnumerable<Video> GetAll()
		{
			return _dbContext.Videos
						.Include(v => v.News)
						.Include(v => v.Category)
						.Include(v => v.Game)
							.ThenInclude(g => g.HomeTeam)
						.Include(v => v.Game)
							.ThenInclude(g => g.VisitorTeam)
						.Include(v => v.Game)
							.ThenInclude(g => g.SchemaGroup)
							.ThenInclude(g => g.EventSchemaItem)
							.ThenInclude(i => i.Event)
							.ThenInclude(equals => equals.Tournament);
		}

		public void Update(Video item)
		{
			throw new NotImplementedException();
		}
	}
}
