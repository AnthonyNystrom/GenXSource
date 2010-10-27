using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Next2Friends.Misc
{
    /// <summary>
    /// Summary description for RegexPatterns
    /// </summary>
    public class RegexPatterns
    {
        public RegexPatterns()
        {

        }

        /// <summary>
        /// tests an email address for syntactical structure
        /// </summary>
        /// <param name="EmailAddress">The string to test</param>
        /// <returns>True if the string is a valid email address</returns>
        public static bool TestEmailRegex(string EmailAddress)
        {
            // this expression is a bit more lenient
            //string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //Regex reLenient = new Regex(patternLenient);

            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
               + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
               + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
               + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
               + @"[a-zA-Z]{2,}))$";

            Regex reStrict = new Regex(patternStrict);

            return reStrict.IsMatch(EmailAddress);
        }


        public static bool TestNickname(string Nickname)
        {
            if (Nickname.Length > 3 && Nickname.Length < 16)
            {
                string pattern = @"^[a-zA-Z0-9]+$";

                Regex reg = new Regex(pattern);

                return reg.IsMatch(Nickname);
            }
            else
            {
                return false;
            }
        }

        public static bool TestPassword(string Password)
        {
            if (Password.Length > 6 && Password.Length < 25)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string FormatHTMLTitle(string Title)
        {
            //Regex regex = new Regex(@"[^\w-\s]");

            string FinalTitle = System.Text.RegularExpressions.Regex.Replace(Title, @"[^\w-\s]", "");

            return FinalTitle.ToLower();
        }

        public static string FormatHTMLTitleAllowQuestionMark(string Title)
        {
            string FinalTitle = System.Text.RegularExpressions.Regex.Replace(Title, @"[^\w\s-?]", "");

            return FinalTitle.ToLower();
        }
        /// <summary>
        /// example: next2friends.com/video/this-is-the-name-of-the-video/
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public static string FormatStringForURL(string Title)
        {
            string FinalTitle = System.Text.RegularExpressions.Regex.Replace(Title, @"[^\w\s]", "");

            FinalTitle = FinalTitle.Replace(" ", "-");

            return FinalTitle.ToLower();
        }


    }

}