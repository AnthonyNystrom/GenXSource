using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class AskAFriendResponse
    {

        /// <summary>
        /// Check if a Member has voted for an AskAFriend or not
        /// </summary>
        /// <param name="member">The Member Object</param>
        /// <param name="AAF">The AskAFriend object</param>
        /// <returns>True if the member has voted otherwise false</returns>
        public static bool HasntYetVoted(Member member, AskAFriend AAF)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_HasntYetVoted");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, member.MemberID);
            db.AddInParameter(dbCommand, "AskAFriendID", DbType.Int32, AAF.AskAFriendID);

            bool HasVoted = false;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    HasVoted = (bool)dr["HasVoted"];
  
                }

                dr.Close();
            }

            return !HasVoted;
        }
    }
}
