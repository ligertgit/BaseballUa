using static BaseballUa.Data.Enums;

namespace BaseballUa.BlData
{
    [Serializable]
    public class Filters
    {
        public bool Baseball { get; set; }
        public bool Softball { get; set; }
        public bool U10 { get; set; }
        public bool U12 { get; set; }
        public bool U15 { get; set; }
        public bool U18 { get; set; }
        public bool U23 { get; set; }
        public bool Adult { get; set; }
        public bool Veteran { get; set; }
        public bool Fun { get; set; }

        public Filters()
        {
            Baseball = false;
            Softball = false;
            U10 = false;
            U12 = false;
            U15 = false;
            U18 = false;
            U23 = false;
            Adult = false;
            Veteran = false;
            Fun = false;
        }

        public Filters FixForSelect(Filters? filters)
        {
            if (filters == null)
            {
                Baseball = true;
                Softball = true;
                U10 = true;
                U12 = true;
                U15 = true;
                U18 = true;
                U23 = true;
                Adult = true;
                Veteran = true;
                Fun = true;
            }
            else
            {
                Baseball = filters.Baseball;
                Softball = filters.Softball;
                U10 = filters.U10;
                U12 = filters.U12;
                U15 = filters.U15;
                U18 = filters.U18;
                U23 = filters.U23;
                Adult = filters.Adult;
                Veteran = filters.Veteran;
                Fun = filters.Fun;
            }

            if (!Baseball && !Softball)
            {
                Baseball = true;
                Softball = true;
            }

            if (!U10 && !U12 && !U15 && !U18 && !U23 && !Adult && !Veteran)
            {
                U10 = true;
                U12 = true;
                U15 = true;
                U18 = true;
                U23 = true;
                Adult = true;
                Veteran = true;
            }

            return this;
        }

        public string chkU10()
        {
            return U10 ? "U10" : "do not show";
        }

        public string chkU12()
        {
            return U12 ? "U12" : "do not show";
        }

        public string chkU15()
        {
            return U15 ? "U15" : "do not show";
        }

        public string chkU18()
        {
            return U18 ? "U18" : "do not show";
        }

        public string chkU23()
        {
            return U23 ? "U23" : "do not show";
        }
        public string chkAdult()
        {
            return Adult ? "Вишка" : "do not show";
        }

        public string chkVeteran()
        {
            return Veteran ? "Ветерани" : "do not show";
        }

        //public bool chkFun()
        //{
        //    return Fun ? true : false;
        //}

        public SportType chkBaseball()
        {
            
            return Baseball ? SportType.Baseball : SportType.NotDefined;
        }

        public SportType chkSoftball()
        {
            return Softball ? SportType.Softball : SportType.NotDefined;
        }
    }
}
