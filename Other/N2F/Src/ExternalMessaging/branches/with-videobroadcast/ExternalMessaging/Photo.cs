using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;


namespace ExternalMessaging
{
    public class Photo
    {
        public static DataSet GetPhotoInformation(int PhotoID,SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("AG_GetPhotoByPhotoIDWithJoin", conn);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter("@PhotoID", PhotoID);
            param.DbType = DbType.Int32;
            param.Direction = ParameterDirection.Input;

            command.Parameters.Add(param);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            try
            {
                dataAdapter.Dispose();
                command.Dispose();                
            }
            catch { }

            return ds;
        }


        /// <summary>
        /// Gets the Photo in the database With a full join with all the manually specified tables in SP code 
        /// </summary>
        public static DataSet GetPhotoByWebPhotoIDWithJoin(string WebPhotoID, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand("HG_GetPhotoByWebPhotoIDWithJoin", conn);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter("@WebPhotoID", WebPhotoID);
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

        //Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewPhoto_Self", Target = "Photo", Event = "FOR INSERT")]
        public static void NewPhotoSelf()
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

                    DataRow insertedRow = insertedDS.Tables[0].Rows[0];

                    if ((bool)insertedRow["Mobile"] == true)
                    {
                        TriggerHandler(connection, ContentType.Photo, insertedDS);
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

                    if (insertedDS != null)
                    {
                        insertedDS.Dispose();
                        insertedDS = null;
                    }
                }
                catch { }
            }
        }

        private static void TriggerHandler(SqlConnection connection, ContentType contentType, DataSet ds)
        {
            //Populate data from the inserted row
            DataRow insertedRow = ds.Tables[0].Rows[0];
            DataColumnCollection insertedColumns = ds.Tables[0].Columns;

            int OwnerMemberID = (int)insertedRow["MemberID"];

            string WebPhotoID = (string)insertedRow["WebPhotoID"];
            DataSet PhotoDS = GetPhotoByWebPhotoIDWithJoin(WebPhotoID, connection);

            DataRow PhotoDSRow = PhotoDS.Tables[0].Rows[0];
            DataColumnCollection PhotoDSColumns = PhotoDS.Tables[0].Columns;

            Member ownerMember = Member.GetMemberByMemberID(OwnerMemberID, connection);
            MemberSettings ms = Member.GetMemberSettingsByMemberID(OwnerMemberID, connection);

            // In case we don't find any settings
            if (ms == null)
                return;

            string TargetEmailAddress = ownerMember.Email;

            string subject = Templates.NewPhotoSelfSubject;
            string body = Templates.NewPhotoSelfBody;

            subject = MergeHelper.GenericMerge(subject, insertedColumns, insertedRow);
            body = MergeHelper.GenericMerge(body, insertedColumns, insertedRow);

            subject = MergeHelper.GenericMerge(subject, PhotoDSColumns, PhotoDSRow);
            body = MergeHelper.GenericMerge(body, PhotoDSColumns, PhotoDSRow);

            subject = MergeHelper.MergeMemberInfo(subject, ownerMember);
            body = MergeHelper.MergeMemberInfo(body, ownerMember);

            try
            {
                MailHelper.SendEmail(TargetEmailAddress, subject, body, @"\\www\Live\user\" + ownerMember.NickName + @"\pmed\" + PhotoDSRow["PhotoResourceFileFileName"]);
            }
            catch { }
        }
    }
}
