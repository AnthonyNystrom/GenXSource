using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Serialization;

namespace CJC.ContentServer
{
    [Serializable]
    public class Photo
    {
        [XmlAttribute]
        public string OwnerName;
        [XmlAttribute]
        public DateTime DateTaken;
        [XmlAttribute]
        public decimal Latitude;
        [XmlAttribute]
        public decimal Longitude;
        [XmlAttribute]
        public string Title;
        [XmlAttribute]
        public string ThumbnailUrl;

        public Photo()
        {
        }

        public Photo( FlickrNet.Photo photo, string urlFormat )
        {
            OwnerName = photo.OwnerName;
            DateTaken = photo.DateTaken;
            Latitude = photo.Latitude;
            Longitude = photo.Longitude;
            Title = photo.Title;
            ThumbnailUrl = string.Format( urlFormat, Uri.EscapeUriString( photo.ThumbnailUrl ) );
        }
    }
}
