using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class AskAFriendPhoto
    {
        /// <summary>
        /// Gets AskAFriendPhoto With a full join with all the manually specified tables in SP code 
        /// </summary>
        /// <param name="AskAFriendID">The ID of the AskAFriend</param>
        /// <returns>A List of AskAFriendPhoto</returns>
        public static List<AskAFriendPhoto> GetAskAFriendPhotoByAskAFriendIDWithJoin(int AskAFriendID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAskAFriendPhotoByAskAFriendIDWithJoin");
            db.AddInParameter(dbCommand, "AskAFriendID", DbType.Int32, AskAFriendID);

            List<AskAFriendPhoto> arr = new List<AskAFriendPhoto>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoin(dr);
            }
          

            return arr;
        }
    }
}
