using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using N2F.Services;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		ClientScript.RegisterClientScriptBlock( GetType(), "images", GetImageScript(), true );
    }

	private string GetImageScript()
	{
		var list = "";

		foreach ( var photo in GetPhotos().Take( 27 /*64*/ ) )
		{
			var url = "Image.ashx?uri="
				+ Convert.ToBase64String( Encoding.UTF8.GetBytes( photo.MainPhotoURL ) );

			list += string.Format( "{0}{{url:\"{1}\"}}",
				( list.Length > 0 ) ? ",\r\n" : "",
				url );
		}

		return string.Format( "function GetPhotos()\n{{\nreturn [\n{0}\n];\n}};\n", list );
	}

	private IEnumerable<PhotoItem> GetPhotos()
	{
		var service = new PhotoOrganise();
		service.CookieContainer = new CookieContainer();
		var loginResult = service.Login( "anthony@next2friends.com", "tonyrene" );

		foreach ( var collection in service.GetCollections() )
		{
			foreach ( var photo in service.GetPhotosByCollection( collection.WebPhotoCollectionID ) )
			{
				yield return photo;
			}
		}
	}
/*
	private IEnumerable<PhotoItem> GetPhotos()
	{
		var path = @"D:\Pictures\2008-01-15 - 2008-01-27 UK";

		foreach ( var file in System.IO.Directory.GetFiles( path, "*.jpg" ) )
		{
			yield return new PhotoItem
			{
				WebPhotoID = "file://" + file,
				MainPhotoURL = "file://" + file
			};
		}
	}*/
}