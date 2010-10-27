using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public enum Gender { Female = 0, Male = 1, NotSet = 2 }
    public enum DefaultGalleryType { Web, Mobile }
    public enum AccountType { Personal, Business }



    public partial class Member
    {
        public DateTime LastOnline { get; set; }
        public bool IsFriend { get; set; }
        public string ISOCode { get; set; }

        private string _TemporaryToken = UniqueID.NewWebID();

        public string TemporaryToken
        {
            get { return _TemporaryToken; }
        }

        public Dashboard DashboardNoCache
        {
            get { return this.GetDashboardByMemberID()[0]; }
        }

        public string SEOUrl
        {
            get
            {
                return "/users/" + this.NickName.ToLower();
            }
        }

        public static void UpdateProfilePhotoResourceFileId(Int32 memberId, Int32 profilePhotoResourceFileId)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_UpdateMemberProfilePhotoResourceFileID");
            db.AddInParameter(dbCommand, "memberId", DbType.Int32, memberId);
            db.AddInParameter(dbCommand, "profilePhotoResourceFileId", DbType.Int32, profilePhotoResourceFileId);

            db.ExecuteNonQuery(dbCommand);
        }

        public static IDataReader GetAllMemberReader()
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("AG_GetAllMember");

            return db.ExecuteReader(dbCommand);
        }

        public static string GetGalleryID(Member member, DefaultGalleryType GalleryType)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetGalleryID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, member.MemberID);
            //db.AddInParameter(dbCommand, "GalleryType", DbType.String, GalleryType);

            string WebGalleryID = string.Empty;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    WebGalleryID = (string)dr[0];
                }

                dr.Close();
            }

            return WebGalleryID;
        }

        public static List<Member> GetTop100LatestMembers()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetTop100LatestMembers");

            List<Member> members = new List<Member>();


            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                members = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            return members;
        }



        public static Member GetMemberAndDeviceByDeviceTagID(string DeviceTagID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberAndDeviceByDeviceTagID");
            db.AddInParameter(dbCommand, "DeviceTagID", DbType.String, DeviceTagID);

            List<Member> member = new List<Member>();


            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                member = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            if (member.Count > 0)
                return member[0];
            else
                return null;
        }

        public static Member GetMemberByPhotoCollectionID(string WebPhotoCollectionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberByPhotoCollectionID");
            db.AddInParameter(dbCommand, "WebPhotoCollectionID", DbType.String, WebPhotoCollectionID);

            List<Member> member = new List<Member>();


            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                member = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            if (member.Count > 0)
                return member[0];
            else
                return null;
        }



        /// <summary>
        /// Hamids chat reference
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static Member GetMemberByEmail(string Email)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberByEmail");
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);

            Member member = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                List<Member> members = Member.PopulateObject(dr);

                if (members.Count > 0)
                {
                    member = members[0];
                }

                dr.Close();
            }


            return member;

        }

        /// <summary>
        /// Is this Member blocked
        /// </summary>
        public bool HasBlockedMember(Member member)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_HasBlockedMember");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, this.MemberID);
            db.AddInParameter(dbCommand, "IsBlockedMemberID", DbType.Int32, member.MemberID);

            bool IsBlocked = false;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                if (dr.Read())
                {
                    IsBlocked = (bool)dr[0];
                }

                dr.Close();
            }


            return IsBlocked;

        }





        /// <summary>
        /// hamids chat reference
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static List<Member> GetAllFriendsByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllFriendsByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Member> members = new List<Member>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                members = Member.PopulateObjectWithJoin(dr);

                dr.Close();
            }

            return members;
        }

        public static List<Member> GetAllBlockedFriendsByMemberIDForPageLister(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllBlockedFriendsByMemberIDForPageLister");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Member> members = new List<Member>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                members = Member.PopulateMemberWithJoin(dr);

                dr.Close();
            }

            return members;
        }

        /// <summary>
        /// hamids chat reference
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static List<Member> GetAllFriendsByMemberIDForPageLister(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllFriendsByMemberIDForPageLister");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Member> members = new List<Member>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                members = Member.PopulateMemberWithJoin(dr);

                dr.Close();
            }

            return members;
        }

        public static bool IsNicknameAvailable(string Nickname)
        {
            bool IsAvailable = false;

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_IsNicknameAvailable");
            db.AddInParameter(dbCommand, "Nickname", DbType.String, Nickname);
            IDataReader dr = null;

            try
            {
                //execute the stored procedure
                dr = db.ExecuteReader(dbCommand);

                if (dr.Read())
                {
                    IsAvailable = Convert.ToBoolean(dr.GetInt32(0));
                }

                dr.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                dr.Close();
            }

            return IsAvailable;
        }

        public static bool IsEmailAddressAvailable(string Email)
        {
            bool IsAvailable = false;

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_IsEmailAddressAvailable");
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            IDataReader dr = null;

            try
            {
                //execute the stored procedure
                dr = db.ExecuteReader(dbCommand);

                if (dr.Read())
                {
                    IsAvailable = Convert.ToBoolean(dr.GetInt32(0));
                }

                dr.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                dr.Close();
            }

            return IsAvailable;
        }

        public static Member MemberLogin(string EmailAddress, string Password)
        {
            IDataReader dr = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("HG_MemberLogin");
                db.AddInParameter(dbCommand, "Email", DbType.String, EmailAddress);
                db.AddInParameter(dbCommand, "Password", DbType.String, Password);

                //execute the stored procedure
                dr = db.ExecuteReader(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<Member> member = Member.PopulateObject(dr);

            if (member.Count > 0)
                return member[0];
            else
                throw new BadCredentialsException(Properties.Resources.BadCredentials_InvalidEmailAddressOrPassword);
        }

        public static Member WebMemberLogin(string EmailAddress, string Password)
        {
            IDataReader dr = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("HG_MemberLogin");
                db.AddInParameter(dbCommand, "Email", DbType.String, EmailAddress);
                db.AddInParameter(dbCommand, "Password", DbType.String, Password);

                //execute the stored procedure
                dr = db.ExecuteReader(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<Member> member = Member.PopulateObject(dr);

            if (member.Count > 0)
                return member[0];
            else
                return null;
        }

        public static Member MemberLoginViaNickName(string NickName, string Password)
        {
            IDataReader dr = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("HG_MemberLoginViaNickName");
                db.AddInParameter(dbCommand, "NickName", DbType.String, NickName);
                db.AddInParameter(dbCommand, "Password", DbType.String, Password);

                //execute the stored procedure
                dr = db.ExecuteReader(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            List<Member> member = Member.PopulateObject(dr);

            if (member.Count > 0)
                return member[0];
            else
                return null;
        }

        public static Member SafeMemberLogin(string Nickname, string Password)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberViaNicknamePassword");
            db.AddInParameter(dbCommand, "Nickname", DbType.String, Nickname);
            db.AddInParameter(dbCommand, "Password", DbType.String, Password);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateObject(dr);

            dr.Close();

            if (members.Count > 0)
                return members[0];
            else
                return null;
        }



        public static int GetMemberID(string WebMemberID, string Password)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_MemberAuthenticate");
            db.AddInParameter(dbCommand, "WebMemberID", DbType.String, WebMemberID);
            db.AddInParameter(dbCommand, "Password", DbType.String, Password);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> member = Member.PopulateObject(dr);

            if (member.Count > 0)
                return member[0].MemberID;
            else
                throw new BadCredentialsException(Properties.Resources.BadCredentials_InvalidWebMemberIDOrPassword);
        }

        /// <summary>
        /// Search for a member
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static List<Member> GetMembersByKeywordSearch(string Keyword, string Country, int Gender)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetMembersByKeywordSearch");
            db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);
            db.AddInParameter(dbcommand, "Country", DbType.String, Country);
            db.AddInParameter(dbcommand, "Gender", DbType.Int16, Gender);

            List<Member> Members;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Members = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            return Members;
        }


        public static Member GetMembersByMemberIDWithFullJoin(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetMembersByMemberIDWithFullJoin");
            db.AddInParameter(dbcommand, "MemberID", DbType.Int32, MemberID);

            List<Member> Members;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Members = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            return Members[0];
        }

        public static List<Member> GetMembersByFullSearch2(string Keyword,
            string FirstName, string LastName, string NickName, 
            int Gender, string Country, string City, 
            bool HasAvatarPhoto, string email, 
            int trade, int hobby, string ProfileKeyWords, string order)
        {

            if (string.IsNullOrEmpty(Keyword))
                Keyword = null;

            if (string.IsNullOrEmpty(FirstName))
                FirstName = null;

            if (string.IsNullOrEmpty(LastName))
                LastName = null;

            if (string.IsNullOrEmpty(NickName))
                NickName = null;

            if (string.IsNullOrEmpty(Country))
                Country = null;

            if (string.IsNullOrEmpty(City))
                City = null;

            if (string.IsNullOrEmpty(email))
                email = null;

            if (string.IsNullOrEmpty(ProfileKeyWords))
                ProfileKeyWords = null;

            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetMembersByFullSearch2");
            db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);            
            db.AddInParameter(dbcommand, "FirstName", DbType.String, FirstName);
            db.AddInParameter(dbcommand, "LastName", DbType.String, LastName);
            db.AddInParameter(dbcommand, "NickName", DbType.String, NickName);
            db.AddInParameter(dbcommand, "Gender", DbType.Int16, Gender );
            db.AddInParameter(dbcommand, "Country", DbType.String, Country);
            db.AddInParameter(dbcommand, "City", DbType.String, City);
            db.AddInParameter(dbcommand, "HasAvatarPhoto", DbType.Int16, HasAvatarPhoto == true ? 1 : -1);
            db.AddInParameter(dbcommand, "Email", DbType.String, email);
            db.AddInParameter(dbcommand, "Hobby", DbType.Int16, hobby);
            db.AddInParameter(dbcommand, "Trade", DbType.Int16, trade);
            db.AddInParameter(dbcommand, "ProfileKeywords", DbType.String, ProfileKeyWords);
            db.AddInParameter(dbcommand, "OrderByClause", DbType.String, order);

            List<Member> Members;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Members = Member.PopulateMemberWithJoin(dr);
                dr.Close();
            }

            return Members;
        }

        public static Member GetMembersViaWebMemberIDWithFullJoin(string WebMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetMemberViaWebMemberIDWithFullJoin");
            db.AddInParameter(dbcommand, "WebMemberID", DbType.String, WebMemberID);

            List<Member> Members;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Members = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            if (Members.Count > 0)
                return Members[0];
            else
                return null;
        }

        /// <summary>
        /// Search for a member
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static List<Member> GetFriendsByKeywordSearch(int MemberID, string Keyword, string Country, int Gender)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetFriendsByKeywordSearch");
            db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);
            db.AddInParameter(dbcommand, "MemberID", DbType.String, MemberID);
            //db.AddInParameter(dbcommand, "Country", DbType.String, Country);
            //db.AddInParameter(dbcommand, "Gender", DbType.Int16, Gender);

            List<Member> Members;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Members = PopulateMemberWithJoin(dr);
                dr.Close();
            }

            return Members;
        }


        public static Member GetMemberViaWebMemberID(string WebMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberViaWebMemberID");
            db.AddInParameter(dbCommand, "WebMemberID", DbType.String, WebMemberID);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateObject(dr);

            dr.Close();

            if (members.Count == 0)
            {
                throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidWebMemberID, WebMemberID));
            }

            return members[0];
        }


        public static Member GetMemberViaWebMemberIDNoEx(string WebMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberViaWebMemberID");
            db.AddInParameter(dbCommand, "WebMemberID", DbType.String, WebMemberID);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateObject(dr);

            Member ReturnMember = null;

            dr.Close();

            if (members.Count > 0)
            {
                ReturnMember = members[0];
            }

            return ReturnMember;
        }




        public static Member AuthenticateMember(string WebMemberID, string Password)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_MemberAuthenticate");
            db.AddInParameter(dbCommand, "WebMemberID", DbType.String, WebMemberID);
            db.AddInParameter(dbCommand, "Password", DbType.String, Password);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateObject(dr);

            dr.Close();

            if (members.Count == 0)
            {
                throw new BadCredentialsException(Properties.Resources.BadCredentials_InvalidWebMemberIDOrPassword);
            }

            return members[0];
        }

        public static Boolean CheckUserExists(String Nickname, String Password)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_CheckUserExists");
            db.AddInParameter(dbCommand, "nickName", DbType.String, Nickname);
            db.AddInParameter(dbCommand, "password", DbType.String, Password);

            return Convert.ToInt32(db.ExecuteScalar(dbCommand)) != 0;
        }

        public static Member GetMemberViaNicknamePassword(string Nickname, string Password)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberViaNicknamePassword");
            db.AddInParameter(dbCommand, "Nickname", DbType.String, Nickname);
            db.AddInParameter(dbCommand, "Password", DbType.String, Password);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateObject(dr);

            dr.Close();

            if (members.Count == 0)
            {
                throw new BadCredentialsException(Properties.Resources.BadCredentials_InvalidNicknameOrPassword);
            }

            return members[0];
        }

        public static Member GetMemberViaNickname(string Nickname)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberViaNickname");
            db.AddInParameter(dbCommand, "Nickname", DbType.String, Nickname);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateObject(dr);

            dr.Close();

            if (members.Count == 0)
            {
                throw new BadCredentialsException(Properties.Resources.BadCredentials_InvalidNickname);
            }

            return members[0];
        }

        public static Member GetMemberViaNicknameNoEx(string Nickname)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberViaNickname");
            db.AddInParameter(dbCommand, "Nickname", DbType.String, Nickname);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            List<Member> members = Member.PopulateMemberWithJoin(dr);

            dr.Close();

            if (members.Count == 0)
            {
                return null;
            }

            return members[0];
        }

        /// <summary>
        /// Gets the number of new messages that havnt been read
        /// </summary>
        /// <returns></returns>
        public int GetNewMessageCount()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNewMessageCount");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, this.MemberID);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            if (dr.Read())
            {
                return dr.GetInt32(0);
            }

            return 0;
        }

        private MemberProfile myMemberProfile;

        /// <summary>
        /// The MemberProfile object for this Member
        /// </summary>
        public MemberProfile MyMemberProfile
        {
            get
            {
                if (myMemberProfile != null)
                {
                    return myMemberProfile;
                }


                if (this != null)
                {
                    this.memberProfile = this.GetMemberProfileByMemberID();
                }
                else
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidOperation_ParentObjectIsNull);
                }

                if (memberProfile.Count > 0)
                {
                    myMemberProfile = memberProfile[0];
                    return myMemberProfile;
                }
                else
                {
                    throw new InvalidOperationException(Properties.Resources.InvalidOperation_BadDbRelationship);
                }
            }
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of Members
        /// </summary>
        public static List<Member> PopulateMemberWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<Member> arr = new List<Member>();

            Member obj;

            while (dr.Read())
            {
                obj = new Member();
                obj._memberID = (int)dr["MemberID"];
                obj._webMemberID = (string)dr["WebMemberID"];
                obj._adminStatusID = (int)dr["AdminStatusID"];
                obj._nickName = (string)dr["NickName"];
                obj._channelID = (int)dr["ChannelID"];
                obj._password = (string)dr["Password"];
                obj._email = (string)dr["Email"];
                obj._gender = (int)dr["Gender"];
                obj._firstName = (string)dr["FirstName"];
                obj._lastName = (string)dr["LastName"];
                obj._profilePhotoResourceFileID = (int)dr["ProfilePhotoResourceFileID"];
                obj._dOB = (DateTime)dr["DOB"];
                obj._iSOCountry = (string)dr["ISOCountry"];
                if (list.IsColumnPresent("ISOCode")) { obj.ISOCode = (string)dr["ISOCode"]; }
                obj._zipPostcode = (string)dr["ZipPostcode"];
                obj._createdDT = (DateTime)dr["CreatedDT"];

                //obj._accountType = (int)dr["AccountType"];
                if (list.IsColumnPresent("AccountType")) { obj._accountType = (int)dr["AccountType"]; }

                if (list.IsColumnPresent("DTOnline")) { obj.LastOnline = (DateTime)dr["DTOnline"]; }
                if (list.IsColumnPresent("IsFriend")) { obj.IsFriend = (bool)dr["IsFriend"]; }

                obj.DefaultPhoto = new ResourceFile();
                if (list.IsColumnPresent("DefaultPhotoResourceFileID")) { obj.DefaultPhoto.ResourceFileID = (int)dr["DefaultPhotoResourceFileID"]; }
                if (list.IsColumnPresent("DefaultPhotoWebResourceFileID")) { obj.DefaultPhoto.WebResourceFileID = (string)dr["DefaultPhotoWebResourceFileID"]; }
                if (list.IsColumnPresent("DefaultPhotoResourceType")) { obj.DefaultPhoto.ResourceType = (int)dr["DefaultPhotoResourceType"]; }
                if (list.IsColumnPresent("DefaultPhotoStorageLocation")) { obj.DefaultPhoto.StorageLocation = (int)dr["DefaultPhotoStorageLocation"]; }
                if (list.IsColumnPresent("DefaultPhotoServer")) { obj.DefaultPhoto.Server = (int)dr["DefaultPhotoServer"]; }
                if (list.IsColumnPresent("DefaultPhotoPath")) { obj.DefaultPhoto.Path = (string)dr["DefaultPhotoPath"]; }
                if (list.IsColumnPresent("DefaultPhotoFileName")) { obj.DefaultPhoto.FileName = (string)dr["DefaultPhotoFileName"]; }
                if (list.IsColumnPresent("DefaultPhotoCreatedDT")) { obj.DefaultPhoto.CreatedDT = (DateTime)dr["DefaultPhotoCreatedDT"]; }


                if (list.IsColumnPresent("DeviceID"))
                {
                    obj.device = new List<Device>();
                    obj.device.Add(new Device());
                }
                if (list.IsColumnPresent("DeviceID")) { obj.device[0].DeviceID = (int)dr["DeviceID"]; }
                if (list.IsColumnPresent("DeviceTagID")) { obj.device[0].DeviceTagID = (string)dr["DeviceTagID"]; }
                if (list.IsColumnPresent("DeviceMemberID")) { obj.device[0].MemberID = (int)dr["DeviceMemberID"]; }
                if (list.IsColumnPresent("DevicePrivateEncryptionKey")) { obj.device[0].PrivateEncryptionKey = (string)dr["DevicePrivateEncryptionKey"]; }
                if (list.IsColumnPresent("DeviceCreatedDT")) { obj.device[0].CreatedDT = (DateTime)dr["DeviceCreatedDT"]; }

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        public void SetDefaultFriends(int MemberID)
        {



        }
    }
}