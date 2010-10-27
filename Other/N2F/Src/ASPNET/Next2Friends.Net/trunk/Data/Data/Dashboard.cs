using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    //-- 1. Favourites this week
    //-- 2. Latest Photo Galleries
    //-- 3. New Friend Requests
    //-- 4. Pending friend reguests
    //-- 5. new Friends
    //-- 6. Profile updates
    //-- 7. inbox
    //-- 8. Lowdown entry
    //-- 9. video Comments
    //-- 10. photo Comments
    //-- 11. Proximity Tags
    //-- 12. Live mobile broadcasts
    //-- 13. New Blog entry
    //-- 14. Private AAF Question
    //-- 15. My AAF Questions
    public enum DashboardField
    {
        NetworkFavourite, NewFriendPhotoGallery, FriendRequest, FriendRequestOutstanding, FriendRequestAccept, FriendProfileUpdate,
        Inbox, ProfileComment, VideoComment, PhotoComment, ProximityTags, LiveMobileBroadcasts, NetworkBlogEntry, PrivateAAF, MyAskAFriendQuestions
    }


    public partial class Dashboard
    {
        public List<DashboardItem> AllDashboardItems { get; set; }
        public List<DashboardItem> MemberDashboardItems { get; set; }

        public List<DashboardItem> GetRow(int Row)
        {
            List<DashboardItem> DashboardItemReturned = new List<DashboardItem>();
            List<DashboardItem> OrderedDashboardItemReturned = new List<DashboardItem>();
            

            for (int i = 0; i < MemberDashboardItems.Count; i++)
            {
                if (MemberDashboardItems[i].Row == Row)
                {
                    DashboardItemReturned.Add(AllDashboardItems[i]);
                }
            }

            int Order = 1;

            while(Order<20)
            {

                for (int i = 0; i < DashboardItemReturned.Count; i++)
                {
                    if (DashboardItemReturned[i].Order == Order)
                    {
                        OrderedDashboardItemReturned.Add(DashboardItemReturned[i]);
                    }
                }

                Order++;
            }

            return OrderedDashboardItemReturned;
        }

        public string GetRowHTML(int Row)
        {

            List<DashboardItem> RowArr = GetRow(Row);
            StringBuilder SbRow = new StringBuilder();
            int index = 1;

            for (int i = 0; i < RowArr.Count; i++)
            {
                string[] parameters = new string[3];
                //parameters[0] = index.ToString();
                parameters[0] = ((int)RowArr[i].DashboardField).ToString();
                parameters[1] = RowArr[i].Name;
                parameters[2] = RowArr[i].HTML;

                index++;


                SbRow.AppendFormat(@" <div id='{0}' class='groupItem'>
		                <div style='-moz-user-select: none;' class='itemHeader'>{1}<a href='#' class='closeEl'>[-]</a></div>
		                <div class='itemContent'>
			                <ul>
				                {2}
			                </ul>
		                </div>
	                </div>", parameters);
            }

           return SbRow.ToString();
        }


        //public void GenerateBashboard()
        //{
        //    // Singlton
        //    if (AllDashboardItems == null)
        //    {
        //        AllDashboardItems = new List<DashboardItem>();
        //        MemberDashboardItems = new List<DashboardItem>();
                

        //         // NetworkFavourite, NetworkNewGallery, FriendRequest, FriendRequestOutstanding, FriendRequestAccept, FriendProfileUpdate,
        //        //Inbox, ProfileComment, VideoComment, PhotoComment, ProximityTags, LiveMobileBroadcasts, NetworkBlogEntry, PrivateAAF, MyAAF

        //        #region block 1-5

        //        DashboardItem dash1 = new DashboardItem();
        //        dash1.Name = "NetworkFavourite";
        //        dash1.DashboardField = DashboardField.NetworkFavourite;
        //        dash1.Order = this.NetworkFavourite;
        //        dash1.Row = this.NetworkFavouriteRow;
        //        AllDashboardItems.Add(dash1);
        //        if (this.NetworkFavourite > 0) MemberDashboardItems.Add(dash1); //only add the items that the member has selected

        //        DashboardItem dash2 = new DashboardItem();
        //        dash2.Name = "NetworkNewGallery";
        //        dash2.DashboardField = DashboardField.NewFriendPhotoGallery;
        //        dash2.Order = this.NetworkNewGallery;
        //        dash2.Row = this.NetworkNewGalleryRow;
        //        AllDashboardItems.Add(dash2);
        //        if (this.NetworkNewGallery > 0) MemberDashboardItems.Add(dash2); //only add the items that the member has selected

        //        DashboardItem dash3 = new DashboardItem();
        //        dash3.Name = "FriendRequest";
        //        dash3.DashboardField = DashboardField.FriendRequest;
        //        dash3.Order = this.FriendRequest;
        //        dash3.Row = this.FriendRequestRow;
        //        AllDashboardItems.Add(dash3);
        //        if (this.FriendRequest > 0) MemberDashboardItems.Add(dash3); //only add the items that the member has selected

        //        DashboardItem dash4 = new DashboardItem();
        //        dash4.Name = "FriendRequestOutstanding";
        //        dash4.DashboardField = DashboardField.FriendRequestOutstanding;
        //        dash4.Order = this.FriendRequestOutstanding;
        //        dash4.Row = this.FriendRequestOutstandingRow;
        //        AllDashboardItems.Add(dash4);
        //        if (this.FriendRequestOutstanding > 0) MemberDashboardItems.Add(dash4); //only add the items that the member has selected

        //        DashboardItem dash5 = new DashboardItem();
        //        dash5.Name = "FriendRequestAccept";
        //        dash5.DashboardField = DashboardField.FriendRequestAccept;
        //        dash5.Order = this.FriendRequestAccept;
        //        dash5.Row = this.FriendRequestAcceptRow;
        //        AllDashboardItems.Add(dash5);
        //        if (this.FriendRequestAccept > 0) MemberDashboardItems.Add(dash5); //only add the items that the member has selected
        //        #endregion

        //        #region block 6-10

        //        DashboardItem dash6 = new DashboardItem();
        //        dash6.Name = "FriendProfileUpdate";
        //        dash6.DashboardField = DashboardField.FriendProfileUpdate;
        //        dash6.Order = this.FriendProfileUpdate;
        //        dash6.Row = this.FriendProfileUpdateRow;
        //        AllDashboardItems.Add(dash6);
        //        if (this.FriendProfileUpdate > 0) MemberDashboardItems.Add(dash6); //only add the items that the member has selected

        //        DashboardItem dash7 = new DashboardItem();
        //        dash7.Name = "Inbox";
        //        dash7.DashboardField = DashboardField.Inbox;
        //        dash7.Order = this.Inbox;
        //        dash7.Row = this.InboxRow;
        //        AllDashboardItems.Add(dash7);
        //        if (this.Inbox > 0) MemberDashboardItems.Add(dash7); //only add the items that the member has selected

        //        DashboardItem dash8 = new DashboardItem();
        //        dash8.Name = "ProfileComment";
        //        dash8.DashboardField = DashboardField.ProfileComment;
        //        dash8.Order = this.ProfileComment;
        //        dash8.Row = this.ProfileCommentRow;
        //        AllDashboardItems.Add(dash8);
        //        if (this.ProfileComment > 0) MemberDashboardItems.Add(dash8); //only add the items that the member has selected

        //        DashboardItem dash9 = new DashboardItem();
        //        dash9.Name = "VideoComment";
        //        dash9.DashboardField = DashboardField.VideoComment;
        //        dash9.Order = this.VideoComment;
        //        dash9.Row = this.VideoCommentRow;
        //        AllDashboardItems.Add(dash9);
        //        if (this.VideoComment > 0) MemberDashboardItems.Add(dash9); //only add the items that the member has selected

        //        DashboardItem dash10 = new DashboardItem();
        //        dash10.Name = "PhotoComment";
        //        dash10.DashboardField = DashboardField.PhotoComment;
        //        dash10.Order = this.PhotoComment;
        //        dash10.Row = this.PhotoCommentRow;
        //        AllDashboardItems.Add(dash10);
        //        if (this.PhotoComment > 0) MemberDashboardItems.Add(dash10); //only add the items that the member has selected

        //        #endregion

        //        #region block 11-15

        //        DashboardItem dash11 = new DashboardItem();
        //        dash11.Name = "ProximityTags";
        //        dash11.DashboardField = DashboardField.ProximityTags;
        //        dash11.Order = this.ProximityTags;
        //        dash11.Row = this.ProximityTagsRow;
        //        AllDashboardItems.Add(dash11);
        //        if (this.ProximityTags > 0) MemberDashboardItems.Add(dash11); //only add the items that the member has selected

        //        DashboardItem dash12 = new DashboardItem();
        //        dash12.Name = "LiveMobileBroadcasts";
        //        dash12.DashboardField = DashboardField.LiveMobileBroadcasts;
        //        dash12.Order = this.LiveMobileBroadcasts;
        //        dash12.Row = this.LiveMobileBroadcastsRow;
        //        AllDashboardItems.Add(dash12);
        //        if (this.LiveMobileBroadcasts > 0) MemberDashboardItems.Add(dash12); //only add the items that the member has selected

        //        DashboardItem dash13 = new DashboardItem();
        //        dash13.Name = "NetworkBlogEntry";
        //        dash13.DashboardField = DashboardField.NetworkBlogEntry;
        //        dash13.Order = this.NetworkBlogEntry;
        //        dash13.Row = this.NetworkBlogEntryRow;
        //        AllDashboardItems.Add(dash13);
        //        if (this.NetworkBlogEntry > 0) MemberDashboardItems.Add(dash13); //only add the items that the member has selected

        //        DashboardItem dash14 = new DashboardItem();
        //        dash14.Name = "PrivateAAF";
        //        dash14.DashboardField = DashboardField.PrivateAAF;
        //        dash14.Order = this.PrivateAAF;
        //        dash14.Row = this.PrivateAAFRow;
        //        AllDashboardItems.Add(dash14);
        //        if (this.PrivateAAF > 0) MemberDashboardItems.Add(dash14); //only add the items that the member has selected

        //        DashboardItem dash15 = new DashboardItem();
        //        dash15.Name = "MyAAF";
        //        dash15.DashboardField = DashboardField.MyAskAFriendQuestions;
        //        dash15.Order = this.MyAAF;
        //        dash15.Row = this.MyAAFRow;
        //        AllDashboardItems.Add(dash15);
        //        if (this.MyAAF > 0) MemberDashboardItems.Add(dash15); //only add the items that the member has selected

        //        #endregion

        //        // now populate it from the DB
        //        PopulateDashboard();
        //    }
        //}

        public List<DashboardItem> PopulateDashboard()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("[HG_GetDashboard]");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            // use a seperate loop to populate so if the SQL fails the base boxes are still created
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {               
                for (int i = 0; i < AllDashboardItems.Count; i++)
                {
                    for (int j = 0; j < MemberDashboardItems.Count; j++)
                    {
                        // if the Dashboard item is present then read it into the item
                        if (AllDashboardItems[i].DashboardField == MemberDashboardItems[j].DashboardField)
                        {
                            AllDashboardItems[i] = PopulateDashboardItem(AllDashboardItems[i], dr);
                            break;
                        }
                    }

                    dr.NextResult();
                }


                dr.Close();
            }

            return MemberDashboardItems;
        }

        public DashboardItem PopulateDashboardItem(DashboardItem DashboardItem, IDataReader dr)
        {
            // the Datareader will return the results in the following order
            //-- 1. Favourites this week
            //-- 2. Latest Photo Galleries
            //-- 3. New Friend Requests
            //-- 4. Pending friend reguests
            //-- 5. new Friends
            //-- 6. Profile updates
            //-- 7. inbox
            //-- 8. Lowdown entry
            //-- 9. video Comments
            //-- 10. photo Comments
            //-- 11. Proximity Tags
            //-- 12. Live mobile broadcasts
            //-- 13. New Blog entry
            //-- 14. Private AAF Question
            //-- 15. My AAF Questions

            //-- 2. Latest Photo Galleries
            if (DashboardItem.DashboardField == DashboardField.NewFriendPhotoGallery)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href='ViewGallery.aspx?g=" + (string)dr["WebPhotoCollectionID"] + "'>" + (string)dr["Name"] + "-" + TimeDistance.ShortDateTime((DateTime)dr["DTCreated"]) + "<img style='width:51px;height:51px' src='" + ParallelServer.Get() + "user/" + (string)dr["PhotoURL"] + "'></a></li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No new Galleries</li>";
                }
               
                
            }

            //-- 3. New Friend Requests
            if (DashboardItem.DashboardField == DashboardField.FriendRequest)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href='Friendrequest.aspx'>" + (string)dr["FriendMemberNickName"] + " - <img style='width:51px;height:51px' src='" + ParallelServer.Get() + "user/" + (string)dr["photoURL"] + "'></a></li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No new Friend requests</li>";
                }
            }

            //-- 4. Pending friend reguests
            if (DashboardItem.DashboardField == DashboardField.FriendRequestOutstanding)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href=''>" + (string)dr["FriendMemberNickname"] + "!</a> - View</li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No pending Friend requests</li>";
                }
            }

            //-- 6. Profile updates
            if (DashboardItem.DashboardField == DashboardField.FriendProfileUpdate)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href='view.aspx?m=" + (string)dr["WebMemberID"] + "'>" + (string)dr["Nickname"] + "</a> </li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No updated profiles</li>";
                }
            }

            //--  7. inbox
            if (DashboardItem.DashboardField == DashboardField.Inbox)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><img src='images/email.gif'>  <a href=''>" + (string)dr["Nickname"] + " - " + TimeDistance.ShortDateTime((DateTime)dr["DTCreated"]) + "</a> </li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No new messages</li>";
                }
            }

            //--  9. video Comments
            if (DashboardItem.DashboardField == DashboardField.VideoComment)
            {
                bool Empty = true;
                int count = 0;
                while (dr.Read())
                {
                    if (count != 10)
                    {
                        DashboardItem.HTML += "<li><a href='view?v=" + (string)dr["WebVideoID"] + "'>" + Trim((string)dr["Title"], 15) + " - " + (string)dr["NickName"] + "</a></li>";
                        Empty = false;
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No video comments</li>";
                }
            }

            //--  10. photo Comments
            if (DashboardItem.DashboardField == DashboardField.PhotoComment)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href=''>" + (string)dr["Name"] + "!</a> - <a href=''>" + (string)dr["NickName"] + "!</a></li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No photo comments</li>";
                }
            }

            //-- 12. Live mobile broadcasts
            if (DashboardItem.DashboardField == DashboardField.LiveMobileBroadcasts)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href='view?v=" + (string)dr["WebVideoID"] + "'>" + Trim((string)dr["Title"], 10) + "</a> - " + (string)dr["NickName"] + " " + TimeDistance.ShortDateTime((DateTime)dr["DTCreated"]) + "</li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No new Videos</li>";
                }
            }

            //-- 15. My AAF Questions
            if (DashboardItem.DashboardField == DashboardField.MyAskAFriendQuestions)
            {
                bool Empty = true;
                while (dr.Read())
                {
                    DashboardItem.HTML += "<li><a href='AskAFriend.aspx?q=" + (string)dr["WebAskAFriendID"] + "'>" + Trim((string)dr["Question"], 15) + "</a> - " + TimeDistance.ShortDateTime((DateTime)dr["WentLiveDT"]) + "</li>";
                    Empty = false;
                }

                if (Empty)
                {
                    DashboardItem.HTML += "<li>No have not posted any Questions</li>";
                }
                else
                {
                    DashboardItem.HTML = "<a href='AAFUpload.aspx'>Ask A Question Now</a><br/><a href='AskAFriend.aspx'>Answer Questions</a><br/><br/><a href='MyAskAFriend.aspx'><strong>My Questions</strong></a><br/>" + DashboardItem.HTML;
                    DashboardItem.HTML += "<br/><strong>New Questions</strong><br/>";
                }

            }

            return DashboardItem;
        }

        ///// <summary>
        ///// Updates the ordering and column then saves back to the db
        ///// </summary>
        ///// <param name="Row1"></param>
        ///// <param name="Row2"></param>
        ///// <param name="Row3"></param>
        //public void UpdateViaArray(int[] Row1, int[] Row2, int[] Row3)
        //{
        //    UpdateRowSource(Row1, 1);
        //    UpdateRowSource(Row2, 2);
        //    UpdateRowSource(Row3, 3);

        //    this.Save();

        //}

        //private void UpdateRowSource(int[] Row, int RowNumber)
        //{
        //    int NextRow = 1;

        //    for (int i = 0; i < Row.Length; i++)
        //    {
        //        #region Set Position
        //        if (Row[i] == (int)DashboardField.NetworkFavourite)
        //        {
        //            this.NetworkFavourite = NextRow++;
        //            this.NetworkFavouriteRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.NewFriendPhotoGallery)
        //        {
        //            this.NetworkNewGallery = NextRow++;
        //            this.NetworkNewGalleryRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.FriendRequest)
        //        {
        //            this.FriendRequest = NextRow++;
        //            this.FriendRequestRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.FriendRequestOutstanding)
        //        {
        //            this.FriendRequestOutstanding = NextRow++;
        //            this.FriendRequestOutstandingRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.FriendRequestAccept)
        //        {
        //            this.FriendRequestAccept = NextRow++;
        //            this.FriendRequestAcceptRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.FriendProfileUpdate)
        //        {
        //            this.FriendProfileUpdate = NextRow++;
        //            this.FriendProfileUpdateRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.Inbox)
        //        {
        //            this.Inbox = NextRow++;
        //            this.InboxRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.ProfileComment)
        //        {
        //            this.ProfileComment = NextRow++;
        //            this.ProfileCommentRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.VideoComment)
        //        {
        //            this.VideoComment = NextRow++;
        //            this.VideoCommentRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.PhotoComment)
        //        {
        //            this.PhotoComment = NextRow++;
        //            this.PhotoCommentRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.ProximityTags)
        //        {
        //            this.ProximityTags = NextRow++;
        //            this.ProximityTagsRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.LiveMobileBroadcasts)
        //        {
        //            this.LiveMobileBroadcasts = NextRow++;
        //            this.LiveMobileBroadcastsRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.NetworkBlogEntry)
        //        {
        //            this.NetworkBlogEntry = NextRow++;
        //            this.NetworkBlogEntryRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.PrivateAAF)
        //        {
        //            this.PrivateAAF = NextRow++;
        //            this.PrivateAAFRow = RowNumber;
        //        }
        //        else if (Row[i] == (int)DashboardField.MyAskAFriendQuestions)
        //        {
        //            this.MyAAF = NextRow++;
        //            this.MyAAFRow = RowNumber;
        //        }
        //        #endregion
        //    }
        //}

        public static string Trim(string str, int Max)
        {
            if (str.Length > Max)
            {
                str = str.Substring(0, Max - 2) + "..";
            }

            return str;
        }
    }


    public class DashboardItem
    {
        public string Name { get; set; }
        public DashboardField DashboardField { get; set; }
        public int Order { get; set; }
        public int Row { get; set; }
        public string HTML { get; set; }
    }
        
}


