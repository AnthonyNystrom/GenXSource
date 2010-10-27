if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.ProgressBar = function( plugIn, onLoad ) 
{
	UserControl.load( plugIn, "ProgressBar.xaml", true, Silverlight.createDelegate( this, function( sender, args )
	{
		if ( onLoad ) onLoad( sender, args );
		this.handleLoad( plugIn, null, args.xaml );
	} ) );
}

Swoosher_SL_1_0.ProgressBar.prototype =
{
	box: null,
	progress: null,

	handleLoad: function(plugIn, userContext, rootElement) 
	{
		this.plugIn = plugIn;
		
		this.box = rootElement.FindName( "box" );
		this.progress = rootElement.FindName( "progress" );
	},

	setProgress: function( progress )
	{
		if ( this.progress )
		{
			this.progress.Width = ( this.box.Width - ( this.box.StrokeThickness * 2 ) )
				* Math.max( 0, Math.min( 1, progress ) );
		}
	},

	getProgress: function()
	{
		return this.progress
			? this.progress.Width / ( this.box.Width - ( this.box.StrokeThickness * 2 ) )
			: 0;
	}
}