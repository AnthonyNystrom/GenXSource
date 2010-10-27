/* ------------------------------------------------
 * N2FDatabase.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System.Configuration;
using System.Data.SqlClient;

namespace Next2Friends.GeoMessage.Databases
{
    static class N2FDatabase
    {
        public static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Next2Friends"].ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
