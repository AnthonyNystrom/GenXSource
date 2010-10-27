using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// Represents an Email sent out of the system
    /// </summary>
    public partial class EmailMessage
    {

        /// <summary>
        /// Gets an EmailMessage by WebID
        /// </summary>
        /// <param name="WebEmailMessageID">The WebID of the EmailMessage</param>
        /// <returns>returns a single EmailMessage object</returns>
        public static EmailMessage GetEmailMessageByWebEmailMessageIDWithJoin(string WebEmailMessageID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetEmailMessageByWebEmailMessageIDWithJoin");
            db.AddInParameter(dbCommand, "WebEmailMessageID", DbType.String, WebEmailMessageID);

            List<EmailMessage> emailMessage = new List<EmailMessage>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                emailMessage = EmailMessage.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            if (emailMessage.Count > 0)
                return emailMessage[0];
            else
                return null;
        }
    }
}
