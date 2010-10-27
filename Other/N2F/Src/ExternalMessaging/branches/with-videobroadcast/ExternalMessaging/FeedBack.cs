using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class FeedBack
    {
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewFeedback", Target = "Feedback", Event = "FOR INSERT")]
        public static void NewMemberInvite()
        {
            SqlCommand command = null;            
            DataSet ds = null;
            SqlDataAdapter dataAdapter = null;

            try
            {
                SqlTriggerContext triggContext = SqlContext.TriggerContext;

                switch (triggContext.TriggerAction)
                {
                    case TriggerAction.Insert:
                        // Retrieve the connection that the trigger is using
                        using (SqlConnection connection
                           = new SqlConnection(@"context connection=true"))
                        {
                            connection.Open();
                            command = new SqlCommand(@"SELECT * FROM INSERTED;",
                               connection);
                            dataAdapter = new SqlDataAdapter(command);
                            ds = new DataSet();
                            dataAdapter.Fill(ds);

                            DataTable table = ds.Tables[0];
                            DataRow row = table.Rows[0];

                            int MemberID = (int)row["MemberID"];

                            string TargetEmailAddress = (string)row["EmailAddress"];

                            Member sendingMember = null;

                            if (MemberID != 0)
                            {
                                sendingMember = Member.GetMemberByMemberID(MemberID, connection);
                            }

                            string subject = "Feedback recieved from " + (string)row["Name"];
                            string body = "The feed back was sent by " + (string)row["Name"];
                            body += " using <a href=\"mailto:" + TargetEmailAddress + "\">" + TargetEmailAddress + "</a>";

                            body += "<br><br>The feed back details are following <br><br>";

                            body += (string)row["Text"];

                            
                            MailHelper.SendEmail("hans@next2friends.com", subject, body);
                            MailHelper.SendEmail("anthony@next2friends.com", subject, body);
                            MailHelper.SendEmail("rachel@next2friends.com", subject, body);
                        }

                        break;
                }
            }
            catch { }
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
