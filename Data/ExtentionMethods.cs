using Azure;
using Azure.Core;
using BaseballUa.DTO.Custom;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static NuGet.Packaging.PackagingConstants;

namespace BaseballUa.Data
{
    public static class ExtensionMethods
    {
        public static SelectList ToSelectList<TEnum>(this TEnum obj)
        where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var ttt = new SelectList(Enum.GetValues(typeof(TEnum))
            .OfType<Enum>()
            .Select(x => new SelectListItem
            {
                Text = Enum.GetName(typeof(TEnum), x),
                Value = (Convert.ToInt32(x))
                .ToString()
            }), "Value", "Text");
            return ttt;
        }

        public static Filters GetFilters(this IRequestCookieCollection cookies)
        {
            var filters = new Filters();
            filters.Baseball = cookies["filterBaseball"] != null ? Convert.ToBoolean(cookies["filterBaseball"]) : filters.Baseball;
            filters.Softball = cookies["filterSoftball"] != null ? Convert.ToBoolean(cookies["filterSoftball"]) : filters.Softball;
            filters.U10 = cookies["filterU10"] != null ? Convert.ToBoolean(cookies["filterU10"]) : filters.U10;
            filters.U12 = cookies["filterU12"] != null ? Convert.ToBoolean(cookies["filterU12"]) : filters.U12;
            filters.U15 = cookies["filterU15"] != null ? Convert.ToBoolean(cookies["filterU15"]) : filters.U15;
            filters.U18 = cookies["filterU18"] != null ? Convert.ToBoolean(cookies["filterU18"]) : filters.U18;
            filters.Adult = cookies["filterAdult"] != null ? Convert.ToBoolean(cookies["filterAdult"]) : filters.Adult;
            filters.Veteran = cookies["filterVeteran"] != null ? Convert.ToBoolean(cookies["filterVeteran"]) : filters.Veteran;
            filters.Fun = cookies["filterFun"] != null ? Convert.ToBoolean(cookies["filterFun"]) : filters.Fun;
            filters.General = cookies["filterGeneral"] != null ? Convert.ToBoolean(cookies["filterGeneral"]) : filters.General;
            filters.Annual = cookies["filterAnnual"] != null ? Convert.ToBoolean(cookies["filterAnnual"]) : filters.Annual;
            filters.Official = cookies["filterOfficial"] != null ? Convert.ToBoolean(cookies["filterOfficial"]) : filters.Official;
            filters.International = cookies["filterInternational"] != null ? Convert.ToBoolean(cookies["filterInternational"]) : filters.International;

            return filters;
        }
        public static Filters GetFilters(this IFormCollection fc)
        {
            var filters = new Filters();
            if (fc["chkBaseball"] == "on") filters.Baseball = true; else filters.Baseball = false;
            if (fc["chkSoftball"] == "on") filters.Softball = true; else filters.Softball = false;
            if (fc["chkU10"] == "on") filters.U10 = true; else filters.U10 = false;
            if (fc["chkU12"] == "on") filters.U12 = true; else filters.U12 = false;
            if (fc["chkU15"] == "on") filters.U15 = true; else filters.U15 = false;
            if (fc["chkU18"] == "on") filters.U18 = true; else filters.U18 = false;
            if (fc["chkU23"] == "on") filters.U23 = true; else filters.U23 = false;
            if (fc["chkAdult"] == "on") filters.Adult = true; else filters.Adult = false;
            if (fc["chkVeteran"] == "on") filters.Veteran = true; else filters.Veteran = false;
            if (fc["chkFun"] == "on") filters.Fun = true; else filters.Fun = false;
            if (fc["chkGeneral"] == "on") filters.General = true; else filters.General = false;
            if (fc["chkAnnual"] == "on") filters.Annual = true; else filters.Annual = false;
            if (fc["chkOfficial"] == "on") filters.Official = true; else filters.Official = false;
            if (fc["chkInternational"] == "on") filters.International = true; else filters.International = false;

            return filters;
        }
        public static void AppendFilters(this IResponseCookies cookies, Filters filters, CookieOptions option)
        {
            cookies.Append("filterBaseball", filters.Baseball.ToString(), option);
            cookies.Append("filterSoftball", filters.Softball.ToString(), option);
            cookies.Append("filterU10", filters.U10.ToString(), option);
            cookies.Append("filterU12", filters.U12.ToString(), option);
            cookies.Append("filterU15", filters.U15.ToString(), option);
            cookies.Append("filterU18", filters.U18.ToString(), option);
            cookies.Append("filterAdult", filters.Adult.ToString(), option);
            cookies.Append("filterVeteran", filters.Veteran.ToString(), option);
            cookies.Append("filterFun", filters.Fun.ToString(), option);
            cookies.Append("filterGeneral", filters.General.ToString(), option);
            cookies.Append("filterAnnual", filters.Annual.ToString(), option);
            cookies.Append("filterOfficial", filters.Official.ToString(), option);
            cookies.Append("filterInternational", filters.International.ToString(), option);
        }

        
    }

    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }

    public static class GamesLib
    {
        public static int GetShowIndex(this List<DayGames> schedule)
        {
            if (schedule == null || schedule.Count == 0) return 0;

            var curDate = DateTime.Now.Date;
            var showDayGames = schedule.FirstOrDefault(dg => dg.GamesDate == curDate);
            if (showDayGames == null)
            {
                if (schedule.Min(gd => gd.GamesDate) > curDate)
                {
                    return 0;
                }
                else if (schedule.Max(gd => gd.GamesDate) < curDate)
                {
                    return schedule.Count - 1;
                }
                else
                {
                    showDayGames = schedule.Where(gd => gd.GamesDate > curDate).OrderBy(gd => gd.GamesDate).First();
                }
            }

            return schedule.IndexOf(showDayGames);
        }

        public static string GetYouTubeEmbeded(this string URL)
        {
            //const string input = "http://www.youtube.com/watch?v=bSiDLCf5u3s " +
            //         "https://www.youtube.com/watch?v=bSiDLCf5u3s " +
            //         "http://youtu.be/bSiDLCf5u3s " +
            //         "www.youtube.com/watch?v=bSiDLCf5u3s " +
            //         "youtu.be/bSiDLCf5u3s " +
            //         "http://www.youtube.com/watch?feature=player_embedded&v=bSiDLCf5u3s " +
            //         "www.youtube.com/watch?feature=player_embedded&v=bSiDLCf5u3s " +
            //         "http://www.youtube.com/watch?v=_-QpUDvTdNY";
            if (URL.IndexOf("shorts") != -1)
            {
                return URL.Replace("shorts", "embed");
            }
            else 
            {
                const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/|)([\w\-]+))(?:[^\s?]+)?)";
                //const string replacement = "<iframe title='YouTube video player' width='480' height='390' src='http://www.youtube.com/embed/$1' frameborder='0' allowfullscreen='1'></iframe>";
                const string replacement = "https://www.youtube.com/embed/$1";

                var rgx = new Regex(pattern);
                return rgx.Replace(URL, replacement);
            }

        }
    }
}
