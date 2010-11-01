using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Web
{
    public struct ContentRangeInfo
    {
        public static readonly ContentRangeInfo Empty;

        public string BytesUnit;
        public int FirstBytePos;
        public int LastBytePos;
        public int InstanceLength;

        static ContentRangeInfo()
        {
            Empty = new ContentRangeInfo();
        }

        public ContentRangeInfo(string bytesUnit, int firstBytePos, int lastBytePos, int instanceLength)
        {
            BytesUnit = bytesUnit;
            FirstBytePos = firstBytePos;
            LastBytePos = lastBytePos;
            InstanceLength = instanceLength;
        }

        public static ContentRangeInfo Parse(string s)
        {
            string bytesUnit = null;
            string firstBytePos = null;
            string lastBytePos = null;
            string instanceLength = null;

            bytesUnit = Genetibase.Network.Sockets.Global.Fetch(ref s, " ", true);
            firstBytePos = Genetibase.Network.Sockets.Global.Fetch(ref s, "-", true);
            lastBytePos = Genetibase.Network.Sockets.Global.Fetch(ref s, "/", true);
            instanceLength = s;

            return new ContentRangeInfo(bytesUnit, int.Parse(firstBytePos), int.Parse(lastBytePos), int.Parse(instanceLength));        
        }

        public static bool TryParse(string s, out ContentRangeInfo value)
        {
            value = Empty;
            try
            {
                value = Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}-{2}/{3}", BytesUnit, FirstBytePos, LastBytePos, InstanceLength);
        }
    }
}
