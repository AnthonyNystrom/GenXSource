using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;

namespace ExternalMessaging
{

    public partial class VideoComment
    {

        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewVideoComment", Target = "VideoComment", Event = "FOR INSERT")]
        public static void NewVideoComment()
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

                            // In Video comment the MemberID is actually the from member id
                            int MemberIDFrom = (int)insertedRow["MemberID"];
                            // Get the VideoID against the inserted Row
                            int VideoID = (int)insertedRow["VideoID"];

                            // Get Video Information against the inserted Video Comment
                            ds = Video.GetVideoInformation(VideoID, connection);

                            // If the video doesn't exist then return
                            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                                return;

                            DataRow VideoRow = ds.Tables[0].Rows[0];
                            DataColumnCollection VideoColumns = ds.Tables[0].Columns;

                            int MemberID = (int)VideoRow["MemberID"];

                            MemberSettings ms = Member.GetMemberSettingsByMemberID(MemberID, connection);

                            // In case we don't find any settings
                            if (ms == null)
                                return;

                            // If the user doesn't want to be notified return
                            if (!ms.NotifyNewVideoComment)
                                return;

                            Member targetMember = Member.GetMemberByMemberID(MemberID, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);


                            string TargetEmailAddress = targetMember.Email;

                            string subject = Templates.NewVideoCommentSubject;
                            string body = Templates.NewVideoCommentBody;

                            subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
                            body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

                            subject = MergeHelper.GenericMerge(subject, VideoColumns, VideoRow);
                            body = MergeHelper.GenericMerge(body, VideoColumns, VideoRow);

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