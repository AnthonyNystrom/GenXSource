/// <reference path="intellisense.js" />
/// <reference path="Sylvester/sylvester.js" />

function BSPImage( plugIn, position, url )
{
	this.position = position;
	this.image = new Swoosher_SL_1_0.SelectableImage( plugIn, url,
		Silverlight.createDelegate( this, function( sender, args )
		{
			this.visual = args.xaml;
			this.loaded = true;
			this.image.click = Silverlight.createDelegate( this, this.onClick );
			this.image.loaded = Silverlight.createDelegate( this, this.onLoaded );
			this.image.failed = Silverlight.createDelegate( this, this.onFailed );
		} ) );
}

BSPImage.prototype =
{
	position: $V( [ 0, 0, 0 ] ),
	image: null,
	visual: null,
	loaded: false,
	itemClick: null,
	itemLoaded: null,
	itemFailed: null,
	visited: false,
	selected: false,
	viewDistance: 1.5,

	intersects: function( plane )
	{
		return plane.distance( this.position );
//		return plane.distanceFromRel( this.position );
	},

	bounds: function()
	{
		return { min: this.position.dup(), max: this.position.dup() };
	},

	onClick: function( sender, e )
	{
		if ( this.itemClick != null ) this.itemClick( this, e );
	},

	onLoaded: function( sender, e )
	{
		if ( this.itemLoaded != null ) this.itemLoaded( this, e );
	},

	onFailed: function( sender, e )
	{
		if ( this.itemFailed != null ) this.itemFailed( this, e );
	},

	refreshState: function()
	{
		this.image.SetDimmed( this.visited && !this.selected );
	}
};