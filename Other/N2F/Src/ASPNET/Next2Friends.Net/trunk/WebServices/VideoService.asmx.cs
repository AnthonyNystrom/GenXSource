using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Next2Friends.Data;
using Next2Friends.WebServices.Video;

namespace Next2Friends.WebServices
{
    [WebService(Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class VideoService : WebService
    {
        [WebMethod]
        public VideoDescriptor[] GetVideos(String nickname, String password, String lastWebVideoID)
        {
            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            return VideoDescriptor.CreateWSVideos(Next2Friends.Data.Video.GetVideosByMemberIDWithJoin(member.MemberID, Next2Friends.Data.PrivacyType.Public));
        }

        /// <summary>
        /// Polls the database for streams and archived videos
        /// </summary>
        /// <param name="Nickname">Given as a flashvar to the player, this param requests videos for this user</param>
        /// <param name="PollType">0 = LiveOnly – Retrieve the LiveClip ID to be streamed from RED5
        /// 1 = LastClip – Retreive the FLV URL for the last clip this user has recorded
        /// 2 = LiveAndLastClip  - Either return the liveID for Red5 or in the instance of no live clip it returns the Last Clip recorded</param>
        /// <returns>returns a single FlashVideoDescriptor, either Live or Archived.</returns>
        [WebMethod]
        public FlashVideoDescriptor FlashVideos(string Nickname)
        {
            FlashPollType flashPollType = FlashPollType.LiveOnly;

            FlashVideoDescriptor ReturnFlashVideoDescriptor = null;
            Member member = Member.GetMemberViaNickname(Nickname);


            if (flashPollType == FlashPollType.LiveOnly)
            {
                LiveBroadcast live = LiveBroadcast.GetLiveBroadcastByMemberID(member.WebMemberID);
                ReturnFlashVideoDescriptor = FlashVideoDescriptor.ParseLive(live);
            }
            else if (flashPollType == FlashPollType.LastClip)
            {
                // Next2Friends.Data.Video video = Next2Friends.Data.Video.GetLatestVideoByNickname(Nickname);
            }
            else if (flashPollType == FlashPollType.LiveAndLastClip)
            {
                //LiveBroadcast live = LiveBroadcast.GetLiveBroadcastByMemberID(member.MemberID);

                //if (live != null)
                //{
                //ReturnFlashVideoDescriptor = FlashVideoDescriptor.ParseLive(live);
                //}
                //else
                //{

                //}
            }

            return ReturnFlashVideoDescriptor;
        }

        //[WebMethod]
        //public FlashVideoDescriptor[] GetArchivedVideos(string Nickname, int Page)
        //{

        //}

        // [WebMethod(EnableSession = true)]
        // public string Login(String Email, string Password)
        // {
        //     Member member = Member.MemberLogin(Email, Password);

        //     if (member == null)
        //     {
        //         return null;
        //     }
        //     else
        //     {
        //         Session["Member"] = member;

        //         return Session.SessionID;
        //     }
        // }

        //[WebMethod(EnableSession = true)]
        // public bool SetVideoPrivacy(String WebVideoID,PrivacyType privacyType)
        // {
        //     Member member = (Member)Session["Member"];

        //     if (member == null)
        //     {
        //         throw new LoginException();
        //     }

        //     Next2Friends.Data.Video video = Next2Friends.Data.Video.GetVideoByWebVideoIDWithJoin(WebVideoID);
        //     video.PrivacyFlag = (int)privacyType;

        //     return true;
        // }
    }
}
