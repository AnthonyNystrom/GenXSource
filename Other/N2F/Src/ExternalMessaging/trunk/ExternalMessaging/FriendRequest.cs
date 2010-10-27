using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class FriendRequest
    {
        // Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewFriendRequest", Target = "FriendRequest", Event = "FOR INSERT")]
        public static void NewFriendRequest()
        {
            SqlCommand command = null;
            SqlDataAdapter dataAdapter = null;
            DataSet ds = null; 

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

                            if (!((int)insertedRow["Response"] == 1 && (int)insertedRow["Status"] == 1))
                            {
                                return;
                            }

                            int MemberIDTo = (int)insertedRow["FriendMemberID"];
                            int MemberIDFrom = (int)insertedRow["MemberID"];

                            MemberSettings ms = Member.GetMemberSettingsByMemberID(MemberIDTo, connection);

                            // In case we don't find any settings
                            if (ms == null)
                                return;

                            // If the user doesn't want to be notified return
                            if (!ms.NotifyOnFriendRequest)
                                return;  

                            Member targetMember = Member.GetMemberByMemberID(MemberIDTo, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);

                            string TargetEmailAddress = targetMember.Email;

                            string subject = Templates.NewFriendRequestSubject;
                            string body = Templates.NewFriendRequestBody;

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

        // Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "AcceptedFriendRequest", Target = "FriendRequest", Event = "FOR UPDATE")]
        public static void AcceptedFriendRequest()
        {
            SqlCommand command = null;
            SqlCommand command2 = null;            
            SqlDataAdapter dataAdapter = null;
            SqlDataAdapter dataAdapter2 = null;
            DataSet ds = null;
            DataSet dsDel = null;

            try
            {
                SqlTriggerContext triggContext = SqlContext.TriggerContext;
                switch (triggContext.TriggerAction)
                {
                    case TriggerAction.Update:
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
                            DataRow updatedRow = ds.Tables[0].Rows[0];
                            DataColumnCollection updatedColumns = ds.Tables[0].Columns;

                            command2 = new SqlCommand(@"SELECT * FROM DELETED;",
                               connection);
                            dataAdapter2 = new SqlDataAdapter(command2);
                            dsDel = new DataSet();
                            dataAdapter2.Fill(dsDel);

                            //Populate data from the inserted row
                            DataRow deletedRow = dsDel.Tables[0].Rows[0];
                            DataColumnCollection deletedColumns = dsDel.Tables[0].Columns;

                            // If the old response value was zero and the new response value is ONE
                            // only then its a request accept
                            // Other wise return
                            if (!((int)deletedRow["Status"] == 0 && (int)updatedRow["Status"] == 1))
                            {
                                return;
                            }

                            // If the status has been updated to ONE but
                            // response is still ZERO
                            // means the friend request was rejected
                            // Don't need to send the email on friend request reject
                            if (((int)updatedRow["Response"] == 0 && (int)updatedRow["Status"] == 1))
                            {
                                return;
                            }    

                            int MemberIDTo = (int)updatedRow["FriendMemberID"];
                            int MemberIDFrom = (int)updatedRow["MemberID"];

                            MemberSettings ms = Member.GetMemberSettingsByMemberID(MemberIDTo, connection);

                            // In case we don't find any settings
                            if (ms == null)
                                return;

                            // If the user doesn't want to be notified return
                            if (!ms.NotifyOnFriendRequest)
                                return;  

                            Member targetMember = Member.GetMemberByMemberID(MemberIDTo, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);

                            string TargetEmailAddress = targetMember.Email;

                            string subject = Templates.AcceptedFriendRequestSubject;
                            string body = Templates.AcceptedFriendRequestBody;

                            subject = MergeHelper.GenericMerge(subject, updatedColumns, updatedRow);
                            body = MergeHelper.GenericMerge(body, updatedColumns, updatedRow);

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

                    if (command2 != null)
                        command2.Dispose();

                    if (ds != null)
                        ds.Dispose();

                    if (dsDel != null)
                        dsDel.Dispose();

                    if (dataAdapter != null)
                        dataAdapter.Dispose();

                    if (dataAdapter2 != null)
                        dataAdapter2.Dispose();
                }
                catch { }
            }
        }


    }
}
