if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.Loading = function( plugIn, onLoad )
{
	UserControl.load( plugIn, "Loading.xaml", true, Silverlight.createDelegate( this, function( sender, args )
	{
		if ( onLoad ) onLoad( sender, args );
		this.handleLoad( plugIn, null, args.xaml );
	} ) );
}

Swoosher_SL_1_0.Loading.prototype =
{
	plugIn: null,
	visual: null,
	stopElement: null,

	handleLoad: function(plugIn, userContext, rootElement) 
	{
		this.plugIn = plugIn;
		this.visual = rootElement;
		this.stopElement = rootElement.Resources.FindName( "stop" );

		this.stopElement.addEventListener( "Completed", Silverlight.createDelegate( this, function( sender, args )
		{
			rootElement.FindName( "rotation" ).Stop();
		} ) );
	},

	stop: function()
	{
		this.stopElement.Begin();
	}
}