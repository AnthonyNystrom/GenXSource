using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class ProfileInvite
    {
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewProfileInvite", Target = "ProfileInvite", Event = "FOR INSERT")]
        public static void NewProfileInvite()
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
                            DataColumnCollection columns = table.Columns;

                            int InvitingMemberID = (int)row["MemberID"];                            

                            string EmailAddress = (string)row["EmailAddress"];

                            Member sendingMember = null;

                            sendingMember = Member.GetMemberByMemberID(InvitingMemberID, connection);
                            
                            string TargetEmailAddress = EmailAddress;

                            string subject = Templates.ProfileInviteSubject;
                            string body = Templates.ProfileInviteBody;

                            subject = MergeHelper.GenericMerge(subject, columns, row);
                            body = MergeHelper.GenericMerge(body, columns, row);

                            subject = MergeHelper.MergeMemberInfo(subject, sendingMember);
                            body = MergeHelper.MergeMemberInfo(body, sendingMember);

                            MailHelper.SendEmail(TargetEmailAddress, subject, body);
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
