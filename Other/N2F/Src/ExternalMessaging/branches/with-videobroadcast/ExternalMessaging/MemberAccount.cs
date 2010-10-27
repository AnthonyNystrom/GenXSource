using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ExternalMessaging
{
    /// <summary>
    /// Represents a MemberAccount in the system
    /// </summary>
    internal sealed class MemberAccount
    {
        public Int32 MemberAccountID { get; set; }
        public Int32 MemberID { get; set; }
        /// <summary>
        /// 0 - None, 1 - Twitter.
        /// </summary>
        public Int32 AccountType { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }

        public static Boolean IsTwitterReady(MemberAccount account)
        {
            if (account == null)
                return false;
            if (account.AccountType != 1)
                return false;
            if (account.MemberAccountID == 0)
                return false;
            if (account.MemberID == 0)
                return false;
            if (String.IsNullOrEmpty(account.Password))
                return false;
            if (String.IsNullOrEmpty(account.Username))
                return false;

            return true;
        }

        public static MemberAccount GetMemberAccountByMemberID(Int32 MemberID, SqlConnection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            var command = new SqlCommand("AG_GetMemberAccountByMemberID", conn) { CommandType = CommandType.StoredProcedure };
            var param = new SqlParameter("@MemberID", MemberID) { DbType = DbType.Int32 };
            command.Parameters.Add(param);

            List<MemberAccount> arr = null;
            using (var dr = command.ExecuteReader())
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else return null;
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of MemberAccounts
        /// </summary>
        public static List<MemberAccount> PopulateObject(IDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            var arr = new List<MemberAccount>();
            MemberAccount obj;

            while (dr.Read())
            {
                obj = new MemberAccount();
                obj.MemberAccountID = (Int32)dr["MemberAccountID"];
                obj.MemberID = (Int32)dr["MemberID"];
                obj.AccountType = (Int32)dr["AccountType"];
                obj.Username = (String)dr["Username"];
                obj.Password = (String)dr["Password"];

                arr.Add(obj);
            }

            dr.Close();
            return arr;
        }
    }
}
