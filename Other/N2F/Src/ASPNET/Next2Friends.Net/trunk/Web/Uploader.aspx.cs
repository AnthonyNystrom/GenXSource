using System;
using System.Collections.Generic;
using System.Web;
using Next2Friends.Data;

public partial class Uploader : System.Web.UI.Page
{
    public string EncWebMemberID = string.Empty;
    public string Params = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Member member = (Member)Session["Member"];

        RememberMeLogin();

        EncWebMemberID = RijndaelEncryption.Encrypt(member.WebMemberID);

        List<PhotoCollection> Gallerys = PhotoCollection.GetAllPhotoCollectionWithEmptyByMemberID(member.MemberID);

        for (int i = 0; i < Gallerys.Count; i++)
        {
           Params += "<PARAM name=\"galleryName::"+i+"\" value=\""+Gallerys[i].Name.Replace("'","")+"\" />";
           Params += "<PARAM name=\"galleryID::" + i + "\" value=\"" + Gallerys[i].WebPhotoCollectionID + "\" />";
        }

        string DefaultGalleryID = Request.Params["DefaultGalleryID"];

        if (DefaultGalleryID != null)
        {
            Params += "<PARAM name=\"DefaultGalleryID\" value=\"" + DefaultGalleryID + "\" />";
        }

    }

    public void RememberMeLogin()
    {
        // If we are already signed in
        if (Session["Member"] != null)
            return;

        HttpCookie aCookie = Request.Cookies["LastActivity"];



        if (aCookie == null)
            return;

        string autoLogin = aCookie.Values["activityHandle"];

        if (autoLogin == "1")
        {

            string login = aCookie.Values["activityDate"];
            string password = aCookie.Values["activityTime"];

            login = RijndaelEncryption.Decrypt(login);
            password = RijndaelEncryption.Decrypt(password);

            Member memberD = Member.WebMemberLogin(login, password);

            Session["Member"] = memberD;

            OnlineNow now = new OnlineNow();
            now.MemberID = memberD.MemberID;
            now.DTOnline = DateTime.Now;
            now.Save();

            Utility.AddToLoggedIn();
        }

    }
}
