using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
namespace Next2Friends.Data
{
    public partial class PhotoTag
    {
        /// <summary>
        /// Deletes the current PhotoTag
        /// </summary>
        public void Delete()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeletePhotoTagByPhotoTagID");
            db.AddInParameter(dbCommand, "PhotoTagID", DbType.Int32, PhotoTagID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch { }
        }
    }
}
