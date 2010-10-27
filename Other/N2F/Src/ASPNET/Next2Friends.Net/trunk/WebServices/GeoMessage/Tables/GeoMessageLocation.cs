/* ------------------------------------------------
 * GeoMessageLocation.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using System.Data.SqlClient;

using Next2Friends.WebServices.GeoMessage.Data;

namespace Next2Friends.WebServices.GeoMessage.Tables
{
    [SqlTable("GeoMessageLocation")]
    sealed class GeoMessageLocation
    {
        [SqlParameter("GeoMessageLocationID", SqlDbType.Int)]
        [SqlPrimaryKey]
        public Int32 ID { get; set; }

        [SqlParameter("AddressInfoID", SqlDbType.Int, true)]
        [SqlForeignKey]
        public Int32? AddressInfoID { get; set; }

        [SqlParameter("Course", SqlDbType.Decimal, true)]
        public Single Course { get; set; }

        [SqlParameter("LocationMethod", SqlDbType.Int)]
        public Int32 LocationMethod { get; set; }

        [SqlParameter("QualifiedCoordinatesID", SqlDbType.Int)]
        [SqlForeignKey]
        public Int32 QualifiedCoordinatesID { get; set; }

        [SqlParameter("Speed", SqlDbType.Decimal, true)]
        public Single Speed { get; set; }

        [SqlParameter("TimeStamp", SqlDbType.DateTime, true)]
        public DateTime? TimeStamp { get; set; }

        [SqlParameter("IsValid", SqlDbType.Bit)]
        public Boolean IsValid { get; set; }

        [SqlParameter("ExtraInfo", SqlDbType.VarChar, 8000, true)]
        public String ExtraInfo { get; set; }
    }
}
