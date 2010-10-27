using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;


namespace ExternalMessaging
{
    public class NewSubscriberContent
    {

        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewBlog_Subscribers", Target = "BlogEntry", Event = "FOR INSERT")]
        public static void NewBlogSubscribers()
        {
            SqlTriggerContext triggContext = SqlContext.TriggerContext;
            SqlCommand command = null;
            DataSet insertedDS = null;
            SqlDataAdapter dataAdapter = null;

            try
            {
                // Retrieve the connection that the trigger is using
                using (SqlConnection connection
                   = new SqlConnection(@"context connection=true"))
                {
                    connection.Open();

                    command = new SqlCommand(@"SELECT * FROM INSERTED;",
                       connection);

                    dataAdapter = new SqlDataAdapter(command);
                    insertedDS = new DataSet();
                    dataAdapter.Fill(insertedDS);

                    TriggerHandler(connection, ContentType.Blog, insertedDS);
                }
            }
            catch { }
            finally
            {
                try
                {

                    if (command != null)
                    {
                        command.Dispose();
                        command = null;
                    }

                    if (dataAdapter != null)
                    {
                        dataAdapter.Dispose();
                        dataAdapter = null;
                    }

                    if (dataAdapter != null)
                    {
                        dataAdapter.Dispose();
                        dataAdapter = null;
                    }

                    if (insertedDS != null)
                    {
                        insertedDS.Dispose();
                        insertedDS = null;
                    }
                }
                catch { }
            }
        }

        //Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewVideoContent", Target = "Video", Event = "FOR UPDATE")]
        public static void NewVideo()
        {   
            SqlTriggerContext triggContext = SqlContext.TriggerContext;
            SqlCommand command = null;
            SqlCommand command2 = null;

            DataSet updatedDS = null;
            DataSet deletedDS = null;

            SqlDataAdapter dataAdapter = null;
            SqlDataAdapter dataAdapter2 = null;

            try
            {
                // Retrieve the connection that the trigger is using
                using (SqlConnection connection
                   = new SqlConnection(@"context connection=true"))
                {                    
                    connection.Open();

                    command = new SqlCommand(@"SELECT * FROM INSERTED;",
                       connection);

                    command2 = new SqlCommand(@"SELECT * FROM DELETED;",
                       connection);

                    dataAdapter = new SqlDataAdapter(command);
                    updatedDS = new DataSet();
                    dataAdapter.Fill(updatedDS);

                    dataAdapter2 = new SqlDataAdapter(command2);
                    deletedDS = new DataSet();
                    dataAdapter2.Fill(deletedDS);
                    
                    DataRow updatedRow = updatedDS.Tables[0].Rows[0];
                    DataRow deletedRow = deletedDS.Tables[0].Rows[0];

                    if ((int)updatedRow["status"] == 0 && (int)deletedRow["status"] != 0)
                    {
                        if ((int)updatedRow["LiveBroadcastID"] > 0)
                        {
                            TriggerHandler(connection, ContentType.MobileVideo, updatedDS);
                        }
                        else
                        {
                            TriggerHandler(connection, ContentType.Video, updatedDS);
                        }
                    }
                }
            }
            catch { }
            finally
            {
                try
                {

                    if (command != null)
                    {
                        command.Dispose();
                        command = null;
                    }

                    if (command2 != null)
                    {
                        command2.Dispose();
                        command2 = null;
                    }

                    if (dataAdapter != null)
                    {
                        dataAdapter.Dispose();
                        dataAdapter = null;
                    }

                    if (dataAdapter2 != null)
                    {
                        dataAdapter2.Dispose();
                        dataAdapter2 = null;
                    }

                    if (dataAdapter != null)
                    {
                        dataAdapter.Dispose();
                        dataAdapter = null;
                    }

                    if (deletedDS != null)
                    {
                        deletedDS.Dispose();
                        deletedDS = null;
                    }


                    if (updatedDS != null)
                    {
                        updatedDS.Dispose();
                        updatedDS = null;
                    }
                }
                catch { }
            }
        }

        private static void TriggerHandler(SqlConnection connection,ContentType contentType,DataSet ds)
        {
            DataSet subscribingMembersDS = null;

            try
            {
                if(connection.State == ConnectionState.Closed )
                    connection.Open();
                
                //Populate data from the inserted row
                DataRow insertedRow = ds.Tables[0].Rows[0];
                DataColumnCollection insertedColumns = ds.Tables[0].Columns;

                int OwnerMemberID = (int)insertedRow["MemberID"];
                Member sendingMember = Member.GetMemberByMemberID(OwnerMemberID, connection);

                subscribingMembersDS = GetSubscribingMembers(OwnerMemberID, connection);
                DataTable subscribingMembers = subscribingMembersDS.Tables[0];

                foreach (DataRow row in subscribingMembers.Rows)
                {
                    MemberSettings ms = Member.GetMemberSettingsByMemberID((int)row["MemberID"], connection);

                    // In case we don't find any settings
                    if (ms == null)
                        continue;

                    // If the user doesn't want to be notified return
                    if (!ms.NotifyOnSubscriberEvent)
                        continue;

                    string TargetEmailAddress = (string)row["Email"];

                    string subject = Templates.NewSubscriberContentSubject;
                    string body = Templates.NewSubscriberContentBody;

                    subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
                    body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

                    subject = MergeHelper.GenericMerge(subject, row.Table.Columns, row);
                    body = MergeHelper.GenericMerge(body, row.Table.Columns, row);

                    subject = MergeHelper.MergeOtherMemberInfo(subject, sendingMember);
                    body = MergeHelper.MergeOtherMemberInfo(body, sendingMember);

                    subject = PopulateContentType(subject, contentType);
                    body = PopulateContentType(body, contentType);
                    body = PopulateLink(body, contentType, insertedRow,sendingMember);

                    try
                    {
                        MailHelper.SendEmail(TargetEmailAddress, subject, body);
                    }
                    catch{}
                }
            }
            catch { }
            finally
            {
                try
                {   if (ds != null)
                    {
                        ds.Dispose();
                        ds = null;
                    }
                 
                    if (subscribingMembersDS != null)
                    {
                        subscribingMembersDS.Dispose();
                        subscribingMembersDS = null;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Get Information on the members that are to be notified
        /// </summary>
        /// <returns></returns>
        private static DataSet GetSubscribingMembers(int MemberID,SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("HG_GetSubscriptionMembersByMemberIDForTrigger", conn);

            SqlParameter param = new SqlParameter("@MemberID", MemberID);
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);

            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            return ds;
        }

        private static string PopulateContentType(string inputString,ContentType contentType)
        {
            return inputString.Replace("<#ContentType#>",contentType.ToString());
        }

        private static string PopulateLink(string inputString, ContentType contentType, DataRow row,Member ownerMember)
        {
            string QueryString = string.Empty;

            switch (contentType)
            {
                case ContentType.Video:
                    inputString = inputString.Replace("<#PageLink#>", "video/" + Utility.FormatStringForURL((string)row["Title"]) + "/" + (string)row["WebVideoID"]);
                    //QueryString = "v=" + (string)row["WebVideoID"];
                    break;

                case ContentType.Blog:
                    inputString = inputString.Replace("<#PageLink#>", "Blog.aspx");
                    QueryString = "?m=" + ownerMember.WebMemberID + "&b=" + (string)row["WebBlogEntryID"];
                    break;

            }

            return inputString.Replace("<#QueryString#>", QueryString);
        }
    }
}
