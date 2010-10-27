using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;


namespace ExternalMessaging
{
    public class MemberSettings
    {
        private int _memberSettingsID;		//
        private int _memberID;		//
        private bool _notifyOnNewMessage;		//Notifty the Member via email when another member posts a message
        private bool _notifyOnAAFComment;		//Notifty the Member via email when someone posts an AskAFriend comment
        private bool _notifyOnFriendRequest;		//Notifty the Member via email when a nother member has made a friend request
        private bool _notifyOnSubscriberEvent;		//Notifty the Member via email when a Subsciber event occurs
        private bool _notifyOnNewsLetter;		//Notifty the Member via email with a newsletter
        private bool _notifyNewProfileComment;		//
        private bool _notifyNewPhotoComment;		//
        private bool _notifyNewVideoComment;		//
        private bool _notifyOnNewVideo;		//
        private bool _notifyOnNewBlog;		//
        private bool _notifyOnThreadReply;		//
 
        /// <summary>
        /// 
        /// </summary>
        public int MemberSettingsID
        {
            get { return _memberSettingsID; }
            set { _memberSettingsID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MemberID
        {
            get { return _memberID; }
            set { _memberID = value; }
        }

        /// <summary>
        /// Notifty the Member via email when another member posts a message
        /// </summary>
        public bool NotifyOnNewMessage
        {
            get { return _notifyOnNewMessage; }
            set { _notifyOnNewMessage = value; }
        }

        /// <summary>
        /// Notifty the Member via email when someone posts an AskAFriend comment
        /// </summary>
        public bool NotifyOnAAFComment
        {
            get { return _notifyOnAAFComment; }
            set { _notifyOnAAFComment = value; }
        }

        /// <summary>
        /// Notifty the Member via email when a nother member has made a friend request
        /// </summary>
        public bool NotifyOnFriendRequest
        {
            get { return _notifyOnFriendRequest; }
            set { _notifyOnFriendRequest = value; }
        }

        /// <summary>
        /// Notifty the Member via email when a Subsciber event occurs
        /// </summary>
        public bool NotifyOnSubscriberEvent
        {
            get { return _notifyOnSubscriberEvent; }
            set { _notifyOnSubscriberEvent = value; }
        }

        /// <summary>
        /// Notifty the Member via email with a newsletter
        /// </summary>
        public bool NotifyOnNewsLetter
        {
            get { return _notifyOnNewsLetter; }
            set { _notifyOnNewsLetter = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotifyNewProfileComment
        {
            get { return _notifyNewProfileComment; }
            set { _notifyNewProfileComment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotifyNewPhotoComment
        {
            get { return _notifyNewPhotoComment; }
            set { _notifyNewPhotoComment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotifyNewVideoComment
        {
            get { return _notifyNewVideoComment; }
            set { _notifyNewVideoComment = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool NotifyOnNewVideo
        {
            get { return _notifyOnNewVideo; }
            set { _notifyOnNewVideo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotifyOnNewBlog
        {
            get { return _notifyOnNewBlog; }
            set { _notifyOnNewBlog = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotifyOnThreadReply
        {
            get { return _notifyOnThreadReply; }
            set { _notifyOnThreadReply = value; }
        }


        public static List<MemberSettings> PopulateObject(IDataReader dr)
        {
            List<MemberSettings> arr = new List<MemberSettings>();

            MemberSettings obj;

            while (dr.Read())
            {
                obj = new MemberSettings();
                obj._memberSettingsID = (int)dr["MemberSettingsID"];
                obj._memberID = (int)dr["MemberID"];
                obj._notifyOnNewMessage = (bool)dr["NotifyOnNewMessage"];
                obj._notifyOnAAFComment = (bool)dr["NotifyOnAAFComment"];
                obj._notifyOnFriendRequest = (bool)dr["NotifyOnFriendRequest"];
                obj._notifyOnSubscriberEvent = (bool)dr["NotifyOnSubscriberEvent"];
                obj._notifyOnNewsLetter = (bool)dr["NotifyOnNewsLetter"];
                obj._notifyNewProfileComment = (bool)dr["NotifyNewProfileComment"];
                obj._notifyNewPhotoComment = (bool)dr["NotifyNewPhotoComment"];
                obj._notifyNewVideoComment = (bool)dr["NotifyNewVideoComment"];
                obj._notifyOnNewBlog = (bool)dr["NotifyOnNewBlog"];
                obj._notifyOnThreadReply = (bool)dr["NotifyOnThreadReply"];
                obj._notifyOnNewVideo = (bool)dr["NotifyOnNewVideo"];

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        /// <summary>
        /// Gets all the MemberSettings in the database 
        /// </summary>
        public static List<MemberSettings> GetAllMemberSettingsWithSubscriberEvent(SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("HG_GetAllMemberSettingsWithSubscriberEvent", conn);            
            command.CommandType = CommandType.StoredProcedure;
            
            List<MemberSettings> arr = null;

            //execute the stored procedure
            using (IDataReader dr = command.ExecuteReader())
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }


    }
}
