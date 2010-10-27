using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;

namespace ExternalMessaging
{

    public partial class AAFComment
    {

        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewAAFComment", Target = "AskAFriendComment", Event = "FOR INSERT")]
        public static void NewAAFComment()
        {
            SqlCommand command;
            SqlTriggerContext triggContext = SqlContext.TriggerContext;
            SqlDataAdapter dataAdapter = null;
            DataSet ds = new DataSet();

            try
            {
                

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

                            // In AskAFriend comment the MemberID is actually the from member id
                            int MemberIDFrom = (int)insertedRow["MemberID"];
                            // Get the AskAFriendID against the inserted Row
                            int AskAFriendID = (int)insertedRow["AskAFriendID"];


                            // Get AskAFriend Information against the inserted AskAFriend Comment
                            ds = AskAFriend.GetAskAFriendInformation(AskAFriendID, connection);

                            // If there is no AskAFriend row found
                            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                                return;

                            DataRow AAFRow = ds.Tables[0].Rows[0];
                            DataColumnCollection AAFColumns = ds.Tables[0].Columns;

                            int MemberID = (int)AAFRow["MemberID"];

                            MemberSettings ms = Member.GetMemberSettingsByMemberID(MemberID, connection);

                            // In case we don't find any settings
                            if (ms == null)
                                return;

                            // If the user doesn't want to be notified return
                            if (!ms.NotifyOnAAFComment)
                                return;  

                            Member targetMember = Member.GetMemberByMemberID(MemberID, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);                          

                            string TargetEmailAddress = targetMember.Email;

                            string subject = Templates.NewAAFCommentSubject;
                            string body = Templates.NewAAFCommentBody;

                            subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
                            body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

                            subject = MergeHelper.GenericMerge(subject, AAFColumns, AAFRow);
                            body = MergeHelper.GenericMerge(body, AAFColumns, AAFRow);

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