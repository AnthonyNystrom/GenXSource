using System.IO;
using Genetibase.Network.Sockets;

namespace Genetibase.Network.Web
{
    public abstract class HttpResponse : HttpMessage
    {       
        #region Properties.Public

        /// <summary>
        /// Gets message body of response. Response message body is a ContentStream property 
        /// of HttpResponse class.
        /// </summary>
        public sealed override Stream MessageBody
        {
            get { return ContentStream; }
        }

        public abstract string StatusLine { get; set;}
        public abstract Stream ContentStream { get; set;}
        public abstract string ResponseText { get; set;}
        public abstract int ResponseCode { get; set;}

        #region Response Headers

        //       response-header = Accept-Ranges           ; Section 14.5
        //                       | Age                     ; Section 14.6
        //                       | ETag                    ; Section 14.19
        //                       | Location                ; Section 14.30
        //                       | Proxy-Authenticate      ; Section 14.33
        //                       | Retry-After             ; Section 14.37
        //                       | Server                  ; Section 14.38
        //                       | Vary                    ; Section 14.44
        //                       | WWW-Authenticate        ; Section 14.47

        /// <summary>
        /// The Accept-Ranges response-header field allows the server to indicate 
        /// its acceptance of range requests for a resource
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.5">RFC 2616, section 14.5</seealso>
        /// </summary>
        public string AcceptRanges
        {
            get { return _rawHeaders[HTTP_AcceptRanges]; }
            set { _rawHeaders[HTTP_AcceptRanges] = value; }
        }

        /// <summary>
        /// The Age response-header field conveys the sender's estimate of the
        /// amount of time since the response (or its revalidation) was
        /// generated at the origin server. A cached response is "fresh" if
        /// its age does not exceed its freshness lifetime. Age values are
        /// calculated as specified in section 13.2.3.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.6">RFC 2616, section 14.6</seealso>
        /// </summary>
        public string Age
        {
            get { return _rawHeaders[HTTP_Age]; }
            set { _rawHeaders[HTTP_Age] = value; }
        }

        /// <summary>
        /// The ETag response-header field provides the current value of the entity tag for the 
        /// requested variant. The headers used with entity tags are described in RFC 2616, sections 
        /// 14.24, 14.26 and 14.44. The entity tag MAY be used for comparison with other entities 
        /// from the same resource (see RFC 2616, section 13.3.3).
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.19">RFC 2616, section 14.19</seealso>
        /// </summary>
        public string ETag
        {
            get { return _rawHeaders[HTTP_ETag]; }
            set { _rawHeaders[HTTP_ETag] = value; }
        }

        /// <summary>
        /// The Location response-header field is used to redirect the recipient to a 
        /// location other than the Request-URI for completion of the request or identification 
        /// of a new resource. For 201 (Created) responses, the Location is that of the new 
        /// resource which was created by the request. For 3xx responses, the location SHOULD 
        /// indicate the server's preferred URI for automatic redirection to the resource. 
        /// The field value consists of a single absolute URI.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.30">RFC 2616, section 14.30</seealso>
        /// </summary>
        public string Location
        {
            get { return _rawHeaders[HTTP_Location]; }
            set { _rawHeaders[HTTP_Location] = value; }
        }
#warning ProxyAuthenticate support
        public HeaderList ProxyAuthenticate
        {
            get { return new HeaderList(); }
            set {  }
        }

        /// <summary>
        /// The Retry-After response-header field can be used with a 503 (Service Unavailable) 
        /// response to indicate how long the service is expected to be unavailable to the 
        /// requesting client. This field MAY also be used with any 3xx (Redirection) response 
        /// to indicate the minimum time the user-agent is asked wait before issuing the 
        /// redirected request. The value of this field can be either an HTTP-date or an integer 
        /// number of seconds (in decimal) after the time of the response.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.37">RFC 2616, section 14.37</seealso>
        /// </summary>
        public string RetryAfter
        {
            get { return _rawHeaders[HTTP_RetryAfter]; }
            set { _rawHeaders[HTTP_RetryAfter] = value; }
        }

        /// <summary>
        /// The Server response-header field contains information about the software used by 
        /// the origin server to handle the request. The field can contain multiple product 
        /// tokens (section 3.8) and comments identifying the server and any significant 
        /// subproducts. The product tokens are listed in order of their significance for 
        /// identifying the application.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.38">RFC 2616, section 14.38</seealso>
        /// </summary>
        public string Server
        {
            get { return _rawHeaders[HTTP_Server]; }
            set { _rawHeaders[HTTP_Server] = value; }
        }

        /// <summary>
        /// The Vary field value indicates the set of request-header fields that fully 
        /// determines, while the response is fresh, whether a cache is permitted to use 
        /// the response to reply to a subsequent request without revalidation. For 
        /// uncacheable or stale responses, the Vary field value advises the user agent 
        /// about the criteria that were used to select the representation. A Vary field 
        /// value of "*" implies that a cache cannot determine from the request headers 
        /// of a subsequent request whether this response is the appropriate representation. 
        /// See section 13.6 for use of the Vary header field by caches.
        /// <seealso cref="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.44">RFC 2616, section 14.44</seealso>
        /// </summary>
        public string Vary
        {
            get { return _rawHeaders[HTTP_Vary]; }
            set { _rawHeaders[HTTP_Vary] = value; }
        }
#warning WWWAuthenticate support
        public HeaderList WWWAuthenticate
        {
            get { return new HeaderList(); }
            set {  }
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

        #endregion 

        #endregion

        //    

        public const string HTTP_AcceptRanges = "Accept-Ranges";
        public const string HTTP_Age = "Age";
        public const string HTTP_ETag = "ETag";
        public const string HTTP_Location = "Location";
        public const string HTTP_ProxyAuthenticate = "Proxy-Authenticate";
        public const string HTTP_RetryAfter = "Retry-After";
        public const string HTTP_Server = "Server";
        public const string HTTP_Vary = "Vary";
        public const string HTTP_WWWAuthenticate = "WWW-Authenticate";
        public const string HTTP_ProxyConnection = "Proxy-Connection";
       
    }
}
