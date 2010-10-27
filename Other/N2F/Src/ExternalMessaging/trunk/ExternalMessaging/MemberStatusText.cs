/* ------------------------------------------------
 * MemberStatusText.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using System.Data.SqlClient;
using ExternalMessaging.Twitter;
using Microsoft.SqlServer.Server;

namespace ExternalMessaging
{
    public class MemberStatusText
    {
        [SqlTrigger(Name = "StatusChangedTrigger", Target = "MemberStatusText", Event = "FOR UPDATE")]
        public static void StatusChanged()
        {
            SqlContext.Pipe.Send("StatusChanged");

            SqlCommand command = null;
            SqlDataAdapter dataAdapter = null;
            DataSet ds = null;

            try
            {
                var triggContext = SqlContext.TriggerContext;

                switch (triggContext.TriggerAction)
                {
                    case TriggerAction.Update:

                        SqlContext.Pipe.Send("TriggerAction.Update");

                        /* Retrieve the connection that the trigger is using. */
                        using (var connection = new SqlConnection(@"context connection=true"))
                        {
                            connection.Open();

                            command = new SqlCommand(@"SELECT * FROM INSERTED;", connection);
                            dataAdapter = new SqlDataAdapter(command);
                            ds = new DataSet();

                            dataAdapter.Fill(ds);

                            var table = ds.Tables[0];
                            var row = table.Rows[0];

                            var columns = table.Columns;

                            var statusText = (String)row["StatusText"] ?? "";
                            var memberId = (Int32)row["MemberID"];

                            SqlContext.Pipe.Send(String.Format("StatusText = {0}", statusText));
                            SqlContext.Pipe.Send(String.Format("MemberID = {0}", memberId));

                            var targetAccount = MemberAccount.GetMemberAccountByMemberID(memberId, connection);

                            if (MemberAccount.IsTwitterReady(targetAccount))
                            {
                                SqlContext.Pipe.Send("Target account is Twitter ready.");
                                TwitterService.UpdateStatus(targetAccount.Username, targetAccount.Password, statusText);
                                SqlContext.Pipe.Send("Twitter status updated successfully.");
                            }
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send(String.Format("StatusChanged trigger exception: {0}", e.Message));
            }
            finally
            {
                try
                {
                    if (command != null)
                        command.Dispose();

                    if (ds != null)
                        ds.Dispose();

                    if (dataAdapter != null)
                        dataAdapter.Dispose();
                }
                catch { }
            }
        }
    }
}
