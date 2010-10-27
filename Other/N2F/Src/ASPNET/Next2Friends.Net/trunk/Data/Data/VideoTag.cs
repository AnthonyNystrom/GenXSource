using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class VideoTag
    {
        /// <summary>
        /// Deletes the current VideoTag
        /// </summary>
        public void Delete()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteVideoTagByVideoTagID");
            db.AddInParameter(dbCommand, "VideoTagID", DbType.Int32, VideoTagID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch { }
        }
    }
}
