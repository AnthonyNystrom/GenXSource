using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;

public partial class UploadPhotoPage : System.Web.UI.Page, System.Web.SessionState.IRequiresSessionState
{
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        // allows the javascript function to do a postback and call the onClick method
        // associated with the linkButton LinkButton1.
        string jscript = "function UploadComplete(){";
        jscript += string.Format("__doPostBack('{0}','');", LinkButton1.ClientID.Replace("_", "$"));
        jscript += "};";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
    }

    protected string GetFlashVars()
    {
        // Adds query string info to the upload page
        // you can also do something like:
        // return "?" + Server.UrlEncode("CategoryID="+CategoryID);
        // we UrlEncode it because of how LoadVars works with flash,
        // we want a string to show up like this 'CategoryID=3&UserID=4' in
        // the uploadPage variable in flash.  If we passed this string withou
        // UrlEncode then flash would take UserID as a seperate LoadVar variable
        // instead of passing it into the uploadPage variable.
        // then in the httpHandler we get the CategoryID and UserID values from 
        // the query string. See Upload.cs in App_Code
        Member member = (Member)Session["Member"];
        string WebMemberID = member.WebMemberID;
        string token = WebMemberID;
        return "?token=" + token + "&type=photo";
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //Session["UploadedPhotos"] = Context.Session["UploadedPhotos"];
        // just use this to refresh the page and reset the flash uploader interface
        Response.Redirect("MyPhotoGallery.aspx");
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member  =(Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("signup.aspx?u=" + Request.Url.AbsoluteUri);
        }

        Master.SkinID = "photo";
        base.OnPreInit(e);
    }
}
