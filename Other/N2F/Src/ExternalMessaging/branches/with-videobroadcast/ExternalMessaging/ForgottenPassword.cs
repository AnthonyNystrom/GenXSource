using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
namespace ExternalMessaging
{
    public class ForgottenPassword
    {
        // Enter existing table or view for the target and uncomment the attribute line
        [Microsoft.SqlServer.Server.SqlTrigger(Name = "NewForgottenPassword", Target = "ForgottenPassword", Event = "FOR INSERT")]
        public static void NewForgottenPassword()
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

                            DataTable table = ds.Tables[0];
                            DataRow row = table.Rows[0];

                            DataColumnCollection columns = table.Columns;

                            string TargetEmailAddress = (string)row["EmailAddress"];
                            int MemberID = (int)row["MemberID"];

                            Member targetMember = Member.GetMemberByMemberID(MemberID, connection);

                            string subject = Templates.ForgottenPasswordSubject;
                            string body = Templates.ForgottenPasswordBody;

                            subject = MergeHelper.GenericMerge(subject, columns, row);
                            body = MergeHelper.GenericMerge(body, columns, row);

                            subject = MergeHelper.MergeMemberInfo(subject, targetMember);
                            body = MergeHelper.MergeMemberInfo(body, targetMember);

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
