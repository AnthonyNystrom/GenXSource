using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using ExternalMessaging.Twitter;

namespace ExternalMessaging
{
    /// <summary>
    /// UPD [12/01/2008] [Alex Nesterov]:
    /// Added VideoID and WebVideoID properties.
    /// Added PopulateObject and GetVideoByVideoID static methods.
    /// Added VideoAdded trigger that cross-posts the video link to Twitter.
    /// </summary>
    public class Video
    {
        public Int32 VideoID { get; set; }
        public String WebVideoID { get; set; }

        public static List<Video> PopulateObject(IDataReader dr)
        {
            var arr = new List<Video>();

            Video obj;

            while (dr.Read())
            {
                obj = new Video();
                obj.VideoID = (Int32)dr["VideoID"];
                obj.WebVideoID = (String)dr["WebVideoID"];

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        public static Video GetVideoByVideoID(Int32 VideoID, SqlConnection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            var command = new SqlCommand("AG_GetVideoByVideoID", conn) { CommandType = CommandType.StoredProcedure };
            var param = new SqlParameter("@VideoID", VideoID) { DbType = DbType.Int32 };
            command.Parameters.Add(param);

            List<Video> arr = null;
            using (var dr = command.ExecuteReader())
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else return null;
        }

        /// <summary>
        /// </summary>
        /// <author>Alex Nesterov</author>
        [SqlTrigger(Name = "VideoAddedTrigger", Target = "Video", Event = "FOR INSERT")]
        public static void VideoAdded()
        {
            SqlContext.Pipe.Send("VideoAdded.");

            SqlCommand command = null;
            SqlDataAdapter dataAdapter = null;
            DataSet ds = null;

            try
            {
                var triggContext = SqlContext.TriggerContext;

                switch (triggContext.TriggerAction)
                {
                    case TriggerAction.Insert:
                        SqlContext.Pipe.Send("TriggerAction.Insert");

                        /* Retrieve the connection that the trigger is using. */
                        using (var connection = new SqlConnection(@"context connection=true"))
                        {
                            connection.Open();

                            command = new SqlCommand(@"SELECT * FROM INSERTED;", connection);
                            dataAdapter = new SqlDataAdapter(command);
                            ds = new DataSet();

                            dataAdapter.Fill(ds);

                            var table = ds.Tables[0];
                            var row = table.Rows[0];

                            var columns = table.Columns;

                            var videoId = (Int32)row["VideoID"];
                            var memberId = (Int32)row["MemberID"];

                            SqlContext.Pipe.Send(String.Format("VideoID = {0}", videoId));
                            SqlContext.Pipe.Send(String.Format("MemberID = {0}", memberId));

                            var targetAccount = MemberAccount.GetMemberAccountByMemberID(memberId, connection);

                            if (MemberAccount.IsTwitterReady(targetAccount))
                            {
                                var video = Video.GetVideoByVideoID(videoId, connection);
                                var member = Member.GetMemberByMemberID(memberId, connection);

                                SqlContext.Pipe.Send("Target account is Twitter ready.");
                                TwitterService.NotifyVideoUploaded(targetAccount.Username, targetAccount.Password, member.NickName, video.WebVideoID);
                                SqlContext.Pipe.Send("Twitter status updated successfully.");
                            }
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send(String.Format("VideoAdded trigger exception: {0}", e.Message));
            }
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

        //Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewVideo_Self", Target = "Video", Event = "FOR INSERT")]
        public static void NewVideoSelf()
        {
            SqlTriggerContext triggContext = SqlContext.TriggerContext;
            SqlCommand command = null;
            DataSet updatedDS = null;

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
                    updatedDS = new DataSet();
                    dataAdapter.Fill(updatedDS);

                    DataRow insertedRow = updatedDS.Tables[0].Rows[0];

                    if ((int)insertedRow["LiveBroadcastID"] > 0)
                    {

                        TriggerHandler(connection, ContentType.Video, updatedDS);
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

                    if (updatedDS != null)
                    {
                        updatedDS.Dispose();
                        updatedDS = null;
                    }
                }
                catch { }
            }
        }

        public static DataSet GetVideoInformation(int VideoID,SqlConnection conn)
        {   
            SqlCommand command = new SqlCommand("AG_GetVideoByVideoID", conn);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter("@VideoID", VideoID);
            param.DbType = DbType.Int32;
            param.Direction = ParameterDirection.Input;

            command.Parameters.Add(param);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            return ds;
        }

        public static DataSet GetVideoByWebVideoIDWithJoin(string WebVideoID, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("HG_GetVideoByWebVideoIDWithJoin", conn);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter("@WebVideoID", WebVideoID);
            param.DbType = DbType.String;
            param.Direction = ParameterDirection.Input;

            command.Parameters.Add(param);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            try
            {
                command.Dispose();
            }
            catch { }

            try
            {
                dataAdapter.Dispose();
            }
            catch { }

            return ds;
        }

        private static void TriggerHandler(SqlConnection connection, ContentType contentType, DataSet ds)
        {
            //Populate data from the inserted row
            DataRow insertedRow = ds.Tables[0].Rows[0];
            DataColumnCollection insertedColumns = ds.Tables[0].Columns;

            int OwnerMemberID = (int)insertedRow["MemberID"];

            string WebVideoID = (string)insertedRow["WebVideoID"];
            DataSet VideoDS = GetVideoByWebVideoIDWithJoin(WebVideoID,connection);

            DataRow videoDSRow = VideoDS.Tables[0].Rows[0];
            DataColumnCollection videoDSColumns = VideoDS.Tables[0].Columns;

            Member ownerMember = Member.GetMemberByMemberID(OwnerMemberID, connection);
            MemberSettings ms = Member.GetMemberSettingsByMemberID(OwnerMemberID, connection);

            // In case we don't find any settings
            if (ms == null)
                return;
           
            string TargetEmailAddress = ownerMember.Email;

            string subject = Templates.NewStreamSelfSubject;
            string body = Templates.NewStreamSelfBody;

            subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
            body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

            subject = MergeHelper.GenericMerge(subject, videoDSColumns, videoDSRow);
            body = MergeHelper.GenericMerge(body, videoDSColumns, videoDSRow);

            subject = MergeHelper.MergeOtherMemberInfo(subject, ownerMember);
            body = MergeHelper.MergeOtherMemberInfo(body, ownerMember);

            try
            {
                MailHelper.SendEmail(TargetEmailAddress, subject, body);
            }
            catch { }                       
        }
    }
}
