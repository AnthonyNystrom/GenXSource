// According to http://msdn2.microsoft.com/en-us/library/system.web.httppostedfile.aspx
// "Files are uploaded in MIME multipart/form-data format. 
// By default, all requests, including form fields and uploaded files, 
// larger than 256 KB are buffered to disk, rather than held in server memory."
// So we can use an HttpHandler to handle uploaded files and not have to worry
// about the server recycling the request do to low memory. 
// don't forget to increase the MaxRequestLength in the web.config.
// If you server is still giving errors, then something else is wrong.
// I've uploaded a 1.3 gig file without any problems. One thing to note, 
// when the SaveAs function is called, it takes time for the server to 
// save the file. The larger the file, the longer it takes.
// So if a progress bar is used in the upload, it may read 100%, but the upload won't
// be complete until the file is saved.  So it may look like it is stalled, but it
// is not.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.SessionState;
using Next2Friends.Data;
using Next2Friends.Misc;

/// <summary>
/// Upload handler for uploading files.
/// </summary>
public class Upload : IHttpHandler, IRequiresSessionState 
{
    private Member member;
    private int PhotoCollectionID;

    public Upload()
    {
    }

    #region IHttpHandler Members

    public bool IsReusable
    {
        get { return true; }
    }

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string token = context.Request.QueryString["token"];
            string URLparams = string.Empty;

            if (token != null)
            {
                member = Member.GetMemberViaWebMemberID(token);
            }
            else
            {
                throw new Exception("Bad token");

            }


            if (member == null)
                throw new Exception("Member not logged in");

            PhotoCollectionID = member.MemberProfile[0].DefaultPhotoCollectionID;

            if (context.Request.Files.Count > 0)
            {
                // get the applications path
                string tempFile = context.Request.PhysicalApplicationPath;
                // loop through all the uploaded files
                for (int j = 0; j < context.Request.Files.Count; j++)
                {
                    // get the current file
                    HttpPostedFile uploadFile = context.Request.Files[j];
                    // if there was a file uploded
                    if (uploadFile.ContentLength > 0)
                    {


                        string VideoTitle = "";


                        try
                        {
                            VideoTitle = context.Request.Files[j].FileName.Substring(0, context.Request.Files[j].FileName.Length - 4);
                        }
                        catch
                        {
                            VideoTitle = "New video";
                        }

                        // upload the flv
                        if (IsVideo(uploadFile.FileName))
                        {
                            if (uploadFile.ContentLength > 150000)
                            {
                                string ext = uploadFile.FileName.Substring(uploadFile.FileName.Length - 3, 3);

                                //Video.QueueVideoForEncoding(video, uploadFile.InputStream, ext, member, VideoTitle);
                            }
                        }
                        else
                        {
                            System.Drawing.Image image = null;

                            try
                            {
                                image = Bitmap.FromStream(uploadFile.InputStream);
                            }
                            catch (Exception ex)
                            {
                                HttpContext.Current.Response.Write(" Not a valid image file");
                                return;
                            }

                            string WebPhotoID = Photo.ProcessMemberPhoto(member, PhotoCollectionID, image, DateTime.Now,false);

                            URLparams += WebPhotoID + "--";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Trace.Tracer(ex.ToString(), "Uploader");
        }

        HttpContext.Current.Response.Write(" ");
    }

    private bool IsVideo(string FileName)
    {
        string ext = FileName.Substring(FileName.Length - 3, 3);

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
        else if (ext.ToLower() == "3GP")
            return true;
        else if (ext.ToLower() == "3G2")
            return true;
        else
            return false;
    }

    #endregion
}
