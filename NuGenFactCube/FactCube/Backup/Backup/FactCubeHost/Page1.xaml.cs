using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Genetibase.FactCube;

namespace FactCubeHost
{
	/// <summary>
	/// Interaction logic for Page1.xaml
	/// </summary>

	public partial class Page1 : System.Windows.Controls.Page
	{
        private const int maxSize = 8;
        private delegate void BindDataDelegate();

        public Page1()
		{
			InitializeComponent();
		}

        protected void search_Click( object sender, RoutedEventArgs e )
        {
			if ( !string.IsNullOrEmpty( search.Text ) )
			{
				searchButton.IsEnabled = false;
				search.IsEnabled = false;
				ThreadPool.QueueUserWorkItem( new WaitCallback( Query ), search.Text );
			}
        }

        private void Query( object tags )
        {
			Flickr.Flickr flickr = new Flickr.Flickr();
			string path = new Uri( flickr.Url ).GetComponents( UriComponents.Path, UriFormat.Unescaped ).Replace( "Flickr.asmx", "" );

			if ( ApplicationDeployment.IsNetworkDeployed
				&& ApplicationDeployment.CurrentDeployment != null
				&& ApplicationDeployment.CurrentDeployment.ActivationUri != null )
			{
				Uri activationUri = ApplicationDeployment.CurrentDeployment.ActivationUri;
				path = ""; // string.Join( "", activationUri.Segments, 1, activationUri.Segments.Length - 2 );
				flickr.Url = activationUri.GetLeftPart( UriPartial.Authority ) + "/Chris/Flickr.asmx";
			}

			Flickr.Photo[] photos = flickr.GetPhotos( "bb8dbf9c10530f2e9c9ea67a589474b0", (string)tags );

			string uriFormat = string.Format( "pack://siteoforigin:,,,/{0}{{0}}/Image.ashx", path );

			Facts facts = PrepareFacts( photos, uriFormat );

			Dispatcher.Invoke( DispatcherPriority.Normal, (BindDataDelegate)delegate()
				{
					factCube.DataContext = facts;
					searchButton.IsEnabled = true;
					search.IsEnabled = true;
				} );
        }

		private Facts PrepareFacts( Flickr.Photo[] photos, string uriFormat )
		{
			Facts facts = new Facts();

			foreach ( Flickr.Photo photo in photos )
			{
				if ( !string.IsNullOrEmpty( photo.Title )
					&& !string.IsNullOrEmpty( photo.OwnerName ) )
				{
					string title = photo.Title;
					string owner = photo.OwnerName[ 0 ].ToString();
					DateTime date = new DateTime( photo.DateTaken.Year, 1, 1 );

					Facts.WhatRow what = facts.What.FindByID( title );
					Facts.WhereRow where = facts.Where.FindByID( owner );
					Facts.WhenRow when = facts.When.FindByID( date );

					if ( ( what != null || facts.What.Count < maxSize )
						&& ( where != null || facts.Where.Count < maxSize )
						&& ( when != null || facts.When.Count < maxSize ) )
					{
						if ( what == null ) what = facts.What.AddWhatRow( title );
						if ( where == null ) where = facts.Where.AddWhereRow( owner );
						if ( when == null ) when = facts.When.AddWhenRow( date );

						string uri = string.Format( uriFormat,
							Uri.EscapeDataString( Convert.ToBase64String( Encoding.UTF8.GetBytes( photo.ThumbnailUrl ) ) ) );

						if ( facts.Fact.FindByWhatWhereWhen( what.ID, where.ID, when.ID ) == null )
						{
							facts.Fact.AddFactRow( what, where, when, uri );
						}
					}
				}
			}

			return facts;
		}
	}
}