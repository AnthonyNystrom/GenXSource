/* ------------------------------------------------
 * DashboardService.asmx.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services;
using Next2Friends.WebServices.Dashboard;
using Next2Friends.WebServices.Utils;
using data = Next2Friends.Data;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to acess Dashboard data.
    /// </summary>
    [WebService(
        Description = "Provides functionality to access Dashboard data.",
        Name = "DashboardService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class DashboardService : WebService
    {
        /// <summary>
        /// Returns a list of current Dashboard items.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of current Dashboard items.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public DashboardItem[] GetItems(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var feedItems = data.FeedItem.GetFeed(member.MemberID);
            var sortedItems = from f in feedItems
                              where f.DateTime > DateTime.Now.AddDays(-4)
                              orderby f.DateTime descending
                              select new DashboardItem()
                              {
                                  AskQuestion = Text2Mobile.Filter(f.AskAFriendQuestion ?? ""),
                                  DateTime = f.DateTime.Ticks.ToString(),
                                  FeedItemType = (Int32)f.FeedItemType,
                                  Friend1FullName = f.Friend1FullName ?? "",
                                  Friend2FullName = f.Friend2FullName ?? "",
                                  FriendNickname1 = f.FriendNickname1 ?? "",
                                  FriendNickname2 = f.FriendNickname2 ?? "",
                                  FriendWebMemberID1 = f.FriendWebMemberID1 ?? "",
                                  FriendWebMemberID2 = f.FriendWebMemberID2 ?? "",
                                  Text = Text2Mobile.Filter(f.Text ?? ""),
                                  ThumbnailUrl = f.Thumbnail ?? "",
                                  Title = Text2Mobile.Filter(f.Title ?? ""),
                                  ObjectID = GetObjectID(f.Url ?? "", f.FeedItemType),
                                  WebPhotoCollectionID = f.WebPhotoCollectionID ?? "",
                                  WebPhotoID = f.WebPhotoID ?? ""
                              };
            return sortedItems.ToArray();
        }

        /// <summary>
        /// Gets the Base64 string representation of the thumbnail with the specified <code>thumbnailUrl</code>.
        /// </summary>
        /// <returns>Base64 string representation of the thumbnail.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>thumbnailUrl</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the resource located at the specified <code>thumbnailUrl</code> is not an image.
        /// </exception>
        /// <exception cref="WebException">If a resource at the specified <code>thumbnailUrl</code> does not exist.</exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Gets the Base64 string representation of the thumbnail with the specified <tt>thumbnailUrl</tt>.</p><p><b>Returns: </b>Base64 string representation of the thumbnail.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>thumbnailUrl</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If the resource located at the specified <tt>thumbnailUrl</tt> is not an image.<br /><tt>WebException</tt> - If a resource at the specified <tt>thumbnailUrl</tt> does not exist.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public String GetThumbnail(String nickname, String password, String thumbnailUrl)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(thumbnailUrl))
                throw new ArgumentNullException("thumbnailUrl");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            String result = null;

            var request = WebRequest.Create(thumbnailUrl);
            var response = request.GetResponse();
            var thumbnailStream = response.GetResponseStream();

            using (var ms = new MemoryStream())
            {
                var buffer = new Byte[1024];
                var count = 0;

                do
                {
                    count = thumbnailStream.Read(buffer, 0, 1024);
                    ms.Write(buffer, 0, count);
                } while (count > 0);
                
                var thumbnailByteArray = ms.ToArray();
                if (data.Photo.IsPhotoFile(thumbnailByteArray))
                    result = Convert.ToBase64String(thumbnailByteArray);
                else
                    throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidImageResource, thumbnailUrl));
            }

            return result;
        }

        /// <summary>
        /// Returns a list of videos from the Dashboard.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of videos from the Dashboard.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public DashboardVideo[] GetVideos(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return data.FeedItem.GetFeed(
                GetMemberID(nickname, password))
                .Where(f => f.FeedItemType == data.FeedItemType.Video)
                .Select(f => new DashboardVideo() {
                    DateTime = f.DateTime.Ticks.ToString(),
                    Nickname = f.FriendNickname1,
                    Text = Text2Mobile.Filter(f.Text),
                    Title = Text2Mobile.Filter(f.Title),
                    ThumbnailUrl = f.Thumbnail })
                .OrderBy(f => f.DateTime)
                .ToArray();
        }

        /// <summary>
        /// Returns a list of photos from the Dashboard.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of photos from the Dashboard.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public DashboardPhoto[] GetPhotos(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return data.FeedItem.GetFeed(
                GetMemberID(nickname, password))
                .Where(f => f.FeedItemType == data.FeedItemType.Photo)
                .Select(f => new DashboardPhoto() {
                    DateTime = f.DateTime.Ticks.ToString(),
                    Nickname = f.FriendNickname1,
                    Text = Text2Mobile.Filter(f.Text),
                    Title = Text2Mobile.Filter(f.Title),
                    ThumbnailUrl = f.Thumbnail
                })
                .OrderBy(f => f.DateTime)
                .ToArray();
        }

        /// <summary>
        /// Returns a list of wall comments from the Dashboard.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of wall comments from the Dashboard.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public DashboardWallComment[] GetWallComments(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return data.FeedItem.GetFeed(
                GetMemberID(nickname, password))
                .Where(f => f.FeedItemType == data.FeedItemType.WallComment)
                .Select(f => new DashboardWallComment() {
                    DateTime = f.DateTime.Ticks.ToString(),
                    Nickname1 = f.FriendNickname1,
                    Nickname2 = f.FriendNickname2,
                    Text = Text2Mobile.Filter(f.Text)
                })
                .OrderBy(f => f.DateTime)
                .ToArray();
        }

        /// <summary>
        /// Returns a list of New Friends notifications from the Dashboard.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of New Friends notifications from the Dashboard.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public DashboardNewFriend[] GetNewFriends(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return data.FeedItem.GetFeed(
                GetMemberID(nickname, password))
                .Where(f => f.FeedItemType == data.FeedItemType.NewFriend)
                .Select(f => new DashboardNewFriend() {
                    DateTime = f.DateTime.Ticks.ToString(),
                    Nickname1 = f.FriendNickname1,
                    Nickname2 = f.FriendNickname2 })
                .OrderBy(f => f.DateTime)
                .ToArray();
        }

        private static Int32 GetMemberID(String nickname, String password)
        {
            return data.Member.GetMemberViaNicknamePassword(nickname, password).MemberID;
        }

        private static Int32 GetObjectID(String url, data.FeedItemType feedItemType)
        {
            switch (feedItemType)
            {
                case data.FeedItemType.WallComment:
                    return data.Member.GetMemberViaNickname(UrlProcessor.ExtractWallName(url)).MemberID;
                case data.FeedItemType.Video:
                case data.FeedItemType.BookmarkedVideo:
                    return data.Video.GetVideoByWebVideoIDWithJoin(UrlProcessor.ExtractWebVideoID(url)).VideoID;
                case data.FeedItemType.Photo:
                    return data.PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(UrlProcessor.ExtractWebPhotoCollectionID(url)).PhotoCollectionID;
                case data.FeedItemType.BookmarkedPhoto:
                    return data.Photo.GetPhotoByWebPhotoIDWithJoin(UrlProcessor.ExtractWebPhotoID(url)).PhotoID;
                case data.FeedItemType.Ask:
                    return data.AskAFriend.GetAskAFriendByWebAskAFriendID(UrlProcessor.ExtractWebAskID(url)).AskAFriendID;
                case data.FeedItemType.Blog:
                    return data.BlogEntry.GetBlogEntryByWebBlogEntryID(UrlProcessor.ExtractWebBlogEntryID(url)).BlogEntryID;
                default:
                    return 0;
            }
        }
    }
}
