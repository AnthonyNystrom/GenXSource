using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum TopNspotType { Featured = 1, Viewed = 2, Discussed = 3, Rated = 4 }

    public partial class NSpot
    {
        public string ShortDescription
        {
            get
            {
                if (this.Description.Length > 100)
                    return this.Description.Substring(0, 100) + "..";
                else
                    return this.Description;
            }

        }

        public string ShortName
        {
            get
            {
                if (this.Name.Length > 30)
                    return this.Name.Substring(0, 30) + "..";
                else
                    return this.Name;
            }

        }

        /// <summary>
        /// Gets the top 100 Nspots
        /// </summary>
        public static List<NSpot> GetTop100NSpots(TopNspotType TabType)
        {
            string OrderByClause = string.Empty;

            switch (TabType)
            {
                case TopNspotType.Featured:
                    OrderByClause = "TotalVoteScore";
                    break;
                case TopNspotType.Viewed:
                    OrderByClause = "NumberOfViews";
                    break;
                case TopNspotType.Discussed:
                    OrderByClause = "NumberOfComments";
                    break;
                case TopNspotType.Rated:
                    OrderByClause = "TotalVoteScore";
                    break;
            }


            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetTop100Nspots");
            db.AddInParameter(dbCommand, "OrderByClause", DbType.String, OrderByClause);

            List<NSpot> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }


        public static List<NSpot> GetAllNSpotByMemberID(Member member)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllNSpotByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, member.MemberID);

            List<NSpot> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        public static NSpot GetNSpotByNSpotWebIDWithJoin(string WebNSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotByNSpotWebIDWithJoin");
            db.AddInParameter(dbCommand, "WebNSpotID", DbType.String, WebNSpotID);

            List<NSpot> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidWebNSpotID, WebNSpotID));
        }  

        
        public static NSpot GetNSpotByNSpotWebID(string WebNSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotByWebNSpotID");
            db.AddInParameter(dbCommand, "WebNSpotID", DbType.String, WebNSpotID);

            List<NSpot> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidWebNSpotID, WebNSpotID));
        }

        /// <summary>
        /// gets all the members of the nspot
        /// </summary>
        /// <param name="NSpotID"></param>
        /// <returns></returns>
        public static List<Member> GetNSpotMembersByNSpotIDWithJoin(int NSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotMembersByNSpotIDWithJoin");
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);

            List<Member> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = Member.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return arr;
        }  

        


        
    }
}
