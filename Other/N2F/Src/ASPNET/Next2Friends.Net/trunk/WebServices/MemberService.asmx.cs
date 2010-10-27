using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Services;
using Next2Friends.Data;
using Next2Friends.WebServices.Properties;
using Next2Friends.Misc;
using System.IO;
using System.Diagnostics;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to retrieve member-related data.
    /// </summary>
    [WebService(Description = "Provides functionality to retrieve member-related data.", Name = "MemberService", Namespace = "http://www.next2friends.com"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class MemberService : WebService
    {
        /// <summary>
        /// Returns <code>true</code> if the user with the specified <code>nickname</code> and <code>password</code> exists; otherwise, returns <code>false</code>.
        /// </summary>
        [WebMethod(Description = "<p>Returns <tt>true</tt> if the user with the specified <tt>nickname</tt> and <tt>password</tt> exists; otherwise, returns <tt>false</tt>.</p><p><b>Returns:</b> </p>")]
        public Boolean CheckUserExists(String nickname, String password)
        {
            return Member.CheckUserExists(nickname, password);
        }

        /// <summary>
        /// Creates new Next2Friends member.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="nickname"></param>
        /// <param name="password"></param>
        /// <param name="gender">0 - Girl, 1 - Boy, 2 - Not saying.</param>
        /// <returns>
        ///  0 - If the operation completed successfully.
        /// -1 - If <code>emailAddress</code> is not valid.
        /// -2 - If <code>emailAddress</code> already registered.
        /// -3 - If <code>nickname</code> is invalid.
        /// -4 - If <code>nickname</code> already registered.
        /// -5 - <code>password</code> minimum length is 7 characters.
        /// -6 - <code>gender should be 0 - Girl, 1 - Boy, 2 - Not saying.</code>
        /// -7 - Error occured while saving new member to the database.
        /// -8 - Error occured during new member setup.
        /// </returns>
        [WebMethod(Description = "<p>Creates new Next2Friends member.</p><p><b>Parameters:</b><br /><tt>emailAddress</tt><br /><tt>nickname</tt><br /><tt>password</tt><br /><tt>gender</tt> - 0 - Girl, 1 - Boy, 2 - Not saying.</p><p><b>Returns: </b>0 - If the operation completed successfully.<br />-1 - If <tt>emailAddress</tt> is not valid.<br />-2 - If <tt>emailAddress</tt> already registered.<br />-3 - If <tt>nickname</tt> is invalid.<br />-4 - If <tt>nickname</tt> already registered.<br />-5 - <tt>password</tt> minimum length is 7 characters.<br />-6 - <tt>gender should be 0 - Girl, 1 - Boy, 2 - Not saying.</tt><br />-7 - Error occured while saving new member to the database.<br />-8 - Error occured during new member setup.</p>")]
        public Int32 CreateUser(String emailAddress, String nickname, String password, Int32 gender /* 0 - Girl, 1 - Boy, 2 - Not saying */)
        {
            if (String.IsNullOrEmpty(emailAddress)
                || !RegexPatterns.TestEmailRegex(emailAddress))
                return -1; // emailAddress is not valid.
            if (!Member.IsEmailAddressAvailable(emailAddress))
                return -2; // emailAddress already registered.
            if (String.IsNullOrEmpty(nickname)
                || !RegexPatterns.TestNickname(nickname))
                return -3; // nickname invalid.
            if (!Member.IsNicknameAvailable(nickname))
                return -4; // nickname already registered.
            if (String.IsNullOrEmpty(password)
                || !RegexPatterns.TestPassword(password))
                return -5; // password minimum length is 7 characters.
            if (gender < 0 || gender > 2)
                return -6; // gender should be 0 - Girl, 1 - Boy, 2 - Not saying.

            var member = new Member()
            {
                AccountType = 0, /* Default */
                Email = emailAddress,
                NickName = nickname,
                FirstName = nickname,
                Password = password,
                Gender = gender,
                ISOCountry = "UNS", /* Unspecified */
                ProfilePhotoResourceFileID = 1, /* Default */
                WebMemberID = UniqueID.NewWebID(),
                CreatedDT = DateTime.Now
            };

            try
            {
                member.Save();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return -7;
            }

            try
            {
                MemberSetUp(member);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return -8;
            }

            return 0;
        }

        /// <summary>
        /// Returns personal encryption key for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an emtpy string, or if the specified <code>password</code> is <code>null</code> or an emtpy string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns personal encryption key for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an emtpy string, or if the specified <tt>password</tt> is <tt>null</tt> or an emtpy string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public String GetEncryptionKey(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return Member.GetMemberViaNicknamePassword(nickname, password).Device[0].PrivateEncryptionKey;
        }

        /// <summary>
        /// Returns the unique identifier for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an emtpy string, or if the specified <code>password</code> is <code>null</code> or an emtpy string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns the unique identifier for the specified user.</p>"
            + "<b>Throws:</b>"
            + "<br/><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an emtpy string, or if the specified <tt>password</tt> is <tt>null</tt> or an emtpy string."
            + "<br/><tt>Exception</tt> - If the user with the specified credentials does not exist.")]
        public String GetMemberID(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return Member.GetMemberViaNicknamePassword(nickname, password).WebMemberID;
        }

        /// <summary>
        /// Returns the link to the profile photo for the user with the specified <code>nickname</code>.
        /// </summary>
        /// <returns>Link to the profile photo for the user with the specified <code>nickname</code>.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string.
        /// </exception>
        [WebMethod(Description = "<p>Returns the link to the profile photo for the user with the specified <tt>nickname</tt>.</p><p><b>Returns: </b>Link to the profile photo for the user with the specified <tt>nickname</tt>.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string.</p>")]
        public String GetProfilePhotoUrl(String nickname)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");

            var member = Member.GetMemberViaNickname(nickname);
            var profilePhotoURL = String.Empty;

            if (member != null)
            {
                var photoRes = new ResourceFile(member.ProfilePhotoResourceFileID);
                profilePhotoURL = "http://www.next2friends.com/" + photoRes.FullyQualifiedURL;
            }

            return profilePhotoURL;
        }

        /// <summary>
        /// Gets the status message for the specified user. If there is no status message empty string is returned.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Gets the status message for the specified user. If there is no status message empty string is returned.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public String GetMemberStatusText(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return Server.HtmlDecode(Member.GetMemberViaNicknamePassword(nickname, password).MemberStatusText[0].StatusText ?? "");
        }

        /// <summary>
        /// Sets the status message for the specified user. <code>statusText</code> can be <code>null</code> or an empty string.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Sets the status message for the specified user. <tt>statusText</tt> can be <tt>null</tt> or an empty string.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void SetMemberStatusText(String nickname, String password, String statusText)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);

            // TODO: New way to update member status.
            MemberStatusText.UpdateStatusText(
                member.MemberID,
                Server.HtmlEncode(statusText ?? ""));
            // TODO: Comment this to make it work new way only.
            member.MyMemberProfile.TagLine = Server.HtmlEncode(statusText ?? "");
            member.MyMemberProfile.Save();
        }

        /// <summary>
        /// Returns the unique user identifier that used during tagging process.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an emtpy string, or if the specified <code>password</code> is <code>null</code> or an emtpy string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns the unique user identifier that used during tagging process.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an emtpy string, or if the specified <tt>password</tt> is <tt>null</tt> or an emtpy string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public String GetTagID(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return Member.GetMemberViaNicknamePassword(nickname, password).Device[0].DeviceTagID;
        }

        /// <summary>
        /// Sends the password to the user with the specified email.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>emailAddress</code> is <code>null</code> or an emtpy string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the user with the specified <code>emailAddress</code> does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Sends the password to the user with the specified email.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>emailAddress</tt> is <tt>null</tt> or an emtpy string.<br /><tt>ArgumentException</tt> - If the user with the specified <tt>emailAddress</tt> does not exist.</p>")]
        public void RemindPassword(String emailAddress)
        {
            if (String.IsNullOrEmpty(emailAddress))
                throw new ArgumentNullException("emailAddress");

            var member = Member.GetMemberByEmail(emailAddress);
            if (member == null)
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.Argument_EmailAddressNotExist, emailAddress));

            ForgottenPassword.RemindPassword(member.MemberID, emailAddress);
        }

        private static void CreateUserDirectories(Member member)
        {
            var root = OSRegistry.GetDiskUserDirectory() + member.NickName;

            Directory.CreateDirectory(root);
            Directory.CreateDirectory(String.Concat(root, @"\plrge"));
            Directory.CreateDirectory(String.Concat(root, @"\pmed"));
            Directory.CreateDirectory(String.Concat(root, @"\pthmb"));

            Directory.CreateDirectory(String.Concat(root, @"\video"));
            Directory.CreateDirectory(String.Concat(root, @"\vthmb"));

            Directory.CreateDirectory(String.Concat(root, @"\aaflrge"));
            Directory.CreateDirectory(String.Concat(root, @"\aafthmb"));

            Directory.CreateDirectory(String.Concat(root, @"\nslrge"));
            Directory.CreateDirectory(String.Concat(root, @"\nsmed"));
            Directory.CreateDirectory(String.Concat(root, @"\nsthmb"));
        }

        private static void MemberSetUp(Member member)
        {
            CreateUserDirectories(member);

            var defaultGallery = new PhotoCollection()
            {
                WebPhotoCollectionID = UniqueID.NewWebID(),
                MemberID = member.MemberID,
                DTCreated = DateTime.Now,
                Name = member.NickName + "'s Gallery",
                Description = "My First Gallery!"
            };
            defaultGallery.Save();

            string StatusText = "New to next2Friends!";

            /* Create a new member profile for the member. */
            var profile = new MemberProfile()
            {
                MemberID = member.MemberID,
                DTLastUpdated = DateTime.Now,
                DefaultPhotoCollectionID = defaultGallery.PhotoCollectionID,
                TagLine = StatusText
            };
            profile.Save();

            MemberStatusText.UpdateStatusText(member.MemberID, StatusText);

            var message = new Message()
            {
                Body = "Welcome to Next2Friends",
                WebMessageID = UniqueID.NewWebID(),
                MemberIDFrom = 31,
                MemberIDTo = member.MemberID,
                DTCreated = DateTime.Now
            };
            message.Save();
            message.InReplyToID = message.MessageID;
            message.Save();

            /* Create the default settings for the member. */
            var settings = new MemberSettings()
            {
                NotifyNewPhotoComment = true,
                NotifyNewProfileComment = true,
                NotifyNewVideoComment = true,
                NotifyOnAAFComment = true,
                NotifyOnFriendRequest = true,
                NotifyOnNewMessage = true,
                NotifyOnNewsLetter = true,
                NotifyOnSubscriberEvent = true,
                MemberID = member.MemberID
            };
            settings.Save();

            var matchProfile = new MatchProfile()
            {
                MemberID = member.MemberID
            };
            matchProfile.Save();

            var device = new Device()
            {
                MemberID = member.MemberID,
                PrivateEncryptionKey = UniqueID.NewEncryptionKey(),
                CreatedDT = DateTime.Now,
                DeviceTagID = Guid.NewGuid().ToString()
            };
            device.Save();

            // Lawrence: Added block to register default friends and profile views
            try
            {
                // Add Lawrence as Auto Friend
                Friend.AddFriend(1, member.MemberID);
                // Add Anthony as Auto Friend
                Friend.AddFriend(3, member.MemberID);
                // Add Hans as Auto Friend
                Friend.AddFriend(24, member.MemberID);
                // Add Becca as Auto Friend
                Friend.AddFriend(30, member.MemberID);

                ContentViewed(new Member(1), member.MemberID, CommentType.Member);
                ContentViewed(new Member(3), member.MemberID, CommentType.Member);
                ContentViewed(new Member(24), member.MemberID, CommentType.Member);
                ContentViewed(new Member(30), member.MemberID, CommentType.Member);
            }
            catch { }

            SetOnlineNow(member.MemberID);
        }

        /// <summary>
        /// Lawrence: added this for the 1st release of social. This method needs to be refactored into data dll.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="ObjectID"></param>
        /// <param name="contentType"></param>
        private static void ContentViewed(Member member, int ObjectID, CommentType contentType)
        {
            if (member != null)
            {
                try
                {
                    DateTime dtNow = DateTime.Now;

                    ContentView contentView = new ContentView();
                    contentView.DTCreated = DateTime.Now;
                    contentView.MemberID = member.MemberID;
                    contentView.ObjectID = ObjectID;
                    contentView.ObjectType = (int)contentType;

                    contentView.SaveWithCheck();
                }
                catch { }
            }
        }

        private static void SetOnlineNow(Int32 memberId)
        {
            try
            {
                var now = new OnlineNow() { MemberID = memberId, DTOnline = DateTime.Now };
                now.Save();
            }
            catch { }
        }
    }
}
