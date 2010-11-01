using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Web
{
#warning Fix Range
    public struct RangeInfo
    {
        public static readonly RangeInfo Empty;

        public string BytesUnit;
        public int FirstBytePos;
        public int LastBytePos;
        
        static RangeInfo()
        {
            Empty = new RangeInfo();
        }

        public RangeInfo(string bytesUnit, int firstBytePos, int lastBytePos)
        {
            BytesUnit = bytesUnit;
            FirstBytePos = firstBytePos;
            LastBytePos = lastBytePos;
        }

        public static RangeInfo Parse(string s)
        {
            string bytesUnit = null;
            string firstBytePos = null;
            string lastBytePos = null;
            string instanceLength = null;

            bytesUnit = Genetibase.Network.Sockets.Global.Fetch(ref s, " ", true);
            firstBytePos = Genetibase.Network.Sockets.Global.Fetch(ref s, "-", true);
            lastBytePos = Genetibase.Network.Sockets.Global.Fetch(ref s, "/", true);
            instanceLength = s;

            return new RangeInfo(bytesUnit, int.Parse(firstBytePos), int.Parse(lastBytePos));        
        }

        public static bool TryParse(string s, out RangeInfo value)
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
            return string.Format("{0} {1}-{2}", BytesUnit, FirstBytePos, LastBytePos);
        }
    }
}
