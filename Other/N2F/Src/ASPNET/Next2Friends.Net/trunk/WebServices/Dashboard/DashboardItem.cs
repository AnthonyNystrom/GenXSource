/* ------------------------------------------------
 * DashboardItem.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Next2Friends.WebServices.Dashboard
{
    public sealed class DashboardItem
    {
        public String AskQuestion { get; set; }
        public String DateTime { get; set; }
        public Int32 FeedItemType { get; set; }
        public String Friend1FullName { get; set; }
        public String Friend2FullName { get; set; }
        public String FriendNickname1 { get; set; }
        public String FriendNickname2 { get; set; }
        public String FriendWebMemberID1 { get; set; }
        public String FriendWebMemberID2 { get; set; }
        public String Text { get; set; }
        public String ThumbnailUrl { get; set; }
        public String Title { get; set; }
        public Int32 ObjectID { get; set; }
        public String WebPhotoCollectionID { get; set; }
        public String WebPhotoID { get; set; }
    }
}
