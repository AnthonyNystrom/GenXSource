using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class Message
    {
        // Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewMessage", Target = "Message", Event = "FOR INSERT")]
        public static void NewMessage()
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

                            //Populate data from the inserted row
                            DataRow insertedRow = ds.Tables[0].Rows[0];
                            DataColumnCollection insertedColumns = ds.Tables[0].Columns;

                            int MemberIDTo = (int)insertedRow["MemberIDTo"];
                            int MemberIDFrom = (int)insertedRow["MemberIDFrom"];
                            string ExternalEmailTo = (string)insertedRow["ExternalEmailTo"];

                            if (MemberIDTo != -1)
                            {
                                MemberSettings ms = Member.GetMemberSettingsByMemberID(MemberIDTo, connection);

                                // In case we don't find any settings
                                if (ms == null)
                                    return;

                                // If the user doesn't want to be notified return
                                if (!ms.NotifyOnNewMessage)
                                    return;
                            }

                            Member targetMember = null;

                            if (MemberIDTo != -1)
                            {
                                targetMember = Member.GetMemberByMemberID(MemberIDTo, connection);
                            }
                            else
                            {
                                targetMember = new Member();
                                targetMember.Email = ExternalEmailTo;
                            }

                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);

                            string TargetEmailAddress = targetMember.Email;

                            string subject = string.Empty;
                            string body = string.Empty;

                            if (MemberIDTo == -1)
                            {
                                subject = Templates.NewExternalMessageSubject;
                                body = Templates.NewExternalMessageBody;
                            }
                            else
                            {
                                subject = Templates.NewMessageSubject;
                                body = Templates.NewMessageBody;
                            }

                            subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
                            body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

                            subject = MergeHelper.MergeMemberInfo(subject, targetMember);
                            body = MergeHelper.MergeMemberInfo(body, targetMember);

                            subject = MergeHelper.MergeOtherMemberInfo(subject, sendingMember);
                            body = MergeHelper.MergeOtherMemberInfo(body, sendingMember);

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
