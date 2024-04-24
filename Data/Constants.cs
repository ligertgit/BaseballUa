using System.Collections.Immutable;
using BaseballUa.Models.Custom;
using static BaseballUa.Data.Enums;

namespace BaseballUa.Data
{
    public static class Constants
	{

		public const string AdminEmail = "ligert@gmail.com";
		public const int DefaulSelectListAmount = 25;
		public const int HugeSelectListAmount = 50;
		public const int MenuTitleLength = 22;
		public const int UnlimitedAmount = 1000;
		public const int DefaultHomeTeamId = 95;
		public const int DefaultVisitorTeamId = 94;


		public static readonly ImmutableList<CalendarConst> Calendars = new List<CalendarConst>()
												{
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "Вишка", CalendarId = "39e496e3e7c94b3771cd1c29e3d8749fcb6e547ca0aff81f426822978b340c79@group.calendar.google.com" },
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "U23", CalendarId = "36f4cdb2a35cd5ad0047b2d2492468865182fbcd844a91c38e4c673881ed088e@group.calendar.google.com" },
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "U18", CalendarId = "2978740c5111b9784c6ef9b4d510c6e11c421bbae3b23bebe12d53fd6b155a67@group.calendar.google.com" },
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "U15", CalendarId = "f3a3c80688a015fe500553ea903c95d81112ef65fbd32ad206b459f75c375822@group.calendar.google.com" },
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "U12", CalendarId = "87781b0e2c219ee604598f659219f155fd0338e7f4b62a451754f6c4eb29d042@group.calendar.google.com" },
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "U10", CalendarId = "34e3d18efdd614dbabc08a692cc1cba42398f6c0804a42c0489adafaad1724a2@group.calendar.google.com" },
													new CalendarConst() { SportType = SportType.Baseball, AgeShortName = "Ветерани", CalendarId = "aba2a9485b98e5ee732e6362e74c4a279e4d8968684a22e5aede92115a77a1e4@group.calendar.google.com" },
                                                    
													new CalendarConst() { SportType = SportType.Softball, AgeShortName = "Вишка", CalendarId = "b2e7bc05bea02abcc81252a192487781b878cab7a0c9315e6135873c79545d69@group.calendar.google.com" },
                                                    new CalendarConst() { SportType = SportType.Softball, AgeShortName = "U23", CalendarId = "dc5b3c8bf17f249e48ea75458b4c0d120872c243ff95ad79450772cb15627aa7@group.calendar.google.com" },
                                                    new CalendarConst() { SportType = SportType.Softball, AgeShortName = "U18", CalendarId = "51b8533d5fcb9f8c376b6115085213fe0472317ca99a2f2da7b75535bd233971@group.calendar.google.com" },
                                                    new CalendarConst() { SportType = SportType.Softball, AgeShortName = "U15", CalendarId = "03c6e80c8f187ad89bf0f4b62d50dd39f11bca1a539317cf4532a217c7604d38@group.calendar.google.com" },
                                                    new CalendarConst() { SportType = SportType.Softball, AgeShortName = "U12", CalendarId = "bbc540d5c79c85a78f3a17baad56e774b2f0292244f5431aa4e861c8a7500165@group.calendar.google.com" },
                                                    new CalendarConst() { SportType = SportType.Softball, AgeShortName = "U10", CalendarId = "b0df7031c00b25ffd742a518fff4a7998f34daaf085f4cf1e019ecd5846dab2f@group.calendar.google.com" },
                                                    new CalendarConst() { SportType = SportType.Softball, AgeShortName = "Ветерани", CalendarId = "646a07f46aaec37d8950b1ec5e45862fdbde535e2dd673702e826335d956b4ae@group.calendar.google.com" },
                                                }.ToImmutableList<CalendarConst>();
		public const int CalendarEventSplitInterval = 10;


        public const int DefaultPhotoAmount = 100;

		public const int DefaulNewsAmount = 25;
		public const int NewsSelectDaysShift = 20;
		public const int NewsPreviewLength = 400;
		public const int TitleAlbumsId = 10;


		public const int DefaulVideosAmount = 25;
		public const int DefaulHeaderVideosAmount = 8;
		public const int DefaulListVideosAmount = 100;
		public const string DefaultVideoSmallImage = "videoSmall.png";
		public const string DefaultVideoBigImage = "videoBig.png";
		public const string DefaultVideoName = "no name";
		public const int VideoPreviewLength = 400;


		public const int DefaulEventAmount = 6;
		public const int DefaulActiveEventDaysShift = 10;
		public const int EventPreviewLength = 400;


		public const int DefaultGameAmount = 6;
		public const int DefaulActiveGamesDaysRange = 1;
		public const int GamesSelectDaysShift = 10;

		public const string DefaultAlbum = "default";
		public const string DefaultAlbumName = "no name";
		public const string DefaultAlbumSmallImage = "small.png";
		public const string DefaultAlbumBigImage = "big.png";
		public const int DefaulAlbumsAmount = 25;
		public const int DefaulHeaderAlbumsAmount = 10;
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
        //public static readonly ImmutableList<int> UaClubIdList = new List<int>() {1, 8, 9, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 }.ToImmutableList<int>();
        public const int UaCountryId = 1;


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
        public const string PlayerBaseDir = "wwwroot\\images\\player\\";
        public const string BigStaffSubDir = "avatarBig";
        public const string BigPlayerSubDir = "avatarBig";
        public const string SmallStaffSubDir = "avatarSmall";
        public const string SmallPlayerSubDir = "avatarSmall";
    }
}
