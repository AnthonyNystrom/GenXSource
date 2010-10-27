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
/// Summary description for AjaxAskAFriend
/// </summary>
public class AjaxAskAFriend
{
    public string Bookmarks { get; set; }
    public string Permalink { get; set; }
    public string HTML { get; set; }
    public string Question { get; set; }
    public string LastAAF { get; set; }
    public string Comments { get; set; }
    public string CommentPost { get; set; }
    public string WebAskAFriendID { get; set; }

    public AjaxAskAFriend()
    {
         
    }
}
