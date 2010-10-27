using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// The status of the friend request.
    /// Possible values are NotRespondedYet, Accepted, NotAccepted
    /// </summary>
    public enum FriendRequestStatus { NotRespondedYet, Accepted, NotAccepted}

    /// <summary>
    /// 
    /// </summary>
    public partial class FriendRequest
    {
        public string PhotoURL { get; set; }

        /// <summary>
        /// Get all the new friend requests for a member
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member</param>
        /// <param name="Origin">An integer value specifying the origin of the request 
        /// 1 is for Friend Requests originating from Proximity Tags
        /// 0 is for Friend Requests originating from Web
        /// -1 is for all Friend Requests
        /// </param>
        /// <returns>A list of new Friend Requests</returns>
        public static List<FriendRequest> GetAllNewFriendRequestByMemberID(int MemberID, int Origin)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllNewFriendRequestByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "Origin", DbType.Int32, Origin);

            List<FriendRequest> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateFriendRequestWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        /// <summary>
        /// Gets the number of friends for a member
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member</param>
        /// <returns>The number of Friends</returns>
        public static int GetNumberOfFriends(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNumberOfFriendByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            int NumberOfFriends = 0;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    NumberOfFriends = (int)dr[0];
                    dr.Close();
                }
            }

            // Create the object array from the datareader
            return NumberOfFriends;
        }


        

        /// <summary>
        /// Gets the number of new friend requests for a Member
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member</param>
        /// <returns>Returns a FriendStat object with number of requests populated</returns>
        public static FriendStats GetNumberOfNewFriendRequests(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNumberOfNewFriendRequests");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            FriendStats stats = new FriendStats();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    stats.WebFriendRequest = (int)dr[0];
                    stats.ProximityTags = (int)dr[1];
                    stats.AllRequests = (int)dr[2];
                }

                dr.Close();
            }

            // Create the object array from the datareader
            return stats;
        }

        /// <summary>
        /// Sets the status of single Friend Request
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member setting the status</param>
        /// <param name="WebFriendRequestID">The WebID of the FriendRequest</param>
        /// <param name="MakeFriend">True if the Friend Request has been accepted. False otherwise. </param>
        public static void SetFriendRequestStatus(int MemberID, string WebFriendRequestID, bool MakeFriend)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_SetFriendRequestStatus");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebFriendRequestID", DbType.String, WebFriendRequestID);
            db.AddInParameter(dbCommand, "MakeFriend", DbType.Boolean, MakeFriend);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Creates a friend request if the members arent already friends and a previous request has not been made
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member making request</param>
        /// <param name="FriendMemberID">The MemberID of the Member who is target of the request</param>
        /// <returns>True if the request succeeded. False otherwise.</returns>
        public static bool CreateWebFriendRequest(int MemberID, int FriendMemberID)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_CreateFriendRequest");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "FriendMemberID", DbType.Int32, FriendMemberID);
            db.AddInParameter(dbCommand, "WebFriendRequestID1", DbType.String, Next2Friends.Misc.UniqueID.NewWebID());
            db.AddInParameter(dbCommand, "WebFriendRequestID2", DbType.String, Next2Friends.Misc.UniqueID.NewWebID());

            bool MadeFriendRequest = false;

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        int FriendRequestValue = (int)dr[0];

                        MadeFriendRequest = (FriendRequestValue == 1) ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return MadeFriendRequest;
        }


        /// <summary>
        /// Creates a friend request from an emailaddress if the members arent already friends and a previous request has not been made
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member making request<</param>
        /// <param name="FriendEmailAddress">The EmailAddress of the friend</param>
        /// <returns>True if the request succeeded. False otherwise.</returns>
        public static bool CreateWebFriendRequestFromEmail(int MemberID, string FriendEmailAddress)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_CreateWebFriendRequestFromEmail");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "FriendEmailAddress", DbType.String, FriendEmailAddress);
            db.AddInParameter(dbCommand, "WebFriendRequestID1", DbType.String, Next2Friends.Misc.UniqueID.NewWebID());
            db.AddInParameter(dbCommand, "WebFriendRequestID2", DbType.String, Next2Friends.Misc.UniqueID.NewWebID());

            bool MadeFriendRequest = false;

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        int FriendRequestValue = (int)dr[0];

                        MadeFriendRequest = (FriendRequestValue == 1) ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return MadeFriendRequest;
        }

        /// <summary>
        /// Creates a FriendRequest from an imported contact
        /// </summary>
        /// <param name="NewMember"></param>
        /// <param name="ContactImportID"></param>
        public static void CreateFriendRequestsFromImport(Member NewMember, int ContactImportID)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_CreateFriendRequestsFromImport");

            db.AddInParameter(dbCommand, "NewMemberID", DbType.Int32, NewMember.MemberID);
            db.AddInParameter(dbCommand, "ContactImportID", DbType.Int32, ContactImportID);
            db.AddInParameter(dbCommand, "Email", DbType.String, NewMember.Email);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {

            }
        }

        
        /// <summary>
        /// Creates a friend request if the members arent already friends and a previous request has not been made
        /// </summary>
        /// <param name="MemberID">MemberID of the Member creating request</param>
        /// <param name="FriendMemberID">MemberID of the Member who is target of the request</param>
        /// <returns>True if the request succeeded. False otherwise</returns>
        public static bool CreateBluetoothFriendRequest(int MemberID, int FriendMemberID)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_CreateBluetoothFriendRequest");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "FriendMemberID", DbType.Int32, FriendMemberID);
            db.AddInParameter(dbCommand, "WebFriendRequestID1", DbType.String, Next2Friends.Misc.UniqueID.NewWebID());
            db.AddInParameter(dbCommand, "WebFriendRequestID2", DbType.String, Next2Friends.Misc.UniqueID.NewWebID());

            bool MadeFriendRequest = false;

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        int FriendRequestValue = (int)dr[0];

                        MadeFriendRequest = (FriendRequestValue == 1) ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return MadeFriendRequest;
        }


        /// <summary>
        /// Checks if a FriendRequest has already been sent between two members
        /// </summary>
        /// <param name="MemberID1">The MemberID of the Member for which the check is being made</param>
        /// <param name="MemberID2">The MemberID of the Member against which the FriendRequest is being checked.</param>
        /// <returns>True MemberID1 already has sent FriendRequest to MemberID2. False otherwise.</returns>
        public static bool IsFriendRequestSent(int MemberID1, int MemberID2)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_IsFriendRequestSent");
            db.AddInParameter(dbCommand, "MemberID1", DbType.Int32, MemberID1);
            db.AddInParameter(dbCommand, "MemberID2", DbType.Int32, MemberID2);

            int rowCount = (int)db.ExecuteScalar(dbCommand);

            if (rowCount > 0)
                return true;

            return false;
        }

          /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of FriendRequests
        /// </summary>
        public static List<FriendRequest> PopulateFriendRequestWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<FriendRequest> arr = new List<FriendRequest>();

            FriendRequest obj;

            while (dr.Read())
            {
                obj = new FriendRequest();
                obj._friendRequestID = (int)dr["FriendRequestID"];
                obj._webFriendRequestID = (string)dr["WebFriendRequestID"];
                obj._memberID = (int)dr["MemberID"];
                obj._origin = (int)dr["Origin"];
                obj._friendMemberID = (int)dr["FriendMemberID"];
                obj._status = (int)dr["Status"];
                obj._dTCreated = (DateTime)dr["DTCreated"];

                obj.FriendMember = new Member();
                if (list.IsColumnPresent("FriendMemberMemberID")) { obj.FriendMember.MemberID = (int)dr["FriendMemberMemberID"]; }
                if (list.IsColumnPresent("FriendMemberWebMemberID")) { obj.FriendMember.WebMemberID = (string)dr["FriendMemberWebMemberID"]; }
                if (list.IsColumnPresent("FriendMemberAdminStatusID")) { obj.FriendMember.AdminStatusID = (int)dr["FriendMemberAdminStatusID"]; }
                if (list.IsColumnPresent("FriendMemberNickName")) { obj.FriendMember.NickName = (string)dr["FriendMemberNickName"]; }
                if (list.IsColumnPresent("FriendMemberChannelID")) { obj.FriendMember.ChannelID = (int)dr["FriendMemberChannelID"]; }
                if (list.IsColumnPresent("FriendMemberPassword")) { obj.FriendMember.Password = (string)dr["FriendMemberPassword"]; }
                if (list.IsColumnPresent("FriendMemberEmail")) { obj.FriendMember.Email = (string)dr["FriendMemberEmail"]; }
                if (list.IsColumnPresent("FriendMemberGender")) { obj.FriendMember.Gender = (int)dr["FriendMemberGender"]; }
                if (list.IsColumnPresent("FriendMemberFirstName")) { obj.FriendMember.FirstName = (string)dr["FriendMemberFirstName"]; }
                if (list.IsColumnPresent("FriendMemberLastName")) { obj.FriendMember.LastName = (string)dr["FriendMemberLastName"]; }
                if (list.IsColumnPresent("FriendMemberProfilePhotoResourceFileID")) { obj.FriendMember.ProfilePhotoResourceFileID = (int)dr["FriendMemberProfilePhotoResourceFileID"]; }
                if (list.IsColumnPresent("FriendMemberDOB")) { obj.FriendMember.DOB = (DateTime)dr["FriendMemberDOB"]; }
                if (list.IsColumnPresent("FriendMemberISOCountry")) { obj.FriendMember.ISOCountry = (string)dr["FriendMemberISOCountry"]; }
                if (list.IsColumnPresent("FriendMemberZipPostcode")) { obj.FriendMember.ZipPostcode = (string)dr["FriendMemberZipPostcode"]; }
                if (list.IsColumnPresent("FriendMemberCreatedDT")) { obj.FriendMember.CreatedDT = (DateTime)dr["FriendMemberCreatedDT"]; }
                if (list.IsColumnPresent("FriendMemberLastOnline")) { obj.FriendMember.LastOnline = (DateTime)dr["FriendMemberLastOnline"]; }

                if (list.IsColumnPresent("PhotoURL")) { obj.PhotoURL = (string)dr["PhotoURL"]; }


                arr.Add(obj);
            }

            return arr;
        }
    }

    /// <summary>
    /// A small data structure for Friend Request data
    /// </summary>
    public class FriendStats
    {
        /// <summary>
        /// Number of Proximity Tags
        /// </summary>
        public int ProximityTags { get; set; }

        /// <summary>
        /// Number of Friend Requests via Web
        /// </summary>
        public int WebFriendRequest { get; set; }

        /// <summary>
        /// Number of All Requests
        /// </summary>
        public int AllRequests { get; set; }
    }
}
