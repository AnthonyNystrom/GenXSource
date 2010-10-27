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
using Next2Friends.Misc;
using Next2Friends.Data;

public partial class ForwardToFriendCtrl : System.Web.UI.UserControl
{
    public CommentType ContentType { get; set; }
    public string ObjectWebID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ForwardToFriendCtrl));
    }

    /// <summary>
    /// Forward content to an external friend
    /// </summary>
    /// <param name="type">Type of content : Video, Photo, AAF etc.</param>
    /// <param name="WebID">The WebID of the content</param>
    /// <param name="Email">Email address of the external friend</param>
    /// <param name="Message">Custom Message</param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public int ForwardToFriend(string type, string WebID, object[] Email, string Message)
    {

        try
        {
            Member member = (Member)HttpContext.Current.Session["Member"];
            CommentType contentType = (CommentType)(Enum.Parse(typeof(CommentType), type));

            for (int i = 0; i < Email.Count(); i++)
            {
                if (Email[i] != null)
                {
                    if (!RegexPatterns.TestEmailRegex(Email[i].ToString()))
                    {
                        //Error parsing email
                        return -1;
                    }

                    ContentInvite invite = new ContentInvite();
                    invite.ObjectID = GetObjectID(contentType, WebID);
                    invite.ObjectType = (int)contentType;
                    invite.MemberID = member.MemberID;
                    invite.EmailAddress = Email[i].ToString();
                    invite.CustomMessage = Message;
                    invite.DTCreated = DateTime.Now;
                    invite.Save();
                }
            }
        }
        catch(Exception ex)
        {
            //general error
            return -2;
        }

        return 0;
    }


    private int GetObjectID(CommentType type, string WebID)
    {
        if (type == CommentType.Wall)
        {
            Member m = Member.GetMemberViaWebMemberID(WebID);
            return m.MemberID;
        }
        else if (type == CommentType.Video)
        {
            Video v = Video.GetVideoByWebVideoIDWithJoin(WebID);
            return v.VideoID;
        }
        else if (type == CommentType.AskAFriend)
        {
            AskAFriend aaf = AskAFriend.GetAskAFriendByWebAskAFriendID(WebID);
            return aaf.AskAFriendID;
        }
        else if (type == CommentType.Blog)
        {
            BlogEntry blog = BlogEntry.GetBlogEntryByWebBlogEntryID(WebID);
            return blog.BlogEntryID;
        }
        else if (type == CommentType.Photo)
        {
            Photo photo = Photo.GetPhotoByWebPhotoIDWithJoin(WebID);
            return photo.PhotoID;
        }
        else if (type == CommentType.PhotoGallery)
        {
            PhotoCollection photoColl = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(WebID);
            return photoColl.PhotoCollectionID;
        }

        return -1;
    }
}
