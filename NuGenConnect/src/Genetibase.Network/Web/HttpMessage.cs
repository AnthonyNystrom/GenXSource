using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Genetibase.Network.Sockets;

namespace Genetibase.Network.Web
{
    /// <summary>
    /// Represents a http message in http response/request process. This class is abstract.
    /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4">RFC 2616, section 4</seealso>
    /// </summary>
    public abstract class HttpMessage : IDisposable
    {

        static HttpMessage()
        {
            _generalHeaderNames = new List<string>();
            _enityHeaderNames = new List<string>();
        }

        protected HttpMessage()
        {
            _rawHeaders = new HeaderList();
        }

        #region Properties.Public

        /// <summary>
        /// Gets start line of http message. In request messages, start line called "Request-Line", in 
        /// response messages "Status-Line"
        /// </summary>
        public string RawStartLine
        {
            get { return GetRawStartLine(); }
            protected set { SetRawStartLine(value); }
        }

        public abstract HttpProtocolVersionEnum HttpVersion { get; set;}

        /// <summary>
        /// The message-body (if any) of an HTTP message is used to carry the entity-body associated with 
        /// the request or response. The message-body differs from the entity-body only when a transfer-coding 
        /// has been applied, as indicated by the Transfer-Encoding header field.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4.3">RFC 2616, section 4.3</seealso>
        /// </summary>
        public abstract Stream MessageBody { get;}

        /// <summary>
        /// Gets list of all headers, assotiated with http message. 
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4.2">RFC 2616, section 4.2</seealso>
        /// </summary>
        public HeaderList RawHeaders
        {
            get { return _rawHeaders; }
        }

        #region General Headers

        //general-header = Cache-Control            ; Section 14.9
        //             | Connection               ; Section 14.10
        //             | Date                     ; Section 14.18
        //             | Pragma                   ; Section 14.32
        //             | Trailer                  ; Section 14.40
        //             | Transfer-Encoding        ; Section 14.41
        //             | Upgrade                  ; Section 14.42
        //             | Via                      ; Section 14.45
        //             | Warning                  ; Section 14.46

        /// <summary>
        /// The Cache-Control general-header field is used to specify directives that MUST be obeyed 
        /// by all caching mechanisms along the request/response chain. The directives specify behavior 
        /// intended to prevent caches from adversely interfering with the request or response. These 
        /// directives typically override the default caching algorithms. Cache directives are unidirectional 
        /// in that the presence of a directive in a request does not imply that the same directive is to be 
        /// given in the response.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.9">RFC 2616, section 14.7</seealso>
        /// </summary>
        public string CacheControl
        {
            get { return _rawHeaders[HTTP_CacheControl]; }
            set { _rawHeaders[HTTP_CacheControl] = value; }
        }

        /// <summary>
        /// The Connection general-header field allows the sender to specify options that are desired 
        /// for that particular connection and MUST NOT be communicated by proxies over further connections.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.10">RFC 2616, section 14.10</seealso>
        /// </summary>
        public string Connection
        {
            get { return _rawHeaders[HTTP_Connection]; }
            set { _rawHeaders[HTTP_Connection] = value; }
        }


        public DateTime Date
        {
            get { return GetDateTimeHeader(HTTP_Date); }
            set { SetDateTimeHeader(HTTP_Date, value); }
        }

        /// <summary>
        /// The Pragma general-header field is used to include implementation- specific directives that 
        /// might apply to any recipient along the request/response chain. All pragma directives specify 
        /// optional behavior from the viewpoint of the protocol; however, some systems MAY require that 
        /// behavior be consistent with the directives.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.32">RFC 2616, section 14.32</seealso>
        /// </summary>
        public string Pragma
        {
            get { return _rawHeaders[HTTP_Pragma]; }
            set { _rawHeaders[HTTP_Pragma] = value; }
        }

