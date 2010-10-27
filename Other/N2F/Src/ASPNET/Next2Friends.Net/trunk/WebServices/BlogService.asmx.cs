/* ------------------------------------------------
 * BlogService.asmx.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Web.Services;

using Microsoft.Practices.EnterpriseLibrary.Data;

using Next2Friends.Data;
using Next2Friends.Misc;

using blogger = Next2Friends.WebServices.Blogger;
using data = Next2Friends.Data;
using Next2Friends.WebServices.Utils;
using System.Web;
using Next2Friends.WebServices.Properties;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Provides functionality to create and modify blog entries, insert photos and videos, read comments, leave your own.
    /// </summary>
    [WebService(
        Description = "Provides functionality to create and modify blog entries, insert photos and videos, read comments, leave your own.",
        Name = "BlogService",
        Namespace = "http://www.next2friends.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class BlogService : WebService
    {
        /// <summary>
        /// Adds new blog entry. Returns new entry identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>newEntry</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(
            Description = "<p>Adds new blog entry. Returns new entry identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>newEntry</tt> is <tt>null</tt>.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public Int32 AddEntry(String nickname, String password, blogger.BlogEntry newEntry)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (newEntry == null)
                throw new ArgumentNullException("newEntry");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var blogEntry = CreateBlogEntry(newEntry);
            blogEntry.MemberID = member.MemberID;
            blogEntry.Save();

            return blogEntry.BlogEntryID;
        }

        /// <summary>
        /// Updates the specified entry.
        /// Set the <code>editedEntry.ID</code> property value to the identifier of the entry to edit.
        /// Other properties should contain new values for this entry.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string, or if the specified <code>editedEntry</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the identifier of the blog entry creator does not match the identifier of the user that is intending to edit this blog entry.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(
           Description = "<p>Updates the specified entry.<br />Set the <tt>editedEntry.ID</tt> property value to the identifier of the entry to edit.<br />Other properties should contain new values for this entry.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string, or if the specified <tt>editedEntry</tt> is <tt>null</tt>.<br /><tt>ArgumentException</tt> - If the identifier of the blog entry creator does not match the identifier of the user that is intending to edit this blog entry.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public void EditEntry(String nickname, String password, blogger.BlogEntry editedEntry)
        {
            if (nickname == null)
                throw new ArgumentNullException("nickname");
            if (password == null)
                throw new ArgumentNullException("password");
            if (editedEntry == null)
                throw new ArgumentNullException("editedEntry");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var blogEntry = CreateBlogEntry(editedEntry);

            if (blogEntry.MemberID != member.MemberID)
                throw new ArgumentException(Resources.Argument_InvalidBlogEntryEditor);

            blogEntry.Save();
        }

        /// <summary>
        /// Removes the entry with the specified identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The identifier of the blog entry creator does not match the identifier of the user that is intending to delete this blog entry.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(
           Description = "<p>Removes the entry with the specified identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - The identifier of the blog entry creator does not match the identifier of the user that is intending to delete this blog entry.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public void RemoveEntry(String nickname, String password, Int32 blogEntryID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var blogEntry = new data.BlogEntry(blogEntryID);

            if (blogEntry.MemberID != member.MemberID)
                throw new ArgumentException(Resources.Argument_InvalidBlogEntryRemover);

            data.BlogEntry.DeleteBlogEntry(blogEntryID);
        }

        /// <summary>
        /// Retrieves entry-related data according to the specified identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the blog entry with the specified identifier does not exist.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves entry-related data according to the specified identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If the blog entry with the specified identifier does not exist.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public blogger.BlogEntry GetEntry(String nickname, String password, Int32 blogEntryID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var entry = data.BlogEntry.GetBlogEntry(blogEntryID);

            if (entry == null)
                throw new ArgumentException(String.Format(Resources.Argument_InvalidBlogEntryIdentifier, blogEntryID));

            return CreateBlogEntry(entry);
        }

        /// <summary>
        /// Retrieves a list of the latest 10 blog entry identifiers for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="Exception">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves a list of the latest 10 blog entry identifiers for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>Exception</tt> - If the user with the specified credentials does not exist.</p>")]
        public Int32[] GetEntryIDs(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");            

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            return data.BlogEntry.GetBlogEntryIDs(member.MemberID);
        }

        /// <summary>
        /// Retrieves a list of the latest 10 blog entries for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves a list of the latest 10 blog entries for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public blogger.BlogEntry[] GetEntries(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = data.Member.GetMemberViaNicknamePassword(nickname, password);
            var ids = data.BlogEntry.GetBlogEntryIDs(member.MemberID);
            var idsLength = ids.Length;
            var result = new blogger.BlogEntry[idsLength];

            for (var i = 0; i < idsLength; i++)
            {
                var entry = data.BlogEntry.GetBlogEntry(ids[i]);
                if (entry == null)
                    throw new ArgumentException(String.Format(Resources.Argument_InvalidBlogEntryIdentifier, ids[i]));
                result[i] = CreateBlogEntry(entry);
            }

            return result;
        }

        /// <summary>
        /// Returns a list of photo identifiers for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of photo identifiers for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public Int32[] GetPhotoIDs(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return GetPhotoIDs(data.Member.GetMemberViaNicknamePassword(nickname, password).MemberID);
        }

        private static Int32[] GetPhotoIDs(Int32 memberID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetBlogPhotoIDs");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);

            var photoIDs = new List<Int32>();

            /* Execute the stored procedure. */
            using (var dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                    photoIDs.Add(Convert.ToInt32(dr["PhotoID"]));
                dr.Close();
            }

            var result = new Int32[photoIDs.Count];
            photoIDs.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Retrieves photo-related data according to the specified identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the photo with the specified <code>photoID</code> does not exist or the specified user does not have access to it.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves photo-related data according to the specified identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If the photo with the specified <tt>photoID</tt> does not exist or the specified user does not have access to it.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public blogger.BlogPhoto GetPhoto(String nickname, String password, Int32 photoID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return GetPhoto(data.Member.GetMemberViaNicknamePassword(nickname, password).MemberID, photoID);
        }

        private static blogger.BlogPhoto GetPhoto(Int32 memberID, Int32 photoID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetBlogPhotos");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);
            db.AddInParameter(dbCommand, "photoID", DbType.Int32, photoID);

            using (var dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    return new blogger.BlogPhoto()
                    {
                        Base64ThumbnailString = data.Photo.ImageToBase64String(new Bitmap(GetThumbnailPath(Convert.ToString(dr["Path"]), Convert.ToString(dr["FileName"])))),
                        CreatedDT = Convert.ToDateTime(dr["CreatedDT"]).Ticks.ToString(),
                        Description = Text2Mobile.Filter(Convert.ToString(dr["Caption"]) ?? ""),
                        ID = Convert.ToInt32(dr["PhotoID"]),
                        Title = Text2Mobile.Filter(Convert.ToString(dr["Title"]) ?? "")
                    };
                }
            }

            throw new ArgumentException(String.Format(Resources.Argument_InvalidBlogPhotoIdentifier, photoID));
        }

        /// <summary>
        /// Returns a list of photos for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of photos for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public blogger.BlogPhoto[] GetPhotos(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var ids = GetPhotoIDs(member.MemberID);
            var idsLength = ids.Length;
            var result = new blogger.BlogPhoto[idsLength];

            for (var i = 0; i < idsLength; i++)
                result[i] = GetPhoto(member.MemberID, ids[i]);

            return result;
        }

        /// <summary>
        /// Returns a list of video identifiers for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of video identifiers for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public Int32[] GetVideoIDs(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return GetVideoIDs(data.Member.GetMemberViaNicknamePassword(nickname, password).MemberID);
        }

        private static Int32[] GetVideoIDs(Int32 memberID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetBlogVideoIDs");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);

            var videoIDs = new List<Int32>();

            /* Execute the stored procedure. */
            using (var dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                    videoIDs.Add(Convert.ToInt32(dr["VideoID"]));
                dr.Close();
            }

            var result = new Int32[videoIDs.Count];
            videoIDs.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Retrieves video-related data according to the specified identifier.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the video with the specified <code>videoID</code> does not exist or the specified user does not have access to it.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Retrieves video-related data according to the specified identifier.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>ArgumentException</tt> - If the video with the specified <tt>videoID</tt> does not exist or the specified user does not have access to it.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public blogger.BlogVideo GetVideo(String nickname, String password, Int32 videoID)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return GetVideo(data.Member.GetMemberViaNicknamePassword(nickname, password).MemberID, videoID);
        }

        private static blogger.BlogVideo GetVideo(Int32 memberID, Int32 videoID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetBlogPhotos");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);
            db.AddInParameter(dbCommand, "videoID", DbType.Int32, videoID);

            using (var dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    return new blogger.BlogVideo()
                    {
                        Base64ThumbnailString = data.Photo.ImageToBase64String(new Bitmap(GetThumbnailPath(Convert.ToString(dr["Path"]), Convert.ToString(dr["FileName"])))),
                        CreatedDT = Convert.ToDateTime(dr["DTCreated"]).Ticks.ToString(),
                        Description = Text2Mobile.Filter(Convert.ToString(dr["Description"]) ?? ""),
                        ID = Convert.ToInt32(dr["VideoID"]),
                        Title = Text2Mobile.Filter(Convert.ToString(dr["Title"]) ?? "")
                    };
                }
            }

            throw new ArgumentException(String.Format(Resources.Argument_InvalidBlogVideoIdentifier, videoID));
        }

        /// <summary>
        /// Returns a list of videos for the specified user.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>nickname</code> is <code>null</code> or an empty string, or if the specified <code>password</code> is <code>null</code> or an empty string.
        /// </exception>
        /// <exception cref="BadCredentialsException">
        /// If the user with the specified credentials does not exist.
        /// </exception>
        [WebMethod(Description = "<p>Returns a list of videos for the specified user.</p><p><b>Throws:</b><br /><tt>ArgumentNullException</tt> - If the specified <tt>nickname</tt> is <tt>null</tt> or an empty string, or if the specified <tt>password</tt> is <tt>null</tt> or an empty string.<br /><tt>BadCredentialsException</tt> - If the user with the specified credentials does not exist.</p>")]
        public blogger.BlogVideo[] GetVideos(String nickname, String password)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var member = Member.GetMemberViaNicknamePassword(nickname, password);
            var ids = GetVideoIDs(member.MemberID);
            var idsLength = ids.Length;
            var result = new blogger.BlogVideo[idsLength];

            for (var i = 0; i < idsLength; i++)
                result[i] = GetVideo(member.MemberID, ids[i]);

            return result;
        }

        private static blogger.BlogEntry CreateBlogEntry(data.BlogEntry descriptor)
        {
            blogger.BlogEntry blogEntry = new blogger.BlogEntry();

            blogEntry.Body = Text2Mobile.Filter(descriptor.Body);
            blogEntry.DTCreated = descriptor.DTCreated.Ticks.ToString();
            blogEntry.ID = descriptor.BlogEntryID;
            blogEntry.Title = Text2Mobile.Filter(descriptor.Title);

            return blogEntry;
        }

        private static data.BlogEntry CreateBlogEntry(blogger.BlogEntry descriptor)
        {
            data.BlogEntry blogEntry = null;

            if (descriptor.ID == 0)
                blogEntry = new data.BlogEntry();
            else
                blogEntry = new data.BlogEntry(descriptor.ID);

            blogEntry.Body = HttpUtility.HtmlEncode(descriptor.Body ?? "");
            String dtCreatedRaw = descriptor.DTCreated;
            if (!String.IsNullOrEmpty(dtCreatedRaw))
                blogEntry.DTCreated = new DateTime(Convert.ToInt64(dtCreatedRaw));
            blogEntry.Title = HttpUtility.HtmlEncode(descriptor.Title ?? "");

            return blogEntry;
        }

        private static String GetThumbnailPath(String path, String fileName)
        {
            return Path.Combine(OSRegistry.GetDiskUserDirectory(), Path.Combine(path, fileName));
        }
    }
}
