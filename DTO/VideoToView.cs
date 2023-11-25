using BaseballUa.Models;
using BaseballUa.ViewModels;
using static BaseballUa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseballUa.DTO
{
	public class VideoToView
	{
		public VideoVM Convert(Video videoDAL, bool doSubConvert = true)
		{
			var videoVL = new VideoVM();
			if (videoDAL != null) 
			{
				videoVL.Id = videoDAL.Id;
				videoVL.SportType = videoDAL.SportType;
				videoVL.IsGeneral = videoDAL.IsGeneral;
				videoVL.Name = videoDAL.Name;
				videoVL.Description = videoDAL.Description;
				videoVL.Fname = videoDAL.Fname;
				videoVL.NewsId = videoDAL.NewsId;
				if (videoDAL.News != null)
				{
					videoVL.News = new NewsToView().Convert(videoDAL.News, false);
				}
				videoVL.CategoryId = videoDAL.CategoryId;
				if (videoDAL.Category != null)
				{
					videoVL.Category = new CategoryToView().Convert(videoDAL.Category, false);
				}
				videoVL.TeamId = videoDAL.TeamId;
				if (videoDAL.Team != null)
				{
					videoVL.Team = new TeamToView().Convert(videoDAL.Team, false);
				}
				videoVL.GameId = videoDAL.GameId;
				if (videoDAL.Game != null)
				{
					videoVL.Game = new GameToView().Convert(videoDAL.Game, false);
					//if (videoDAL.Game?.SchemaGroup?.EventSchemaItem?.Event != null) 
					//{
					//	videoVL.GameEvent = new EventToView().Convert(videoDAL.Game.SchemaGroup.EventSchemaItem.Event, false);
					//}
				}
			}
			

			return videoVL;
		}

		public List<VideoVM> ConvertAll(List<Video>? videosDAL, bool doSubConvert = true)
		{
			var videosVL = new List<VideoVM>();
			if (videosDAL != null) 
			{
				foreach (var videoDAL in videosDAL)
				{
					if (videoDAL != null) 
					{
						videosVL.Add(Convert(videoDAL, doSubConvert));
					}
					
				}
			}

			return videosVL;
		}

		public Video ConvertBack(VideoVM videoVL)
		{
			var videoDAL = new Video();
			videoDAL.Id = videoVL.Id;
			videoDAL.SportType = videoVL.SportType;
			videoDAL.IsGeneral = videoVL.IsGeneral;
			videoDAL.Name = videoVL.Name;
			videoDAL.Description = videoVL.Description;
			videoDAL.Fname = videoVL.Fname;
			videoDAL.NewsId = videoVL.NewsId;
			videoDAL.CategoryId = videoVL.CategoryId;
			videoDAL.TeamId = videoVL.TeamId;
			videoDAL.GameId = videoVL.GameId;

			return videoDAL;
		}

        public VideoVM CreateEmpty()
        {
            var videoVL = new VideoVM();

            return videoVL;
        }
    }
}
