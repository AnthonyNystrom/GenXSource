using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web.Services;
using Next2Friends.Data;
using Next2Friends.Misc;
using Next2Friends.WebServices.Properties;
using Next2Friends.WebServices.SnapUp;
using Next2Friends.WebServices.Twitter;
using data = Next2Friends.Data;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to upload images to the user's gallery.
    /// </summary>
    [WebService(
        Description = "Provides functionality to upload images to the Next2Friends server.",
        Name = "SnapUpService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public sealed class SnapUpService : WebService
    {
        public const String Identifier = "SnapUpService";

        /// <summary>
        /// Creates a new collection for the specified member.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String, or if the specified <code>collectionName</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <code>collectionName</code> length exceeds 50 characters.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(
            Description = "<p>Creates a new collection for the specified member.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or if the specified <tt>collectionName</tt> is <tt>null</tt> or an empty String.<br /><tt>ArgumentException</tt> - If <tt>collectionName</tt> length exceeds 50 characters.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void CreateCollection(String nickname, String password, String collectionName)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException("collectionName");
            if (collectionName.Length > 50)
                throw new ArgumentException(Resources.Argument_LongCollectionName);

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var photoCollection = new PhotoCollection()
            {
                MemberID = member.MemberID,
                WebPhotoCollectionID = UniqueID.NewWebID(),
                Name = collectionName
            };
            photoCollection.Save();
        }

        /// <summary>
        /// Sets the photo with the specified <code>webPhotoID</code> to inactive.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String, or if the specified <code>webPhotoID</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Sets the photo with the specified <tt>webPhotoID</tt> to inactive.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or if the specified <tt>webPhotoID</tt> is <tt>null</tt> or an empty String.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void DeletePhoto(String nickname, String password, String webPhotoID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(webPhotoID))
                throw new ArgumentNullException("webPhotoID");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            // Delete the photo from disk.
            var photo = data.Photo.GetPhotoByWebPhotoIDWithJoin(webPhotoID);
            photo.Active = false;
            photo.Save();
        }

        /// <summary>
        /// Uploads the specified image to the default user gallery. Intended to be used from a handheld device.
        /// <code>dateTime</code> is expected to be a String representing ticks in Microsoft .NET Framework.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String, or if the specified <code>base64StringPhoto</code> is <code>null</code> or an empty String, or if the specified <code>dateTime</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <code>base64StringPhoto</code> is not a valid image encoded as a Base64 String.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Uploads the specified image to the default user gallery. Intended to be used from a handheld device.<br /><tt>dateTime</tt> is expected to be a String representing ticks in Microsoft .NET Framework.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or if the specified <tt>base64StringPhoto</tt> is <tt>null</tt> or an empty String, or if the specified <tt>dateTime</tt> is <tt>null</tt> or an empty String.<br /><tt>ArgumentException</tt> - If the specified <tt>base64StringPhoto</tt> is not a valid image encoded as a Base64 String.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void DeviceUploadPhoto(String nickname, String password, String base64StringPhoto, String dateTime)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(base64StringPhoto))
                throw new ArgumentNullException("base64StringPhoto");
            if (String.IsNullOrEmpty(dateTime))
                throw new ArgumentNullException("dateTime");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            PhotoCollection photoCollection = PhotoCollection.GetOrCreatePhotoGalleryForToday(member.MemberID);
            UploadPhoto(member, photoCollection.WebPhotoCollectionID, base64StringPhoto, dateTime, true);
        }

        /// <summary>
        /// Gets all the photo collections for the specified member.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Gets all the photo collections for the specified member.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public PhotoCollectionItem[] GetCollections(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var photoCollection = member.PhotoCollection;

            var result = new PhotoCollectionItem[photoCollection.Count];

            for (var i = 0; i < result.Length; i++)
                result[i] = new PhotoCollectionItem() { WebPhotoCollectionID = photoCollection[i].WebPhotoCollectionID, Name = photoCollection[i].Name };

            return result;
        }

        [WebMethod]
        public String[] GetMember(String lastActivity)
        {
            String[] ret = LoggedIn.GetLoggedInUserPass(lastActivity);
            return ret;
        }

        private const String _baseUrl = "http://www.next2friends.com/";

        /// <summary>
        /// Gets all the photos for the specified member inside the specified photo collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String, or if the specified <code>webPhotoCollectionID</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Gets all the photos for the specified member inside the specified photo collection.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or if the specified <tt>webPhotoCollectionID</tt> is <tt>null</tt> or an empty String.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public PhotoItem[] GetPhotosByCollection(String nickname, String password, String webPhotoCollectionID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(webPhotoCollectionID))
                throw new ArgumentNullException("webPhotoCollectionID");

            try
            {
                var member = Member.GetMemberViaNicknamePassword(nickname, password);
                var photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(webPhotoCollectionID);
                var photoCollectionID = photoCollection.PhotoCollectionID;
                var photos = data.Photo.GetPhotoByPhotoCollectionIDWithJoin(photoCollectionID);

                var result = new PhotoItem[photos.Length];

                for (var i = 0; i < photos.Length; i++)
                {
                    var item = new PhotoItem();
                    item.WebPhotoID = photos[i].WebPhotoID;

                    if (photos[i].PhotoResourceFile != null)
                        item.MainPhotoURL = _baseUrl + photos[i].PhotoResourceFile.FullyQualifiedURL;
                    if (photos[i].ThumbnailResourceFile != null)
                        item.ThumbnailURL = _baseUrl + photos[i].ThumbnailResourceFile.FullyQualifiedURL;

                    result[i] = item;
                }

                return result;
            }
            catch (Exception e)
            {
                Log.Logger(String.Format(CultureInfo.InvariantCulture, "GetPhotosByCollection exception: {0}", e.Message), Identifier);
                Trace.Tracer(e.ToString());
                throw e;
            }
        }

        /// <summary>
        /// Uploads the specified image to the specified user gallery. Intended to be used from an applet.
        /// <code>dateTime</code> is expected to be a String representing ticks in Microsoft .NET Framework.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>encryptedWebMemberID</code> is <code>null</code> or an empty String, or if the specified <code>webPhotoCollectionID</code> is <code>null</code> or an empty String, or if the specified <code>base64StringPhoto</code> is <code>null</code> or an empty String, or if the specified <code>dateTime</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <code>base64StringPhoto</code> is not a valid image encoded as a Base64 String.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Uploads the specified image to the specified user gallery. Intended to be used from an applet.<br /><tt>dateTime</tt> is expected to be a String representing ticks in Microsoft .NET Framework.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>encryptedWebMemberID</tt> is <tt>null</tt> or an empty String, or if the specified <tt>webPhotoCollectionID</tt> is <tt>null</tt> or an empty String, or if the specified <tt>base64StringPhoto</tt> is <tt>null</tt> or an empty String, or if the specified <tt>dateTime</tt> is <tt>null</tt> or an empty String.<br /><tt>ArgumentException</tt> - If the specified <tt>base64StringPhoto</tt> is not a valid image encoded as a Base64 String.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void JavaUploadPhoto(String encryptedWebMemberID, String webPhotoCollectionID, String base64StringPhoto, String dateTime)
        {
            if (String.IsNullOrEmpty(encryptedWebMemberID))
                throw new ArgumentNullException("encryptedWebMemberID");
            if (String.IsNullOrEmpty(webPhotoCollectionID))
                throw new ArgumentNullException("webPhotoCollectionID");
            if (String.IsNullOrEmpty(base64StringPhoto))
                throw new ArgumentNullException("base64StringPhoto");
            if (String.IsNullOrEmpty(dateTime))
                throw new ArgumentNullException("dateTime");

            Member member = null;

            try
            {
                var webMemberID = RijndaelEncryption.Decrypt(encryptedWebMemberID);
                member = Member.GetMemberViaWebMemberID(webMemberID);
            }
            catch (Exception e)
            {
                Log.Logger(String.Format("JavaUploadPhoto exception: {0}", e.Message), Identifier);
                Trace.Tracer(e.ToString());
                throw e;
            }

            UploadPhoto(member, webPhotoCollectionID, base64StringPhoto, dateTime, false);
        }

        /// <summary>
        /// Renames the specified photo collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String, or if the specified <code>webPhotoCollectionID</code> is <code>null</code> or an empty String, or if the specified <code>newName</code> is <code>null</code> or an empty String.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified member cannot modify the photo collection with the specified <code>webPhotoCollectionID</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Renames the specified photo collection.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or if the specified <tt>webPhotoCollectionID</tt> is <tt>null</tt> or an empty String, or if the specified <tt>newName</tt> is <tt>null</tt> or an empty String.<br /><tt>ArgumentException</tt> - If the specified member cannot modify the photo collection with the specified <tt>webPhotoCollectionID</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void RenameCollection(String nickname, String password, String webPhotoCollectionID, String newName)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(webPhotoCollectionID))
                throw new ArgumentNullException("webPhotoCollectionID");
            if (String.IsNullOrEmpty(newName))
                throw new ArgumentNullException("newName");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var collection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(webPhotoCollectionID);

            if (collection.MemberID != member.MemberID)
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, Resources.Argument_InvalidPhotoCollectionModifier, webPhotoCollectionID));
            collection.Name = newName;
            collection.Save();
        }

        /// <summary>
        /// Uploads the photo to the specified collection.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty String, or if the specified <code>password</code> is <code>null</code> or an empty String, or if the specified <code>photoFileBytes</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Uploads the photo to the specified collection.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty String, or if the specified <tt>password</tt> is <tt>null</tt> or an empty String, or if the specified <tt>photoFileBytes</tt> is <tt>null</tt>.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public void UploadPhoto(String nickname, String password, String webPhotoCollectionID, Byte[] photoFileBytes, DateTime takenDT)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (photoFileBytes == null)
                throw new ArgumentNullException("photoFileBytes");

            try
            {
                var member = Member.GetMemberViaNicknamePassword(nickname, password);
                var photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(webPhotoCollectionID);            
                data.Photo.ProcessMemberPhoto(member, photoCollection.PhotoCollectionID, photoFileBytes, takenDT, true);
                NotifyPhotoUploaded(member, photoCollection);
            }
            catch (Exception e)
            {
                Log.Logger(String.Format(CultureInfo.InvariantCulture, "UploadPhoto exception: {0}", e.Message), Identifier);
                Trace.Tracer(e.ToString());
                throw e;
            }
        }

        private void NotifyPhotoUploaded(Member member, PhotoCollection photoCollection)
        {
            System.Diagnostics.Debug.Assert(member != null, "member != null");
            var memberAccount = MemberAccount.GetMemberAccountByMemberID(member.MemberID);
            if (MemberAccount.IsTwitterReady(memberAccount))
            {
                if (TwitterService.CanPost(MemberAccountActivity.GetLastActivity(member.MemberID, 1 /* Twitter */, 2 /* PhotoGallery */)))
                {
                    TwitterService.NotifyPhotoUploaded(memberAccount.Username, memberAccount.Password, member.WebMemberID, photoCollection.WebPhotoCollectionID);
                    MemberAccountActivity.SetLastActivity(member.MemberID, 1 /* Twitter */, 2 /* PhotoGallery */);
                }
            }
        }

        private void UploadPhoto(Member member, String photoCollectionID, String base64StringPhoto, String dateTime, Boolean SnappedFromMobile)
        {
            var byteImage = Convert.FromBase64String(base64StringPhoto);
            var longDateTime = Int64.Parse(dateTime);
            var takenDT = new DateTime(longDateTime);

            Image img = null;

            try
            {
                img = data.Photo.ByteArrayToImage(byteImage);
            }
            catch
            {
                throw new ArgumentException(Resources.Argument_InvalidBase64PhotoString);
            }

            var photoCollection = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(photoCollectionID);

            if (photoCollection == null)
            {
                photoCollection = new PhotoCollection()
                {
                    DTCreated = DateTime.Now,
                    Description = Resources.SnapUp_DefaultGalleryDescrpiption,
                    MemberID = member.MemberID,
                    Name = Resources.SnapUp_DefaultGalleryName
                };
                photoCollection.Save();
            }

            if (member.ProfilePhotoResourceFileID == 1 && !data.Photo.MemberHasAnyPhotos(member.MemberID))
                data.Photo.ProcessProfilePhoto(member, img);

            data.Photo.ProcessMemberPhoto(member, photoCollection.PhotoCollectionID, byteImage, takenDT, SnappedFromMobile);
            NotifyPhotoUploaded(member, photoCollection);
        }
    }
}
