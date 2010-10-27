/* ------------------------------------------------
 * MemberAccountActivity.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;

namespace Next2Friends.Data
{
    public enum MemberAccountType { None, Twitter };
    public enum MemberActivityType { None, Video, PhotoGallery, Photo, Status };

    public partial class MemberAccountActivity
    {
        /// <summary>
        /// If activity not found for the member with the specified <code>memberId</code>, returns <code>DateTime.MinValue</code>.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="accountType">0 - None, 1 - Twitter.</param>
        /// <param name="activityType">0 - None, 1 - Video, 2 - PhotoGallery, 3 - Photo, 4 - Status.</param>
        /// <exception cref="ArgumentException">
        /// If <code>accountType</code> is not defined in <code>MemberAccountType</code> enumeration, or
        /// if <code>activityType</code> is not defined in <code>MemberActivityType</code> enumeration.
        /// </exception>
        public static DateTime GetLastActivity(Int32 memberId, Int32 accountType, Int32 activityType)
        {
            if (!Enum.IsDefined(typeof(MemberAccountType), accountType))
                throw new ArgumentException(Properties.Resources.Argument_InvalidMemberAccountType);
            if (!Enum.IsDefined(typeof(MemberActivityType), activityType))
                throw new ArgumentException(Properties.Resources.Argument_InvalidMemberActivityType);

            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetLastActivity");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, memberId);
            db.AddInParameter(dbCommand, "AccountType", DbType.Int32, accountType);
            db.AddInParameter(dbCommand, "ActivityType", DbType.Int32, activityType);

            var lastActivity = db.ExecuteScalar(dbCommand);
            if (lastActivity != null)
                return Convert.ToDateTime(lastActivity);
            return DateTime.MinValue;
        }

        /// <summary>
        /// Returns <code>true</code> if the operation completed successfully; otherwise, <code>false</code>.
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="accountType">0 - None, 1 - Twitter.</param>
        /// <param name="activityType">0 - None, 1 - Video, 2 - PhotoGallery, 3 - Photo, 4 - Status.</param>
        /// <exception cref="ArgumentException">
        /// If <code>accountType</code> is not defined in <code>MemberAccountType</code> enumeration, or
        /// if <code>activityType</code> is not defined in <code>MemberActivityType</code> enumeration.
        /// </exception>
        public static Boolean SetLastActivity(Int32 memberId, Int32 accountType, Int32 activityType)
        {
            if (!Enum.IsDefined(typeof(MemberAccountType), accountType))
                throw new ArgumentException(Properties.Resources.Argument_InvalidMemberAccountType);
            if (!Enum.IsDefined(typeof(MemberActivityType), activityType))
                throw new ArgumentException(Properties.Resources.Argument_InvalidMemberActivityType);

            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_SetLastActivity");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, memberId);
            db.AddInParameter(dbCommand, "AccountType", DbType.Int32, accountType);
            db.AddInParameter(dbCommand, "ActivityType", DbType.Int32, activityType);

            return Convert.ToInt32(db.ExecuteScalar(dbCommand), CultureInfo.InvariantCulture) != 0;
        }
    }
}
