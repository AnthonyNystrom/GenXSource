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


public partial class UploadVideo2 : System.Web.UI.Page
{
    public Member member = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;

            RadProgressArea1.ProgressIndicators = ProgressIndicators.CurrentFileName |
            ProgressIndicators.FilesCount |
            ProgressIndicators.FilesCountBar |
            ProgressIndicators.FilesCountPercent |
            ProgressIndicators.RequestSize |
            ProgressIndicators.TimeElapsed |
            ProgressIndicators.TimeEstimated |
            ProgressIndicators.TotalProgress |
            ProgressIndicators.TotalProgressBar |
            ProgressIndicators.TotalProgressPercent |
            ProgressIndicators.TransferSpeed;

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

    protected void buttonSubmit_Click(object sender, System.EventArgs e)
    {
        UploadedFile file = RadUploadContext.Current.UploadedFiles[inputFile.UniqueID];

        ProcessVideo(file);
    }

    //private void LooongMethodWhichUpdatesTheProgressContext(UploadedFile file)
    //{
    //    const int total = 100;

    //    RadProgressContext progress = RadProgressContext.Current;

    //    for (int i = 0; i < total; i++)
    //    {
    //        progress["PrimaryTotal"] = total.ToString();
    //        progress["PrimaryValue"] = i.ToString();
    //        progress["PrimaryPercent"] = i.ToString();
    //        progress["CurrentOperationText"] = file.GetName() + " is being processed...";

    //        if (!Response.IsClientConnected)
    //        {
    //            //Cancel button was clicked or the browser was closed, so stop processing
    //            break;
    //        }

    //        //Stall the current thread for 0.1 seconds
    //        System.Threading.Thread.Sleep(100);
    //    }
    //}

    public void ProcessVideo(UploadedFile VideoFile)
    {
        litCompleted.Text = "";
        try
        {
            member = (Member)Session["Member"];
            bool ValidExtention = true;
            string VideoTitle = string.Empty;
            string Ext = string.Empty;

            Video video = new Video();

            video.Title = (txtTitle.Text != "") ? txtTitle.Text : "New video";
            video.Description = (txtCaption.Text != "") ? txtCaption.Text : "No Description"; 

            try
            {
                Ext = VideoFile.GetExtension();
            }
            catch
            {
                Ext = "xxx";
                ValidExtention = false;
            }

            Ext = Ext.Replace(".", "");

            // upload the flv
            if (IsVideo(Ext))
            {
                if (VideoFile.ContentLength > 150000)
                {
                    Video.QueueVideoForEncoding(video, VideoFile.InputStream, Ext, member, VideoTitle);
                    litCompleted.Text = "<script>parent.location.href='MyVideoGallery.aspx?p=1';</script><img src='images/check.gif'/>&nbsp;&nbsp; Your video was successfully uploaded ";
                }
                else
                {
                    litCompleted.Text = "<img src='images/na.gif'/>&nbsp;&nbsp;Your video must be at least 2 seconds long";
                }
            }
            else
            {
                if (ValidExtention)
                    litCompleted.Text = "<img src='images/na.gif'/>&nbsp;&nbsp;Videos with the extention <strong>" + Ext + "</strong> arent supported";
                else
                    litCompleted.Text = "<img src='images/na.gif'/>&nbsp;&nbsp;Videos files must have an extention name. For example: .avi or .mov";
            }
        }
        catch(Exception ex)
        {
            string MachineName = Environment.MachineName;
            Next2Friends.Data.Trace.Tracer(ex.ToString(), "RadVideoUpload");
        }
    }

    private bool IsVideo(string ext)
    {
        if (ext.ToLower() == "wmv")
            return true;
        else if (ext.ToLower() == "avi")
            return true;
        else if (ext.ToLower() == "mov")
            return true;
        else if (ext.ToLower() == "mpg")
            return true;
        else if (ext.ToLower() == "mpeg")
            return true;
        else if (ext.ToLower() == "mp4")
            return true;
        else if (ext.ToLower() == "3gp")
            return true;
        else if (ext.ToLower() == "3g2")
            return true;
        else
            return false;
    }

    ///// <summary>
    ///// set the page skin
    ///// </summary>
    ///// <param name="e"></param>
    //protected override void OnPreInit(EventArgs e)
    //{
    //    member = (Member)Session["Member"];

    //    if (member == null)
    //    {
    //        Response.Redirect("signup.aspx?u=" + Request.Url.AbsoluteUri);
    //    }

    //    Master.SkinID = "video";
    //    base.OnPreInit(e);
    //}
}
