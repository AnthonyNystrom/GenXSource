/// <reference path="intellisense.js" />

if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.Scene = function( photos )
{
	this.photos = photos;
}

Swoosher_SL_1_0.Scene.prototype =
{
	photos: null,
	swoosher: null,

	handleLoad: function(plugIn, userContext, rootElement) 
	{
		this.plugIn = plugIn;
		
		this.swoosher = new Swoosher_SL_1_0.Swoosher( plugIn, this.photos, Silverlight.createDelegate( this, function( sender, args )
		{
			var xaml = args.xaml;
			var container = sender.FindName( "swoosher" );
			container.Children.Add( xaml );
		} ) );

		plugIn.content.onResize = Silverlight.createDelegate( this, this.onResize );
		plugIn.content.onFullscreenChange = Silverlight.createDelegate( this, this.onFullscreenChange );
	},

	onResize: function( sender, args )
	{
		this.swoosher.resized = true;
	},

	onFullscreenChange: function( sender, args )
	{
		this.swoosher.resized = true;
	}
}