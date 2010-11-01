using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.Network.Web;
using Genetibase.Network;
using System.IO;
using System.Text.RegularExpressions;
using System.Resources;

namespace SimpleHttpProxy
{
    interface ISettings
    {
        List<string> BannedSites { get;}
    }

    internal class GetRequestHandler : IWebRequestHandler 
    {
        private ISettings _settings;
        private object _syncRoot = new object();

        public GetRequestHandler(ISettings settings)
        {
            _settings = settings;
        }

        #region IWebRequestHandler Members

        public void HandleCommand(HttpServer server, HttpRequestInfo request, HttpResponseInfo response, ref bool handled)
        {
            if (request.CommandType != HttpCommandEnum.Get) return;

            Logger.Log("Command: {0}",request.RawHttpCommand);

            handled = true;
            try
            {

                if (IsBannedUri(request.RequestURI))
                {
                    ProcessBannedRequest(request, response);
                }
                else
                    ProcessRequest(request, response);
            }
            catch (Exception ex)
            {
                Logger.Log("Error: {0} on command {1}", ex.Message, request.RequestURI);
            }

        }

        public bool IsBannedUri(string requestUri)
        { 
            foreach (string bannedSite in _settings.BannedSites)
            {
                //string pattern = bannedSite;
                //pattern = pattern.Replace(".", @"\.");
                //pattern = pattern.Replace("*", ".*");
                //MatchCollection mc = Regex.Matches(requestUri, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                //if (mc.Count > 0) return true;
            }
            return false;
        }


        public void SessionStart(HttpSession session)
        {
        }

        public void SessionEnd(HttpSession session)
        {
        }

        private void ProcessBannedRequest(HttpRequestInfo request, HttpResponseInfo response)
        {
            string page = File.ReadAllText(
                string.Format(@"{0}\BannedSitePage.htm", 
                new FileInfo(this.GetType().Assembly.Location).Directory));

            response.ResponseNo = 200;
            response.ContentText = page;                        
        }
        

        private void ProcessRequest(HttpRequestInfo request, HttpResponseInfo response)
        {
            //lock (_syncRoot)
            //{
                HttpClient client = new HttpClient();
                try
                {

                    client.Host = request.Host;
                    client.Port = 80;
                    client.Connect();
                    try
                    {
                        client.Request.Accept = request.Accept;
                        client.Request.AcceptCharset = request.AcceptCharSet;
                        client.Request.AcceptEncoding = request.AcceptEncoding;
                        client.Request.AcceptLanguage = request.AcceptLanguage;
                       // client.Request.Authorization = request.Authentication;
                        client.Request.BasicAuthentication = request.BasicAuthentication;
                        client.Request.CacheControl = request.CacheControl;
                        client.Request.Connection = request.Connection;
                        client.Request.ContentEncoding = request.ContentEncoding;
                        client.Request.ContentLanguage = request.ContentLanguage;
                        client.Request.ContentLength = request.ContentLength;
                        //client.Request.ContentRangeEnd = request.ContentRangeEnd;
                        //client.Request.ContentRangeStart = request.ContentRangeStart;
                        client.Request.ContentType = request.ContentType;
                        //client.Request.ContentVersion = request.ContentVersion;
                        //client.Request.CustomHeaders = request.CustomHeaders;
                        client.Request.Date = request.Date;
                        client.Request.Expires = request.Expires;
                        client.Request.From = request.From;
                        client.Request.Host = request.Host;
                        client.Request.LastModified = request.LastModified;
                        client.Request.Method = HttpCommandEnum.Get;
                        client.Request.Password = request.Password;
                        client.Request.Pragma = request.Pragma;
                        client.Request.ProxyConnection = request.ProxyConnection;

                        //client.Request.RawHeaders.Clear();
                        //foreach (string header in request.RawHeaders)
                        //{
                        //    client.Request.RawHeaders.Add(header);
                        //}

                        client.Request.Referer = request.Referer;
                        client.Request.UserAgent = request.UserAgent;
                        client.Request.UserName = request.UserName;

                        MemoryStream mem = new MemoryStream();
                        client.Get(request.RequestURI, mem);

                        response.CacheControl = client.Response.CacheControl;
                        response.Connection = client.Response.Connection;
                        response.ContentEncoding = client.Response.ContentEncoding;
                        response.ContentLanguage = client.Response.ContentLanguage;
                        response.ContentLength = client.Response.ContentLength;
                        //response.ContentRangeEnd = client.Response.ContentRangeEnd;
                        //response.ContentRangeStart = client.Response.ContentRangeEnd;
                        response.ContentStream = client.Response.ContentStream;
                        response.ContentType = client.Response.ContentType;
                        //response.ContentVersion = client.Response.ContentVersion;
                        //response.CustomHeaders = client.Response.CustomHeaders;
                        response.Date = client.Response.Date;
                        response.Expires = client.Response.Expires;

                        response.LastModified = client.Response.LastModified;
                        response.Location = client.Response.Location;
                        response.Pragma = client.Response.Pragma;

                        request.RawHeaders.Clear();
                        foreach (string header in client.Response.RawHeaders)
                        {
                            request.RawHeaders.Add(header);
                        }

                        response.ResponseNo = client.Response.ResponseCode;
                        response.ResponseText = client.Response.ResponseText;
                        response.Server = client.Response.Server;
                        response.WWWAuthenticate = client.Response.WWWAuthenticate;
                    }

                    finally { client.Disconnect(); }
                }
                catch (Exception ex)
                {
                    response.ResponseNo = 500;
                    response.ContentText = string.Format("Exception: {0}", ex.Message);
                }
            //}
        }

        #endregion
    }
}
