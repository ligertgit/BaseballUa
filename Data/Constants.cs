using System.Collections.Immutable;
using System.Security.Policy;

namespace BaseballUa.Data
{
	public static class Constants
	{
		public const int DefaulSelectListAmount = 25;
		public const int MenuTitleLength = 22;

		public const int DefaultPhotoAmount = 100;

		public const int DefaulNewsAmount = 25;
		public const int NewsSelectDaysShift = 20;
		public const int NewsPreviewLength = 400;
		public const int TitleAlbumsId = 10;


		public const int DefaulVideosAmount = 25;
		public const int DefaulListVideosAmount = 100;
		public const string DefaultVideoSmallImage = "videoSmall.png";
		public const string DefaultVideoBigImage = "videoBig.png";
		public const string DefaultVideoName = "no name";
		public const int VideoPreviewLength = 400;


		public const int DefaulEventAmount = 6;
		public const int DefaulActiveEventDaysShift = 100;
		public const int EventPreviewLength = 400;


		public const int DefaultGameAmount = 6;
		public const int DefaulActiveGamesDaysRange = 1;
		public const int GamesSelectDaysShift = 10;

		public const string DefaultAlbum = "default";
		public const string DefaultAlbumName = "no name";
		public const string DefaultAlbumSmallImage = "small.png";
		public const string DefaultAlbumBigImage = "big.png";
		public const int DefaulAlbumsAmount = 25;
		public const int DefaulListAlbumsAmount = 100;
		public const int AlbumPreviewLength = 400;
		public const int MaxThumbMediumAlbumName = 28;


		public const string DefaultTeamSmallImage = "defaultSmall.png";
		public const string DefaultTeamBigImage = "defaultBig.png";

        public const string DefaultCountrySmallImage = "default.png";
        public const string DefaultCountryBigImage = "default.png";

        public const int MaxClubStaffShow = 10;
		public const string DefaultStaffSmallImage = "defaultSmall.png";
		public const string DefaultStaffBigImage = "defaultBig.png";
		public const int EuroClubId = 4;
		public const string DefaultClubSmallImage = "defaultSmall.png";
		public const string DefaultClubBigImage = "defaultBig.png";
        public static readonly ImmutableList<int> UaClubIdList = new List<int>() {1, 8, 9, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 }.ToImmutableList<int>();

        public const string DefaultPlayerSmallImage = "defaultSmall.png";
        public const string DefaultPlayerBigImage = "defaultBig.png";

		public const long MinImageSize = 3000;
		public const long MaxImageSize = 6097152;
        public const double MaxImageRatio = 1.9;
		public const int BigImageHeight = 769;
		public const int SmallImageHeight = 188;
		public const string ImageBaseDir = "wwwroot\\images\\photo\\";
		public const string BigImageSubDir = "big";
		public const string SmallImageSubDir = "small";

        public const long MinIconSize = 300;
        public const long MaxIconSize = 3000000;
        public const double MaxIconRatio = 2.5;
        public const int BigIconHeight = 100;
        public const int SmallIconHeight = 60;
        public const string FlagBaseDir = "wwwroot\\images\\country\\";
        public const string ClubBaseDir = "wwwroot\\images\\club\\";
        public const string TeamBaseDir = "wwwroot\\images\\team\\";
        public const string BigFlagSubDir = "flagBig";
        public const string BigClubSubDir = "logoBig";
        public const string BigTeamSubDir = "logoBig";
        public const string SmallFlagSubDir = "flagSmall";
        public const string SmallClubSubDir = "logoSmall";
        public const string SmallTeamSubDir = "logoSmall";

        public const long MinAvatarSize = 300;
        public const long MaxAvatarSize = 3000000;
        public const double MaxAvatarRatio = 2.5;
        public const int BigAvatarHeight = 200;
        public const int SmallAvatarHeight = 80;
        public const string StaffBaseDir = "wwwroot\\images\\staff\\";
        public const string BigStaffSubDir = "avatarBig";
        public const string SmallStaffSubDir = "avatarSmall";
    }
}
