using System;
using System.ComponentModel;
using System.Web.Services;
using Next2Friends.Data;
using Next2Friends.WebServices.Tag;
using Next2Friends.WebServices.Utils;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to process proximity tags.
    /// </summary>
    [WebService(
        Description = "Provides functionality to process proximity tags.",
        Name = "TagService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class TagService : WebService
    {
        public const String Identifier = "TagService";

        /// <summary>
        /// Uploads the specified tag validation strings along with device identifiers.
        /// </summary>
        /// <returns>Returns an array of entities which denote whether the uploaded tags were confirmed by the server.</returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>tags</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Uploads the specified tag validation strings along with device identifiers.</p><p><b>Returns: </b>Returns an array of entities which denote whether the uploaded tags were confirmed by the server.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>tags</tt> is <tt>null</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public TagConfirmation[] UploadTags(String nickname, String password, TagUpdate tags)
        {
            if (String.IsNullOrEmpty(nickname))
            {
                Log.Logger("UpdateTags: nickname is null or an empty string.", Identifier);
                throw new ArgumentNullException("nickname");
            }

            if (String.IsNullOrEmpty(password))
            {
                Log.Logger("UpdateTags: password is null or an empty string.", Identifier);
                throw new ArgumentNullException("password");
            }
            
            if (tags == null)
            {
                Log.Logger("UpdateTags: tags is null.", Identifier);
                throw new ArgumentNullException("tags");
            }

            Log.Logger(String.Format("UpdateTags: nickname = \"{0}\".", nickname), Identifier);
            Log.Logger(String.Format("UpdateTags: password = \"{0}\".", password), Identifier);
            Log.Logger(String.Format("UpdateTags: tags.DeviceTagID.Length = {0}.", tags.DeviceTagID.Length));
            Log.Logger(String.Format("UpdateTags: tags.TagValidationString = {0}.", tags.TagValidationString.Length));
            Log.Logger(String.Format("UpdateTags: tags.DeviceTagID = {0}.", LogProcessor.ArrayToString(tags.DeviceTagID)), Identifier);
            Log.Logger(String.Format("UpdateTags: tags.TagValidationString = {0}.", LogProcessor.ArrayToString(tags.TagValidationString)), Identifier);

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var confirmationArray = new TagConfirmation[tags.CheckArraylength()];

            for (var i = 0; i < tags.CheckArraylength(); i++)
            {
                var deviceTagID = tags.DeviceTagID[i];
                // Device tag table needs to be added and this needs to be hooked up.
                var friendMember = Member.GetMemberAndDeviceByDeviceTagID(deviceTagID);
                var confirmedByServer = false;

                if (friendMember != null)
                {
                    var friendTag = new FriendTag()
                    {
                        FirstMemberID = member.MemberID,
                        SecondMemberID = friendMember.MemberID,
                        TaggedDT = DateTime.Now,
                        CreatedDT = DateTime.Now
                    };

                    friendTag.SaveWithFriendRequest();
                    confirmedByServer = true;
                    FriendRequest.CreateBluetoothFriendRequest(member.MemberID, friendMember.MemberID);
                }

                confirmationArray[i] = new TagConfirmation(deviceTagID, confirmedByServer);
            }

            return confirmationArray;
        }
    }
}
