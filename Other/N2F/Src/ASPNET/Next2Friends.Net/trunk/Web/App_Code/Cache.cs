using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;

/// <summary>
/// Summary description for Cache
/// </summary>
public class NCache
{
    public NCache()
    {

    }

    public static string GetCategoryName(int CategoryID)
    {
        string CacheKey = "Categories";

        List<Category> Categories = (List<Category>)System.Web.HttpContext.Current.Cache[CacheKey];

        if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        {

            Categories = Category.GetAllCategory();
            System.Web.HttpContext.Current.Cache.Insert(CacheKey, Categories, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        for (int i = 0; i < Categories.Count; i++)
        {
            if (Categories[i].CategoryID == CategoryID)
            {
                return Categories[i].Name;
            }

        }

        return "";
    }

    public static string GetCategoryLister()
    {
        string ListerHTML = string.Empty;

        string CacheKey = "CategoryLister";

        ListerHTML = (string)System.Web.HttpContext.Current.Cache[CacheKey];

        if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        {
            List<Category> Categories = Category.GetAllCategory();

            for (int i = 1; i < Categories.Count; i++)
            {
                ListerHTML += "<li><a href='/video/?cat=" + Categories[i].CategoryID + "'>" + Categories[i].Name + "</a></li>";
            }

            System.Web.HttpContext.Current.Cache.Insert(CacheKey, ListerHTML, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        return ListerHTML;
    }


    public static string GetVideosByTagNOTIMPLEMENTED(OrderByType orderByType, string Tag)
    {
        string ListerHTML = string.Empty;

        Video.GetVideosByTag(orderByType, Tag);

        //string CacheKey = "VideoByTag";

        //ListerHTML = (string)System.Web.HttpContext.Current.Cache[CacheKey];

        //if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        //{
        //    List<Category> Categories = Category.GetAllCategory();

        //    for (int i = 1; i < Categories.Count; i++)
        //    {
        //        ListerHTML += "<li><a href='video.aspx?cat=" + Categories[i].CategoryID + "'>" + Categories[i].Name + "</a></li>";
        //    }

        //    System.Web.HttpContext.Current.Cache.Insert(CacheKey, ListerHTML, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        //}

        return ListerHTML;
    }


    
}
