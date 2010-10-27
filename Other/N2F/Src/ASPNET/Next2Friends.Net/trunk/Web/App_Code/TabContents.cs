using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

/// <summary>
/// Summary description for TabContents
/// </summary>
public class TabContents
{

    public int TabType { get; set; }
    public string HTML { get; set; }
    public string PagerHTML { get; set; }
    public string PagerHTMLRight { get; set; }


}
