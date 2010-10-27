using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalMessaging
{
    class Templates
    {

        static Templates()
        {
            newMemberBody = Utility.GetTemplate("NewMember.html");
            forgottenPasswordBody = Utility.GetTemplate("ForgottenPassword.html");
            newMemberCommentBody = Utility.GetTemplate("NewMemberComment.html");
            newVideoCommentBody = Utility.GetTemplate("NewVideoComment.html");
            newPhotoCommentBody = Utility.GetTemplate("NewPhotoComment.html");
            newAAFCommentBody = Utility.GetTemplate("NewAAFComment.html");
            newMessageBody = Utility.GetTemplate("NewMessage.html");
            newMessageBody = Utility.GetTemplate("NewMessage.html");
            newExternalMessageBody = Utility.GetTemplate("NewExternalMessage.html");
            newFriendRequestBody = Utility.GetTemplate("NewFriendRequest.html");
            acceptedFriendRequestBody = Utility.GetTemplate("AcceptedFriendRequest.html");
            memberInviteBody = Utility.GetTemplate("NewMemberInvite.html");
            profileInviteBody = Utility.GetTemplate("NewProfileInvite.html");
            newThreadCommentBody = Utility.GetTemplate("NewThreadComment.html");
            newBlogCommentBody = Utility.GetTemplate("NewBlogComment.html");
            newSubscriberContentBody = Utility.GetTemplate("NewSubscriberContent.html");
            newFriendContentBody = Utility.GetTemplate("NewFriendContent.html");
            contentInviteBody = Utility.GetTemplate("NewContentInvite.html");
            newImportInviteBody = Utility.GetTemplate("ImportInvite.html");
            newStreamSelfBody = Utility.GetTemplate("NewStreamSelf.html");
            newPhotoSelfBody = Utility.GetTemplate("NewPhotoSelf.html");
        }

        #region ImportInvite

        private static readonly string newImportInviteBody = string.Empty;

        private static readonly string newImportInviteSubject = @"<#FirstName#> has invited you to Next2Friends";

        public static string NewImportInviteSubject
        {
            get { return newImportInviteSubject; }
        }

        public static string NewImportInviteBody
        {
            get { return newImportInviteBody; }
        }
        #endregion

        #region New SubscriberContent

        private static readonly string newSubscriberContentBody = string.Empty;

        private static readonly string newSubscriberContentSubject = @"New <#ContentType#> From <#OtherFirstName#>";

        public static string NewSubscriberContentSubject
        {
            get { return newSubscriberContentSubject; }
        }

        public static string NewSubscriberContentBody
        {
            get { return newSubscriberContentBody; }   
        }
        #endregion

        #region New Friend Content

        private static readonly string newFriendContentBody = string.Empty;

        private static readonly string newFriendContentSubject = @"New <#ContentType#> From <#OtherFirstName#>";

        public static string NewFriendContentSubject
        {
            get { return newFriendContentSubject; }
        }

        public static string NewFriendContentBody
        {
            get { return newFriendContentBody; }
        }
        #endregion

        #region MemberInvite

        private static readonly string memberInviteBody = String.Empty;

        private static readonly string memberInviteSubject = @"<#FirstName#> has invited you to Next2Friends";

        public static string MemberInviteSubject
        {
            get { return memberInviteSubject; }
        }

        public static string MemberInviteBody
        {
            get { return memberInviteBody; }
        }
        #endregion

        #region contentInvite

        private static readonly string contentInviteBody = String.Empty;

        private static readonly string contentInviteSubject = @"<#FirstName#> wants you to see this <#ContentType#> on Next2friends";

        public static string ContentInviteSubject
        {
            get { return contentInviteSubject; }
        }

        public static string ContentInviteBody
        {
            get { return contentInviteBody; }
        }
        #endregion

        #region ProfileInvite

        private static readonly string profileInviteBody = String.Empty;

        private static readonly string profileInviteSubject = @"Check out <#FirstName#>'s Next2Friends.com profile";

        public static string ProfileInviteSubject
        {
            get { return profileInviteSubject; }
        }

        public static string ProfileInviteBody
        {
            get { return profileInviteBody; }
        }
        #endregion

        #region New Member

        private static readonly string newMemberBody = string.Empty;

        private static readonly string newMemberSubject = @"Welcome to Next2Friends";

        public static string NewMemberSubject
        {
            get { return newMemberSubject; }
        }

        public static string NewMemberBody
        {
            get { return newMemberBody; }   
        }
        #endregion

        #region Forgotten Password

        private static readonly string forgottenPasswordBody = String.Empty;
        private static readonly string forgottenPasswordSubject = @"Your Next2Friends password";

        public static string ForgottenPasswordSubject
        {
            get { return forgottenPasswordSubject; }
        }

        public static string ForgottenPasswordBody
        {
            get { return forgottenPasswordBody; }
        }
        #endregion

        #region New Message

        private static readonly string newMessageBody = String.Empty;

        private static readonly string newMessageSubject = @"New Message From <#OtherFirstName#>";

        public static string NewMessageSubject
        {
            get { return newMessageSubject; }
        }

        public static string NewMessageBody
        {
            get { return newMessageBody; }
        }
        #endregion

        #region New External Video Message

        private static readonly string newExternalMessageBody = String.Empty;

        private static readonly string newExternalMessageSubject = @"New Message From <#OtherFirstName#>";

        public static string NewExternalMessageSubject
        {
            get { return newExternalMessageSubject; }
        }

        public static string NewExternalMessageBody
        {
            get { return newExternalMessageBody; }
        }
        #endregion

        #region Thread Comment

        private static readonly string newThreadCommentBody = string.Empty;

        private static readonly string newThreadCommentSubject = @"New Comment from <#OtherFirstName#>";

        public static string NewThreadCommentSubject
        {
            get { return newThreadCommentSubject; }
        }

        public static string NewThreadCommentBody
        {
            get { return newThreadCommentBody; }
        }
        #endregion

        #region Member Comment

        private static readonly string newMemberCommentBody = string.Empty;

        private static readonly string newMemberCommentSubject = @"New Profile Comment from <#OtherFirstName#>";

        public static string NewMemberCommentSubject
        {
            get { return newMemberCommentSubject; }
        }

        public static string NewMemberCommentBody
        {
            get { return newMemberCommentBody; }
        }
        #endregion

        #region Video Comment

        private static readonly string newVideoCommentBody = String.Empty;

        private static readonly string newVideoCommentSubject = @"Comment on your video from <#OtherFirstName#>";

        public static string NewVideoCommentSubject
        {
            get { return newVideoCommentSubject; }
        }

        public static string NewVideoCommentBody
        {
            get { return newVideoCommentBody; }
        }
        #endregion

        #region Blog Comment

        private static readonly string newBlogCommentBody = String.Empty;

        private static readonly string newBlogCommentSubject = @"Comment on your Blog from <#OtherFirstName#>";

        public static string NewBlogCommentSubject
        {
            get { return newBlogCommentSubject; }
        }

        public static string NewBlogCommentBody
        {
            get { return newBlogCommentBody; }
        }
        #endregion

        #region Photo Comment

        private static readonly string newPhotoCommentBody = string.Empty;

        private static readonly string newPhotoCommentSubject = @"Comment on your photo from <#OtherFirstName#>";

        public static string NewPhotoCommentSubject
        {
            get { return newPhotoCommentSubject; }
        }

        public static string NewPhotoCommentBody
        {
            get { return newPhotoCommentBody; }
        }
        #endregion

        #region AskAFriend Comment

        private static readonly string newAAFCommentBody = string.Empty;

        private static readonly string newAAFCommentSubject = @"New Ask-A-Friend Comment";

        public static string NewAAFCommentSubject
        {
            get { return newAAFCommentSubject; }
        }

        public static string NewAAFCommentBody
        {
            get { return newAAFCommentBody; }
        }
        #endregion

        #region New Video

        private static readonly string newVideoBody = @"Dear <#FirstName#> , <#LastName#>
                                             New Video";

        private static readonly string newVideoSubject = @"<#FirstName#>, Video";

        public static string NewVideoSubject
        {
            get { return newVideoSubject; }
        }

        public static string NewVideoBody
        {
            get { return newVideoBody; }
        }
        #endregion

        #region New FriendRequest

        private static readonly string newFriendRequestBody = String.Empty;

        private static readonly string newFriendRequestSubject = @"<#OtherFirstName#> has requested to be your friend";

        public static string NewFriendRequestSubject
        {
            get { return newFriendRequestSubject; }
        }

        public static string NewFriendRequestBody
        {
            get { return newFriendRequestBody; }
        }
        #endregion

        #region Accepted FriendRequest

        private static readonly string acceptedFriendRequestBody = String.Empty;

        private static readonly string acceptedFriendRequestSubject = @"<#OtherFirstName#> has accepted your friend request";

        public static string AcceptedFriendRequestSubject
        {
            get { return acceptedFriendRequestSubject; }
        }

        public static string AcceptedFriendRequestBody
        {
            get { return acceptedFriendRequestBody; }
        }
        #endregion

        #region New LiveBroadcast

        private static readonly string newLiveBroadcastBody = @"Dear <#FirstName#> , <#LastName#>
                                             You got a LiveBroadcast from <#OtherFirstName#>,<#OtherLastName#>";

        private static readonly string newLiveBroadcastSubject = @"<#FirstName#>, LiveBroadcast";

        public static string NewLiveBroadcastSubject
        {
            get { return newLiveBroadcastSubject; }
        }

        public static string NewLiveBroadcastBody
        {
            get { return newLiveBroadcastBody; }
        }
        #endregion

        #region New StreamSelf

        private static readonly string newStreamSelfBody = string.Empty;

        private static readonly string newStreamSelfSubject = @"Downloadable file of your recent stream";

        public static string NewStreamSelfSubject
        {
            get { return newStreamSelfSubject; }
        }

        public static string NewStreamSelfBody
        {
            get { return newStreamSelfBody; }
        }
        #endregion

        #region New PhotoSelf

        private static readonly string newPhotoSelfBody = string.Empty;

        private static readonly string newPhotoSelfSubject = @"Attached is your recently snapped Photo";

        public static string NewPhotoSelfSubject
        {
            get { return newPhotoSelfSubject; }
        }

        public static string NewPhotoSelfBody
        {
            get { return newPhotoSelfBody; }
        }
        #endregion
    }
}
