/* ------------------------------------------------
 * GeoMessageAddressInfo.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using Next2Friends.WebServices.GeoMessage.Data;
using System.Data;

namespace Next2Friends.WebServices.GeoMessage.Tables
{
    [SqlTable("GeoMessageAddressInfo")]
    sealed class GeoMessageAddressInfo
    {
        [SqlParameter("GeoMessageAddressInfoID", SqlDbType.Int)]
        [SqlPrimaryKey]
        public Int32 ID { get; set; }

        [SqlParameter("Extension", SqlDbType.VarChar, 100, true)]
        public String Extension { get; set; }

        [SqlParameter("Street", SqlDbType.VarChar, 100, true)]
        public String Street { get; set; }

        [SqlParameter("PostalCode", SqlDbType.VarChar, 30, true)]
        public String PostalCode { get; set; }

        [SqlParameter("City", SqlDbType.VarChar, 50, true)]
        public String City { get; set; }

        [SqlParameter("County", SqlDbType.VarChar, 50, true)]
        public String County { get; set; }

        [SqlParameter("State", SqlDbType.VarChar, 50, true)]
        public String State { get; set; }

        [SqlParameter("Country", SqlDbType.VarChar, 50, true)]
        public String Country { get; set; }

        [SqlParameter("CountryCode", SqlDbType.VarChar, 3, true)]
        public String CountryCode { get; set; }

        [SqlParameter("District", SqlDbType.VarChar, 100, true)]
        public String District { get; set; }

        [SqlParameter("BuildingName", SqlDbType.VarChar, 100, true)]
        public String BuildingName { get; set; }

        [SqlParameter("BuildingFloor", SqlDbType.VarChar, 30, true)]
        public String BuildingFloor { get; set; }

        [SqlParameter("BuildingRoom", SqlDbType.VarChar, 30, true)]
        public String BuildingRoom { get; set; }

        [SqlParameter("BuildingZone", SqlDbType.VarChar, 100, true)]
        public String BuildingZone { get; set; }

        [SqlParameter("Crossing1", SqlDbType.VarChar, 100, true)]
        public String Crossing1 { get; set; }

        [SqlParameter("Crossing2", SqlDbType.VarChar, 100, true)]
        public String Crossing2 { get; set; }

        [SqlParameter("Url", SqlDbType.VarChar, 100, true)]
        public String Url { get; set; }

        [SqlParameter("PhoneNumber", SqlDbType.VarChar, 30, true)]
        public String PhoneNumber { get; set; }
    }
}
