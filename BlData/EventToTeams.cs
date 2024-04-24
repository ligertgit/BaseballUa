using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.BlData
{
	public class EventToTeamsCrud : ICrud<EventToTeams>
	{
		private readonly BaseballUaDbContext _dbContext;

        public EventToTeamsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(EventToTeams item)
		{
			_dbContext.Add(item);
			_dbContext.SaveChanges();
		}

		public void Add(int eventId, int teamId)
		{
            if (eventId > 0 && teamId > 0)
            {
				var eventToTeam = new EventToTeams { TeamId = teamId, EventId = eventId };
				_dbContext.EventToTeams.Add(eventToTeam);
				_dbContext.SaveChanges();
            }
        }
		public void Delete(EventToTeams item)
		{
			if(item != null) 
			{ 
				_dbContext.EventToTeams.Remove(item);
				_dbContext.SaveChanges();
			}
		}

        public void Delete(int eventToTeamsId)
        {
            _dbContext.EventToTeams.Where(ett => ett.Id == eventToTeamsId).ExecuteDelete();
            _dbContext.SaveChanges();
        }
        
        public void Delete(int eventId, int teamId)
        {
            _dbContext.EventToTeams.Where(ett => ett.EventId == eventId && ett.TeamId == teamId).ExecuteDelete();
            _dbContext.SaveChanges();
        }

        public void DeleteList(int eventId)
        {
            _dbContext.EventToTeams.Where(i => i.EventId == eventId).ExecuteDelete();
            _dbContext.SaveChanges();
        }


        //public void Delete(int eventId, int teamId)
        //{
        //	if(eventId != null && eventId > 0 && teamId != null && teamId > 0)
        //	{
        //		var EventToteam = _dbContext.EventToTeams.Where(ett => ett.EventId == eventId && ett.TeamId == teamId).FirstOrDefault();
        //		if (EventToteam != null) 
        //		{ 
        //			_dbContext.Remove(EventToteam);
        //			_dbContext.SaveChanges();
        //		}
        //	}
        //}

        public EventToTeams Get(int itemId)
		{
			return _dbContext.EventToTeams.Where(ntp => ntp.Id == itemId).FirstOrDefault();
		}

        public IEnumerable<EventToTeams> GetForEvent(int eventId)
        {
			return _dbContext.EventToTeams.Where(ntp => ntp.EventId == eventId);
        }

        public IEnumerable<EventToTeams> GetAll()
		{
			return _dbContext.EventToTeams;
		}



        public void Update(EventToTeams item)
		{
			throw new NotImplementedException();
		}
	}
}
