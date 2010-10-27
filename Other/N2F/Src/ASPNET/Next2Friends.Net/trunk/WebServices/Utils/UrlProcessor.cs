/* ------------------------------------------------
 * UrlProcessor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Text.RegularExpressions;

namespace Next2Friends.WebServices.Utils
{
    internal static class UrlProcessor
    {
        public static String ExtractWebBlogEntryID(String url)
        {
            return ExtractID(@"/blog.aspx\?m=.+\&b=(?<id>.+)", url);
        }

        public static String ExtractWebAskID(String url)
        {
            return url.Substring(5);
        }

        public static String ExtractWallName(String url)
        {
            return ExtractID(@"/users/(?<id>.+)/wall", url);
        }

        public static String ExtractWebVideoID(String url)
        {
            return ExtractID(@"/video/.+/(?<id>.+)", url);
        }

        public static String ExtractWebPhotoCollectionID(String url)
        {
            return ExtractID(@"/gallery/\?g=(?<id>.+)\&m=.+", url);
        }

        public static String ExtractWebPhotoID(String url)
        {
            return ExtractID(@"/gallery/\?g=.+\&m=.+\&wp=(?<id>.+)", url);
        }

        private static String ExtractID(String pattern, String url)
        {
            var regex = new Regex(pattern);
            return regex.Replace(url, m => m.Groups["id"].Value);
        }
    }
}
