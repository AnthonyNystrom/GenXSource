using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;


namespace ExternalMessaging
{
    
    public class Comment
    {
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewComment", Target = "Comment", Event = "FOR INSERT")]
        public static void NewComment()
        {
            SqlCommand command = null;            
            DataSet ds = null;
            SqlDataAdapter dataAdapter = null;

            string TargetEmailAddress = null;
            string OwnerEmailAddress = null;
            int OwnerMemberID = -1;

            string subject = null;
            string body = null;

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

                            DataRow insertedRow = ds.Tables[0].Rows[0];
                            
                            int ObjectID = (int)insertedRow["ObjectID"];
                            int MemberIDFrom = (int)insertedRow["MemberIDFrom"];
                            ContentType commentType = (ContentType)((int)insertedRow["CommentType"]);
                            int ThreadNo = (int)insertedRow["ThreadNo"];
                            string Path = (string)insertedRow["Path"];

                            // The object on which the comment was made along with the owner information
                            // Can be a Video, Photo, Nspot, Member etc...
                            DataSet objectDS = Utility.GetObjectByObjectIDWithJoin(ObjectID, commentType, connection);
                            OwnerMemberID = (int)objectDS.Tables[0].Rows[0]["MemberID"];

                            if (commentType == ContentType.Wall)
                            {
                                OwnerEmailAddress = (string)objectDS.Tables[0].Rows[0]["Email"];                                
                            }
                            else
                            {
                                OwnerEmailAddress = (string)objectDS.Tables[0].Rows[0]["MemberEmail"];
                            }                            

                            // The target members to which the notification email should be sent
                            // The owner of the object is not included in this list.
                            DataSet targetMembers = GetCommentTargetMembers(ObjectID, commentType, ThreadNo, connection);
                            Member sendingMember = Member.GetMemberByMemberID(MemberIDFrom, connection);


                            string unMergedSubject = GetSubject(Path == "/", commentType);
                            string unMergedBody = GetBody(Path == "/", commentType);

                            unMergedBody = Utility.PopulateLink( unMergedBody, commentType, objectDS.Tables[0].Rows[0] );

                            // Send notification email to all members participating in the thread
                            foreach (DataRow row in targetMembers.Tables[0].Rows)
                            {   
                                // if the posting member is also found in target member.skip
                                if ( (int)row["MemberID"] == MemberIDFrom )
                                    continue;

                                TargetEmailAddress = (string)row["Email"];
                                
                                subject = unMergedSubject;
                                body = unMergedBody;

                                if (OwnerEmailAddress == TargetEmailAddress)
                                    continue;
                                
                                if (!Notify(row, ContentType.ThreadReply))
                                    continue;

                                

                                subject = MergeHelper.GenericMerge(subject, insertedRow.Table.Columns, insertedRow);
                                body = MergeHelper.GenericMerge(body, insertedRow.Table.Columns, insertedRow);

                                subject = MergeHelper.GenericMerge(subject, row.Table.Columns, row);
                                body = MergeHelper.GenericMerge(body, row.Table.Columns, row);

                                subject = MergeHelper.MergeOtherMemberInfo(subject, sendingMember);
                                body = MergeHelper.MergeOtherMemberInfo(body, sendingMember);

                                //body = MergeHelper.MergeBanner(body, "New " + commentType.ToString() + " comment",connection);

                                MailHelper.SendEmail(TargetEmailAddress, subject, body);
                            }

                            if (MemberIDFrom != OwnerMemberID 
                                && 
                                Notify(
                                    GetMemberSettingsByMemberID(OwnerMemberID, connection).Tables[0].Rows[0], commentType)                                
                                )
                            {
                                subject = GetSubject(true, commentType);
                                body = GetBody(true, commentType);                                

                                subject = MergeHelper.GenericMerge(subject, insertedRow.Table.Columns, insertedRow);
                                body = MergeHelper.GenericMerge(body, insertedRow.Table.Columns, insertedRow);

                                subject = MergeHelper.GenericMerge(subject, objectDS.Tables[0].Columns, objectDS.Tables[0].Rows[0]);
                                body = MergeHelper.GenericMerge(body, objectDS.Tables[0].Columns, objectDS.Tables[0].Rows[0]);

                                subject = MergeHelper.MergeOtherMemberInfo(subject, sendingMember);
                                body = MergeHelper.MergeOtherMemberInfo(body, sendingMember);

                                //body = MergeHelper.MergeBanner(body, "New " + commentType.ToString() + " comment", connection);
                                
                                MailHelper.SendEmail(OwnerEmailAddress, subject, body);
                            }
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

       

        private static string GetBody(bool rootComment,ContentType commentType)
        {
            if (rootComment)
            {
                switch (commentType)
                {
                    case ContentType.Wall:
                        return Templates.NewMemberCommentBody;

                    case ContentType.Video:
                        return Templates.NewVideoCommentBody;

                    case ContentType.Blog:
                        return Templates.NewBlogCommentBody;

                    case ContentType.Photo:
                        return Templates.NewPhotoCommentBody;

                    case ContentType.AskAFriend:
                        return Templates.NewAAFCommentBody;
                }
            }
            return Templates.NewThreadCommentBody;
        }

        private static string GetSubject(bool rootComment, ContentType commentType)
        {
            if (rootComment)
            {
                switch (commentType)
                {
                    case ContentType.Wall:
                        return Templates.NewMemberCommentSubject;

                    case ContentType.Video:
                        return Templates.NewVideoCommentSubject;

                    case ContentType.Blog:
                        return Templates.NewBlogCommentSubject;

                    case ContentType.Photo:
                        return Templates.NewPhotoCommentSubject;

                    case ContentType.AskAFriend:
                        return Templates.NewAAFCommentSubject;
                }

            }
            return Templates.NewThreadCommentSubject;
        }

        private static bool Notify(DataRow row, ContentType type)
        {
            switch (type)
            {
                case ContentType.Wall:
                    return (bool)row["NotifyNewProfileComment"];

                case ContentType.Video:
                    return (bool)row["NotifyNewVideoComment"];

                case ContentType.ThreadReply:
                    return (bool)row["NotifyOnThreadReply"];

                case ContentType.Blog:
                    return true;

                case ContentType.Photo:
                    return (bool)row["NotifyNewPhotoComment"];

                case ContentType.AskAFriend:
                    return (bool)row["NotifyOnAAFComment"];
            }

            return false;
        }

        /// <summary>
        /// Get Information on the members that are to be notified
        /// </summary>
        /// <returns></returns>
        private static DataSet GetCommentTargetMembers(int ObjectID,ContentType type,int ThreadNo,SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("HG_GetCommentTargetMembersForTrigger", conn);
            
            SqlParameter param = new SqlParameter("@ObjectID", ObjectID);
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);


            param = new SqlParameter("@CommentType", (int)type);
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);


            param = new SqlParameter("@ThreadNo", ThreadNo);
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);

            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            return ds;
        }

        private static DataSet GetMemberSettingsByMemberID(int MemberID, SqlConnection conn)
        {
            SqlCommand command = null;

            command = new SqlCommand("AG_GetMemberSettingsByMemberID", conn);
            SqlParameter param = new SqlParameter("@MemberID", MemberID);
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);
            

            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            return ds;
        }

        


    }
}
