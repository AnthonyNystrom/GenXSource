/* ------------------------------------------------
 * CommentService.asmx.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.ComponentModel;
using System.Web;
using System.Web.Services;
using Next2Friends.Data;
using Next2Friends.Misc;
using Next2Friends.WebServices.Comment;
using Next2Friends.WebServices.Properties;
using Next2Friends.WebServices.Utils;
using data = Next2Friends.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to add, edit, and remove comments.
    /// </summary>
    [WebService(
        Description = "Provides functionality to add, edit, and remove comments.",
        Name = "CommentService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class CommentService : WebService
    {
        /// <summary>
        /// Posts a new comment.
        /// Set <code>newComment.InReplyToCommentID</code> to <code>0</code> for first-level comments.
        /// Use the following values for <code>newComment.CommentType</code>:
        /// 
        /// None = 0,
        /// Video = 1,
        /// Photo = 2,
        /// Wall = 3,
        /// NSpot = 4,
        /// LiveBroadcast = 5,
        /// Blog = 6,
        /// AskAFriend = 7,
        /// PhotoGallery = 21,
        /// Member = 22.
        /// </summary>
        /// <returns>New comment identifier.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>newComment</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <code>CommentType</code> enumeration does not contain any member associated with the specified <code>newComment.CommentType</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Posts a new comment.<br />Set <tt>newComment.InReplyToCommentID</tt> to <tt>0</tt> for first-level comments.<br />Use the following values for <tt>newComment.CommentType</tt>:<br /><br />None = 0,<br />Video = 1,<br />Photo = 2,<br />Wall = 3,<br />NSpot = 4,<br />LiveBroadcast = 5,<br />Blog = 6,<br />AskAFriend = 7,<br />PhotoGallery = 21,<br />Member = 22.</p><p><b>Returns: </b>New comment identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>newComment</tt> is <tt>null</tt>.<br /><tt>ArgumentException</tt> - If <tt>CommentType</tt> enumeration does not contain any member associated with the specified <tt>newComment.CommentType</tt>.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public Int32 AddComment(String nickname, String password, WebComment newComment)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (newComment == null)
                throw new ArgumentNullException("newComment");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            if (!Enum.IsDefined(typeof(CommentType), newComment.CommentType))
                throw new ArgumentException(String.Format(Resources.Argument_InvalidCommentType, newComment.CommentType));
            var comment = CreateComment(newComment, member);
            MakeFriends(comment);
            MarkAsFovorite(comment);
            return comment.CommentID;
        }

        /// <summary>
        /// Updates the comment using <code>editedComment</code> data. Note, that you can edit your own comments only.
        /// Use the following values for <code>newComment.CommentType</code>:
        /// 
        /// None = 0,
        /// Video = 1,
        /// Photo = 2,
        /// Wall = 3,
        /// NSpot = 4,
        /// LiveBroadcast = 5,
        /// Blog = 6,
        /// AskAFriend = 7,
        /// PhotoGallery = 21,
        /// Member = 22.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>editedComment</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the identifier of the comment creator does not match the identifier of the user that is intending to edit this comment, or if <code>CommentType</code> enumeration does not contain any member with the specified <code>editedComment.CommentType</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        /// <exception cref="Exception">
        /// If the comment with the <code>editedComment.ID</code> does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Updates the comment using <tt>editedComment</tt> data. Note, that you can edit your own comments only.<br />Use the following values for <tt>newComment.CommentType</tt>:<br /><br />None = 0,<br />Video = 1,<br />Photo = 2,<br />Wall = 3,<br />NSpot = 4,<br />LiveBroadcast = 5,<br />Blog = 6,<br />AskAFriend = 7,<br />PhotoGallery = 21,<br />Member = 22.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>editedComment</tt> is <tt>null</tt>.<br /><tt>ArgumentException</tt> - If the identifier of the comment creator does not match the identifier of the user that is intending to edit this comment, or if <tt>CommentType</tt> enumeration does not contain any member with the specified <tt>editedComment.CommentType</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.<br /><tt>Exception</tt> - If the comment with the <tt>editedComment.ID</tt> does not exist.</p>")]
        public void EditComment(String nickname, String password, WebComment editedComment)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (editedComment == null)
                throw new ArgumentNullException("editedComment");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            if (!Enum.IsDefined(typeof(CommentType), editedComment.CommentType))
                throw new ArgumentException(String.Format(Resources.Argument_InvalidCommentType, editedComment.CommentType));
            var comment = new data.Comment(editedComment.ID);
            if (comment.MemberIDFrom != member.MemberID)
                throw new ArgumentException(Resources.Argument_InvalidCommentEditor);
            UpdateComment(comment, editedComment);
        }

        /// <summary>
        /// Retrieves the comment with the specified <code>commentID</code>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        /// <exception cref="Exception">
        /// If the comment with the <code>commentID</code> does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves the comment with the specified <tt>commentID</tt>.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.<br /><tt>Exception</tt> - If the comment with the <tt>commentID</tt> does not exist.</p>")]
        public WebComment GetComment(String nickname, String password, Int32 commentID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var comment = data.Comment.GetComment(commentID);
            return CreateComment(comment);
        }

        /// <summary>
        /// Retrieves identifiers of the comments that are newer than the comment with <code>lastCommentID</code>.
        /// Use the following values for <code>commentType</code>:
        /// 
        /// None = 0,
        /// Video = 1,
        /// Photo = 2,
        /// Wall = 3,
        /// NSpot = 4,
        /// LiveBroadcast = 5,
        /// Blog = 6,
        /// AskAFriend = 7,
        /// PhotoGallery = 21,
        /// Member = 22.
        /// </summary>
        /// <returns>An array of comment identifiers.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <code>CommentType</code> enumeration does not contain any member associated with the specified <code>commentType</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves identifiers of the comments that are newer than the comment with <tt>lastCommentID</tt>.<br />Use the following values for <tt>commentType</tt>:<br /><br />None = 0,<br />Video = 1,<br />Photo = 2,<br />Wall = 3,<br />NSpot = 4,<br />LiveBroadcast = 5,<br />Blog = 6,<br />AskAFriend = 7,<br />PhotoGallery = 21,<br />Member = 22.</p><p><b>Returns: </b>An array of comment identifiers.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If <tt>CommentType</tt> enumeration does not contain any member associated with the specified <tt>commentType</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public Int32[] GetCommentIDs(String nickname, String password, Int32 objectID, Int32 lastCommentID, Int32 commentType)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);

            if (Enum.IsDefined(typeof(CommentType), commentType))
                return data.Comment.GetCommentIDs(objectID, lastCommentID, (CommentType)commentType);
            else
                throw new ArgumentException(
                    String.Format(Properties.Resources.Argument_InvalidCommentType, typeof(CommentType), commentType));
        }

        /// <summary>
        /// Retrieves comments that are newer than the comment with <code>lastCommentID</code>.
        /// Use the following values for <code>commentType</code>:
        /// 
        /// None = 0,
        /// Video = 1,
        /// Photo = 2,
        /// Wall = 3,
        /// NSpot = 4,
        /// LiveBroadcast = 5,
        /// Blog = 6,
        /// AskAFriend = 7,
        /// PhotoGallery = 21,
        /// Member = 22.
        /// </summary>
        /// <returns>An array of comments.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <code>CommentType</code> enumeration does not contain any member associated with the specified <code>commentType</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves comments that are newer than the comment with <tt>lastCommentID</tt>.<br />Use the following values for <tt>commentType</tt>:<br /><br />None = 0,<br />Video = 1,<br />Photo = 2,<br />Wall = 3,<br />NSpot = 4,<br />LiveBroadcast = 5,<br />Blog = 6,<br />AskAFriend = 7,<br />PhotoGallery = 21,<br />Member = 22.</p><p><b>Returns: </b>An array of comments.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If <tt>CommentType</tt> enumeration does not contain any member associated with the specified <tt>commentType</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public WebComment[] GetComments(String nickname, String password, Int32 objectID, Int32 lastCommentID, Int32 commentType)
        {
            var comments = data.Comment.GetCommentByObjectIDWithJoin(objectID, (CommentType)commentType);
            var commentsLength = comments.Length;
            var result = new WebComment[commentsLength];

            for (var i = 0; i < commentsLength; i++)
                result[i] = CreateComment(comments[i]);

            return result;
        }

        /// <summary>
        /// Checks whether there are new comments.
        /// Use the following values for <code>commentType</code>:
        /// 
        /// None = 0,
        /// Video = 1,
        /// Photo = 2,
        /// Wall = 3,
        /// NSpot = 4,
        /// LiveBroadcast = 5,
        /// Blog = 6,
        /// AskAFriend = 7,
        /// PhotoGallery = 21,
        /// Member = 22.
        /// </summary>
        /// <returns><code>true</code> if there are comments newer than the comment with <code>lastCommentID</code>.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or
        /// if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <code>CommentType</code> enumeration does not contain any member associated with the specified <code>commentType</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Checks whether there are new comments.<br />Use the following values for <tt>commentType</tt>:<br /><br />None = 0,<br />Video = 1,<br />Photo = 2,<br />Wall = 3,<br />NSpot = 4,<br />LiveBroadcast = 5,<br />Blog = 6,<br />AskAFriend = 7,<br />PhotoGallery = 21,<br />Member = 22.</p><p><b>Returns: </b><tt>true</tt> if there are comments newer than the comment with <tt>lastCommentID</tt>.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or<br />if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If <tt>CommentType</tt> enumeration does not contain any member associated with the specified <tt>commentType</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public Boolean HasNewComments(String nickname, String password, Int32 objectID, Int32 lastCommentID, Int32 commentType)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);

            if (Enum.IsDefined(typeof(CommentType), commentType))
                return data.Comment.HasNewComments(objectID, lastCommentID, (CommentType)commentType);
            else
                throw new ArgumentException(
                    String.Format(Properties.Resources.Argument_InvalidCommentType, typeof(CommentType), commentType));
        }

        /// <summary>
        /// Removes the comment with the specified <code>commentID</code>. Note that you can remove your own comments only.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the identifier of the comment creator does not match the identifier of the user that is intending to remove this comment. 
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        /// <exception cref="Exception">
        /// If the comment with the specified <code>commentID</code> does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Removes the comment with the specified <tt>commentID</tt>. Note that you can remove your own comments only.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If the identifier of the comment creator does not match the identifier of the user that is intending to remove this comment.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.<br /><tt>Exception</tt> - If the comment with the specified <tt>commentID</tt> does not exist.</p>")]
        public void RemoveComment(String nickname, String password, Int32 commentID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var comment = new data.Comment(commentID);
            if (comment.MemberIDFrom != member.MemberID)
                throw new ArgumentException(Resources.Argument_InvalidCommentRemover);
            data.Comment.DeleteComment(comment.WebCommentID);
        }

        private static WebComment CreateComment(data.Comment comment)
        {
            return new WebComment()
            {
                CommentType = comment.CommentType,
                DTCreated = comment.DTCreated.ToTicksString(),
                ID = comment.CommentID,
                Nickname = new Member(comment.MemberIDFrom).NickName,
                Text = Text2Mobile.Filter(comment.Text ?? ""),
                InReplyToCommentID = comment.InReplyToCommentID,
                ObjectID = comment.ObjectID,
                ParentCommentID = ExtractParentCommentID(comment.Path)
            };
        }

        private static void UpdateComment(data.Comment commentToUpdate, WebComment comment)
        {
            commentToUpdate.Text = HttpUtility.HtmlEncode(comment.Text);
            commentToUpdate.Save();
        }

        private static Boolean CanBeFriends(Int32 memberId, Int32 friendId)
        {
            return (memberId != friendId) && !Friend.IsFriend(memberId, friendId);
        }

        private static void MakeFriends(data.Comment comment)
        {
            Debug.Assert(comment != null, "comment != null");

            if (comment.InReplyToCommentID == comment.CommentID) /* First-level comment. */
            {
                MakeFriends(comment.MemberIDFrom, GetPotentialFriendId(comment));
            }
            else /* 2nd-level comment. */
            {
                var parentComment = data.Comment.GetComment(comment.InReplyToCommentID);
                MakeFriends(comment.MemberIDFrom, parentComment.MemberIDFrom);
            }
        }

        private static void MakeFriends(Int32 memberId, Int32 friendId)
        {
            if (friendId == 0)
                return; /* No sense to make friends. */

            if (!CanBeFriends(memberId, friendId))
                return; /* No sense to make friends again. */

            Friend.AddFriend(memberId, friendId);
        }

        private static Int32 GetPotentialFriendId(data.Comment comment)
        {
            Debug.Assert(comment != null, "comment != null");

            switch (comment.CommentType)
            {
                case 1: /* Video */
                    return new data.Video(comment.ObjectID).MemberID;
                case 2: /* Photo */
                    return new data.Photo(comment.ObjectID).MemberID;
                case 3: /* Wall */
                    return comment.ObjectID;
                case 6: /* Blog */
                    return new data.BlogEntry(comment.ObjectID).MemberID;
                case 7: /* AskAFriend */
                    return new data.AskAFriend(comment.ObjectID).MemberID;
            }

            return 0;
        }

        private static data.Comment CreateComment(WebComment comment, Member member)
        {
            var inReplyToCommentID = comment.InReplyToCommentID;
            var inReplyToComment = inReplyToCommentID == 0 ? null : data.Comment.GetComment(inReplyToCommentID);

            var result = new data.Comment()
            {
                ObjectID = comment.ObjectID,
                MemberIDFrom = member.MemberID,
                Text = HttpUtility.HtmlEncode(comment.Text),
                DTCreated = DateTime.Now,
                IsDeleted = false,
                CommentType = comment.CommentType,
                WebCommentID = UniqueID.NewWebID(),
                Path = inReplyToComment == null ? "/" : String.Format("/{0}/", inReplyToComment.InReplyToCommentID),
                SentFromMobile = 1
            };
            result.Save();

            result.ThreadNo = inReplyToComment == null ? result.CommentID : inReplyToComment.ThreadNo;
            result.InReplyToCommentID = inReplyToCommentID == 0 ? result.CommentID : inReplyToCommentID;
            result.Save();

            try
            {
                IncrementCommentCount((CommentType)comment.CommentType, comment.ObjectID);
            }
            catch { }

            return result;
        }

        private static void IncrementCommentCount(CommentType type, int ObjID)
        {
            if (type == CommentType.Wall)
            {
                //Do Nothing
            }
            else if (type == CommentType.Video)
            {
                Next2Friends.Data.Video v = Next2Friends.Data.Video.GetVideoByVideoIDWithJoin(ObjID);
                v.NumberOfComments++;
                v.Save();
            }
            else if (type == CommentType.AskAFriend)
            {
                //Do Nothing
            }
            else if (type == CommentType.Blog)
            {
                //Do Nothing
            }
            else if (type == CommentType.Photo)
            {
                Next2Friends.Data.Photo p = Next2Friends.Data.Photo.GetPhotoByPhotoIDWithJoin(ObjID);
                p.NumberOfComments++;
                p.Save();
            }
        }

        private static Int32 ExtractParentCommentID(String path)
        {
            if (String.IsNullOrEmpty(path))
                return 0;
            var regex = new Regex(@"/(?<id>\d+)/");
            var idString = regex.Match(path).Groups["id"].Value;
            if (String.IsNullOrEmpty(idString))
                return 0;
            return Convert.ToInt32(idString);
        }

        private static void MarkAsFovorite(data.Comment comment)
        {
            Debug.Assert(comment != null, "comment != null");

            switch (comment.CommentType)
            {
                case 1:
                case 2:
                    {
                        var favorite = new Favourite()
                        {
                            MemberID = comment.MemberIDFrom,
                            TheFavouriteObjectID = comment.ObjectID,
                            ObjectType = comment.CommentType,
                            DTCreated = DateTime.Now
                        };
                        favorite.SaveWithCheck();
                        break;
                    }
            }
        }
    }
}
