using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace ExternalMessaging
{
    class MailHelper
    {
        public static void SendEmail( string to, string subject, string body)
        {
            // comment before deployment
            // For testing only
            //to = "testextmsg@next2friends.com ";

            SmtpClient s = new SmtpClient();
            s.Host = Parameters.SmtpHost;
            MailMessage m = new MailMessage();
            
            m.From = new MailAddress(Parameters.SmtpSender,Parameters.SmtpSenderName);
            m.To.Add(new MailAddress(to));
            
            m.Body = body;
            m.IsBodyHtml = true;            
            
            m.Subject = subject;
            s.Send(m);
        }

        public static void SendEmail(string to, string subject, string body,string attachment)
        {
            // comment before deployment
            // For testing only
            //to = "testextmsg@next2friends.com";

            SmtpClient s = new SmtpClient();
            s.Host = Parameters.SmtpHost;
            MailMessage m = new MailMessage();
            Attachment attch = new Attachment(attachment);
            
            m.Attachments.Add(attch);

            m.From = new MailAddress(Parameters.SmtpSender, Parameters.SmtpSenderName);
            m.To.Add(new MailAddress(to));

            m.Body = body;
            m.IsBodyHtml = true;

            m.Subject = subject;
            s.Send(m);
        }  
    }
}
