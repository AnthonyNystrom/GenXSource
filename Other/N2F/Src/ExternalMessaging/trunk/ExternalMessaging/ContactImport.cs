using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public class ContactImport
    {
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "UpdateContactImport", Target = "ContactImport", Event = "FOR UPDATE")]
        public static void NewImportInvite()
        {
            SqlCommand command = null;
            SqlCommand command2 = null;
            SqlDataAdapter dataAdapter = null;
            SqlDataAdapter dataAdapter2 = null;
            DataSet ds = null;
            DataSet dsDel = null;

            //try
            {
                SqlTriggerContext triggContext = SqlContext.TriggerContext;

                switch (triggContext.TriggerAction)
                {
                    case TriggerAction.Update:
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

                            if (((int)deletedRow["InviteState"] == (int)updatedRow["InviteState"]) || ((int)updatedRow["InviteState"] != 2))
                            {
                                return;
                            }                           

                            int MemberID = (int)updatedRow["ImporterMemberID"];

                            string TargetEmailAddress = (string)updatedRow["Email"];

                            Member sendingMember = Member.GetMemberByMemberID(MemberID, connection);



                            string ReferralString1 = InsertInviteClick(updatedRow, sendingMember, "/signup",connection);//Referral.Encrypt(ReferrerType.ContactImportID, (int)updatedRow["ContactImportID"], "/signup");
                            string ReferralString2 = InsertInviteClick(updatedRow, sendingMember, "/users/" + sendingMember.NickName, connection);//Referral.Encrypt(ReferrerType.ContactImportID, (int)updatedRow["ContactImportID"], "/users/" + sendingMember.NickName);

                            //ReferralString1 = ReferralString1.Replace(@"/", "xxyyxx12345");
                            //ReferralString1 = ReferralString1.Replace(@"\", "12345xxyyxx");

                            //ReferralString2 = ReferralString2.Replace(@"/", "xxyyxx12345");
                            //ReferralString2 = ReferralString2.Replace(@"\", "12345xxyyxx");


                            string subject = Templates.NewImportInviteSubject;
                            string body = Templates.NewImportInviteBody;

                            subject = MergeHelper.GenericMerge(subject, updatedRow.Table.Columns, updatedRow);
                            body = MergeHelper.GenericMerge(body, updatedRow.Table.Columns, updatedRow);

                            subject = MergeHelper.MergeMemberInfo(subject, sendingMember);
                            body = MergeHelper.MergeMemberInfo(body, sendingMember);

                            body = MergeHelper.MergeReferral(body, 1, Utility.UrlEncode(ReferralString1));
                            body = MergeHelper.MergeReferral(body,2, Utility.UrlEncode(ReferralString2));

                            MailHelper.SendEmail(TargetEmailAddress, subject, body);
                        }

                        break;
                }
            }
            //catch { }
            //finally
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

        private static string InsertInviteClick(DataRow updatedRow,Member sendingMember,string forwardURL,SqlConnection conn)
        {
            String insertSQL = @"INSERT INTO InviteClick
                                                                        (
                                                                            WebInviteClickID,
                                                                            ForwardURL,
                                                                            ContactImportID
                                                                        )
                                                                        VALUES
                                                                        (
                                                                        '{0}',
                                                                        '{1}',
                                                                        {2}
                                                                        )";

            StringBuilder sbinsertSQL = new StringBuilder();

            object[] param = new object[3];
            param[0] = Utility.NewWebID();
            param[1] = forwardURL;
            param[2] = (int)updatedRow["ContactImportID"];

            sbinsertSQL.AppendFormat(insertSQL,param);
            SqlCommand insertCommand = new SqlCommand(sbinsertSQL.ToString(),conn);
            insertCommand.ExecuteNonQuery();

            return (string)param[0];
        }
    }
}
