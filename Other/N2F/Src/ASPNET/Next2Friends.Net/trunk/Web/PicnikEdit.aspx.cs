/* ------------------------------------------------
 * PicnikEdit.aspx.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Web;
using System.Web.UI;
using Next2Friends.Data;
using System.Diagnostics;
using System.Text;

public partial class PicnikEdit : Page
{
    private String _photoUrl;
    private String _picnikUrl;
    public String PicnikUrl
    {
        get { return _picnikUrl; }
    }

    protected void Page_Load(Object sender, EventArgs e)
    {
        var member = Session["Member"] as Member;
        if (member != null)
            StartPicnik(Request.QueryString["WebPhotoId"], member);
    }

    private void StartPicnik(String webPhotoId, Member member)
    {
        Debug.Assert(!String.IsNullOrEmpty(webPhotoId), "!String.IsNullOrEmpty(webPhotoId)");

        Session.Add("PicnikWebPhotoId", webPhotoId);
        _photoUrl = String.Format("http://www.next2friends.com/user/{0}/plrge/{1}.jpg", member.NickName, webPhotoId);

        var preparedData = new StringBuilder()
            .Append("_apikey=06bde521efb8c116efc2766a87377ba3")
            .Append("&_export=http://localhost:4804/PicnikSave.aspx")
            .Append("&_close_target=http://localhost:4804/PicnikSave.aspx")
            .Append("&_export_method=GET")
            .Append("&_export_agent=browser")
            .Append("&_export_title=").Append("Save My Changes")
            .Append("&_replace=yes")
            .Append("&_host_name=Next2Friends")
            .Append("&_exclude=out")
            .Append("&_import=").Append(HttpUtility.UrlEncode(_photoUrl));
        _picnikUrl = String.Format("http://www.picnik.com/service/?{0}", preparedData);
    }
}
