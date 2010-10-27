using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class Channel
    {
        public static void UpdateFeaturedChannels()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcommand = db.GetSqlStringCommand("select top 10 * from Channel");
            IDataReader dr = db.ExecuteReader(dbcommand);

            List<Channel> VideosByFeature = PopulateObject(dr);

            db = DatabaseFactory.CreateDatabase();

            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();

            DbTransaction transaction = dbConn.BeginTransaction();

            DbCommand dbcommandDelete = db.GetSqlStringCommand("DELETE FROM FeaturedChannel");
            db.ExecuteNonQuery(dbcommandDelete);

            for (int i = 0; i < VideosByFeature.Count; i++)
            {
                FeaturedChannel FeaturedChan = new FeaturedChannel();
                FeaturedChan.Position = (i + 1);
                FeaturedChan.ChannelID = VideosByFeature[i].ChannelID;
                FeaturedChan.Save() ;
            }

            transaction.Commit();
        }

        public static List<Channel> GetFeaturedChannels()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetFeaturedChannels");

            List<Channel> arr = new List<Channel>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = Channel.PopulateObject(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }
    }
}
