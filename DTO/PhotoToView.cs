using BaseballUa.Models;
using BaseballUa.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseballUa.DTO
{
	public class PhotoToView
	{

		public PhotoVM Convert(Photo photoDAL, bool doSubConvert = true)
		{
			var photoVL = new PhotoVM();
			photoVL.Id = photoDAL.Id;
			photoVL.Name = photoDAL.Name;
			photoVL.Description = photoDAL.Description;
			photoVL.FnameOrig = photoDAL.FnameOrig;
			photoVL.FnameBig = photoDAL.FnameBig;
			photoVL.FnameSmall = photoDAL.FnameSmall;
			photoVL.AlbumId = photoDAL.AlbumId;
			if (photoDAL.Album != null)
			{
				photoVL.Album = new AlbumToView().Convert(photoDAL.Album, false);
			}
			//if (doSubConvert && )
			//public List<NewsVM>? News { get; set; }

			return photoVL;
		}

		public List<PhotoVM> ConvertAll(List<Photo> photosDAL, bool doSubConvert = true)
		{
			var photosVL = new List<PhotoVM>();
			foreach (var photoDAL in photosDAL)
			{ 
				photosVL.Add(Convert(photoDAL, doSubConvert));
			}

			return photosVL;
		}

		public Photo ConvertBack(PhotoVM photoVL)
		{
			var photoDAL = new Photo();
			photoDAL.Id = photoVL.Id;
			photoDAL.Name = photoVL.Name;
			photoDAL.Description = photoVL.Description;
			photoDAL.FnameOrig = photoVL.FnameOrig;
			photoDAL.FnameBig = photoVL.FnameBig;
			photoDAL.FnameSmall = photoVL.FnameSmall;
			photoDAL.AlbumId = photoVL.AlbumId;

			return photoDAL;
		}

		public PhotoVM CreateEmpty(int albumId = 0)
		{
			var photoVL = new PhotoVM();
			if (albumId != 0)
			{
				photoVL.AlbumId = albumId;
			}

			return photoVL;
		}
	}
}
