using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;
using Telerik.WebControls;

public partial class UploadVideo : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;

            RadProgressArea1.Localization["UploadedFiles"] = "Processed ";
            RadProgressArea1.Localization["TotalFiles"] = "";
            RadProgressArea1.Localization["CurrentFileName"] = "File: ";

            RadProgressContext progress = RadProgressContext.Current;
            //Prevent the secondary progress from appearing when the file is uploaded (FileCount etc.)
            progress["SecondaryTotal"] = "0";
            progress["SecondaryValue"] = "0";
            progress["SecondaryPercent"] = "0";
        }
    }

    private void buttonSubmit_Click(object sender, System.EventArgs e)
    {
        UploadedFile file = RadUploadContext.Current.UploadedFiles[inputFile.UniqueID];

        if (file != null)
        {
            LooongMethodWhichUpdatesTheProgressContext(file);
        }
    }

    private void LooongMethodWhichUpdatesTheProgressContext(UploadedFile file)
    {
        const int total = 100;

        RadProgressContext progress = RadProgressContext.Current;

        for (int i = 0; i < total; i++)
        {
            progress["SecondaryTotal"] = total.ToString();
            progress["SecondaryValue"] = i.ToString();
            progress["SecondaryPercent"] = i.ToString();
            progress["CurrentOperationText"] = file.GetName() + " is being processed...";

            if (!Response.IsClientConnected)
            {
                //Cancel button was clicked or the browser was closed, so stop processing
                break;
            }

            //Stall the current thread for 0.1 seconds
            System.Threading.Thread.Sleep(100);
        }
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
        return "?token=" + token+"&type=photo";
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        // just use this to refresh the page and reset the flash uploader interface
        Response.Redirect("MyVideoGallery.aspx");
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("signup.aspx?u=" + Request.Url.AbsoluteUri);
        }

        Master.SkinID = "video";
        base.OnPreInit(e);
    }
}
