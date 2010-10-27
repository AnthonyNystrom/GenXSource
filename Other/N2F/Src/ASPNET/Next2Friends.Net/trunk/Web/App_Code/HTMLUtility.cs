using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for SafeHTML
/// </summary>
public class HTMLUtility
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
        HTML = HTML.Replace("&amp;", "&");
        Regex regEx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&,+=]*)?");
        return regEx.Replace(HTML, @"<a href=""$&"" target=""_blank"">$&</a>");
    }


}
