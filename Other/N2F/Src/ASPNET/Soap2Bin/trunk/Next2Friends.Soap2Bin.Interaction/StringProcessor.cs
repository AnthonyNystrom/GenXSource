using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Interaction
{
    static class StringProcessor
    {
        public static Boolean IsGet(this String requestType)
        {
            return String.Equals("GET", requestType, StringComparison.OrdinalIgnoreCase);
        }

        public static Boolean IsPost(this String requestType)
        {
            return String.Equals("POST", requestType, StringComparison.OrdinalIgnoreCase);
        }

        public static DateTime ToDateTime(this String ticksString)
        {
            return new DateTime(Convert.ToInt64(ticksString));
        }
    }
}
