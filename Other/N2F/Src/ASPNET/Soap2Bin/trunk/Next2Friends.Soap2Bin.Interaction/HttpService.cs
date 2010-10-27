using System;
using System.Globalization;
using System.Web;

namespace Next2Friends.Soap2Bin.Interaction
{
    static class HttpService
    {
        public static void SetContentLength(this HttpResponse response, Int64 value)
        {
            response.AppendHeader("Content-Length", value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
