using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

/// <summary>
/// Summary description for Flickr
/// </summary>
[WebService( Namespace = "http://www.next2friends.com/" )]
[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Flickr : System.Web.Services.WebService
{
	public Flickr()
	{

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}

	[WebMethod]
	[ScriptMethod]
	public List<string[]> GetPhotos( string apiKey, string tags )
	{
		FlickrNet.Flickr.CacheDisabled = true;
		FlickrNet.Flickr flickr = new FlickrNet.Flickr( apiKey );

		FlickrNet.PhotoSearchOptions options = new FlickrNet.PhotoSearchOptions();
		options.Tags = tags;
		options.Extras = FlickrNet.PhotoSearchExtras.All;
		options.PerPage = 343;
		options.Text = tags;
		options.TagMode = FlickrNet.TagMode.AllTags;

		List<string[]> photos = new List<string[]>();

		foreach ( FlickrNet.Photo photo in flickr.PhotosSearch( options ).PhotoCollection )
		{
			photos.Add( new string[] {
                    photo.MediumUrl,
                    photo.WebUrl,
                    photo.Title,
                    System.Xml.XmlConvert.ToString( photo.DateTaken, System.Xml.XmlDateTimeSerializationMode.Utc ),
                    photo.OwnerName,
                    photo.Latitude.ToString(),
                    photo.Longitude.ToString() } );
			//                try { photos.Add( new Photo( photo, "{0}" ) ); }
			//                catch { }
		}

		return photos;
	}
}