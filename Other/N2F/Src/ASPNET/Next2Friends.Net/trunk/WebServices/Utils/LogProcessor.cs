using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Next2Friends.Data;

namespace Next2Friends.WebServices.Utils
{
    internal static class LogProcessor
    {
        public static String ArrayToString<T>(T[] value)
        {
            var result = new StringBuilder();

            foreach (var item in value)
                result.AppendFormat("\"{0}\" ", item);

            return result.ToString();
        }

        public static String ToHtml(String webServiceName)
        {
            if (String.IsNullOrEmpty(webServiceName))
                throw new ArgumentNullException("webServiceName");

            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetSqlStringCommand("SELECT LogID, Text, DTCreated FROM Log WHERE Source = @webServiceName");
            db.AddInParameter(dbCommand, "@webServiceName", DbType.String, webServiceName);
            List<Log> items = null;

            /* Execute the query. */
            using (var dr = db.ExecuteReader(dbCommand))
            {
                items = Log.PopulateObject(dr);
            }

            var output = new StringBuilder();

            if (items != null)
            {
                foreach (var item in items)
                    output.AppendFormat(String.Format("{0}\t{1}<br/>", item.DTCreated, item.Text));
            }

            return output.ToString();
        }
    }
}
