// File       : GSM Triangulation
// Date       : July 10, 2008
// Description: This gsmMapping class originally found on the web.
// Modified to initialize lat and lon variables correctly and to
// return the success of SetLocation method.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace GSMtriangulation
{
    class gsmMapping
    {

        /* 
         * Sample code to obtain geo codes from a cell info
         * "GSM/UMTS" setting revealed by smuraro, thanks!
         */

        /* (c) "Neil Young" (neil.young@freenet.de)
         * 
         * This script/program is provided "as is".
         *
         * This program is free software; you can redistribute it and/or modify
         * it under the terms of the GNU General Public License as published by
         * the Free Software Foundation; either version 3 of the License, or
         * (at your option) any later version.
         *
         * This program is distributed in the hope that it will be useful,
         * but WITHOUT ANY WARRANTY; without even the implied warranty of
         * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
         * GNU General Public License for more details.
         *
         * GNU General Public License, see <http://www.gnu.org/licenses/>.
         */


        private double lat;
        private double lon;

        static byte[] PostData(int MCC, int MNC, int LAC, int CID, bool shortCID)
        {
            /* The shortCID parameter follows heuristic experiences:
             * Sometimes UMTS CIDs are build up from the original GSM CID (lower 4 hex digits)
             * and the RNC-ID left shifted into the upper 4 digits.
             */
            byte[] pd = new byte[] {
                0x00, 0x0e,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,

                0x1b, 
                0x00, 0x00, 0x00, 0x00, // Offset 0x11
                0x00, 0x00, 0x00, 0x00, // Offset 0x15
                0x00, 0x00, 0x00, 0x00, // Offset 0x19
                0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, // Offset 0x1f
                0x00, 0x00, 0x00, 0x00, // Offset 0x23
                0x00, 0x00, 0x00, 0x00, // Offset 0x27
                0x00, 0x00, 0x00, 0x00, // Offset 0x2b
                0xff, 0xff, 0xff, 0xff,
                0x00, 0x00, 0x00, 0x00
            };

            bool isUMTSCell = ((Int64)CID > 65535);

            if (isUMTSCell)
                Console.WriteLine("UMTS CID. {0}", shortCID ? "Using short CID to resolve." : "");
            else
                Console.WriteLine("GSM CID given.");

            if (shortCID)
                CID &= 0xFFFF;      /* Attempt to resolve the cell using the GSM CID part */

            if ((Int64)CID > 65536) /* GSM: 4 hex digits, UTMS: 6 hex digits */
                pd[0x1c] = 5;
            else
                pd[0x1c] = 3;

            pd[0x11] = (byte)((MNC >> 24) & 0xFF);
            pd[0x12] = (byte)((MNC >> 16) & 0xFF);
            pd[0x13] = (byte)((MNC >> 8) & 0xFF);
            pd[0x14] = (byte)((MNC >> 0) & 0xFF);

            pd[0x15] = (byte)((MCC >> 24) & 0xFF);
            pd[0x16] = (byte)((MCC >> 16) & 0xFF);
            pd[0x17] = (byte)((MCC >> 8) & 0xFF);
            pd[0x18] = (byte)((MCC >> 0) & 0xFF);

            pd[0x27] = (byte)((MNC >> 24) & 0xFF);
            pd[0x28] = (byte)((MNC >> 16) & 0xFF);
            pd[0x29] = (byte)((MNC >> 8) & 0xFF);
            pd[0x2a] = (byte)((MNC >> 0) & 0xFF);

            pd[0x2b] = (byte)((MCC >> 24) & 0xFF);
            pd[0x2c] = (byte)((MCC >> 16) & 0xFF);
            pd[0x2d] = (byte)((MCC >> 8) & 0xFF);
            pd[0x2e] = (byte)((MCC >> 0) & 0xFF);

            pd[0x1f] = (byte)((CID >> 24) & 0xFF);
            pd[0x20] = (byte)((CID >> 16) & 0xFF);
            pd[0x21] = (byte)((CID >> 8) & 0xFF);
            pd[0x22] = (byte)((CID >> 0) & 0xFF);

            pd[0x23] = (byte)((LAC >> 24) & 0xFF);
            pd[0x24] = (byte)((LAC >> 16) & 0xFF);
            pd[0x25] = (byte)((LAC >> 8) & 0xFF);
            pd[0x26] = (byte)((LAC >> 0) & 0xFF);

            return pd;
        }

        public bool SetLocation(int MCC, int MNC, int LAC, int CID)
        {
            // Typical values: 0 310 29844 47211

            string shortCID = "";   // Default, no change at all. May be needed in future for UMTS
            bool success = false;   // Assume failure until success.
            lat = 0; lon = 0;       // Initialize lat and lon to zero in case of failure.
            
            try
            {
                String url = "http://www.google.com/glm/mmap";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
                req.Method = "POST";

                byte[] pd = PostData(MCC, MNC, LAC, CID, shortCID == "shortcid");

                req.ContentLength = pd.Length;
                req.ContentType = "application/binary";
                Stream outputStream = req.GetRequestStream();
                outputStream.Write(pd, 0, pd.Length);
                outputStream.Close();

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                byte[] ps = new byte[res.ContentLength];
                int totalBytesRead = 0;
                while (totalBytesRead < ps.Length)
                {
                    totalBytesRead += res.GetResponseStream().Read(ps, totalBytesRead, ps.Length - totalBytesRead);
                }

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    short opcode1 = (short)(ps[0] << 8 | ps[1]);
                    byte opcode2 = ps[2];
                    System.Diagnostics.Debug.Assert(opcode1 == 0x0e);
                    System.Diagnostics.Debug.Assert(opcode2 == 0x1b);
                    int ret_code = (int)((ps[3] << 24) | (ps[4] << 16) | (ps[5] << 8) | (ps[6]));

                    if (ret_code == 0)
                    {
                        lat = ((double)((ps[7] << 24) | (ps[8] << 16) | (ps[9] << 8) | (ps[10]))) / 1000000;
                        lon = ((double)((ps[11] << 24) | (ps[12] << 16) | (ps[13] << 8) | (ps[14]))) / 1000000;
                        success = true;
                        Console.WriteLine("Latitude: {0}, Longitude: {1}", lat, lon);
                    }
                    else
                        Console.WriteLine("Error {0}", ret_code);
                }
                else
                    Console.WriteLine("HTTP Status {0} {1}", res.StatusCode, res.StatusDescription);
            }
            catch (Exception)
            {
                throw;
            }
            return success;
        }

        public double GetLatitude()
        {
            return lat;
        }

        public double GetLongitude()
        {
            return lon;
        }
    }
}

