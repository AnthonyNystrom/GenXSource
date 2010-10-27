using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using N2F.Services.PhotoOrganise;

[ServiceContract( Namespace = "" ), ServiceBehavior( IncludeExceptionDetailInFaults = true )]
[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
public class Photos
{
	// Add [WebGet] attribute to use HTTP GET
	[OperationContract]
	public PhotoItem[] GetPhotos( string username, string password )
	{
		return GetPhotoItems( username, password ).ToArray();
	}

	private IEnumerable<PhotoItem> GetPhotoItems( string username, string password )
	{
		var service = new PhotoOrganise();
		service.CookieContainer = new CookieContainer();
		var loginResult = service.Login( username, password );

		foreach ( var collection in service.GetCollections() )
		{
			foreach ( var photo in service.GetPhotosByCollection( collection.WebPhotoCollectionID ) )
			{
				yield return photo;
			}
		}
	}
}