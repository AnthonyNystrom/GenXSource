using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Collections.Generic;

namespace ExternalMessaging
{
    public partial class Member
    {
        private int _memberID;		//DB identifier for the Member
        private string _webMemberID = "";		//Base64 Guid for the Member
        private int _adminStatusID;		//
        private string _nickName = "";		//The display nickname of the  member
        private int _channelID;		//The members channel (this is their home page profile/video/hotspots/photos)
        private string _password = "";		//The logon password of the member
        private string _email = "";		//The members email address
        private int _gender;		//NotSet | Female | Male
        private string _firstName = "";		//The members First Name
        private string _lastName = "";		//The members Last Name
        private int _profilePhotoResourceFileID;		//
        private DateTime _dOB = new DateTime(1900, 1, 1);		//The members Date Of Birth
        private string _iSOCountry = "";		//The ISo country code that the member resides in
        private string _zipPostcode = "";		//
        private DateTime _createdDT = new DateTime(1900, 1, 1);		//The date the user regiestered


        /// <summary>
        /// DB identifier for the Member
        /// </summary>
        public int MemberID
        {
            get { return _memberID; }
            set { _memberID = value; }
        }

        /// <summary>
        /// Base64 Guid for the Member
        /// </summary>
        public string WebMemberID
        {
            get { return _webMemberID; }
            set { _webMemberID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AdminStatusID
        {
            get { return _adminStatusID; }
            set { _adminStatusID = value; }
        }

        /// <summary>
        /// The display nickname of the  member
        /// </summary>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        /// <summary>
        /// The members channel (this is their home page profile/video/hotspots/photos)
        /// </summary>
        public int ChannelID
        {
            get { return _channelID; }
            set { _channelID = value; }
        }

        /// <summary>
        /// The logon password of the member
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// The members email address
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// NotSet | Female | Male
        /// </summary>
        public int Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        /// <summary>
        /// The members First Name
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// The members Last Name
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ProfilePhotoResourceFileID
        {
            get { return _profilePhotoResourceFileID; }
            set { _profilePhotoResourceFileID = value; }
        }

        /// <summary>
        /// The members Date Of Birth
        /// </summary>
        public DateTime DOB
        {
            get { return _dOB; }
            set { _dOB = value; }
        }

        /// <summary>
        /// The ISo country code that the member resides in
        /// </summary>
        public string ISOCountry
        {
            get { return _iSOCountry; }
            set { _iSOCountry = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZipPostcode
        {
            get { return _zipPostcode; }
            set { _zipPostcode = value; }
        }

        /// <summary>
        /// The date the user regiestered
        /// </summary>
        public DateTime CreatedDT
        {
            get { return _createdDT; }
            set { _createdDT = value; }
        }


        // Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewMember", Target = "Member", Event = "FOR INSERT")]
        public static void NewMember()
        {

            SqlCommand command = null;            
            DataSet ds = null;
            SqlDataAdapter dataAdapter = null;

            try
            {
                SqlTriggerContext triggContext = SqlContext.TriggerContext;
                switch (triggContext.TriggerAction)
                {
                    case TriggerAction.Insert:
                        // Retrieve the connection that the trigger is using
                        using (SqlConnection connection
                           = new SqlConnection(@"context connection=true"))
                        {
                            connection.Open();
                            command = new SqlCommand(@"SELECT * FROM INSERTED;",
                               connection);
                            dataAdapter = new SqlDataAdapter(command);
                            ds = new DataSet();
                            dataAdapter.Fill(ds);

                            DataTable table = ds.Tables[0];
                            DataRow row = table.Rows[0];
                            DataColumnCollection columns = table.Columns;

                            string TargetEmailAddress = (string)row["Email"];

                            string subject = Templates.NewMemberSubject;
                            string body = Templates.NewMemberBody;
                            subject = MergeHelper.GenericMerge(subject, columns, row);
                            body = MergeHelper.GenericMerge(body, columns, row);

                            MailHelper.SendEmail(TargetEmailAddress, subject, body);
                        }

                        break;
                }
            }
            catch { }
            finally
            {
                try
                {
                    if (command != null)
                        command.Dispose();

                    if (ds != null)
                        ds.Dispose();

                    if (dataAdapter != null)
                        dataAdapter.Dispose();
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets all the Member in the database With a full join with all the manually specified tables in SP code 
        /// </summary>
        public static Member GetMemberByMemberID(int MemberID, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("AG_GetMemberByMemberID", conn);
            SqlParameter param = new SqlParameter("@MemberID", MemberID);
            param.DbType = DbType.Int32;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(param);

            List<Member> arr = null;

            // Populate the datareader
            using (IDataReader dr = command.ExecuteReader())
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Member.PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else return null;
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of Members
        /// </summary>
        public static List<Member> PopulateObject(IDataReader dr)
        {
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
                obj._zipPostcode = (string)dr["ZipPostcode"];
                obj._createdDT = (DateTime)dr["CreatedDT"];

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        /// <summary>
        /// Calls the database and gets all the MemberSettings objects for this Member
        /// </summary>
        public static MemberSettings GetMemberSettingsByMemberID(int MemberID, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("AG_GetMemberSettingsByMemberID", conn);
            SqlParameter param = new SqlParameter("@MemberID", MemberID);
            param.DbType = DbType.Int32;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(param);

            List<MemberSettings> arr = null;

            // Populate the datareader
            using (IDataReader dr = command.ExecuteReader())
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = MemberSettings.PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else return null;
        }

    }

}