        /// <summary>
        /// The Trailer general field value indicates that the given set of header fields is present in the 
        /// trailer of a message encoded with chunked transfer-coding.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.40">RFC 2616, section 14.40</seealso>
        /// </summary>
        public string Trailer
        {
            get { return _rawHeaders[HTTP_Trailer]; }
            set { _rawHeaders[HTTP_Trailer] = value; }
        }

        /// <summary>
        /// The Transfer-Encoding general-header field indicates what (if any) type of transformation has been 
        /// applied to the message body in order to safely transfer it between the sender and the recipient. 
        /// This differs from the content-coding in that the transfer-coding is a property of the message, 
        /// not of the entity.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.41">RFC 2616, section 14.41</seealso>
        /// </summary>
        public string TransferEncoding
        {
            get { return _rawHeaders[HTTP_TransferEncoding]; }
            set { _rawHeaders[HTTP_TransferEncoding] = value; }
        }

        /// <summary>
        /// The Upgrade general-header allows the client to specify what additional communication protocols 
        /// it supports and would like to use if the server finds it appropriate to switch protocols. 
        /// The server MUST use the Upgrade header field within a 101 (Switching Protocols) response to 
        /// indicate which protocol(s) are being switched.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.42">RFC 2616, section 14.42</seealso>
        /// </summary>
        public string Upgrade
        {
            get { return _rawHeaders[HTTP_Upgrade]; }
            set { _rawHeaders[HTTP_Upgrade] = value; }
        }

        /// <summary>
        /// The Via general-header field MUST be used by gateways and proxies to indicate the intermediate 
        /// protocols and recipients between the user agent and the server on requests, and between the 
        /// origin server and the client on responses. It is analogous to the "Received" field of RFC 822 and 
        /// is intended to be used for tracking message forwards, avoiding request loops, and identifying the 
        /// protocol capabilities of all senders along the request/response chain.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.45">RFC 2616, section 14.45</seealso>
        /// </summary>
        public string Via
        {
            get { return _rawHeaders[HTTP_Via]; }
            set { _rawHeaders[HTTP_Via] = value; }
        }

        /// <summary>
        /// The Warning general-header field is used to carry additional information about the status or 
        /// transformation of a message which might not be reflected in the message. This information is 
        /// typically used to warn about a possible lack of semantic transparency from caching operations or 
        /// transformations applied to the entity body of the message.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.46">RFC 2616, section 14.46</seealso>
        /// </summary>
        public string Warning
        {
            get { return _rawHeaders[HTTP_Warning]; }
            set { _rawHeaders[HTTP_Warning] = value; }
        }

        #endregion

        #region Enity Headers

        //entity-header  = Allow                    ; Section 14.7
        //       | Content-Encoding         ; Section 14.11
        //       | Content-Language         ; Section 14.12
        //       | Content-Length           ; Section 14.13
        //       | Content-Location         ; Section 14.14
        //       | Content-MD5              ; Section 14.15
        //       | Content-Range            ; Section 14.16
        //       | Content-Type             ; Section 14.17
        //       | Expires                  ; Section 14.21
        //       | Last-Modified            ; Section 14.29

        /// <summary>
        /// The Allow entity-header field lists the set of methods supported by the resource
        /// identified by the Request-URI. The purpose of this field is strictly to inform the
        /// recipient of valid methods associated with the resource. An Allow header field MUST be
        /// present in a 405 (Method Not Allowed) response.        
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.7">RFC 2616, section 14.7</seealso>       
        /// </summary>        
        public string Allow
        {
            get { return _rawHeaders[HTTP_Allow]; }
            set { _rawHeaders[HTTP_Allow] = value; }
        }


        /// <summary>
        /// The Content-Encoding entity-header field is used as a modifier to the media-type.
        /// When present, its value indicates what additional content codings have been applied to
        /// the entity-body, and thus what decoding mechanisms must be applied in order to obtain
        /// the media-type referenced by the Content-Type header field. Content-Encoding is
        /// primarily used to allow a document to be compressed without losing the identity of its
        /// underlying media type.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.11">RFC 2616, section 14.11</seealso>
        /// </summary>
        public string ContentEncoding
        {
            get { return _rawHeaders[HTTP_ContentEncoding]; }
            set { _rawHeaders[HTTP_ContentEncoding] = value; }
        }

