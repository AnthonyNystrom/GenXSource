using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Next2Friends.Data
{
    public enum ReferrerType { MemberID, CampaignID,ContactImportID }
    /// <summary>
    /// Summary description for Referr
    /// </summary>
    public partial class Referral
    {
        public string ForwardURL { get; set; }


        /// <summary>
        /// call this when a user lands on the Referal Page
        /// </summary>
        /// <param name="context"></param>
        public static bool ProcessReferal(HttpContext context)
        {
            string DecryptString = (string)context.Items["encryptedparams"];

            DecryptString = DecryptString.Replace("xxyyxx12345", @"/");
            DecryptString = DecryptString.Replace("12345xxyyxx", @"\");

            //DecryptString = System.Web.HttpUtility.UrlDecode(DecryptString);
            Referral Refer = null;

            //try
            //{
            DecryptString = RijndaelEncryption.Decrypt(DecryptString);
            string[] Parts = DecryptString.Split(new char[] { ':' });

            ReferrerType RType = (ReferrerType)Int32.Parse(Parts[0]);
            int ID = Int32.Parse(Parts[1]);
            string ForwardURL = Parts[2];

            Refer = new Referral();
            Refer.ReferralType = (int)RType;

            if (Refer.ReferralType == (int)ReferrerType.CampaignID)
            {
                Refer.CampaignID = ID;
                Refer.ReferrerMemberID = 0;
                Refer.ContactImportID = 0;
            }
            else if (Refer.ReferralType == (int)ReferrerType.MemberID)
            {
                Refer.CampaignID = 0;
                Refer.ReferrerMemberID = ID;
                Refer.ContactImportID = 0;
            }
            else if (Refer.ReferralType == (int)ReferrerType.ContactImportID)
            {
                Refer.CampaignID = 0;
                Refer.ReferrerMemberID = 0;
                Refer.ContactImportID = ID;
            }

            Refer.Save();

            Refer.ForwardURL = ForwardURL;

            context.Session["Referral"] = Refer;
            context.Response.Redirect(Refer.ForwardURL);

            return true;
            //}
            //catch { }

            //return false;
        }

        /// <summary>
        /// call this when a user lands on the Referal Page
        /// </summary>
        /// <param name="context"></param>
        public static bool ProcessReferalFromInviteClickID(HttpContext context)
        {
            try
            {
                string WebInviteClickID = (string)context.Items["encryptedparams"];

                InviteClick inviteClick = InviteClick.GetInviteClickByWebInviteClickID(WebInviteClickID);

                Referral Refer = new Referral();

                Refer.ContactImportID = inviteClick.ContactImportID;
                Refer.ForwardURL = inviteClick.ForwardURL;
                Refer.ReferralType = (int)ReferrerType.ContactImportID;

                Refer.Save();

                ContactImport contactImport = new ContactImport(inviteClick.ContactImportID);
                contactImport.ClickedEmailInvite = true;
                contactImport.Save();

                context.Session["Referral"] = Refer;
                context.Response.Redirect(Refer.ForwardURL);

                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// once the user has signed up, this methods determines if the user is a referal and updates the Referral table
        /// </summary>
        /// <param name="context"></param>
        /// <param name="MemberID"></param>
        public static int ProcessSignupFromReferral(HttpContext context, int MemberID)
        {
            try
            {
                Referral Refer = (Referral)context.Session["Referral"];

                if (Refer != null)
                {
                    Refer.BecameMemberID = MemberID;
                    Refer.Save();

                    if (Refer.ReferralType == (int)ReferrerType.ContactImportID)
                    {
                        ContactImport contactImport = new ContactImport(Refer.ContactImportID);
                        contactImport.BecameMemberID = MemberID;
                        contactImport.Save();

                        return Refer.ContactImportID;
                    }
                }
            }
            catch { }

            return 0;
        }

        /// <summary>
        /// once the user has signed up, this methods determines if the user is a referal and updates the Referral table
        /// </summary>
        /// <param name="context"></param>
        /// <param name="MemberID"></param>
        public static string  GetEmailReferrer(HttpContext context)
        {
            try
            {
                Referral Refer = (Referral)context.Session["Referral"];

                if (Refer != null)
                {
                    ContactImport contactImport = new ContactImport(Refer.ContactImportID);

                    if (contactImport != null)
                    {
                        Member ReferrerMember = new Member(contactImport.ImporterMemberID);

                        return ReferrerMember.Email;
                    }

                }
            }
            catch { }

            return string.Empty;
        }


        /// <summary>
        /// Obselete Method...
        /// </summary>
        /// <param name="RType"></param>
        /// <param name="ID"></param>
        /// <param name="ForwardURL"></param>
        /// <returns></returns>
        public static string Encrypt(ReferrerType RType, int ID, string ForwardURL)
        {
            string EncryptString = ((int)RType).ToString() + ":" + ID.ToString() + ":" + ForwardURL;

            EncryptString = RijndaelEncryption.Encrypt(EncryptString);

            //EncryptString = EncryptString.Substring(0, EncryptString.Length - 2);

            //EncryptString = System.Web.HttpUtility.UrlEncodeUnicode(EncryptString);

            return EncryptString;
        }
    }
}