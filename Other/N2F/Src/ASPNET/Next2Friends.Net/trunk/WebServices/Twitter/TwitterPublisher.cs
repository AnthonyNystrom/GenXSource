using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;

namespace Next2Friends.WebServices.Twitter
{
    internal sealed class TwitterPublisher
    {
        private String source = null;

        private String twitterClient = null;
        private String twitterClientVersion = null;
        private String twitterClientUrl = null;

        /// <summary>
        /// Source is an additional parameters that will be used to fill the "From" field.
        /// Currently you must talk to the developers of Twitter at:
        /// http://groups.google.com/group/twitter-development-talk/
        /// Otherwise, Twitter will simply ignore this parameter and set the "From" field to "web".
        /// </summary>
        public String Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// Sets the name of the Twitter client.
        /// According to the Twitter Fan Wiki at http://twitter.pbwiki.com/API-Docs and supported by
        /// the Twitter developers, this will be used in the future (hopefully near) to set more information
        /// in Twitter about the client posting the information as well as future usage in a clients directory.
        /// </summary>
        public String TwitterClient
        {
            get { return twitterClient; }
            set { twitterClient = value; }
        }

        /// <summary>
        /// Sets the version of the Twitter client.
        /// According to the Twitter Fan Wiki at http://twitter.pbwiki.com/API-Docs and supported by
        /// the Twitter developers, this will be used in the future (hopefully near) to set more information
        /// in Twitter about the client posting the information as well as future usage in a clients directory.
        /// </summary>
        public String TwitterClientVersion
        {
            get { return twitterClientVersion; }
            set { twitterClientVersion = value; }
        }

        /// <summary>
        /// Sets the URL of the Twitter client.
        /// Must be in the XML format documented in the "Request Headers" section at:
        /// http://twitter.pbwiki.com/API-Docs.
        /// According to the Twitter Fan Wiki at http://twitter.pbwiki.com/API-Docs and supported by
        /// the Twitter developers, this will be used in the future (hopefully near) to set more information
        /// in Twitter about the client posting the information as well as future usage in a clients directory.		
        /// </summary>
        public String TwitterClientUrl
        {
            get { return twitterClientUrl; }
            set { twitterClientUrl = value; }
        }

        private const String TwitterBaseUrlFormat = "http://twitter.com/{0}/{1}.{2}";

        private String GetObjectTypeString(ObjectType objectType)
        {
            return objectType.ToString().ToLower();
        }

        private String GetActionTypeString(ActionType actionType)
        {
            return actionType.ToString().ToLower();
        }

        private String GetFormatTypeString(OutputFormatType format)
        {
            return format.ToString().ToLower();
        }

        /// <summary>
        /// Executes an HTTP GET command and retrives the information.		
        /// </summary>
        /// <param name="url">The URL to perform the GET operation</param>
        /// <param name="userName">The username to use with the request</param>
        /// <param name="password">The password to use with the request</param>
        /// <returns>The response of the request, or null if we got 404 or nothing.</returns>
        private String ExecuteGetCommand(String url, String userName, String password)
        {
            using (WebClient client = new WebClient())
            {
                if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
                {
                    client.Credentials = new NetworkCredential(userName, password);
                }

                try
                {
                    using (Stream stream = client.OpenRead(url))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException ex)
                {
                    //
                    // Handle HTTP 404 errors gracefully and return a null String to indicate there is no content.
                    //
                    if (ex.Response is HttpWebResponse)
                    {
                        if ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                        {
                            return null;
                        }
                    }

                    throw ex;
                }
            }
        }

        /// <summary>
        /// Executes an HTTP POST command and retrives the information.		
        /// This function will automatically include a "source" parameter if the "Source" property is set.
        /// </summary>
        /// <param name="url">The URL to perform the POST operation</param>
        /// <param name="userName">The username to use with the request</param>
        /// <param name="password">The password to use with the request</param>
        /// <param name="data">The data to post</param> 
        /// <returns>The response of the request, or null if we got 404 or nothing.</returns>
        private String ExecutePostCommand(String url, String userName, String password, String data)
        {
            WebRequest request = WebRequest.Create(url);
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                request.Credentials = new NetworkCredential(userName, password);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                if (!String.IsNullOrEmpty(TwitterClient))
                {
                    request.Headers.Add("X-Twitter-Client", TwitterClient);
                }

                if (!String.IsNullOrEmpty(TwitterClientVersion))
                {
                    request.Headers.Add("X-Twitter-Version", TwitterClientVersion);
                }

                if (!String.IsNullOrEmpty(TwitterClientUrl))
                {
                    request.Headers.Add("X-Twitter-URL", TwitterClientUrl);
                }

                if (!String.IsNullOrEmpty(Source))
                {
                    data += "&source=" + HttpUtility.UrlEncode(Source);
                }

                Byte[] bytes = Encoding.UTF8.GetBytes(data);

                request.ContentLength = bytes.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);

                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            return null;
        }

        public String Update(String userName, String password, String status, OutputFormatType format)
        {
            if (format != OutputFormatType.JSON && format != OutputFormatType.XML)
            {
                throw new ArgumentException("Update support only XML and JSON output format", "format");
            }

            String url = String.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Update), GetFormatTypeString(format));
            String data = String.Format("status={0}", HttpUtility.UrlEncode(status));

            return ExecutePostCommand(url, userName, password, data);
        }

        public String UpdateAsJSON(String userName, String password, String text)
        {
            return Update(userName, password, text, OutputFormatType.JSON);
        }

        public XmlDocument UpdateAsXML(String userName, String password, String text)
        {
            String output = Update(userName, password, text, OutputFormatType.XML);
            if (!String.IsNullOrEmpty(output))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(output);

                return xmlDocument;
            }

            return null;
        }
    }
}
