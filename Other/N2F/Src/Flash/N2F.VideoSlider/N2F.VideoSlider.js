function VideoSlider()
{
}

VideoSlider.prototype =
{
	plugin: null,
	selectHandler: null,

	// Initialize the video slider
	init: function( plugin, selectHandler )
	{
		this.plugin = plugin;
		this.selectHandler = selectHandler;
	},

	// Insert a slider item at a specified index
	insert: function( index, uniqueId, url, thumbnailUrl, title, isLive, isArchived, tag )
	{
		if ( !this.plugin ) return null;

		return this.plugin.insert(
			this.create( url, thumbnailUrl, title, isLive, isArchived, uniqueId, tag ),
			index );
	},

	// Add a new slider item
	add: function( uniqueId, url, thumbnailUrl, title, isLive, isArchived, tag )
	{
		if ( !this.plugin ) return null;

		return this.plugin.add(
			this.create( url, thumbnailUrl, title, isLive, isArchived, uniqueId, tag ) );
	},

	remove: function( uniqueId )
	{
		if ( !this.plugin ) return;
		return this.plugin.remove( uniqueId );
	},

	// Create a new slider item (internal)
	create: function( url, thumbnailUrl, title, isLive, isArchived, uniqueId, tag )
	{
		return {
			thumbnailUrl: thumbnailUrl,
			url: url,
			title: title,
			isLive: isLive,
			isArchived: isArchived,
			uniqueId: uniqueId,
			tag: tag
		};
	}
};

var videoSlider = new VideoSlider();

function selectThumbnail( item )
{
	videoSlider.selectHandler( item );
}