using Microsoft.AspNetCore.Mvc.Rendering;

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
}
