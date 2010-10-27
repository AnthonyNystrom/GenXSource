/* ------------------------------------------------
 * GeoMessageQualifiedCoordinates.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using Next2Friends.WebServices.GeoMessage.Data;

namespace Next2Friends.WebServices.GeoMessage.Tables
{
    [SqlTable("GeoMessageQualifiedCoordinates")]
    sealed class GeoMessageQualifiedCoordinates
    {
        [SqlParameter("GeoMessageQualifiedCoordinatesID", SqlDbType.Int)]
        [SqlPrimaryKey]
        public Int32 ID { get; set; }

        [SqlParameter("Latitude", SqlDbType.Decimal)]
        public Double Latitude { get; set; }

        [SqlParameter("Longitude", SqlDbType.Decimal)]
        public Double Longitude { get; set; }

        [SqlParameter("Altitude", SqlDbType.Decimal, true)]
        public Single Altitude { get; set; }

        [SqlParameter("HorizontalAccuracy", SqlDbType.Decimal, true)]
        public Single HorizontalAccuracy { get; set; }

        [SqlParameter("VerticalAccuracy", SqlDbType.Decimal, true)]
        public Single VerticalAccuracy { get; set; }
    }
}
