using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Next2Friends.Data;

/// <summary>
/// Summary description for CategoryDropdown
/// </summary>
public class CategoryDropdown : DropDownList
{
    public CategoryDropdown()
    {
        this.Items.Add(new ListItem("--", ""));

        string CacheKey = "Categories";

        List<Category> categories = (List<Category>)System.Web.HttpContext.Current.Cache[CacheKey];

        if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        {
            categories = Category.GetAllCategory();
            System.Web.HttpContext.Current.Cache.Insert(CacheKey, categories, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);

        }

        for (int i = 0; i < categories.Count; i++)
        {
            this.Items.Add(new ListItem(categories[i].Name, categories[i].CategoryID.ToString()));
        }

        this.Width = new Unit(50);
    }
}



