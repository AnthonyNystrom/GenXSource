using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class LoggedIn
    {
       public static void DeleteLoggedInByMemberID(Int32 MemberID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_DeleteLoggedInByMemberID");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, MemberID);
            
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


       public void SaveWithCheck()
       {
           Database db = DatabaseFactory.CreateDatabase();
           DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveLoggedInWithCheck");

           db.AddInParameter(dbCommand, "LoggedInID", DbType.Int32, LoggedInID);
           db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
           db.AddInParameter(dbCommand, "WebLoggedInID", DbType.String, WebLoggedInID);
           db.AddInParameter(dbCommand, "DTCreated", DbType.DateTime, DTCreated);

           using (IDataReader dr = db.ExecuteReader(dbCommand))
           {

               // get the returned ID
               if (dr.Read())
               {
                   int ID = Int32.Parse(dr[0].ToString());
                   //if the ID is NOT zero then the query was an insert
                   if (ID != 0)
                       this.LoggedInID = ID;
               }

               dr.Close();
           }

       }

       /// <summary>
       /// Calls the database and gets all the LoggedIn objects for this Member
       /// </summary>
       public static string[] GetLoggedInUserPass(string WebLoggedInID)
       {
           Database db = DatabaseFactory.CreateDatabase();

           DbCommand dbCommand = db.GetStoredProcCommand("HG_UserPassByWebLoggedInID");
           db.AddInParameter(dbCommand, "WebLoggedInID", DbType.String, WebLoggedInID);

           string[] retUserPass = new string[2];
           
           // Populate the datareader
           using (IDataReader dr = db.ExecuteReader(dbCommand))
           {
               if (dr.Read())
               {
                   // Call the PopulateObject method passing the datareader to return the object array
                   retUserPass[0] = (string)dr["NickName"];
                   retUserPass[1] = (string)dr["Password"];
               }
               dr.Close();
           }

           return retUserPass;
       }
    }
}
