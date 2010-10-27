/* ------------------------------------------------
 * PicnikSave.aspx.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Drawing;
using Next2Friends.Data;

public partial class PicnikSave : Page
{
    protected void Page_Load(Object sender, EventArgs e)
    {
        var member = Session["Member"] as Member;
        if (member == null)
        {
            Response.Write("member == null");
            return;
        }

        var webPhotoId = Convert.ToString(Session["PicnikWebPhotoId"]);
        
        Response.Write("webPhotoId = " + webPhotoId);
        Response.Write("<br />");

        if (!String.IsNullOrEmpty(webPhotoId))
        {
            var photo = Photo.GetPhotoByWebPhotoIDWithJoin(webPhotoId);
            if (photo == null)
                return;

            Session.Remove("PicnikWebPhotoId");
            var picnikPhotoUrl = "";

            if (Request.QueryString["file"] != null)
                picnikPhotoUrl = HttpUtility.UrlDecode(Request.QueryString["file"]);

            Response.Write("picnikPhotoUrl = " + picnikPhotoUrl);
            Response.Write("<br />");

            if (!String.IsNullOrEmpty(picnikPhotoUrl))
                SavePicnikPhoto(member, photo, picnikPhotoUrl);
        }
    }

    private void SavePicnikPhoto(Member member, Photo photo, String picnikPhotoUrl)
    {
        Debug.Assert(member != null, "member != null");
        Debug.Assert(photo != null, "photo != null");
        Debug.Assert(!String.IsNullOrEmpty(picnikPhotoUrl), "!String.IsNullOrEmpty(picnikPhotoUrl)");

        using (var client = new WebClient())
        using (var stream = client.OpenRead(picnikPhotoUrl))
        using (var largeImage = Image.FromStream(stream))
        {
            using (var mediumImage = Photo.Resize480x480(largeImage))
            {
                Photo.SaveToDiskRelativePath(mediumImage, GetMediumFullPath(member, photo));
            }

            using (var thumbImage = Photo.ResizeTo124x91(largeImage))
            {
                Photo.SaveToDiskRelativePath(thumbImage, GetThumbFullPath(member, photo));
            }

            Photo.SaveToDiskRelativePath(largeImage, GetLargeFullPath(member, photo));
        }
    }

    private String GetFullPath(Member member)
    {
        Debug.Assert(member != null, "member != null");
        return String.Concat(ASP.global_asax.DiskUserRoot, member.NickName);
    }

    private String GetLargeFullPath(Member member, Photo photo)
    {
        Debug.Assert(member != null, "member != null");
        Debug.Assert(photo != null, "photo != null");
        return String.Format("{0}\\plrge\\{1}", GetFullPath(member), photo.PhotoResourceFile.FileName);
    }

    private String GetMediumFullPath(Member member, Photo photo)
    {
        Debug.Assert(member != null, "member != null");
        Debug.Assert(photo != null, "photo != null");
        return String.Format("{0}\\pmed\\{1}", GetFullPath(member), photo.PhotoResourceFile.FileName);
    }

    private String GetThumbFullPath(Member member, Photo photo)
    {
        Debug.Assert(member != null, "member != null");
        Debug.Assert(photo != null, "photo != null");
        return String.Format("{0}\\pthmb\\{1}", GetFullPath(member), photo.PhotoResourceFile.FileName);
    }
}
