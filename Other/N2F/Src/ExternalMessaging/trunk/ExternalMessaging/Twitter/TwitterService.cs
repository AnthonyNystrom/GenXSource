/* ------------------------------------------------
 * TwitterService.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace ExternalMessaging.Twitter
{
    internal static class TwitterService
    {
        private static readonly TwitterPublisher _publisher = new TwitterPublisher();

        public static void UpdateStatus(String username, String password, String status)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (status == null)
                throw new ArgumentNullException("status");

            _publisher.Update(username, password, status, OutputFormatType.XML);
        }

        public static void NotifyVideoUploaded(String username, String password, String nickname, String webVideoId)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            if (String.IsNullOrEmpty(webVideoId))
                throw new ArgumentNullException("webVideoId");

            var url = MakeTinyUrl(String.Format("http://www.next2friends.com/video/{0}/{1}", nickname, webVideoId));
            
            _publisher.Update(
                username
                , password
                , String.Format("Video: {0}", url)
                , OutputFormatType.XML);
        }

        private static String MakeTinyUrl(String url)
        {
            Debug.Assert(!String.IsNullOrEmpty(url), "!String.IsNullOrEmpty(url)");

            try
            {
                if (url.Length <= 30)
                {
                    return url;
                }

                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("ftp"))
                {
                    url = String.Format("http://{0}", url);
                }

                var request = WebRequest.Create(String.Format("http://tinyurl.com/api-create.php?url={0}", url));
                var res = request.GetResponse();
                var text = "";
                
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                
                return text;
            }
            catch (Exception)
            {
                return url;
            }
        }
    }
}
