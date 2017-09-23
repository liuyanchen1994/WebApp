using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public class StringHelper
    {
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return default;
            }
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length) + "...";

            return strText;
        }

        public static string SubString(string str,int length)
        {
            if (string.IsNullOrWhiteSpace(str)) return default;
            if (str.Length > length)
            {
                return str.Substring(0, length) + "...";
            }
            return str;
        }
    }
}
