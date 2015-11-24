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

            return new DateTime(1900, 1, 1);
        }
    }

    public static class StringExtensions
    {
        public static string ValidAnimeFolder(this string value)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(System.Web.HttpUtility.HtmlDecode(value));
            sb.Replace(":", " ");
            sb.Replace("?", " ");
            sb.Replace("\"", " ");
            sb.Replace("\\", " ");


            return sb.ToString();

        }
    }
}
