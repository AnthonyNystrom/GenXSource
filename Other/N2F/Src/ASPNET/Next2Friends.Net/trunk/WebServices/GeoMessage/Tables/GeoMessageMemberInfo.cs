/* ------------------------------------------------
 * GeoMessageMemberInfo.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using Next2Friends.WebServices.GeoMessage.Data;

namespace Next2Friends.WebServices.GeoMessage.Tables
{
    [SqlTable("GeoMessageMemberInfo")]
    sealed class GeoMessageMemberInfo
    {
        [SqlParameter("GeoMessageMemberInfoID", SqlDbType.Int)]
        [SqlPrimaryKey]
        public Int32 ID { get; set; }

        [SqlParameter("MessageID", SqlDbType.Int)]
        [SqlForeignKey]
        public Int32 MessageID { get; set; }

        [SqlParameter("MessageCreatorID", SqlDbType.Int)]
        [SqlForeignKey]
        public Int32 MessageCreatorID { get; set; }

        [SqlParameter("MessageReceiverID", SqlDbType.Int)]
        [SqlForeignKey]
        public Int32 MessageReceiverID { get; set; }
    }
}
