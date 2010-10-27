/* ------------------------------------------------
 * N2FDatabase.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System.Configuration;
using System.Data.SqlClient;

namespace Next2Friends.WebServices.GeoMessage.Databases
{
    static class N2FDatabase
    {
        public static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataAccessQuickStart"].ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
