using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class LiveBroadcast
    {
        // Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewLiveBroadcast", Target = "LiveBroadcast", Event = "FOR INSERT")]
        public static void NewLiveBroadcast()
        {
            try
            {
                SqlCommand command;
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
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            dataAdapter.Fill(ds);

                            //Populate data from the inserted row
                            DataRow insertedRow = ds.Tables[0].Rows[0];
                            DataColumnCollection insertedColumns = ds.Tables[0].Columns;

                            int MemberIDTo = (int)insertedRow["MemberIDTo"];
                            int MemberIDFrom = (int)insertedRow["MemberIDFrom"];

                            Member targetMember = Member.GetMemberByMemberID(MemberIDTo, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);

                            string TargetEmailAddress = targetMember.Email;

                            string subject = Templates.NewLiveBroadcastSubject;
                            string body = Templates.NewLiveBroadcastBody;

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
        }
    }
}
