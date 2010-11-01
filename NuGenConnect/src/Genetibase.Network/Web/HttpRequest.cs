using System;
using System.IO;
using Genetibase.Network.Sockets.Protocols.Authentication;

namespace Genetibase.Network.Web
{

    /// <summary>
    /// Represents a general request http message. This class is abstract.
    /// </summary>
    public abstract class HttpRequest : HttpMessage
    {       
        #region Properties.Public

        /// <summary>
        /// Gets message body of request. Response message body is a Postream property 
        /// of HttpRequest class.
        /// </summary>
        public sealed override Stream MessageBody
        {
            get { return PostStream; }
        }

        public abstract string RequestLine { get; }
        public abstract Stream PostStream { get; set;}
        public abstract string RequestUri { get; set; }
        public abstract HttpCommandEnum Method { get; set;}

        #region Request Headers
        //         request-header = Accept                   ; Section 14.1
        //                      | Accept-Charset           ; Section 14.2
        //                      | Accept-Encoding          ; Section 14.3
        //                      | Accept-Language          ; Section 14.4
        //                      | Authorization            ; Section 14.8
        //                      | Expect                   ; Section 14.20
        //                      | From                     ; Section 14.22
        //                      | Host                     ; Section 14.23
        //                      | If-Match                 ; Section 14.24
        //                      | If-Modified-Since        ; Section 14.25
        //                      | If-None-Match            ; Section 14.26
        //                      | If-Range                 ; Section 14.27
        //                      | If-Unmodified-Since      ; Section 14.28
        //                      | Max-Forwards             ; Section 14.31
        //                      | Proxy-Authorization      ; Section 14.34
        //                      | Range                    ; Section 14.35
        //                      | Referer                  ; Section 14.36
        //                      | TE                       ; Section 14.39
        //                      | User-Agent               ; Section 14.43

        /// <summary>
        /// The Accept request-header field can be used to specify certain media types which 
        /// are acceptable for the response. Accept headers can be used to indicate that the 
        /// request is specifically limited to a small set of desired types, as in the case 
        /// of a request for an in-line image.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.1">RFC 2616, section 14.1</seealso>
        /// </summary>
        public string Accept
        {
            get { return _rawHeaders[HTTP_Accept]; }
            set { _rawHeaders[HTTP_Accept] = value; }
        }

        /// <summary>
        /// The Accept-Charset request-header field can be used to indicate what character 
        /// sets are acceptable for the response. This field allows clients capable of 
        /// understanding more comprehensive or special- purpose character sets to signal 
        /// that capability to a server which is capable of representing documents in those 
        /// character sets
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.2">RFC 2616, section 14.2</seealso>
        /// </summary>
        public string AcceptCharset
        {
            get { return _rawHeaders[HTTP_AcceptCharset]; }
            set { _rawHeaders[HTTP_AcceptCharset] = value; }
        }


        /// <summary>
        /// The Accept-Encoding request-header field is similar to Accept, but restricts the 
        /// content-codings (section 3.5) that are acceptable in the response.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.3">RFC 2616, section 14.3</seealso>
        /// </summary>
        public string AcceptEncoding
        {
            get { return _rawHeaders[HTTP_AcceptEncoding]; }
            set { _rawHeaders[HTTP_AcceptEncoding] = value; }
        }

        /// <summary>
        /// The Accept-Language request-header field is similar to Accept, but restricts 
        /// the set of natural languages that are preferred as a response to the request. 
        /// Language tags are defined in RFC 2616, section 3.10.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.4">RFC 2616, section 14.4</seealso>
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.10">RFC 2616, section 3.10</seealso>
        /// </summary>
        public string AcceptLanguage
        {
            get { return _rawHeaders[HTTP_AcceptLanguage]; }
            set { _rawHeaders[HTTP_AcceptLanguage] = value; }
        }

#warning Autorisation support
        public abstract AuthenticationBase Authorization { get; set;}

        /// <summary>
        /// The Expect request-header field is used to indicate that particular 
        /// server behaviors are required by the client.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.20">RFC 2616, section 14.20</seealso>
        /// </summary>
        public string Expect
        {
            get { return _rawHeaders[HTTP_Expect]; }
            set { _rawHeaders[HTTP_Expect] = value; }
        }

        /// <summary>
        /// The From request-header field, if given, SHOULD contain an Internet e-mail address 
        /// for the human user who controls the requesting user agent. The address SHOULD be 
        /// machine-usable, as defined by "mailbox" in RFC 822 as updated by RFC 1123
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.22">RFC 2616, section 14.22</seealso>
        /// </summary>        
        public string From
        {
            //TODO Make this property not string. Make it as "EmailAddress class"
            get { return _rawHeaders[HTTP_From]; }
            set { _rawHeaders[HTTP_From] = value; }
        }

        /// <summary>
        /// The Host request-header field specifies the Internet host and port number of the 
        /// resource being requested, as obtained from the original URI given by the user or 
        /// referring resource (generally an HTTP URL, as described in RFC 2616, section 3.2.2). 
        /// The Host field value MUST represent the naming authority of the origin server or 
        /// gateway given by the original URL. This allows the origin server or gateway to 
        /// differentiate between internally-ambiguous URLs, such as the root "/" URL of a 
        /// server for multiple host names on a single IP address.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.23">RFC 2616, section 14.23</seealso>
        /// </summary>
        public string Host
        {
            get { return _rawHeaders[HTTP_Host]; }
            set { _rawHeaders[HTTP_Host] = value; }
        }

#warning ProxyAuthorization support
        public string ProxyAuthorization 
        {
            get { return _rawHeaders[HTTP_ProxyAuthorization]; }
            set { _rawHeaders[HTTP_ProxyAuthorization] = value; }
        }

