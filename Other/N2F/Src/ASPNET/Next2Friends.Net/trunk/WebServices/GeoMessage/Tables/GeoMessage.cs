/* ------------------------------------------------
 * GeoMessage.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using Next2Friends.WebServices.GeoMessage.Data;

namespace Next2Friends.WebServices.GeoMessage.Tables
{
    [SqlTable("GeoMessage")]
    sealed class GeoMessage
    {
        [SqlParameter("GeoMessageID", SqlDbType.Int)]
        [SqlPrimaryKey]
        public Int32 ID { get; set; }

        [SqlParameter("MessageType", SqlDbType.Int)]
        public Int32 MessageType { get; set; }

        [SqlParameter("MeasureUnits", SqlDbType.Int)]
        public Int32 MeasureUnits { get; set; }

        [SqlParameter("Spot", SqlDbType.Decimal)]
        public Single Spot { get; set; }

        [SqlParameter("ShouldRepeat", SqlDbType.Bit)]
        public Boolean ShouldRepeat { get; set; }

        [SqlParameter("RepeatTimes", SqlDbType.Int)]
        public Int32 RepeatTimes { get; set; }

        [SqlParameter("LocationID", SqlDbType.Int)]
        [SqlForeignKey]
        public Int32 LocationID { get; set; }

        [SqlParameter("MessageText", SqlDbType.VarChar, 8000)]
        public String MessageText { get; set; }

        [SqlParameter("DTCreated", SqlDbType.DateTime, true)]
        public DateTime? DTCreated { get; set; }
    }
}
