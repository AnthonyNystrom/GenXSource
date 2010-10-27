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
using System.Xml.Linq;

namespace Next2Friends.WebControls
{

    public class DropDownBase : DropDownList
    {
        public bool ShowTitle { get; set; }
    }

    /// <summary>s
    /// Summary description for CountryDropdown
    /// </summary>
    public class CategoryDropdown : DropDownBase
    {

        public CategoryDropdown()
        {
            string CacheKey = "Categories";

            List<Category> categories = (List<Category>)System.Web.HttpContext.Current.Cache[CacheKey];

            //if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
            ///{
            categories = Category.GetAllCategory();
            // System.Web.HttpContext.Current.Cache.Insert(CacheKey, categories, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);

            //}

            for (int i = 0; i < categories.Count; i++)
            {
                this.Items.Add(new ListItem(categories[i].Name, categories[i].CategoryID.ToString()));
            }

            this.Width = new Unit(50);
        }        
    }

    public class DayDropdown : DropDownBase
    {
        public DayDropdown()
        {

            if( ShowTitle )
                this.Items.Add(new ListItem("Day", ""));

            for (int i = 0; i < 31; i++)
            {
                this.Items.Add(new ListItem((i + 1).ToString("00"), (i + 1).ToString()));
            }

            this.Width = new Unit(50);

        }
    }

    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class MonthDropdown : DropDownBase
    {
        public MonthDropdown()
        {
            if (ShowTitle)
                this.Items.Add(new ListItem("Month", ""));

            for (int i = 0; i < 12; i++)
            {
                this.Items.Add(new ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1), (i + 1).ToString()));
            }

            this.Width = new Unit(70);
        }
    }

    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class YearDropdown : DropDownBase
    {
        public YearDropdown()
        {
            if (ShowTitle)
                this.Items.Add(new ListItem("Year", ""));

            for (int i = 1991; i > 1920; i--)
            {
                this.Items.Add(new ListItem((i - 1).ToString(), (i - 1).ToString()));
            }

            this.Width = new Unit(60);
        }
    }

    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class CurrentYearDropdown : DropDownBase
    {
        public CurrentYearDropdown()
        {
            if (ShowTitle)
                this.Items.Add(new ListItem("Year", ""));

            for (int i = 2007; i < 2009; i++)
            {
                this.Items.Add(new ListItem((i).ToString(), (i).ToString()));
            }

            this.Width = new Unit(60);
        }
    }

    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class HourDropdown : DropDownBase
    {
        public HourDropdown()
        {
            if (ShowTitle)
                this.Items.Add(new ListItem("Hour", ""));

            for (int i = 1; i < 25; i++)
            {
                this.Items.Add(new ListItem((i - 1).ToString(), (i - 1).ToString()));
            }

            this.Width = new Unit(60);
        }
    }


    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class HourDurationDropdown : DropDownBase
    {
        public HourDurationDropdown()
        {
            for (int i = 1; i < 24; i++)
            {
                this.Items.Add(new ListItem((i).ToString(), (i).ToString()));
            }

            this.Width = new Unit(60);
        }
    }

    /// <summary>
    /// Summary description for CountryDropdown
    /// </summary>
    public class MinuteDropdown : DropDownBase
    {
        public MinuteDropdown()
        {
            if (ShowTitle)
                this.Items.Add(new ListItem("Minute", ""));

            for (int i = 1; i < 61; i++)
            {
                this.Items.Add(new ListItem((i - 1).ToString(), (i - 1).ToString()));
            }

            this.Width = new Unit(60);
        }
    }
}