/*
 * Created by: Alexey Preskenis
 * Created: 9 may 2007
 */

using System;
using System.IO;

namespace Genetibase.Network.Web
{
    public class HttpServerRequest : HttpRequest
    {        

        public override string RequestLine
        {
            get
            {
                return RawStartLine;
            }            
        }

        public override Stream PostStream
        {
            get { return _postStream; }
            set { _postStream = value; }
        }

        public override string RequestUri
        {
            get { return GetRequestUriFromRequestLine(RequestLine); }
            set { throw new NotSupportedException(""); }
        }
       
        public override HttpCommandEnum Method
        {
            get
            {
                return GetHttpCommandFromRequestLine(RequestLine);
            }
            set
            {
                throw new NotSupportedException("");
            }
        }

        public string RemoteIP
        {
            get { return _remoteIP; }
            internal set { _remoteIP = value; }
        }

        public override Genetibase.Network.Sockets.Protocols.Authentication.AuthenticationBase Authorization
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        public override HttpProtocolVersionEnum HttpVersion
        {
            get { return GetHttpVersionFromRequestLine(RequestLine); }
            set { throw new NotSupportedException(""); }
        }

        protected override string GetRawStartLine()
        {
            return _rawHttpCommand;
        }

        protected override void SetRawStartLine(string value)
        {
            _rawHttpCommand = value;            
        }

       

        private Stream _postStream = null;
        private string _rawHttpCommand = string.Empty;
        private string _remoteIP = string.Empty;
        
    }
}