        /// <summary>
        /// 
        /// <seealso cref="">RFC 2616, section X.X</seealso>
        /// </summary>
        public string ProxyConnection
        {
            get { return _rawHeaders[HTTP_ProxyConnection]; }
            set { _rawHeaders[HTTP_ProxyConnection] = value; }
        }

        /// <summary>
        /// HTTP retrieval requests using conditional or unconditional GET methods MAY 
        /// request one or more sub-ranges of the entity, instead of the entire entity, 
        /// using the Range request header, which applies to the entity returned as the 
        /// result of the request
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.35">RFC 2616, section 14.35</seealso>
        /// </summary>
        public RangeInfo Range
        {
            get
            {
                if (string.IsNullOrEmpty(_rawHeaders[HTTP_Range]))
                    return RangeInfo.Empty;
                RangeInfo result;
                return RangeInfo.TryParse(_rawHeaders[HTTP_Range], out result) ?
                result : RangeInfo.Empty;
            }
            set
            {
                if (value.Equals(RangeInfo.Empty))
                    _rawHeaders.Remove(HTTP_Range);
                else
                    _rawHeaders[HTTP_Range] = value.ToString();
            }
        }

        /// <summary>
        /// The Referer[sic] request-header field allows the client to specify, for the server's 
        /// benefit, the address (URI) of the resource from which the Request-URI was obtained 
        /// (the "referrer", although the header field is misspelled.) The Referer request-header 
        /// allows a server to generate lists of back-links to resources for interest, logging, 
        /// optimized caching, etc. It also allows obsolete or mistyped links to be traced for 
        /// maintenance. The Referer field MUST NOT be sent if the Request-URI was obtained from 
        /// a source that does not have its own URI, such as input from the user keyboard.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.36">RFC 2616, section 14.36</seealso>
        /// </summary>
        public string Referer
        {
            get { return _rawHeaders[HTTP_Referer]; }
            set { _rawHeaders[HTTP_Referer] = value; }
        }

        /// <summary>
        /// The TE request-header field indicates what extension transfer-codings it is willing 
        /// to accept in the response and whether or not it is willing to accept trailer fields 
        /// in a chunked transfer-coding. Its value may consist of the keyword "trailers" and/or 
        /// a comma-separated list of extension transfer-coding names with optional accept 
        /// parameters (as described in RFC 2616, section 3.6).
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.39">RFC 2616, section 14.39</seealso>
        /// </summary>
        public string TE
        {
            get { return _rawHeaders[HTTP_TE]; }
            set { _rawHeaders[HTTP_TE] = value; }
        }

        /// <summary>
        /// The User-Agent request-header field contains information about the user agent 
        /// originating the request. This is for statistical purposes, the tracing of protocol 
        /// violations, and automated recognition of user agents for the sake of tailoring 
        /// responses to avoid particular user agent limitations. User agents SHOULD include 
        /// this field with requests. The field can contain multiple product tokens (RFC 2616, section 3.8) 
        /// and comments identifying the agent and any subproducts which form a significant part 
        /// of the user agent. By convention, the product tokens are listed in order of their 
        /// significance for identifying the application.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.43">RFC 2616, section 14.43</seealso>
        /// </summary>
        public string UserAgent
        {
            get { return _rawHeaders[HTTP_UserAgent]; }
            set { _rawHeaders[HTTP_UserAgent] = value; }
        }
       
        #endregion

        #endregion

        protected static string GetRequestUriFromRequestLine(string requestLine)
        {
            throw new NotImplementedException("");
        }

        protected static HttpProtocolVersionEnum GetHttpVersionFromRequestLine(string requestLine)
        {
            throw new NotImplementedException("");
        }

        protected static HttpCommandEnum GetHttpCommandFromRequestLine(string requestLine)
        {
            throw new NotImplementedException("");
        }


        public const string HTTP_Accept = "Accept";
        public const string HTTP_AcceptCharset = "Accept-Charset";
        public const string HTTP_AcceptEncoding = "Accept-Encoding";
        public const string HTTP_AcceptLanguage = "Accept-Language";
        public const string HTTP_Authorization = "Authorization";
        public const string HTTP_Expect = "Expect";
        public const string HTTP_From = "From";
        public const string HTTP_Host = "Host";
        public const string HTTP_IfMatch = "If-Match";
        public const string HTTP_IfModifiedSince = "If-Modified-Since";
        public const string HTTP_IfNoneMatch = "If-None-Match";
        public const string HTTP_IfRange = "If-Range";
        public const string HTTP_IfUnmodifiedSince = "If-Unmodified-Since";
        public const string HTTP_MaxForwards = "Max-Forwards";
        public const string HTTP_ProxyAuthorization = "Proxy-Authorization";
        public const string HTTP_ProxyConnection = "Proxy-Connection";
        public const string HTTP_Range = "Range";
        public const string HTTP_Referer = "Referer";
        public const string HTTP_TE = "TE";
        public const string HTTP_UserAgent = "User-Agent";
    
    }
}
