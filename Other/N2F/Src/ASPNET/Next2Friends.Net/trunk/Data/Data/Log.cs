using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Next2Friends.Data
{
    public partial class Log
    {
        public static Int32 Delete(String webServiceName)
        {
            if (String.IsNullOrEmpty(webServiceName))
                throw new ArgumentNullException("webServiceName");

            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetSqlStringCommand("DELETE FROM Log WHERE Source = @webServiceName");
            db.AddInParameter(dbCommand, "@webServiceName", DbType.String, webServiceName);
            return db.ExecuteNonQuery(dbCommand);
        }

        public static void Logger(String message)
        {
            new Log() { DTCreated = DateTime.Now, Text = message }.Save();
        }

        public static void Logger(String message, String source)
        {
            new Log() { DTCreated = DateTime.Now, Text = message, Source = source }.Save();
        }

        public static void Logger(String message, String source, String machineName)
        {
            new Log()
            {
                DTCreated = DateTime.Now,
                Text = String.Format("{0} said: {1}", machineName, message),
                Source = source
            }.Save();
        }
    }
}
