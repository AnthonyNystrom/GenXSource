/* ------------------------------------------------
 * ForgottenPassword.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Next2Friends.Data
{
    /// <summary>
    /// Deals with forgot password requests
    /// </summary>
    partial class ForgottenPassword
    {
        /// <summary>
        /// Takes a Member ID and an email address and sends the password reminder to that address.
        /// The email sending part is handled by CLR triggers
        /// </summary>
        /// <param name="memberID">The memberID of the Member requesting reminder</param>
        /// <param name="emailAddress">The email address on which the reminder is to be sent</param>
        public static void RemindPassword(Int32 memberID, String emailAddress)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_RemindPassword");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, memberID);
            db.AddInParameter(dbCommand, "EmailAddress", DbType.String, emailAddress);
            db.ExecuteNonQuery(dbCommand);

            var feeds = Next2Friends.Data.FeedItem.GetFeed(9);
            
        }
    }
}
