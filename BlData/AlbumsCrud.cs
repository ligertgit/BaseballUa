using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using static BaseballUa.Data.Enums;

namespace BaseballUa.BlData
{
	public class AlbumsCrud : ICrud<Album>
	{
		private readonly BaseballUaDbContext _dbContext;

        public AlbumsCrud(BaseballUaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public void Add(Album item)
		{
			_dbContext.Albums.Add(item);
			_dbContext.SaveChanges();
		}

		public void Delete(Album item)
		{
			if(item != null)
            {
                _dbContext.Albums.Remove(item);
                _dbContext.SaveChanges();
            }
            
		}

		public Album Get(int itemId)
		{
			return _dbContext.Albums.Where(a => a.Id == itemId)
										.Include(a => a.News)
										.Include(a => a.Category)
										.Include(a => a.Team)
										.Include(a => a.Game)
											.ThenInclude(g => g.SchemaGroup)
												.ThenInclude(g => g.EventSchemaItem)
													.ThenInclude(i => i.Event)
														.ThenInclude(e => e.Tournament)
															.ThenInclude(t => t.Category)
                                        .Include(a => a.Photos.OrderByDescending(p => p.Id).Take(Constants.DefaultPhotoAmount))?
                                    .FirstOrDefault();
		}

        public Album? GetWithTitlePhotos(int itemId)
        {
            return _dbContext.Albums.Where(a => a.Id == itemId)
                                    .Include(a => a.Photos)
                                        .ThenInclude(p => p.NewsTitlePhotos)
                                    .FirstOrDefault();
        }


        public IEnumerable<Album> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Album> GetAll(SportType? sportType = null,
                                bool? isGeneral = null,
                                int? newsId = null,
                                int? categoryId = null,
                                int? teamId = null,
                                int? gameId = null,
                                DateTime? lastDate = null,
                                int? lastId = null,
                                int? amount = null,
                                bool notForTeamOnly = false)
        {
            return _dbContext.Albums.Where(a => (sportType == null || a.SportType == sportType)
                                            && (isGeneral == null || a.IsGeneral == isGeneral)
                                            && (newsId == null || a.NewsId == newsId)
                                            && (categoryId == null || a.CategoryId == categoryId)
                                            && (teamId == null || a.TeamId == teamId)
                                            && (gameId == null || a.GameId == gameId)
                                            && (lastDate == null || (lastId == null ? a.PublishDate < lastDate : a.PublishDate <= lastDate && a.Id < lastId))
                                            && (!notForTeamOnly || (a.IsGeneral || a.News.EventId != null || a.CategoryId != null || a.GameId != null))
                                            )
                                            .OrderByDescending(a => a.PublishDate).ThenByDescending(a => a.Id)
                                            .Take(amount == null ? Constants.DefaulAlbumsAmount : (int)amount)
                                            .Include(a => a.Photos);
        }

        public IEnumerable<Album> GetAllEventAlbums(int? eventId, int amount = Constants.DefaulHeaderAlbumsAmount)
		{
			var eventAlbums = new List<Album>();
			if ( eventId != null && amount > 0)
			{
				var albumsForGams = (from albums in _dbContext.Albums
									   join games in _dbContext.Games on albums.GameId equals games.Id
									   join eventGroups in _dbContext.SchemaGroups on games.SchemaGroupId equals eventGroups.Id
									   join eventSchemas in _dbContext.EventSchemaItems on eventGroups.EventSchemaItemId equals eventSchemas.Id
									   where eventSchemas.EventId == eventId
									   select albums)
									   .Take(amount)
									   .Include(a => a.Photos)
									   .ToList();

                var albumsForNews = (from albums in _dbContext.Albums
									   join news in _dbContext.News on albums.NewsId equals news.Id
									   where news.EventId == eventId
									   select albums)
									   .Take(amount)
									   .Include(a => a.Photos)
									   .ToList();

                eventAlbums = albumsForGams.Concat(albumsForNews).Distinct().Take(amount).ToList();
            }

            return eventAlbums;
		}

        public IEnumerable<Album> GetAllCategoryAlbums(int? categoryId, int amount = Constants.DefaulAlbumsAmount)
        {
            var resultAlbums = new List<Album>();
            if (categoryId != null && amount > 0)
            {
                var albumsForGames = (from albums in _dbContext.Albums
                                     join games in _dbContext.Games on albums.GameId equals games.Id
                                     join eventGroups in _dbContext.SchemaGroups on games.SchemaGroupId equals eventGroups.Id
                                     join eventSchemas in _dbContext.EventSchemaItems on eventGroups.EventSchemaItemId equals eventSchemas.Id
									 join events in _dbContext.Events on eventSchemas.EventId equals events.Id
									 join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
                                     where tournaments.CategoryId == categoryId
                                     select albums)
                                       .Take(amount)
                                       .Include(a => a.Photos)
                                       .ToList();

                var albumsForNews = (from albums in _dbContext.Albums
                                     join news in _dbContext.News on albums.NewsId equals news.Id
                                     join events in _dbContext.Events on news.EventId equals events.Id
                                     join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
                                     where tournaments.CategoryId == categoryId
                                     select albums)
                                       .Take(amount)
                                       .Include(a => a.Photos)
                                       .ToList();

				var albumsForCategory = (from albums in _dbContext.Albums
										 where albums.CategoryId == categoryId
										 select albums)
											.Take(amount)
											.Include(a => a.Photos)
											.ToList();

				var tempAlbums = new List<Album>();
                tempAlbums.AddRange(albumsForGames);
                tempAlbums.AddRange(albumsForNews);
                tempAlbums.AddRange(albumsForCategory);
				
				resultAlbums = tempAlbums.Distinct().Take(amount).ToList();
            }

            return resultAlbums;
        }

        public IEnumerable<Album> GetAllTeamAlbums(int? teamId, int amount = Constants.DefaulAlbumsAmount)
        {
            var resultAlbums = new List<Album>();
            if (teamId != null && amount > 0)
            {
                var albumsForGames = (from albums in _dbContext.Albums
                                      join games in _dbContext.Games on albums.GameId equals games.Id
                                      where games.HomeTeamId == teamId || games.VisitorTeamId == teamId
                                      select albums)
                                       .Take(amount)
                                       .Include(a => a.Photos)
                                       .ToList();

                var albumsForNews = (from albums in _dbContext.Albums
                                     join news in _dbContext.News on albums.NewsId equals news.Id
                                     where news.TeamId == teamId
                                     select albums)
                                       .Take(amount)
                                       .Include(a => a.Photos)
                                       .ToList();

                var albumsForTeam = (from albums in _dbContext.Albums
                                     where albums.TeamId == teamId
									 select albums)
									 .Take(amount)
									 .Include(a => a.Photos)
									 .ToList();

                var tempAlbums = new List<Album>();
                tempAlbums.AddRange(albumsForGames);
                tempAlbums.AddRange(albumsForNews);
                tempAlbums.AddRange(albumsForTeam);

                resultAlbums = tempAlbums.Distinct().Take(amount).ToList();
            }

            return resultAlbums;
        }

        public IEnumerable<Album> GetAllClubAlbums(int? clubId, int amount = Constants.DefaulAlbumsAmount)
        {
            var albumsForClub = new List<Album>();
            if (clubId != null && amount > 0)
            {
                var teamIds = _dbContext.Teams.Where(t => t.ClubId == clubId).Select(t => t.Id);
                //var teamIds = new List<int>();
                //teamIds.Add(1);
                //teamIds.Add(2);
                if (teamIds != null && teamIds.Count() > 0)
                {
                    albumsForClub = (from albums in _dbContext.Albums
                                     join games in _dbContext.Games on albums.GameId equals games.Id into gj
                                     from subGames in gj.DefaultIfEmpty()
                                     join news in _dbContext.News on albums.NewsId equals news.Id into ggj
                                     from subNews in ggj.DefaultIfEmpty()
                                     where teamIds.Contains(subGames.HomeTeamId ?? 0)
                                           || teamIds.Contains(subGames.VisitorTeamId ?? 0)
                                           || teamIds.Contains(subNews.TeamId ?? 0)
                                           || teamIds.Contains(albums.TeamId ?? 0)
                                     //where subGames.HomeTeamId == 1 || subGames.VisitorTeamId == 1 || subNews.TeamId == 1 || albums.TeamId == 1
                                     select albums)
                                       .Distinct()
                                       .Take(amount)
                                       .OrderBy(i => i.PublishDate)
                                       .Include(a => a.Photos)
                                       .ToList();
                }
            }

            return albumsForClub;
        }

        public IEnumerable<Album> GetAllSportTypeAlbums(SportType? sportType, int amount = Constants.DefaulAlbumsAmount)
        {
            var resultAlbums = new List<Album>();
            if (sportType != null && amount > 0)
            {
                var albumsForGames = (from albums in _dbContext.Albums
                                      join games in _dbContext.Games on albums.GameId equals games.Id
                                      join eventGroups in _dbContext.SchemaGroups on games.SchemaGroupId equals eventGroups.Id
                                      join eventSchemas in _dbContext.EventSchemaItems on eventGroups.EventSchemaItemId equals eventSchemas.Id
                                      join events in _dbContext.Events on eventSchemas.EventId equals events.Id
                                      join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
                                      where tournaments.Sport == sportType
                                      select albums)
                                       .Take(amount)
                                       .Include(a => a.Photos)
                                       .ToList();

                var albumsForNews = (from albums in _dbContext.Albums
                                     join news in _dbContext.News on albums.NewsId equals news.Id
                                     join events in _dbContext.Events on news.EventId equals events.Id
                                     join tournaments in _dbContext.Tournaments on events.TournamentId equals tournaments.Id
                                     where tournaments.Sport == sportType
                                     select albums)
                                       .Take(amount)
                                       .Include(a => a.Photos)
                                       .ToList();

                var albumsForTeam = (from albums in _dbContext.Albums
                                     join teams in _dbContext.Teams on albums.TeamId equals teams.Id
                                     where teams.SportType == sportType
                                     select albums)
                                     .Take(amount)
                                     .Include(a => a.Photos)
                                     .ToList();

                var tempAlbums = new List<Album>();
                tempAlbums.AddRange(albumsForGames);
                tempAlbums.AddRange(albumsForNews);
                tempAlbums.AddRange(albumsForTeam);

                resultAlbums = tempAlbums.Distinct().Take(amount).ToList();
            }

            return resultAlbums;
        }



        public IEnumerable<Album> GetAllFiltered(out int countt, 
                                        SportType sportType = SportType.NotDefined,
                                        bool includeAllGeneral = false,
                                        bool includeAllFun = false,
                                        bool isOfficial = false,
                                        bool isInternational = false,
                                        bool isAnnual = false,
                                        int? eventId = null,
                                        IEnumerable<int>? categoryIds = null,
                                        IEnumerable<int>? teamIds = null,
                                        DateTime? newestDate = null,
                                        int skip = 0,
                                        int amount = Constants.DefaulAlbumsAmount)
        {

            var fixxedNewestDate = newestDate ?? DateTime.Now.Date;

            var result = (  from albums in _dbContext.Albums
                            join photos in _dbContext.Photos on albums.Id equals photos.AlbumId
                            join jnews in _dbContext.News on albums.NewsId equals jnews.Id into subnews
                                from news in subnews.DefaultIfEmpty()
                                join jevent in _dbContext.Events on news.EventId equals jevent.Id into subevent
                                    from eventt in subevent.DefaultIfEmpty()
                                    join jtour in _dbContext.Tournaments on eventt.TournamentId equals jtour.Id into subtour
                                        from tour in subtour.DefaultIfEmpty()
                            join jgame in _dbContext.Games on albums.GameId equals jgame.Id into subgame
                                from game in subgame.DefaultIfEmpty()
                                join jeventGroup in _dbContext.SchemaGroups on game.SchemaGroupId equals jeventGroup.Id into subgroup
                                    from eventGroup in subgroup.DefaultIfEmpty()
                                    join jeventItem in _dbContext.EventSchemaItems on eventGroup.EventSchemaItemId equals jeventItem.Id into subeventItem
                                        from eventItem in subeventItem.DefaultIfEmpty()
                                        join jeventG in _dbContext.Events on eventItem.EventId equals jeventG.Id into subeventG
                                            from eventG in subeventG.DefaultIfEmpty()
                                            join jtourG in _dbContext.Tournaments on eventG.TournamentId equals jtourG.Id into subtourG
                                                from tourG in subtourG.DefaultIfEmpty()
                            where   albums.PublishDate <= fixxedNewestDate
                                    && ((includeAllGeneral && (news.IsGeneral || albums.IsGeneral))
                                        || (includeAllFun && (tour.IsFun || tourG.IsFun))
                                        || ((sportType == SportType.NotDefined
                                                        || albums.SportType == sportType
                                                        || tour.Sport == sportType
                                                        || tourG.Sport == sportType
                                                        || news.SportType == sportType
                                                            )
                                            && (!isOfficial || tour.IsOfficial || tourG.IsOfficial)
                                            && (!isInternational || tour.IsInternational || tourG.IsInternational)
                                            && (!isAnnual || tour.IsAnual || tourG.IsAnual)
                                            && (eventId == null || news.EventId == eventId || eventG.Id == eventId)
                                            && (categoryIds.IsNullOrEmpty()
                                                || categoryIds.Any(c => c == albums.CategoryId)
                                                || categoryIds.Any(c => c == news.CategoryId)
                                                || categoryIds.Any(c => c == tour.CategoryId)
                                                || categoryIds.Any(c => c == tourG.CategoryId))
                                            && (teamIds.IsNullOrEmpty()
                                                || teamIds.Any(t => t == albums.TeamId.GetValueOrDefault())
                                                || teamIds.Any(t => t == news.TeamId.GetValueOrDefault())
                                                || teamIds.Any(t => t == game.HomeTeamId.GetValueOrDefault())
                                                || teamIds.Any(t => t == game.VisitorTeamId.GetValueOrDefault()))
                                           )
                                        )
                            select albums)
                            .Distinct()
                            .OrderByDescending(a => a.PublishDate)
                                .ThenByDescending(a => a.Id);


            countt = result.Count();

            return result.Take(amount)
                            .Include(a => a.Photos);
        }



        public IEnumerable<Album> GetAllHard(out int countt,
                                        SportType? sportType = null,
                                        bool? isGeneral = null,
                                        int? newsId = null,
                                        int? categoryId = null,
                                        int? teamId = null,
                                        int? gameId = null,
                                        DateTime? lastDate = null,
                                        int skip = 0,
                                        int amount = Constants.DefaulAlbumsAmount,
										bool notForTeamOnly = false)
		{
            var lastDateFixxed = lastDate == null ? DateTime.MaxValue : lastDate;
            var result = _dbContext.Albums.Where(a => (sportType == null || a.SportType == sportType)
                                            && (isGeneral == null || a.IsGeneral == isGeneral)
                                            && (newsId == null || a.NewsId == newsId)
                                            && (categoryId == null || a.CategoryId == categoryId)
                                            && (teamId == null || a.TeamId == teamId)
                                            && (gameId == null || a.GameId == gameId)
                                            && (a.PublishDate <= lastDateFixxed)
                                            && (!notForTeamOnly || (a.IsGeneral || a.News.EventId != null || a.CategoryId != null || a.GameId != null))
                                            )
                                            .OrderByDescending(a => a.Id);
            countt = result.Count();

            return result.Skip(skip)
                        .Take(amount)
						.Include(a => a.News)
						.Include(a => a.Category)
						.Include(a => a.Team)
						.Include(a => a.Game)
							.ThenInclude(g => g.SchemaGroup)
								.ThenInclude(g => g.EventSchemaItem)
									.ThenInclude(i => i.Event)
										.ThenInclude(e => e.Tournament)
											.ThenInclude(t => t.Category)
						.Include(a => a.Photos);
		}

		public void Update(Album item)
		{
			if(item != null)
            {
                _dbContext.Albums.Update(item);
                _dbContext.SaveChanges();
            }
		}

		public void Update(int id, 
							SportType? sportType = null, 
							bool? isGeneral = null, 
							string? name = null, 
							string? description = null, 
							DateTime? publishDate = null, 
							int? newsId = null, 
							int? categoryId = null,
							int? teamId = null,
							int? gameId = null)
		{
            if (sportType != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.SportType, sportType));
            if (isGeneral != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.IsGeneral, isGeneral));
			if (name != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.Name, name));
			if (description != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.Description, description));
			if (publishDate != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.PublishDate, publishDate));
			if (newsId != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.NewsId, newsId));
			if (categoryId != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.CategoryId, categoryId));
			if (teamId != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.TeamId, teamId));
			if (gameId != null) _dbContext.Albums.Where(a => a.Id == id)?.ExecuteUpdate(a => a.SetProperty(i => i.GameId, gameId));

		}

        public List<SelectListItem> GetSelectItemList(bool isNewsEmpty = false)
        {
            var albumsSL = _dbContext.Albums.Where(a => !isNewsEmpty || a.NewsId == null).OrderByDescending(a => a.Id).Take(Constants.DefaulSelectListAmount)
                                    .Select(c => new SelectListItem
                                    {
                                        Text = c.Name,
                                        Value = c.Id.ToString()
                                    }).ToList();

            return albumsSL;
        }

        public void UnlinkFromNews(int newsId)
        {
            var albumsDAL = _dbContext.Albums.Where(a => a.NewsId == newsId).ToList();
            albumsDAL.ForEach(a => a.NewsId = null);
            _dbContext.SaveChanges();
        }

        public void UnlinkFromGames(int gameId)
        {
            var albums = _dbContext.Albums.Where(a => a.GameId == gameId).ToList();
            albums.ForEach(v => v.GameId = null);
            _dbContext.SaveChanges();
        }
    }
}
