using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Next2Friends.Data
{
    public partial class MemberAccount
    {
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

        public static MemberAccount GetMemberAccountByMemberID(Int32 memberId)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("AG_GetMemberAccountByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, memberId);

            var accounts = new List<MemberAccount>();

            using (var dr = db.ExecuteReader(dbCommand))
            {
                accounts = PopulateObject(dr);
                dr.Close();
            }

            if (accounts.Count > 0)
                return accounts[0];
            else
                return null;
        }
    }
}
