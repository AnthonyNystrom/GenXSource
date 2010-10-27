using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data;

namespace ExternalMessaging
{
    public class Utility
    {

        protected readonly static string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public static string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// example: next2friends.com/video/this-is-the-name-of-the-video/
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public static string FormatStringForURL(string Title)
        {
            string FinalTitle = System.Text.RegularExpressions.Regex.Replace(Title, @"[^\w\s]", "");
            FinalTitle = FinalTitle.Replace("&", "");
            FinalTitle = FinalTitle.Replace(" ", "-");

            return FinalTitle.ToLower();
        }

        [Microsoft.SqlServer.Server.SqlFunction()]
        public static string NewWebID()
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(guid);

            return Convert.ToBase64String(bytes).Substring(0, 8);
        }

        public static string GetTemplate(string templateName)
        {
            try
            {
                String templateString = String.Empty;
                Assembly assembly = Assembly.GetAssembly(typeof(Utility));
                Stream s = assembly.GetManifestResourceStream("ExternalMessaging.Templates." + templateName);
                StreamReader sr = new StreamReader(s);
                templateString = sr.ReadToEnd();
                return templateString;
            }
            catch { return string.Empty; }
        }

        public static DataSet GetObjectByObjectIDWithJoin(int ObjectID, ContentType type, SqlConnection conn)
        {
            SqlCommand command = null;
            SqlParameter param = null;

            if (type == ContentType.Wall)
            {
                command = new SqlCommand("AG_GetMemberByMemberID", conn);
                param = new SqlParameter("@MemberID", ObjectID);
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);
            }
            else if (type == ContentType.Video)
            {
                command = new SqlCommand("AG_GetVideoByVideoIDWithJoin", conn);
                param = new SqlParameter("@VideoID", ObjectID);
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);
            }
            else if (type == ContentType.Blog)
            {
                command = new SqlCommand("AG_GetBlogEntryByBlogEntryIDWithJoin", conn);
                param = new SqlParameter("@BlogEntryID", ObjectID);
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);
            }
            else if (type == ContentType.Photo)
            {
                command = new SqlCommand("AG_GetPhotoByPhotoIDWithJoin", conn);
                param = new SqlParameter("@PhotoID", ObjectID);
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);
            }
            else if (type == ContentType.AskAFriend)
            {
                command = new SqlCommand("AG_GetAskAFriendByAskAFriendIDWithJoin", conn);
                param = new SqlParameter("@AskAFriendID", ObjectID);
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);
            }
            else if (type == ContentType.PhotoGallery)
            {
                command = new SqlCommand("HG_GetPhotoCollectionByPhotoCollectionIDWithJoin", conn);
                param = new SqlParameter("@PhotoCollectionID", ObjectID);
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);
            }

            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            return ds;
        }


        /// <summary>
        /// Used for populating url for thread replies
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="commentType"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string PopulateLink(string inputString, ContentType commentType, DataRow row)
        {
            string QueryString = string.Empty;

            switch (commentType)
            {
                case ContentType.Wall:
                    inputString = inputString.Replace("<#PageLink#>", "/users/" + (string)row["NickName"] + "/wall");
                    //QueryString = "m=" + (string)row["WebMemberID"];
                    break;

                case ContentType.Video:
                    inputString = inputString.Replace("<#PageLink#>", "/video/" + Utility.FormatStringForURL((string)row["Title"]) + "/" + row["WebVideoID"]);
                    //inputString = inputString.Replace("<#PageLink#>", "VideoView.aspx");
                    //QueryString = "v=" + row["WebVideoID"];
                    break;

                case ContentType.Blog:
                    inputString = inputString.Replace("<#PageLink#>", "Blog.aspx");
                    QueryString = "b=" + row["WebBlogEntryID"] + "&m=" + (string)row["MemberWebMemberID"];
                    break;

                case ContentType.Photo:
                    inputString = inputString.Replace("<#PageLink#>", "ViewGallery.aspx");
                    QueryString = "g=" + row["ParentPhotoCollectionWebPhotoCollectionID"] + "&m=" + (string)row["MemberWebMemberID"] + "&wp=" + (string)row["WebPhotoID"];
                    break;

                case ContentType.AskAFriend:
                    inputString = inputString.Replace("<#PageLink#>", "AskAFriend.aspx");
                    QueryString = "q=" + row["WebAskAFriendID"];
                    break;

                case ContentType.PhotoGallery:
                    inputString = inputString.Replace("<#PageLink#>", "ViewGallery.aspx");
                    QueryString = "g=" + row["WebPhotoCollectionID"] + "&m=" + (string)row["MemberWebMemberID"];
                    break;

            }

            return inputString.Replace("<#QueryString#>", QueryString);
        }

        public static string GetBanner(string type,SqlConnection conn)
        {
            SqlCommand command = null;
            SqlParameter param = null;

            command = new SqlCommand("HG_GetNextBanner", conn);
            param = new SqlParameter("@BannerType", 2);
            param.DbType = DbType.Int32;
            command.Parameters.Add(param);

            param = new SqlParameter("@URL", type);
            param.DbType = DbType.String;
            command.Parameters.Add(param);

            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);


            StringBuilder sbHTML = new StringBuilder();

            string[] parameters = new string[3];

            parameters[0] = "http://www.next2friends.com/ad/" + (string)ds.Tables[0].Rows[0]["WebBannerID"];// + "/" + MemberIDOfTheEmail;
            parameters[1] = "http://www.next2friends.com/" + (string)ds.Tables[0].Rows[0]["FileLocation"];
            parameters[2] = "";

            sbHTML.AppendFormat(@"<a href='{0}' style='border:0px'><img src='{1}' alt='{2}'></a>", parameters);
            return sbHTML.ToString();
        }

    }
}