        /// <summary>
        /// The Content-Language entity-header field describes the natural language(s) of the intended 
        /// audience for the enclosed entity. Note that this might not be equivalent to all the languages 
        /// used within the entity-body.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.12">RFC 2616, section 14.12</seealso>
        /// </summary>
        public string ContentLanguage
        {
            get { return _rawHeaders[HTTP_ContentLanguage]; }
            set { _rawHeaders[HTTP_ContentLanguage] = value; }
        }

        /// <summary>
        /// The Content-Length entity-header field indicates the size of the entity-body, 
        /// in decimal number of OCTETs, sent to the recipient or, in the case of the HEAD method, 
        /// the size of the entity-body that would have been sent had the request been a GET.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.13">RFC 2616, section 14.13</seealso>
        /// </summary>
        public long ContentLength
        {
            get 
            {
                if (string.IsNullOrEmpty(_rawHeaders[HTTP_ContentLength])) return -1;
                long result;
                return long.TryParse(_rawHeaders[HTTP_ContentLength], out result) ? result : -1;
            }
            set 
            { 
                if (value <= 0) _rawHeaders.Remove(HTTP_ContentLength); 
            }
        }

        /// <summary>
        /// The Content-Location entity-header field MAY be used to supply the resource location 
        /// for the entity enclosed in the message when that entity is accessible from a location 
        /// separate from the requested resource's URI. A server SHOULD provide a Content-Location 
        /// for the variant corresponding to the response entity; especially in the case where a resource 
        /// has multiple entities associated with it, and those entities actually have separate locations 
        /// by which they might be individually accessed, the server SHOULD provide a Content-Location for 
        /// the particular variant which is returned.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.14">RFC 2616, section 14.14</seealso>
        /// </summary>
        public string ContentLocation
        {
            get { return _rawHeaders[HTTP_ContentLocation]; }
            set { _rawHeaders[HTTP_ContentLocation] = value; }
        }

        /// <summary>
        /// The Content-MD5 entity-header field, as defined in RFC 1864, is an MD5 digest of the entity-body 
        /// for the purpose of providing an end-to-end message integrity check (MIC) of the entity-body. 
        /// (Note: a MIC is good for detecting accidental modification of the entity-body in transit, but 
        /// is not proof against malicious attacks.)
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.15">RFC 2616, section 14.15</seealso>
        /// </summary>
        public string ContentMD5
        {
            get { return _rawHeaders[HTTP_ContentMD5]; }
            set { _rawHeaders[HTTP_ContentMD5] = value; }
        }

        /// <summary>
        /// The Content-Range entity-header is sent with a partial entity-body to specify where in the full entity-body 
        /// the partial body should be applied.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.16">RFC 2616, section 14.16</seealso>
        /// </summary>
        public ContentRangeInfo ContentRange
        {
            get 
            {
                if (string.IsNullOrEmpty(_rawHeaders[HTTP_ContentRange])) 
                    return ContentRangeInfo.Empty;
                ContentRangeInfo result;
                return ContentRangeInfo.TryParse(_rawHeaders[HTTP_ContentRange], out result) ? 
                result : ContentRangeInfo.Empty;
            }
            set 
            {
                if (value.Equals(ContentRangeInfo.Empty))
                    _rawHeaders.Remove(HTTP_ContentRange);
                else 
                _rawHeaders[HTTP_ContentRange] = value.ToString();
            }
        }

