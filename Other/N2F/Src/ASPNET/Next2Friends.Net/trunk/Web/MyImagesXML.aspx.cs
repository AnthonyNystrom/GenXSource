using System;
using System.IO;
using System.Collections.Generic;
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
using Next2Friends.Misc;

public partial class MyImagesXML : System.Web.UI.Page
{
    public string GalleryXML = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWebMemberID = Request.Params["m"];
        string strWebNSpotID = Request.Params["n"];

        if (strWebMemberID != null)
        {
            Member ViewingMember = Member.GetMemberViaWebMemberID(strWebMemberID);

            List<MemberProfile> profile = ViewingMember.MemberProfile;

            if (profile[0] != null)
            {
                //if (profile[0].ReserializeXML)
                //{

                    GalleryXML = Photo.MemberXMLGallery(ViewingMember.MemberID);
                    //Photo.SaveMemberXML(GalleryXML, ViewingMember);
                //}
                //else
                //{
                //    string path = OSRegistry.GetDiskUserDirectory() + ViewingMember.NickName + "/gallery.xml";
                //    StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8);
                //    GalleryXML = reader.ReadToEnd();
                //}
            }
        }
        else if (strWebNSpotID != null)
        {
            NSpot ViewingNSpot = NSpot.GetNSpotByNSpotWebID(strWebNSpotID);

            if (ViewingNSpot != null)
            {

                GalleryXML = Photo.NSpotXMLGallery(ViewingNSpot.NSpotID);
                //Photo.SaveMemberXML(GalleryXML, ViewingMember);

            }
        }

    }
}
