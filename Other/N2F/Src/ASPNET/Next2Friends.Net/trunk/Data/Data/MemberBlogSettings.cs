using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace Next2Friends.Data
{
    public partial class MemberBlogSettings
    {
        public static MemberBlogSettings GetMemberBlogSettingsByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("AG_GetMemberBlogSettingsByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<MemberBlogSettings> arr = null;

            try
            {
                IDataReader dr = db.ExecuteReader(dbCommand);
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.MemberBlogSettings.PopulateObject(dr);
                dr.Close();
            }
            catch(Exception ex)
            {

                throw ex;
            }


            if (arr.Count == 0)
            {
                MemberBlogSettings memberBlogSettings = new MemberBlogSettings();
                memberBlogSettings.MemberID = MemberID;
                return memberBlogSettings;
            }
            else
            {
                return arr[0];
            }
        }
    }
}
