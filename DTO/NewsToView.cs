using BaseballUa.Data;
using BaseballUa.Models;
using BaseballUa.ViewModels;
using System.Text.RegularExpressions;

namespace BaseballUa.DTO
{
	public class NewsToView
	{

		public NewsVM Convert(News newsDAL, bool doSubConvert = true)
		{
			NewsVM newsVL = new NewsVM();
			if (newsDAL != null)
			{
                newsVL.Id = newsDAL.Id;
                newsVL.SportType = newsDAL.SportType;
                newsVL.IsGeneral = newsDAL.IsGeneral;
                newsVL.PublishDate = newsDAL.PublishDate;
                newsVL.Title = newsDAL.Title;
                newsVL.Description = newsDAL.Description;
                newsVL.EventId = newsDAL.EventId;
                if (newsDAL.Event != null)
                {
                    newsVL.Event = new EventToView().Convert(newsDAL.Event, false);
                }
                newsVL.CategoryId = newsDAL.CategoryId;
                if (newsDAL.Category != null)
                {
                    newsVL.Category = new CategoryToView().Convert(newsDAL.Category);
                }
                newsVL.TeamId = newsDAL.TeamId;
                if (newsDAL.Team != null)
                {
                    newsVL.Team = new TeamToView().Convert(newsDAL.Team, false);
                }
                if (doSubConvert && newsDAL.Albums != null)
                {
                    newsVL.Albums = new AlbumToView().ConvertAll(newsDAL.Albums.ToList(), false);
                }
                if (doSubConvert && newsDAL.NewsTitlePhotos != null)
                {
                    var photosDAL = newsDAL.NewsTitlePhotos.Select(i => i.Photo).ToList();
                    newsVL.Photos = new PhotoToView().ConvertAll(photosDAL, false);
                }
                if (doSubConvert && newsDAL.Videos != null)
                {
                    newsVL.Videos = new VideoToView().ConvertAll(newsDAL.Videos.ToList(), false);
                }
            }



			return newsVL;
		}

		public List<NewsVM> ConvertAll(List<News>? newsDAL, bool doSubConvert = true)
		{
			var newsVL = new List<NewsVM>();
            if (newsDAL != null) 
            {
                foreach (var item in newsDAL)
                {
                    newsVL.Add(Convert(item, doSubConvert));
                }
            }
			
			return newsVL;
		}

		public News ConvertBack(NewsVM newsVM)
		{
			var newsDAL = new News();
			newsDAL.Id = newsVM.Id;
			newsDAL.Title = newsVM.Title;
			newsDAL.Description = newsVM.Description;
			newsDAL.SportType = newsVM.SportType;
			newsDAL.IsGeneral = newsVM.IsGeneral;
			newsDAL.PublishDate = newsVM.PublishDate;
			newsDAL.CategoryId = newsVM.CategoryId;
			newsDAL.EventId = newsVM.EventId;
			newsDAL.TeamId = newsVM.TeamId;

			if (newsDAL.Description != null)
			{
				Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
				newsDAL.Description = rRemScript.Replace(newsDAL.Description, "");
			}

			return newsDAL;
		}

        public NewsVM CreateEmpty()
        {
            var newsVL = new NewsVM();
            newsVL.PublishDate = DateTime.Now.CurDateNoSeconds();

            return newsVL;
        }
	}
}
