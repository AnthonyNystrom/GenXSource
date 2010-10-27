using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace Next2Friends.Data
{

    public enum InviteState { None, AlreadyAMember, InviteSent, InviteAcepted }
    public enum FriendState { None, AlreadyAFriend, InviteSent, FriendRequestAcepted }

    public partial class ContactImport
    {
        public string WebID { get; set; }
        public int IsInitialImport { get; set; }

        public void SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveContactImportWithCheck");

            db.AddInParameter(dbCommand, "ContactImportID", DbType.Int32, ContactImportID);
            db.AddInParameter(dbCommand, "ImporterMemberID", DbType.Int32, ImporterMemberID);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "InviteState", DbType.Int32, InviteState);
            db.AddInParameter(dbCommand, "FriendState", DbType.Int32, FriendState);
            db.AddInParameter(dbCommand, "ClickedEmailInvite", DbType.Boolean, ClickedEmailInvite);
            db.AddInParameter(dbCommand, "JoinedFromInvite", DbType.Boolean, JoinedFromInvite);
            db.AddInParameter(dbCommand, "BecameMemberID", DbType.Int32, BecameMemberID);
            db.AddInParameter(dbCommand, "ImportToken", DbType.String, ImportToken);
            db.AddInParameter(dbCommand, "IsInitialImport", DbType.Int32, IsInitialImport);
            

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int ID = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (ID != 0)
                        this.ContactImportID = ID;
                }

                dr.Close();
            }

        }

        public static List<ContactImport> GetAllContactImportByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllContactImportByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<ContactImport> Contacts = new List<ContactImport>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Contacts = PopulateObject(dr);

                dr.Close();
            }

            return Contacts;
        }


        public static List<ContactImport> JoinEmailsWithMembers(int MemberID, string[] EmailList, string ImportToken)
        {
            //string SecurityCheckedEmailList = string.Empty;

            //for (int i = 0; i < EmailList.Length; i++)
            //{
            //    if (Next2Friends.Misc.RegexPatterns.TestEmailRegex(EmailList[i]))
            //    {
            //        SecurityCheckedEmailList += "'" + EmailList[i].Trim() + "'" + ((i != EmailList.Length - 1) ? "," : string.Empty);
            //    }
            //}

            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_JoinEmailsWithMembers");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "ImportToken", DbType.String, ImportToken);

            List<ContactImport> Contacts = new List<ContactImport>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                Contacts = PopulateContactImport(dr);

                dr.Close();
            }

            return Contacts;
        }

        public static List<ContactImport> PopulateContactImport(IDataReader dr)
        {
            List<ContactImport> arr = new List<ContactImport>();

            ContactImport contactImport;

            while (dr.Read())
            {
                contactImport = new ContactImport();
                contactImport.Email = (string)dr["Email"];
                contactImport.FriendState = (int)dr["FriendState"];
                contactImport.InviteState = (int)dr["InviteState"];              

                arr.Add(contactImport);
            }

            dr.Close();

            return arr;
        }
    }
}