        /// <summary>
        /// The Content-Type entity-header field indicates the media type of the entity-body 
        /// sent to the recipient or, in the case of the HEAD method, the media type that would 
        /// have been sent had the request been a GET.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.17">RFC 2616, section 14.17</seealso>
        /// </summary>
        public string ContentType
        {
            get { return _rawHeaders[HTTP_ContentType]; }
            set { _rawHeaders[HTTP_ContentType] = value; }
        }

#warning add zero values support
        /// <summary>
        /// The Expires entity-header field gives the date/time after which the response is considered 
        /// stale. A stale cache entry may not normally be returned by a cache (either a proxy cache or 
        /// a user agent cache) unless it is first validated with the origin server (or with an intermediate 
        /// cache that has a fresh copy of the entity).
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.21">RFC 2616, section 14.21</seealso>
        /// </summary>
        public DateTime Expires
        {
            get { return GetDateTimeHeader(HTTP_Expires); }
            set { SetDateTimeHeader(HTTP_Expires, value); }
        }    
        /// <summary>
        /// The Last-Modified entity-header field indicates the date and time at which the origin server 
        /// believes the variant was last modified.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.29">RFC 2616, section 14.29</seealso>
        /// </summary>
        public DateTime LastModified
        {
            get { return GetDateTimeHeader(HTTP_LastModified); }
            set { SetDateTimeHeader(HTTP_LastModified, value); }
        }

        [Obsolete()]
        public bool HasContentLength
        {
            get { return ContentLength >= 0; }
        }

        #endregion

        #endregion

        #region Methods.Protected

        protected abstract string GetRawStartLine();
        protected abstract void SetRawStartLine(string value);

        protected DateTime GetDateTimeHeader(string header)
        {
            string temp = _rawHeaders[header];
            return !string.IsNullOrEmpty(temp) ? Http.StrInternetToDateTime(temp) : DateTime.MinValue;
        }

        protected void SetDateTimeHeader(string header, DateTime value)
        {
            if (value == DateTime.MinValue) _rawHeaders.Remove(header);
            else
                _rawHeaders[header] = Http.DateTimeGmtToHttpStr(value);
        }



        protected virtual bool IsGeneralHeader(string header)
        {
            return _generalHeaderNames.Contains(header.Trim().ToLower());
        }

        protected virtual bool IsEnityHeader(string header)
        {
            return _enityHeaderNames.Contains(header.Trim().ToLower());
        }
       
        protected virtual void DoDispose(){}

        #endregion

        #region Methods.Public

        public virtual string BuildHeadersString()
        {            
            PrepareHeaders();
            StringBuilder sb = new StringBuilder();
            foreach (string headerLine in _rawHeaders)
            {
                sb.AppendLine(headerLine);
            }
            return sb.ToString();
        }

        public virtual void PrepareHeaders()
        {

        }

        public virtual void ProcessHeaders()
        {

        }

        void IDisposable.Dispose()
        {
            DoDispose();
        }

        #endregion


        private static List<string> _generalHeaderNames;
        private static List<string> _enityHeaderNames;
        protected HeaderList _rawHeaders;

        // general header names
        public const string HTTP_CacheControl = "Cache-Control";
        public const string HTTP_Connection = "Connection";
        public const string HTTP_Date = "Date";
        public const string HTTP_Pragma = "Pragma";
        public const string HTTP_Trailer = "Trailer";
        public const string HTTP_TransferEncoding = "Transfer-Encoding";
        public const string HTTP_Upgrade = "Upgrade";
        public const string HTTP_Via = "Via";
        public const string HTTP_Warning = "Warning";

        // enity header names
        public const string HTTP_Allow = "Allow";
        public const string HTTP_ContentEncoding = "Content-Encoding";
        public const string HTTP_ContentLanguage = "Content-Language";
        public const string HTTP_ContentLength = "Content-Length";
        public const string HTTP_ContentLocation = "Content-Location";
        public const string HTTP_ContentMD5 = "Content-MD5";
        public const string HTTP_ContentRange = "Content-Range";
        public const string HTTP_ContentType = "Content-Type";
        public const string HTTP_Expires = "Expires";
        public const string HTTP_LastModified = "Last-Modified";
        
    }
}
