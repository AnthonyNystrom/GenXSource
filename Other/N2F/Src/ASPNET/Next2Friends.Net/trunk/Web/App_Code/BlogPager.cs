using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
/// Summary description for BlogPager
/// </summary>
public class BlogPager
{
    /// <summary>
    /// The page that the click will submit to
    /// </summary>
    public string Page { get; set; }

    /// <summary>
    /// Any extra parameters that the url should contain
    /// </summary>
    public string MiscParameterString { get; set; }

    /// <summary>
    /// The current page index
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// The number of items to display on a page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The number of boxes to display [1][2][3][4][5][6][7] etc
    /// </summary>
    public int ShowIndexNumber { get; set; }

    /// <summary>
    /// The current page index
    /// </summary>
    public int MaxItems { get; set; }

    private string HTML { get; set; }


    public BlogPager(string Page, string MiscParameterString, int PageIndex, int MaxItems)
    {
        this.Page = Page;
        this.MiscParameterString = MiscParameterString;
        this.PageIndex = PageIndex;
        this.MaxItems = MaxItems;

        this.PageSize = 20;
        this.ShowIndexNumber = 10;
    }

    public BlogPager(string Page, int PageIndex, int MaxItems)
    {
        this.Page = Page;
        this.MiscParameterString = string.Empty;
        this.PageIndex = PageIndex;
        this.MaxItems = MaxItems;

        this.PageSize = 20;
        this.ShowIndexNumber = 10;
    }

    public static int TryGetPageIndex(string PageIndexToTry)
    {
        int Page = 1;
        Int32.TryParse(PageIndexToTry, out Page);

        // set the page to 1 if no preference is found in the URL
        Page = (Page == 0) ? 1 : Page;

        return Page;
    }

    /// <summary>
    /// renders the object to HTML
    /// </summary>
    /// <returns></returns>
    /// 
    public override string ToString()
    {
        // Singlton pattern
        if (HTML == null)
            HTML = GenerateHTML();

        return HTML;
    }

    private string GenerateHTML()
    {
        int numberOfPages = MaxItems / PageSize+1;

        if (MaxItems % PageSize  == 0)
            numberOfPages = MaxItems / PageSize;


        int startIndex, endIndex;

        
        int pading=ShowIndexNumber/2;

        
        if (PageIndex > pading)

            startIndex = PageIndex - pading;

        else
            
            startIndex = 1;

        
        if (PageIndex + pading < numberOfPages)

            endIndex = PageIndex + pading;

        else

            endIndex = numberOfPages;


        if (MiscParameterString != string.Empty)

            MiscParameterString = "&" + MiscParameterString;

        else

            MiscParameterString = "";


        
        StringBuilder sbHTML = new StringBuilder();


        sbHTML.Append("<div class=\"pagenav2\">"); //begin


        if(PageIndex>1)

            sbHTML.AppendFormat("<a title=\"page {0}\" href=\"{1}?p={0}{2}\">« Previous</a>",

                PageIndex - 1, Page, MiscParameterString);

        else if(PageIndex==1)

            sbHTML.Append(@"<span>« Previous</span>");

        
      
        
        for (int i = startIndex; i <= endIndex; i++)
        {

            if(i!=PageIndex)
            
                sbHTML.AppendFormat("<a title=\"page {0}\" href=\"{1}?p={0}{2}\">{0}</a>",
                i, Page, MiscParameterString);
            else

                sbHTML.AppendFormat("<span class=\"current\">{0}</span>",i);


        }

        if(PageIndex<numberOfPages)

            sbHTML.AppendFormat("<a title=\"page {0}\" href=\"{1}?p={0}{2}\">Next »</a>",

               PageIndex+1, Page, MiscParameterString);

        else if (PageIndex == numberOfPages)

            sbHTML.Append(@"<span>Next »</span>");

  

        sbHTML.Append("</div>"); //end


        return sbHTML.ToString();
    }
}

