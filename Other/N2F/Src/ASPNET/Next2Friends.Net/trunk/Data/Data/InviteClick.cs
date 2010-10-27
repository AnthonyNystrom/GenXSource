using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// Correponds to clicks that are performed on the links sent via invites
    /// </summary>
    partial class InviteClick
    {
        /// <summary>
        /// Get an InviteClick by WebID
        /// </summary>
        /// <param name="WebInviteClickID">The WebID associated to the Click</param>
        /// <returns>A Single InviteClick object</returns>
        public static InviteClick GetInviteClickByWebInviteClickID(string WebInviteClickID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetInviteClickByInviteClickID");
            db.AddInParameter(dbCommand, "WebInviteClickID", DbType.String, WebInviteClickID);

            InviteClick inviteClick = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ColumnFieldList list = new ColumnFieldList(dr);

                if (dr.Read())
                {
                    inviteClick = new InviteClick();

                    if (list.IsColumnPresent("InviteClickID")) { inviteClick._inviteClickID = (int)dr["InviteClickID"]; }
                    if (list.IsColumnPresent("WebInviteClickID")) { inviteClick._webInviteClickID = (string)dr["WebInviteClickID"]; }
                    if (list.IsColumnPresent("ForwardURL")) { inviteClick._forwardURL = (string)dr["ForwardURL"]; }
                    if (list.IsColumnPresent("ContactImportID")) { inviteClick._contactImportID = (int)dr["ContactImportID"]; }

                }
                else
                {
                    throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidWebInviteClickID, WebInviteClickID));
                }

                dr.Close();
            }

            return inviteClick;
        }
    }
}
