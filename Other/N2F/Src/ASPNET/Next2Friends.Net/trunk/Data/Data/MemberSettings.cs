using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace Next2Friends.Data
{
    /// <summary>
    /// The Member personal settings
    /// </summary>
    public partial class MemberSettings
    {
    
        /// <summary>
        /// Gets the MemberSettings for a Member
        /// </summary>
        /// <param name="MemberID">The MemberID of the Member</param>
        /// <returns>A MemberSettings object</returns>
        public static MemberSettings GetMemberSettingsByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("AG_GetMemberSettingsByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<MemberSettings> arr = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.MemberSettings.PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                throw new ArgumentException(String.Format(Properties.Resources.Argument_NoMemberSettingsForMemberID, MemberID));
        }
        
    }
}
