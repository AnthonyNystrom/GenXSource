using System;
using System.Text;
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
/// <summary>
/// Summary description for PopupHTML
/// </summary>
public class PopupHTML
{
    public PopupHTML()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetMiniProgileHTML(string WebMemberID, Member member)
    {
        Member MiniProfileMember = Member.GetMembersViaWebMemberIDWithFullJoin(WebMemberID);
        string MiniProfileHTML = string.Empty;

        int MutualFriendCount = Friend.GetMutualFriendCount(MiniProfileMember, member);

        try
        {

            if (MiniProfileMember != null)
            {
                StringBuilder sbHTML = new StringBuilder();

                string[] Parameters = new string[7];
                Parameters[0] = MiniProfileMember.NickName;
                ResourceFile PhotoRes = new ResourceFile(MiniProfileMember.ProfilePhotoResourceFileID);
                Parameters[1] = "http://www.next2friends.com/" + PhotoRes.FullyQualifiedURL;
                Parameters[2] = MiniProfileMember.FirstName + " " + MiniProfileMember.LastName;
                Parameters[3] = UserStatus.IsUserOnline(MiniProfileMember.WebMemberID) ? "<img class=\"online-offline\" src=\"/images/online.gif\" alt=\"Online\" /> Online now" : "<img class=\"online-offline\"  src=\"/images/offline.gif\" alt=\"Offline\" /> Offline";
                Parameters[4] = MiniProfileMember.AgeYears.ToString();
                Parameters[5] = MiniProfileMember.WebMemberID;
                Parameters[6] = MutualFriendCount.ToString();


                sbHTML.AppendFormat(@"<div class='popupActions'>
				<ul class='friend_actions' style='width:150px'>
					<li><a class='send_message' onmouseover='return true;' href='javascript:openMsg();'>Send Message</a></li>		
					<li><a class='forward' onmouseover='return true;' href='/Inbox.aspx?f={5}'>Forward to a friend</a></li>
                    <li><a class='forward' onmouseover='return true;' href='/users/{0}'>View full profile</a></li>
				</ul>
			</div>
			<div class='popupProfileContent'>
				<p class='profile_pic'>
						<img id='imgProfile' alt='Comosicus profile pic' src='{1}'/>
				</p>
				<dl>
					<dt>Name:</dt><dd><strong>{2}</strong></dd>
                    <dt>Age:</dt><dd><strong>{4}</strong></dd>
					<dt>Friends:</dt><dd>{6} mutual friends</dd>
				</dl>
                <div class='clear'/>
                 <div id='divMsg' style='display:none'>
                    <p>Send message to {2}:</p>
                    <p><textarea rows='3' style='width: 100%;'/></textarea></p>
                    <p style='text-align:right;'><input type='button' class='form_btn2' value='cancel' onclick='closeMsg();'/><input type='button' class='form_btn2' value='send'/></p>
                </div>
			</div>", Parameters);
                MiniProfileHTML = sbHTML.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return MiniProfileHTML;

    }

    public static string GetMiniPhotoGalleryeHTML(string WebPhotoGallery)
    {
        PhotoCollection collection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(WebPhotoGallery);
        List<Photo> photos = Photo.GetPhotoByPhotoCollectionIDWithJoinPager(collection.PhotoCollectionID,1, 9);

        string MiniHTML = string.Empty;

        if (photos != null)
        {
            StringBuilder sbHTML = new StringBuilder();

            for (int i = 0; i < 9; i++)
            {
                if (i >= photos.Count)
                {
                    break;
                }
                
                object[] parameters = new object[2];

                parameters[0] = ParallelServer.Get(photos[i].ThumbnailResourceFile.FullyQualifiedURL) + photos[i].ThumbnailResourceFile.FullyQualifiedURL;
                parameters[1] = i.ToString();

                sbHTML.AppendFormat(@"<li>
                            	    <a href=''><img src='{0}' alt='thumb' /></a>
                                </li>", parameters);
            }


            MiniHTML = "<ul class='profile_gallery clearfix'>" + sbHTML.ToString() + "</ul>";
        }

        return MiniHTML;

    }
}
