using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class ContentInvite
    {
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewContentInvite", Target = "ContentInvite", Event = "FOR INSERT")]
        public static void NewContentInvite()
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
                            DataRow insertedRow = table.Rows[0];
                            DataColumnCollection columns = table.Columns;

                            int MemberID = (int)insertedRow["MemberID"];
                            int ObjectID = (int)insertedRow["ObjectID"];
                            string EmailAddress = (string)insertedRow["EmailAddress"];
                            ContentType contentType = (ContentType)((int)insertedRow["ObjectType"]);

                            // The object on which the invite was made along with the owner information
                            // Can be a Video, Photo, Nspot, Member etc...
                            DataSet objectDS = Utility.GetObjectByObjectIDWithJoin(ObjectID, contentType, connection);

                            Member sendingMember = null;

                            if (MemberID != 0)
                            {
                                sendingMember = Member.GetMemberByMemberID(MemberID, connection);
                            }
                            else
                            {
                                sendingMember = new Member();
                                sendingMember.FirstName = "Your friend";
                            }

                            string TargetEmailAddress = EmailAddress;

                            string subject = MergeHelper.MergeContentType(Templates.ContentInviteSubject, contentType);
                            string body = MergeHelper.MergeContentType(Templates.ContentInviteBody,contentType);

                            subject = Utility.PopulateLink(subject, contentType, objectDS.Tables[0].Rows[0]);
                            body = Utility.PopulateLink(body, contentType, objectDS.Tables[0].Rows[0]);

                            subject = MergeHelper.GenericMerge(subject, columns, insertedRow);
                            body = MergeHelper.GenericMerge(body, columns, insertedRow);

                            subject = MergeHelper.MergeMemberInfo(subject, sendingMember);
                            body = MergeHelper.MergeMemberInfo(body, sendingMember);

                            //body = MergeHelper.MergeBanner(body, "New " + contentType.ToString() + " Invite", connection);

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
