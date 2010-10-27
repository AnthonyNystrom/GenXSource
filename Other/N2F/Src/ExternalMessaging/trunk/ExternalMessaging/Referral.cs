using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Data;

namespace ExternalMessaging
{
    public enum ReferrerType { MemberID, CampaignID, ContactImportID }

    public class Referral
    {
        public static string Encrypt(ReferrerType RType, int ID, string ForwardURL)
        {
            string EncryptString = ((int)RType).ToString() + ":" + ID.ToString() + ":" + ForwardURL;

            EncryptString = RijndaelEncryption.Encrypt(EncryptString);

            //EncryptString = EncryptString.Substring(0, EncryptString.Length - 2);

            return EncryptString;
        }
    }
}
