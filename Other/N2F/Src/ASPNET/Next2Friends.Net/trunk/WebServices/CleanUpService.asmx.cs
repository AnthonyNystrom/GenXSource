using System;
using System.ComponentModel;
using System.Web.Services;
using Next2Friends.Data;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Deletes "foo bar" items from the site.
    /// </summary>
    [WebService(
        Description = "Deletes \"foo bar\" items from the Next2Friends database.",
        Name = "CleanUpService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class CleanUpService : WebService
    {
        /// <summary>
        /// Deletes the specified Ask question.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>webAskQuestionID</code> is <code>null</code> or an empty string.
        /// </exception>
        [WebMethod(Description = "<p>Deletes the specified Ask question.</p>"
            + "<b>Throws:</b>"
            + "<br/><tt>ArgumentNullException</tt> - If the specified <tt>webAskQuestionID</tt> is <tt>null</tt> or an empty string.")]
        public void DeleteAskQuestion(String webAskQuestionID)
        {
            if (String.IsNullOrEmpty(webAskQuestionID))
                throw new ArgumentNullException("webAskQuestionID");
            AskAFriend.Delete(webAskQuestionID);
        }

        /// <summary>
        /// Deletes all Ask questions for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">If the user the specified credentials does not exist.</exception>
        [WebMethod(Description = "<p>Deletes all Ask questions for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user the specified credentials does not exist.</p>")]
        public void DeleteAllUserAskQuestions(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            foreach (var item in AskAFriend.GetAAFQuestionByMemberIDSinceLastIDWithJoin(Member.GetMemberViaNicknamePassword(nickname, password).MemberID, null))
                DeleteAskQuestion(item.WebAskAFriendID);
        }

        /// <summary>
        /// Deletes log entries for the specified <code>webServiceName</code>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>webServiceName</code> is <code>null</code> or an empty string.
        /// </exception>
        [WebMethod(Description = "<p>Deletes log entries for the specified <tt>webServiceName</tt>.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>webServiceName</tt> is <tt>null</tt> or an empty string.</p>")]
        public Int32 DeleteLog(String webServiceName)
        {
            if (String.IsNullOrEmpty(webServiceName))
                throw new ArgumentNullException("webServiceName");
            return Log.Delete(webServiceName);
        }
    }
}
