using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class MemberComment
    {
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewMemberComment", Target = "MemberComment", Event = "FOR INSERT")]
        public static void NewMemberComment()
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

                            int MemberID = (int)row["MemberID"];
                            int MemberIDFrom = (int)row["MemberIDFrom"];

                            MemberSettings ms = null;
                            ms = Member.GetMemberSettingsByMemberID(MemberID, connection);

                            // In case we don't find any settings
                            if (ms == null)
                                return;

                            if (!ms.NotifyNewProfileComment)
                                return;

                            Member targetMember = Member.GetMemberByMemberID(MemberID, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);


                            string TargetEmailAddress = targetMember.Email;

                            string subject = Templates.NewMemberCommentSubject;
                            string body = Templates.NewMemberCommentBody;

                            subject = MergeHelper.GenericMerge(subject, columns, row);
                            body = MergeHelper.GenericMerge(body, columns, row);

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
