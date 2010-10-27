using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;

namespace ExternalMessaging
{

    public partial class PhotoComment
    {

        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewPhotoComment", Target = "PhotoComment", Event = "FOR INSERT")]
        public static void NewPhotoComment()
        {
            SqlCommand command = null;
            DataSet ds = null;
            DataSet dsPhotoInfo = null;
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

                        // In photo comment the MemberID is actually the from member id
                        int MemberIDFrom = (int)insertedRow["MemberID"];
                        // Get the PhotoID against the inserted Row
                        int PhotoID = (int)insertedRow["PhotoID"];

                        // Get Photo Information against the inserted Photo Comment
                        dsPhotoInfo = Photo.GetPhotoInformation(PhotoID, connection);

                        if (dsPhotoInfo.Tables.Count == 0 || dsPhotoInfo.Tables[0].Rows.Count == 0)
                            return;

                        DataRow photoRow = dsPhotoInfo.Tables[0].Rows[0];
                        DataColumnCollection photoColumns = dsPhotoInfo.Tables[0].Columns;

                        int MemberID = (int)photoRow["MemberID"];

                        Member targetMember = Member.GetMemberByMemberID(MemberID, connection);
                        Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);
                        MemberSettings ms = Member.GetMemberSettingsByMemberID(MemberID, connection);

                        string TargetEmailAddress = targetMember.Email;

                        // In case we don't find any settings
                        if (ms == null)
                            return;

                        // If the user doesn't want to be notified return
                        if (!ms.NotifyNewPhotoComment)
                            return;

                        string subject = Templates.NewPhotoCommentSubject;
                        string body = Templates.NewPhotoCommentBody;

                        subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
                        body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

                        subject = MergeHelper.GenericMerge(subject, photoColumns, photoRow);
                        body = MergeHelper.GenericMerge(body, photoColumns, photoRow);

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

                    if (dsPhotoInfo != null)
                        ds.Dispose();

                    if (dataAdapter != null)
                        dataAdapter.Dispose();
                }
                catch { }
            }
        }
    }
}