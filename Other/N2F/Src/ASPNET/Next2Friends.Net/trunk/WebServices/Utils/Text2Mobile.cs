/* ------------------------------------------------
 * Text2Mobile.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Next2Friends.WebServices.Utils
{
    internal static class Text2Mobile
    {
        /// <summary>
        /// Removes <code><br /></code> or <code><br></code> tags from the input string,
        /// replaces <code><a href=...>value</a></code> with <code>value</code>,
        /// replaces special characters such as <code>&quot;</code> or <code>&lt;</code> with their equivalents - <code>"</code> and <code><</code>.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Returns <code>null</code> if the specified <code>str</code> is <code>null</code>.</returns>
        public static String Filter(String str)
        {
            if (str == null)
                return null;

            String result = null;
            
            /* Replaces special characters such as &quot; or &lt; with their equivalents - " and <. */
            result = HttpUtility.HtmlDecode(str);
            
            /* Replaces <br /> with a space. */
            var brTags = new Regex("<br\\s*/?>");
            result = brTags.Replace(result, " ");

            /* Replaces <a href=...>value</a> with value. */
            var aTags = new Regex("<a href=\"[^>]+\">(?<value>.*?)</a>");
            result = aTags.Replace(result, m => m.Groups["value"].Value);
            
            return result;
        }
    }
}
