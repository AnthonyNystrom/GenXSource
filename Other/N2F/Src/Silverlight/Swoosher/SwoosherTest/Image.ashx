<%@ WebHandler Language="C#" Class="Image" %>

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

public class Image : IHttpHandler
{
	public void ProcessRequest( HttpContext context )
	{
		string uri = Encoding.UTF8.GetString( Convert.FromBase64String( context.Request.QueryString[ "uri" ] ) );
		uri = uri.Replace( "/n2fweb", "" );	// TODO: Remove hack
		WebRequest request = HttpWebRequest.Create( uri );

		WebResponse response = request.GetResponse();

		using ( Stream stream = response.GetResponseStream() )
		{
			context.Response.ContentType = response.ContentType;

			byte[] buffer = new byte[ 102400 ];
			int count;

			while ( ( count = stream.Read( buffer, 0, buffer.Length ) ) > 0 )
			{
				context.Response.OutputStream.Write( buffer, 0, count );
			}
		}
	}

	public bool IsReusable { get { return true; } }
}