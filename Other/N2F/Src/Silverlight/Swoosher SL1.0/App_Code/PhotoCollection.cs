using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using N2F.Services;

/// <summary>
/// Summary description for PhotoCollection
/// </summary>
[WebService( Namespace = "http://next2friends.com/" )]
[WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
[ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class PhotoCollection : System.Web.Services.WebService
{
	private CookieContainer cookieJar = null;

	public PhotoCollection()
	{

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}

	[WebMethod]
	[ScriptMethod( ResponseFormat = ResponseFormat.Json )]
	public string Hello()
	{
		return "Hello";
	}

	[WebMethod( EnableSession = true )]
	[ScriptMethod( ResponseFormat = ResponseFormat.Json )]
	public string Login( string email, string password )
	{
//		return "dummy";
		return Service.Login( email, password );
	}

	[WebMethod( EnableSession = true )]
	[ScriptMethod( ResponseFormat = ResponseFormat.Json )]
	public PhotoCollectionItem[] GetCollections()
	{
//		return new PhotoCollectionItem[] { new PhotoCollectionItem { Name = "a", WebPhotoCollectionID = "a" } };
		return Service.GetCollections();
	}

	[WebMethod( EnableSession = true )]
	[ScriptMethod( ResponseFormat = ResponseFormat.Json )]
	public PhotoItem[] GetPhotosByCollection( string webPhotoCollectionID )
	{
/*		var items = new List<PhotoItem>();
		var flickr = new Flickr();

		foreach ( var item in flickr.GetPhotos( "aef42e1856b3cb10604d5fdc9bcbe3c3", "waterfall" ).Take( 27 ) )
		{
			items.Add( new PhotoItem
			{
				WebPhotoID = item[ 2 ],
				MainPhotoURL = item[ 0 ]
			} );
		}

		return items.ToArray();
*/
		return Service.GetPhotosByCollection( webPhotoCollectionID );
	}

	protected PhotoOrganise Service
	{
		get
		{
			var cookieJar = (CookieContainer)Session[ "cookieJar" ];
			if ( cookieJar == null ) Session[ "cookieJar" ] = cookieJar = new CookieContainer();

			var service = new PhotoOrganise();
			service.CookieContainer = cookieJar;

			return service;
		}
	}
}