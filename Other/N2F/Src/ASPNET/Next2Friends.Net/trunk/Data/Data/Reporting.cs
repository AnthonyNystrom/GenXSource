using System;
using System.Collections.Generic;
using System.Text;
using Next2Friends.Data;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public class Reporting
    {

        public static List<Signups> GetSignups()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_ReportingGetSignupNumbers");

            List<Signups> arr = new List<Signups>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Signups s = new Signups();
                    s.NumberOfSignups = (int)dr[0];
                    s.Date = new DateTime((int)dr[3], (int)dr[2], (int)dr[1]);
                    arr.Add(s);
                }

                dr.Close();
            }

            return arr;
        }

        public static int ReportingGetTotalSignupNumbers()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_ReportingGetTotalSignupNumbers");

            int Total = 0;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Total = (int)dr[0];
                }

                dr.Close();
            }

            return Total;
        }  


        

        public static List<Signups> GetSignupsByHour()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_ReportingGetSignupNumbersByHours");

            List<Signups> arr = new List<Signups>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Signups s = new Signups();
                    s.NumberOfSignups = (int)dr[0];
                    s.Date = new DateTime((int)dr[4], (int)dr[3], (int)dr[2], (int)dr[1],0,0);
                    arr.Add(s);
                }

                dr.Close();
            }

            return arr;
        }  
    }

    

    public class Signups
    {
        public int NumberOfSignups { get; set; }
        public DateTime Date { get; set; }
    }
}
