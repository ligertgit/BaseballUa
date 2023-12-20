using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.Data
{

    public static class Enums
    {
        public enum SportType
        {
            Baseball = 1,
            Softball = 2,
            NotDefined = 3
        }

        public enum GameStatus
        {
            Upcoming = 1,
            Runnig = 2,
            Deleyed = 3,
            Finished = 4,
            Canceled = 5
        }

        public enum GameType
        {
            Group1 = 1,
            Group2 = 2,
            Superround = 3,
            WinLose = 4,
            ForPlace = 5,
            ForPlaceSeries = 6,
            PlacePlace = 7,
            Other = 8
        }

        public enum TourNumber
        {
            Tour1 = 1,
            Tour2 = 2,
            Tour3 = 3,
            Tour4 = 4,
            Tour5 = 5,
            Tour6 = 6,
            Tour7 = 7,
            Tour8 = 8,
            Tour9 = 9,
            Tour10 = 10,
            Tour11 = 11,
            Tour12 = 12,
            Tour13 = 13,
            Tour14 = 14,
            Tour15 = 15,
            Tour16 = 16,
            Tour17 = 17,
            Tour18 = 18,
            Tour19 = 19,
            Tour20 = 20
        }

        public enum ClubRole
        {
            Admin = 1,
            HeadCoach = 2,
            Coach = 3,
            Manager = 4
        }

        public enum Sex
        {
            Male = 1,
            Female = 2,
            NotDefined = 3
        }

        public enum ListToShow
        {
            News,
            Videos,
            Albums,
            Events
        }
    }
}
