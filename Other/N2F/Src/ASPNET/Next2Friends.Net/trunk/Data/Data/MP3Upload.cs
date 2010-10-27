using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public partial class MP3Upload
    {

        public static void MP3UploadDone(int MemberID, string Title, string WebMP3ID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_MP3UploadDone");
            db.AddInParameter(dbcommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbcommand, "Title", DbType.String, Title);
            db.AddInParameter(dbcommand, "Path", DbType.String, WebMP3ID);

            try
            {
                db.ExecuteNonQuery(dbcommand);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteMP3UploadByWebMP3UploadID(string WebMP3ID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_DeleteMP3UploadByWebMP3UploadID");
            db.AddInParameter(dbcommand, "WebMP3UploadID", DbType.String, WebMP3ID);

            try
            {
                db.ExecuteNonQuery(dbcommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MP3Upload GetMP3UploadByWebMP3UploadID(string WebMP3UploadID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetMP3UploadByWebMP3UploadID");
            db.AddInParameter(dbcommand, "WebMP3UploadID", DbType.String, WebMP3UploadID);

            List<MP3Upload> MP3UploadList = new List<MP3Upload>();

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                MP3UploadList = MP3Upload.PopulateObject(dr);
            }

            if (MP3UploadList.Count > 0)
            {
                return MP3UploadList[0];
            }
            else
            {
                return null;
            }
        }

        public void Copy(int MemberID)
        {
            this.MemberID = MemberID;
            this.WebMP3UploadID = UniqueID.NewWebID();
            this.Mp3UploadID = 0;// set this to zero to perform an insert 
            this.Save();
        }
    }
}
