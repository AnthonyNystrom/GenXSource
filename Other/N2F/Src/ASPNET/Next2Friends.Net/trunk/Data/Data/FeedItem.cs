using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public enum FeedItemType { Video = 0 , Photo = 1, Ask = 2, WallComment = 3, NewFriend = 4, Blog = 5, BookmarkedVideo = 6, 
                               BookmarkedPhoto = 7, StatusText = 8, Birthday = 9, Mp3Upload = 10 }

    public class FeedItem
    {
        public string MainWebID { get; set; }
        public string Friend1FullName { get; set; }
        public string Friend2FullName { get; set; }

        public FeedItemType FeedItemType { get; set; }

        public string Url
        {
            get {

                if (this.FeedItemType == FeedItemType.Video)
                {
                    return "/video/" + RegexPatterns.FormatStringForURL(Title) + "/" + MainWebID;
                }
                else if (this.FeedItemType == FeedItemType.Photo)
                {
                    return "/gallery/?g=" + MainWebID+"&m="+this.FriendWebMemberID1;
                }
                else if (this.FeedItemType == FeedItemType.Ask)
                {
                    return "/ask/" + MainWebID;
                }
                else if (this.FeedItemType == FeedItemType.WallComment)
                {
                    return "/users/" + FriendNickname2 + "/wall";
                }
                else if (this.FeedItemType == FeedItemType.Blog)
                {
                    return "/blog.aspx?m=" + FriendWebMemberID1 + "&b=" + MainWebID;
                }
                else if (this.FeedItemType == FeedItemType.BookmarkedVideo)
                {
                    return "/video/" + this.FriendWebMemberID1 + "/" + MainWebID;
                }
                else if (this.FeedItemType == FeedItemType.BookmarkedPhoto)
                {
                    return "/gallery/?g=" + this.WebPhotoCollectionID + "&m=" + this.FriendWebMemberID2 + "&wp=" + this.WebPhotoID;
                }
                else if (this.FeedItemType == FeedItemType.Mp3Upload)
                {
                    return "/user/" + this.FriendNickname1 + "/mp3/" + MainWebID;
                }
                else if (this.FeedItemType == FeedItemType.NewFriend)
                {
                    return string.Empty;
                }
                else if (this.FeedItemType == FeedItemType.StatusText)
                {
                    return "/user/" + this.FriendNickname1;
                }

                else
                {

                    return "view.aspx?v=" + MainWebID;

                }
             }
        }

        public string Thumbnail { get; set; }
        public string AskAFriendQuestion { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string FriendNickname1 { get; set; }
        public string FriendNickname2 { get; set; }
        public string FriendWebMemberID1 { get; set; }
        public string FriendWebMemberID2 { get; set; }
        public string WebPhotoCollectionID { get; set; }
        public string WebPhotoID { get; set; }
        public DateTime DateTime { get; set; }

        public static List<FeedItem> GetFeed(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNewsFeed");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<FeedItem> FeedItems = new List<FeedItem>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                FeedItems = FeedItem.PopulateObjectWithJoin(dr, FeedItems);

                while (dr.NextResult())
                {
                    FeedItems = FeedItem.PopulateObjectWithJoin(dr, FeedItems);
                }

                dr.Close();
            }



            return FeedItems;
        }



        public static List<FeedItem> PopulateObjectWithJoin(IDataReader dr, List<FeedItem> FeedItems)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            FeedItem feedItem;

            while (dr.Read())
            {
                feedItem = new FeedItem();

                feedItem.Friend1FullName = string.Empty;
                feedItem.Friend2FullName = string.Empty;

                if (list.IsColumnPresent("FeedItemType")) { feedItem.FeedItemType = (FeedItemType)dr["FeedItemType"]; }
                if (list.IsColumnPresent("Url")) { feedItem.MainWebID = (string)dr["Url"]; }
                if (list.IsColumnPresent("Thumbnail")) { feedItem.Thumbnail = "http://www.next2friends.com/user/"+(string)dr["Thumbnail"]; }
                if (list.IsColumnPresent("AskAFriendQuestion")) { feedItem.AskAFriendQuestion = (string)dr["AskAFriendQuestion"]; }
                if (list.IsColumnPresent("Text")) { feedItem.Text = (string)dr["Text"]; }
                if (list.IsColumnPresent("Title")) { feedItem.Title = (string)dr["Title"]; }
                if (list.IsColumnPresent("FriendNickname1")) { feedItem.FriendNickname1 = (string)dr["FriendNickname1"]; }
                if (list.IsColumnPresent("FriendNickname2")) { feedItem.FriendNickname2 = (string)dr["FriendNickname2"]; }
                if (list.IsColumnPresent("FriendWebMemberID1")) { feedItem.FriendWebMemberID1 = (string)dr["FriendWebMemberID1"]; }
                if (list.IsColumnPresent("FriendWebMemberID2")) { feedItem.FriendWebMemberID2 = (string)dr["FriendWebMemberID2"]; }
                if (list.IsColumnPresent("WebPhotoCollectionID")) { feedItem.WebPhotoCollectionID = (string)dr["WebPhotoCollectionID"]; }
                if (list.IsColumnPresent("WebPhotoID")) { feedItem.WebPhotoID = (string)dr["WebPhotoID"]; }
                if (list.IsColumnPresent("DateTime")) { feedItem.DateTime = (DateTime)dr["DateTime"]; }
                if (list.IsColumnPresent("Friend1FullName")) { feedItem.Friend1FullName = (string)dr["Friend1FullName"]; }
                if (list.IsColumnPresent("Friend2FullName")) { feedItem.Friend2FullName = (string)dr["Friend2FullName"]; }

                FeedItems.Add(feedItem);
            }

            return FeedItems;
        }
    }
}
