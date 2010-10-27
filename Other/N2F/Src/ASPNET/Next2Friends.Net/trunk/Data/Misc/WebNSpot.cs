using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    [Serializable]
    public class WebNSpot
    {
        public static string RootURL = @"http://12.206.33.16/n2fweb/";

        public string Name{ get; set; } // the display name of the hotspot
        public string Description { get; set; } // the display description of the hotspot
        public DateTime BeginTime { get; set; } // the time the NSpot began
        public DateTime EndTime { get; set; }   // the time the NSpot ended

        public NspotVideo[] Videos { get; set; }    // an array of hotspot videos
        public NspotPhoto[] Photos { get; set; }    // an array of hotspot photos
        public NSpotMemberWS[] NSpotMembers { get; set; }// an array of hotspot members
        
        public WebNSpot()
        {

        }

        public WebNSpot(int NspotID)
        {
            NSpot nspot = new NSpot(NspotID);

            this.Name = nspot.Name;
            this.Description = nspot.Description;
            this.BeginTime = nspot.StartDateTime;
            this.EndTime = nspot.EndDateTime;

            Videos = GetAllNSpotMemberAndVideoByNSpotID(NspotID);
            Photos = GetNSpotPhotoByNSpotID(NspotID);
            NSpotMembers = GetNSpotMemberByNSpotID(NspotID);
        }

        /// <summary>
        /// Gets all the NspotVideo in the for the NSpot 
        /// </summary>
        public static NspotVideo[] GetAllNSpotMemberAndVideoByNSpotID(int NSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotVideoNSpotID");
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);

            List<NspotVideo> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = NspotVideo.PopulateNSpotMemberWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr.ToArray();
        }

        /// <summary>
        /// Gets all the NspotPhoto in the for the NSpot 
        /// </summary>
        public static NspotPhoto[] GetNSpotPhotoByNSpotID(int NSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotPhotoNSpotID");
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);

            List<NspotPhoto> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = NspotPhoto.PopulateNSpotMember(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr.ToArray();
        }

        /// <summary>
        /// Gets all the Members in the for the NSpot 
        /// </summary>
        public static NSpotMemberWS[] GetNSpotMemberByNSpotID(int NSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotMemberByNSpotID");
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);

            List<NSpotMemberWS> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = NSpotMemberWS.PopulateNSpotMember(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr.ToArray();
        }

    }
}