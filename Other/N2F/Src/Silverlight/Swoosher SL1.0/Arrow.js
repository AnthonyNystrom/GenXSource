if (!window.Swoosher_SL_1_0)
	window.Swoosher_SL_1_0 = {};

Swoosher_SL_1_0.Arrow = function( plugIn, x, angle, onLoad ) 
{
	UserControl.load( plugIn, "Arrow.xaml", true, Silverlight.createDelegate( this, function( sender, args )
	{
		args.xaml.SetValue( "Canvas.Left", x );
		if ( onLoad ) onLoad( sender, args );
		this.handleLoad( plugIn, angle, args.xaml );
	} ) );
}

Swoosher_SL_1_0.Arrow.prototype =
{
	root: null,
	click: null,

	handleLoad: function(plugIn, rotationAngle, rootElement) 
	{
		this.plugIn = plugIn;
		this.root = rootElement;

		var content = rootElement.FindName( "content" );
		content.addEventListener( "MouseEnter", Silverlight.createDelegate( this, this.mouse_Enter ) );
		content.addEventListener( "MouseLeave", Silverlight.createDelegate( this, this.mouse_Leave ) );
		content.addEventListener( "MouseLeftButtonUp", Silverlight.createDelegate( this, this.mouse_LeftButtonUp ) );

		var angle = rootElement.FindName( "angle" );
		angle.Angle = rotationAngle;
	},

	beginStoryboard: function( name )
	{
		this.root.Resources.FindName( name ).Begin();
	},

	stopStoryboard: function( name )
	{
		this.root.Resources.FindName( name ).Stop();
	},

	mouse_Enter: function( sender, args )
	{
		this.beginStoryboard( "highlight" );
	},

	mouse_Leave: function( sender, args )
	{
		this.beginStoryboard( "dim" );
	},

	mouse_LeftButtonUp: function( sender, args )
	{
		if ( this.click ) this.click( this, args );
	}
}