using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace CJC.ContentServer
{
    /// <summary>
    /// Summary description for Photos
    /// </summary>
    [WebService( Namespace = "http://www.chriscavanagh.com/Flickr" )]
    [WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    [ToolboxItem( false )]
    public class Flickr : System.Web.Services.WebService
    {
        [WebMethod]
        public Photo[] GetPhotos( string apiKey, string tags )
        {
            FlickrNet.Flickr.CacheDisabled = true;
            FlickrNet.Flickr flickr = new FlickrNet.Flickr( apiKey );

            FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
            options.Tags = tags;
            options.Extras = FlickrNet.PhotoSearchExtras.DateTaken | FlickrNet.PhotoSearchExtras.Geo | FlickrNet.PhotoSearchExtras.OwnerName;
			options.PerPage = 1000;
			options.Text = tags;
			options.TagMode = FlickrNet.TagMode.AllTags;

            List<Photo> photos = new List<Photo>();

            foreach ( FlickrNet.Photo photo in flickr.PhotosSearch( options ).PhotoCollection )
            {
				try { photos.Add( new Photo( photo, "{0}" ) ); }
				catch { }
            }

            return photos.ToArray();
        }
    }
}
