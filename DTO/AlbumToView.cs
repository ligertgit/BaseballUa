using BaseballUa.Models;
using BaseballUa.ViewModels;
using BaseballUa.Data;

namespace BaseballUa.DTO
{
	public class AlbumToView
	{

		public AlbumVM Convert(Album albumDAL, bool doSubConvert = true)
		{
			var albumVL = new AlbumVM();
			if (albumDAL != null) 
			{
				albumVL.Id = albumDAL.Id;
				albumVL.SportType = albumDAL.SportType;
				albumVL.IsGeneral = albumDAL.IsGeneral;
				albumVL.Name = albumDAL.Name;
				albumVL.Description	= albumDAL.Description;
				albumVL.PublishDate = albumDAL.PublishDate;
				albumVL.NewsId = albumDAL.NewsId;
				if (albumDAL.News != null)
				{
					albumVL.News = new NewsToView().Convert(albumDAL.News, false);
				}
				albumVL.CategoryId = albumDAL.CategoryId;
				if (albumDAL.Category != null) 
				{ 
					albumVL.Category = new CategoryToView().Convert(albumDAL.Category, false);
				}
				albumVL.TeamId = albumDAL.TeamId;
				if (albumDAL.Team != null)
				{
					albumVL.Team = new TeamToView().Convert(albumDAL.Team, false);
				}
				albumVL.GameId = albumDAL.GameId;
				if (albumDAL.Game != null)
				{
					albumVL.Game = new GameToView().Convert(albumDAL.Game, false);
				}
				if (doSubConvert && albumDAL.Photos != null)
				{
					albumVL.Photos = new PhotoToView().ConvertAll(albumDAL.Photos.ToList(), false);
				}

			}
			
			return albumVL;
			
		}

		public List<AlbumVM> ConvertAll(List<Album>? albumsDAL, bool doSubConvert = true)
		{
			var albumsVL = new List<AlbumVM>();
			foreach (var albumDAL in albumsDAL)
			{
				albumsVL.Add(Convert(albumDAL, doSubConvert));
			}

			return albumsVL;
		}


		public Album ConvertBack(AlbumVM albumVL)
		{
			var albumDAL = new Album();
			albumDAL.Id = albumVL.Id;
			albumDAL.Name = albumVL.Name;
			albumDAL.Description = albumVL.Description;
			albumDAL.IsGeneral = albumVL.IsGeneral;
			albumDAL.SportType = albumVL.SportType;
			albumDAL.PublishDate = albumVL.PublishDate;
			albumDAL.NewsId	= albumVL.NewsId;
			albumDAL.CategoryId = albumVL.CategoryId;
			albumDAL.GameId = albumVL.GameId;
			albumDAL.TeamId = albumVL.TeamId;

			return albumDAL;
		}

		public AlbumVM CreateEmpty()
		{
			var albumVL = new AlbumVM();
			albumVL.PublishDate = DateTime.Now.CurDateNoSeconds();

            return albumVL;
		}
	}
}
