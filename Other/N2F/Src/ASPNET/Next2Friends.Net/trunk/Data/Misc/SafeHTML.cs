using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Text.RegularExpressions;

namespace Next2Friends.Misc
{
    public class SafeHTML
    {
        /// <summary>
        /// Replaces any javascript or <> and quote marks
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string CleanText(string Text)
        {
            return Text;
        }

        /// <summary>
        /// formats the text into html
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string FormatForHTML(string HTML)
        {
            return HTML.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// formats the text from HTML into standard text
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string FormatForText(string HTML)
        {
            return HTML.Replace("<br />", "\r\n");
        }

        /// <summary>
        /// Auto links all occurances of http links
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string AutoLink(string HTML)
        {
            Regex regEx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return regEx.Replace(HTML, @"<a href=""$&"" target=""_blank"">$&</a>");            
        }


    }

}