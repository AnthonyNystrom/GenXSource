using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Next2Friends;
using Next2Friends.SwoosherHost.PhotoOrganize;

namespace Next2Friends.SwoosherHost
{
	[Scriptable]
	public partial class Page : Canvas
	{
		private PhotoCollection service;

		public void Page_Loaded( object o, EventArgs e )
		{
			// Required to initialize variables
			InitializeComponent();

			BrowserHost.Resize += delegate { Resize(); };

			swoosher.Fullscreen += delegate { BrowserHost.IsFullScreen = true; Resize(); };
			swoosher.Windowed += delegate { BrowserHost.IsFullScreen = false; Resize(); };

			WebApplication.Current.RegisterScriptableObject( "SwoosherPage", this );

			var args = WebApplication.Current.StartupArguments;
			var maxItems = args.ContainsKey( "maxItems" ) ? int.Parse( args[ "maxItems" ] ) : 64;

			if ( args.ContainsKey( "email" ) && args.ContainsKey( "password" ) )
			{
				Service.BeginLogin( args[ "email" ], args[ "password" ], delegate( IAsyncResult result )
				{
					if ( args.ContainsKey( "collectionID" ) ) SetCollection( args[ "collectionID" ], maxItems );
					else GetCollections( maxItems );
				}, null );
			}
			else if ( args.ContainsKey( "collectionID" ) )
			{
				if ( args.ContainsKey( "collectionID" ) ) SetCollection( args[ "collectionID" ], maxItems );
			}
		}

		private void Resize()
		{
			swoosher.Width = Width = BrowserHost.ActualWidth;
			swoosher.Height = Height = BrowserHost.ActualHeight;
		}

		[Scriptable]
		public void StartDemo( string email, string password )
		{
			Service.BeginLogin( email, password, delegate( IAsyncResult result )
			{
				GetCollections( 64 );
			}, null );
		}

		protected void GetCollections( int maxItems )
		{
			Service.BeginGetCollections( delegate( IAsyncResult result )
			{
				var collections = Service.EndGetCollections( result );

				if ( collections.Count() > 0 ) SetCollection( collections.First().WebPhotoCollectionID, maxItems );
			}, null );
		}

		[Scriptable]
		public void SetCollection( string collectionID, int maxItems )
		{
			var baseUri = BaseUri;

			Service.BeginGetPhotosByCollection( collectionID, delegate( IAsyncResult result )
			{
				try
				{
					swoosher.Items = from p in Service.EndGetPhotosByCollection( result ).Take( maxItems )
									 select baseUri + "/Image.ashx?uri="
									   + Convert.ToBase64String( Encoding.UTF8.GetBytes( p.MainPhotoURL ) );
				}
				catch ( Exception ex )
				{
					// TODO: Report error
				}
			}, null );
		}

		protected string BaseUri
		{
			get
			{
				return HtmlPage.DocumentUri.GetComponents( UriComponents.SchemeAndServer, UriFormat.Unescaped )
					+ ( System.IO.Path.GetDirectoryName( HtmlPage.DocumentUri.LocalPath ) ?? "/" ).Replace( '\\', '/' );
			}
		}

		public bool OnExpand( string collectionID )
		{
			// TODO: Something...
			return true;
		}

/*
		protected void GetPhotosByCollection( string collectionID )
		{
			var baseUri = BaseUri;

			Service.BeginGetPhotosByCollection( collectionID, delegate( IAsyncResult result )
			{
				swoosher.Items = from p in Service.EndGetPhotosByCollection( result )
								 select baseUri + "/Image.ashx?uri="
								   + Convert.ToBase64String( Encoding.UTF8.GetBytes( p.MainPhotoURL ) );
			}, null );
		}
*/
		protected PhotoCollection Service
		{
			get
			{
				if ( service == null )
				{
					service = new PhotoCollection();
					service.Url = BaseUri + "PhotoCollection.asmx";
				}

				return service;
			}
		}
	}
}