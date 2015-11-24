using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityImplementations.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetValidDate(this string value)
        {
            DateTime temp;
            if (DateTime.TryParse(value, out temp)) return temp;

            var dateSplit = value.Split('-').ToArray();
            int year = Convert.ToInt32(dateSplit[0]);
            int month = Convert.ToInt32(dateSplit[1]) == 0 ? 1 : Convert.ToInt32(dateSplit[1]);
            int day = Convert.ToInt32(dateSplit[2]) == 0 ? 1 : Convert.ToInt32(dateSplit[2]);
            if (year == 0) return new DateTime(1900, 1, 1);
           
            return new DateTime(year, month, day);
        }
    }

    public static class StringExtensions
    {
        public static string ValidAnimeFolder(this string value)
        {
            var sb = new System.Text.StringBuilder();
            value = value.StartsWith(".")? value.Replace(".", "dot") : value;
            sb.Append(System.Web.HttpUtility.HtmlDecode(value));
            sb.Replace(":", " ");
            sb.Replace("?", " ");
            sb.Replace("\"", " ");
            sb.Replace("\\", " ");
            sb.Replace("/", " ");
            sb.Replace("*", " ");
            sb.Replace("|", " ");
            sb.Replace(">", " ");
            sb.Replace("<", " ");
            return sb.ToString();

        }
    }
}
