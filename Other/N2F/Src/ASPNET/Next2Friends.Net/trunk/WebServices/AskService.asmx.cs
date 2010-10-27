using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.Services;
using Next2Friends.Data;
using Next2Friends.Misc;
using Next2Friends.WebServices.Ask;
using Next2Friends.WebServices.Properties;
using Next2Friends.WebServices.Utils;
using data = Next2Friends.Data;
using System.Collections.Generic;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to create questions with photos attached, retrieve responses as well as private questions.
    /// </summary>
    [WebService(
        Description = "Provides functionality to create questions with photos attached, retrieve responses as well as private questions.",
        Name = "AskService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class AskService : WebService
    {
        public const String Identifier = "AskService";

        /// <summary>
        /// Call this method to start a question. Only once the <code>numberOfPhotos</code> has equal value to the number of attached photos can <code>CompleteQuestion</code> method be called.
        /// </summary>
        /// <param name="numberOfPhotos">Specifies the total number of photos to be submitted.</param>
        /// <param name="responseType">Specifies the type of question to ask. <code>0</code> = YesNo, <code>1</code> = AB, <code>2</code> = RateTo10, <code>3</code> = MultipleSelect.</param>
        /// <param name="durationType">Specifies how long the question is open for. <code>0</code> = 3 min, <code>1</code> = 15 min, <code>2</code> = 60 min, <code>3</code> = 1 day.</param>
        /// <param name="isPrivate">Specifies the value indicating whether the question is intended for friends only.</param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>questionText</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Call this method to start a question. Only once the <tt>numberOfPhotos</tt> has equal value to the number of attached photos can <tt>CompleteQuestion</tt> method be called.</p><p><b>Parameters:</b><br /><tt>numberOfPhotos</tt> - Specifies the total number of photos to be submitted.<br /><tt>responseType</tt> - Specifies the type of question to ask. <tt>0</tt> = YesNo, <tt>1</tt> = AB, <tt>2</tt> = RateTo10, <tt>3</tt> = MultipleSelect.<br /><tt>durationType</tt> - Specifies how long the question is open for. <tt>0</tt> = 3 min, <tt>1</tt> = 15 min, <tt>2</tt> = 60 min, <tt>3</tt> = 1 day.<br /><tt>isPrivate</tt> - Specifies the value indicating whether the question is intended for friends only.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>questionText</tt> is <tt>null</tt>.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public AskQuestionConfirm SubmitQuestion(
            String nickname, String password,
            String questionText,
            Int32 numberOfPhotos,
            Int32 responseType, String[] customResponses,
            Int32 durationType,
            Boolean isPrivate)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(questionText))
                throw new ArgumentNullException("questionText");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var aaf = new AskAFriend()
            {
                WebAskAFriendID = Misc.UniqueID.NewWebID(),
                MemberID = member.MemberID,
                RejectScore = 10,
                Question = Server.HtmlEncode(questionText),
                NumberOfPhotos = numberOfPhotos,
                ResponseType = responseType,
                Active = false,
                Duration = durationType,
                IsPrivate = isPrivate,
                SubmittedIP = HttpContext.Current.Request.UserHostAddress
            };

            if (customResponses != null)
            {
                aaf.ResponseA = (customResponses.Length > 0) ? Server.HtmlEncode(customResponses[0]) : String.Empty;
                aaf.ResponseB = (customResponses.Length > 1) ? Server.HtmlEncode(customResponses[1]) : String.Empty;
            }

            aaf.Save();

            var result = new AskQuestionConfirm()
            {
                AskQuestionID = aaf.WebAskAFriendID,
                AdvertURL = @"http://www.google.co.uk/images/firefox/tshirt.gif",
                AdvertImage = ""
            };

            return result;
        }

        /// <summary>
        /// Attaches a single photo to an Ask Question.
        /// </summary>
        /// <param name="askQuestionID">Specifies the identifier of the question to attach the photo to.</param>
        /// <param name="indexOrder">First photo - <code>1</code>, Second photo - <code>2</code>, Third photo - <code>3</code>.</param>
        /// <exception cref="ArgumentException">
        /// If the specified <code>askQuestionID</code> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>askQuestionID</code> is <code>null</code> or an empty string, or if the specified <code>photoBase64String</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Attaches a single photo to an Ask Question.</p><p><b>Parameters:</b><br /><tt>askQuestionID</tt> - Specifies the identifier of the question to attach the photo to.<br /><tt>indexOrder</tt> - First photo - <tt>1</tt>, Second photo - <tt>2</tt>, Third photo - <tt>3</tt>.</p><p><b>Throws:</b><br /><tt>ArgumentException</tt> - If the specified <tt>askQuestionID</tt> is invalid.<br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>askQuestionID</tt> is <tt>null</tt> or an empty string, or if the specified <tt>photoBase64String</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public void AttachPhoto(String nickname, String password, String askQuestionID, Int32 indexOrder, String photoBase64String)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(askQuestionID))
                throw new ArgumentNullException("askQuestionID");
            if (String.IsNullOrEmpty(photoBase64String))
                throw new ArgumentNullException("photoBase64String");
            if (indexOrder < 1 || indexOrder > 3)
                throw new ArgumentException(Resources.Argument_InvalidIndexOrder);

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var aaf = AskAFriend.GetAskAFriendByWebAskAFriendID(member.MemberID, askQuestionID);

            if (aaf == null)
                throw new ArgumentException(
                    String.Format(CultureInfo.CurrentCulture, Resources.Argument_InvalidAskQuestionID, askQuestionID));

            data.Photo.ProcessAAFPhoto(member, aaf, photoBase64String, indexOrder);
            aaf.Save();
        }

        /// <summary>
        /// Completes an Ask question and promotes it to live.
        /// </summary>
        /// <param name="askQuestionID">Specifies the identifier of the question that has been completed.</param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>askQuestionID</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// At least 2 photos should be attached to the MultipleSelect question and at least 1 photo should be attached to the questions of other types.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Completes an Ask question and promotes it to live.</p><p><b>Parameters:</b><br /><tt>askQuestionID</tt> - Specifies the identifier of the question that has been completed.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>askQuestionID</tt> is <tt>null</tt>.<br /><tt>ArgumentException</tt> - At least 2 photos should be attached to the MultipleSelect question and at least 1 photo should be attached to the questions of other types.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public void CompleteQuestion(String nickname, String password, String askQuestionID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(askQuestionID))
                throw new ArgumentNullException("askQuestionID");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var memberID = member.MemberID;
            var aaf = AskAFriend.GetAskAFriendByWebAskAFriendID(askQuestionID);
            var makeLive = false;

            if (aaf.ResponseType == (Int32)AskResponseType.MultipleSelect)
            {
                // Must have at least 2 photos.
                if (aaf.Photo.Count > 1)
                    makeLive = true;
                else
                    throw new ArgumentException(Resources.Argument_InvalidAskMultipleSelectPhotoCount);
            }
            else
            {
                if (aaf.Photo.Count > 0)
                    makeLive = true;
                else
                    throw new ArgumentException(Resources.Argument_InvalidAskPhotoCount);
            }

            if (makeLive)
            {
                aaf.WentLiveDT = DateTime.Now;
                aaf.SubmittedDT = DateTime.Now;
                aaf.Active = true;

                aaf.Save();
            }
        }

        /// <summary>
        /// Retrieves question-related data according to the specified identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the question with the specified <code>questionID</code> does not exist.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves question-related data according to the specified identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If the question with the specified <tt>questionID</tt> does not exist.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public AskQuestion GetQuestion(String nickname, String password, Int32 questionID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var askAFriend = AskAFriend.GetAskQuestion(questionID);
            if (askAFriend == null)
                throw new ArgumentException(String.Format(Resources.Argument_AskQuestionNotExist, questionID));

            return CreateQuestion(askAFriend);
        }

        /// <summary>
        /// Returns a list of the latest 10 question identifiers for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of the latest 10 question identifiers for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public Int32[] GetQuestionIDs(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return AskAFriend.GetAskQuestionIDs(Member.GetMemberViaNicknamePassword(nickname, password).MemberID);
        }

        /// <summary>
        /// Returns a list of the latest 10 questions for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of the latest 10 questions for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public AskQuestion[] GetQuestions(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var ids = AskAFriend.GetAskQuestionIDs(Member.GetMemberViaNicknamePassword(nickname, password).MemberID);
            var idsLength = ids.Length;
            var result = new AskQuestion[idsLength];

            for (var i = 0; i < idsLength; i++)
            {
                var question = AskAFriend.GetAskQuestion(ids[i]);
                if (question == null)
                    throw new ArgumentException(String.Format(Resources.Argument_AskQuestionNotExist, ids[i]));
                result[i] = CreateQuestion(question);
            }

            return result;
        }

        /// <summary>
        /// Gets the response for the question with the specified identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or
        /// if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Gets the response for the question with the specified identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or<br />if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public AskResponse GetResponse(String nickname, String password, Int32 questionID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var question = AskAFriend.GetAskAFriendByAskAFriendIDWithJoin(questionID);

            var response = new AskResponse()
            {
                AskQuestionID = question.AskAFriendID,
                Question = Text2Mobile.Filter(question.Question),
                PhotoBase64Binary = ResourceProcessor.GetThumbnailBase64String(question.DefaultPhotoResourceFile.SavePath),
                ResponseValues = AskAFriend.GetAskAFriendResult(question).ToArray(),
                ResponseType = question.ResponseType,
                Average = 0,
                CustomResponses = new String[2]
            };

            /* Set the average value (only applicable if the ResponseType is Rate1To10). */

            for (var i = 0; i < response.ResponseValues.Length; i++)
                response.Average += response.ResponseValues[i];

            response.Average = response.Average / Convert.ToDouble(response.ResponseValues.Length);

            /* Custom responses will be blank strings if not applicable. */

            response.CustomResponses[0] = Text2Mobile.Filter(question.ResponseA);
            response.CustomResponses[1] = Text2Mobile.Filter(question.ResponseB);

            return response;
        }

        /// <summary>
        /// Skips the question and marks it as skipped in the database.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Skips the question and marks it as skipped in the database.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public void SkipQuestion(String nickname, String password, Int32 questionID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            AskAFriend.SkipAskQuestion(questionID);
        }

        /// <summary>
        /// Votes for the question with the specified identifier.
        /// <code>result</code> value should be appropriate to the <code>ResponseType</code> of the question.
        /// If the type of the response is <code>YesNo</code>, <code>1</code> = <code>Yes</code>, <code>2</code> = <code>No</code>.
        /// If the type of the response is <code>AB</code>, <code>1</code> = <code>A</code>, <code>2</code> = <code>B</code>.
        /// If the type of the response is <code>Rate1To10</code>, <code>result</code> should be an integer from 1 to 10.
        /// If the type of the response is <code>MultipleSelect</code>, <code>result</code> should identify the choice that is take values from 1 to 3.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Votes for the question with the specified identifier.<br /><tt>result</tt> value should be appropriate to the <tt>ResponseType</tt> of the question.<br />If the type of the response is <tt>YesNo</tt>, <tt>1</tt> = <tt>Yes</tt>, <tt>2</tt> = <tt>No</tt>.<br />If the type of the response is <tt>AB</tt>, <tt>1</tt> = <tt>A</tt>, <tt>2</tt> = <tt>B</tt>.<br />If the type of the response is <tt>Rate1To10</tt>, <tt>result</tt> should be an integer from 1 to 10.<br />If the type of the response is <tt>MultipleSelect</tt>, <tt>result</tt> should identify the choice that is take values from 1 to 3.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public void VoteForQuestion(String nickname, String password, Int32 questionID, Int32 result)
        {
            if (String.IsNullOrEmpty(nickname))
            {
                Log.Logger("VoteForQuestion: nickname is null or empty.", Identifier);
                throw new ArgumentNullException("nickname");
            }
            if (String.IsNullOrEmpty(password))
            {
                Log.Logger("VoteForQuestion: password is null or empty.", Identifier);
                throw new ArgumentNullException("password");
            }

            Log.Logger(String.Format("VoteForQuestion: nickname = \"{0}\".", nickname), Identifier);
            Log.Logger(String.Format("VoteForQuestion: password = \"{0}\".", password), Identifier);
            Log.Logger(String.Format("VoteForQuestion: questionID = \"{0}\".", questionID), Identifier);
            Log.Logger(String.Format("VoteForQuestion: result = \"{0}\".", result), Identifier);

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            AskAFriend.VoteForAskQuestion(member.MemberID, questionID, result);
        }

        private static AskQuestion CreateQuestion(AskAFriend askAFriend)
        {
            return new AskQuestion()
            {
                ID = askAFriend.AskAFriendID,
                Question = Text2Mobile.Filter(askAFriend.Question),
                DTCreated = askAFriend.SubmittedDT.ToTicksString()
            };
        }
    }
}
