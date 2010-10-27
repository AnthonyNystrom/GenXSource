/* ------------------------------------------------
 * PhotoService.asmx.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;
using Next2Friends.WebServices.Photo;
using Next2Friends.WebServices.Utils;
using data = Next2Friends.Data;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to retrieve photos.
    /// </summary>
    [WebService(
        Description = "Provides functionality to retrieve photos.",    
        Name = "PhotoService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class PhotoService : WebService
    {
        /// <summary>
        /// Returns top 10 photos for the collection with the specified <code>photoCollectionID</code>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns top 10 photos for the collection with the specified <tt>photoCollectionID</tt>.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.</p>")]
        public WebPhoto[] GetPhotosForCollection(String nickname, String password, Int32 photoCollectionID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var photos = data.Photo.GetPhotoByPhotoCollectionIDWithJoinPager(photoCollectionID, 0, 10);
            return ConvertToWebPhotoArray(photos);
        }

        /// <summary>
        /// Returns a Base64 encoded string representation of the thumbnail for the photo with the specified <code>photoID</code>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the member with the specified credentials does not exist.
        /// </exception>
        /// <exception cref="Exception">
        /// If the photo with the specified <code>photoID</code> does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a Base64 encoded string representation of the thumbnail for the photo with the specified <tt>photoID</tt>.</p><p><b>Parameters:</b><br /><tt>nickname</tt><br /><tt>password</tt><br /><tt>photoID</tt></p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the member with the specified credentials does not exist.<br /><tt>Exception</tt> - If the photo with the specified <tt>photoID</tt> does not exist.</p>")]
        public String GetPhotoThumbnail(String nickname, String password, Int32 photoID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var photo = data.Photo.GetPhotoByPhotoIDWithJoin(photoID);
            return ResourceProcessor.GetThumbnailBase64String(photo.ThumbnailResourceFile.SavePath);
        }

        private static WebPhoto[] ConvertToWebPhotoArray(IList<data.Photo> photos)
        {
            var result = new List<WebPhoto>();

            foreach (var photo in photos)
            {
                result.Add(
                    new WebPhoto()
                    {
                        ID = photo.PhotoID,
                        CreatedDT = photo.CreatedDT.Ticks.ToString(),
                        Description = Text2Mobile.Filter(photo.Caption ?? ""),
                        Title = Text2Mobile.Filter(photo.Title ?? "")
                    });
            }

            return result.ToArray();
        }
    }
}
