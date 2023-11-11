using BaseballUa.Models;
using BaseballUa.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseballUa.DTO
{
	public class NewsTitlePhotoToView
	{

		public NewsTitlePhotoVM Convert(NewsTitlePhoto newsTFDALL, bool doSubConvert = true)
		{
			var newsTFVL = new NewsTitlePhotoVM();
			newsTFVL.Id = newsTFDALL.Id;
			newsTFVL.Name = newsTFDALL.Name;
			newsTFVL.Description = newsTFDALL.Description;
			newsTFVL.NewsId = newsTFDALL.NewsId;
			newsTFVL.PhotoId = newsTFDALL.PhotoId;


			return newsTFVL;
		}

		public List<NewsTitlePhotoVM> ConvertAll(List<NewsTitlePhoto> newsTFDALL, bool doSubConvert = true)
		{
			var newsTFVL = new List<NewsTitlePhotoVM>();
			foreach(var item in newsTFDALL)
			{
				newsTFVL.Add(Convert(item,doSubConvert));
			}

			return newsTFVL;
		}

		public NewsTitlePhoto ConvertBack(NewsTitlePhotoVM newsTFVL, bool doSubConvert = true)
		{
			var newsTFDALL = new NewsTitlePhoto();
			newsTFDALL.Id = newsTFVL.Id;
			newsTFDALL.Name = newsTFVL.Name;
			newsTFDALL.Description = newsTFVL.Description;
			newsTFDALL.NewsId = newsTFVL.NewsId;
			newsTFDALL.PhotoId = newsTFVL.PhotoId;

			return newsTFDALL;
		}
	}
}
