using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public class NSpotMemberWS
    {
        public string WebMemberID { get; set; } // the Web memberid of the Member (used for launching profiles back to the browser)
        public string Nickname { get; set; } // The nickname of the member
        public string ProfilePhotoURL { get; set; }// the URl of their profile photo
        public DateTime DateTimeJoined { get; set; } // the datetime they were first seen at the nspot

        public static List<NSpotMemberWS> PopulateNSpotMember(IDataReader dr)
        {
            List<NSpotMemberWS> arr = new List<NSpotMemberWS>();

            while (dr.Read())
            {
                NSpotMemberWS nsMember = new NSpotMemberWS();

                nsMember.WebMemberID = (string)dr["WebMemberID"];
                nsMember.Nickname = (string)dr["Nickname"];
                nsMember.ProfilePhotoURL = WebNSpot.RootURL  + (string)dr["ProfilePhotoURL"];
                nsMember.DateTimeJoined = (DateTime)dr["DateTimeJoined"];

                arr.Add(nsMember);
            }

            return arr;
        }
    }
}
