using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class Favourite
    {
        public void SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveFavouriteWithCheck");

            db.AddInParameter(dbCommand, "FavouriteID", DbType.Int32, FavouriteID);
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "TheFavouriteObjectID", DbType.Int32, TheFavouriteObjectID);
            db.AddInParameter(dbCommand, "ObjectType", DbType.Int32, ObjectType);
            db.AddInParameter(dbCommand, "DTCreated", DbType.DateTime, DTCreated);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int ID = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (ID != 0)
                        this.FavouriteID = ID;
                }

                dr.Close();
            }

        }

        public void Delete()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteFavouritByFavouriteID");
            db.AddInParameter(dbCommand, "FavouriteID", DbType.Int32, FavouriteID);

            //execute the stored procedure
            db.ExecuteNonQuery(dbCommand);
        }
        public static void Delete(int FavouriteID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteFavouritByFavouriteID");
            db.AddInParameter(dbCommand, "FavouriteID", DbType.Int32, FavouriteID);

            //execute the stored procedure
            db.ExecuteNonQuery(dbCommand);
        }
        public static void Delete(int MemberID,int TheFavouriteObjectID,int ObjectType)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteFavourite");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "TheFavouriteObjectID", DbType.Int32, TheFavouriteObjectID);
            db.AddInParameter(dbCommand, "ObjectType", DbType.Int32, ObjectType);

            //execute the stored procedure
            db.ExecuteNonQuery(dbCommand);
        }
    }
}
