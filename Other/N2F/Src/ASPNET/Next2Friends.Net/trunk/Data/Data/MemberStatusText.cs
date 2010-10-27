using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace Next2Friends.Data
{
    /// <summary>
    /// Represents the Member Status
    /// </summary>
    public partial class MemberStatusText
    {
        //public int  MemberStatusTextID { get; set; }
        //public int  MemberID { get; set; }
        //public string  StatusText { get; set; }
        //public DateTime UpdatedDT { get; set; }

        /// <summary>
        /// Update the status text for a member
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member</param>
        /// <param name="StatusText">The updated status text</param>
        public static void UpdateStatusText(int MemberID, string StatusText)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_UpdateMemberStatusText");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "NewStatusText", DbType.String, StatusText);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
