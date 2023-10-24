using BaseballUa.DTO.Custom;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

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
            var curDate = DateTime.Now.Date.AddMonths(-1);
            var showDayGames = schedule.Where(dg => dg.GamesDate == curDate).FirstOrDefault();
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
    }
}
