function VMPlayer( config, VideoFile, newQueryParams, newAffiliateId, newVideoUrl, newPreviewBid )
{
    this.width = "100%";
    this.height = "100%";
    this.target = null;
    this.rootFile = "VMPlayer.swf";
    this.autoPlay = null;
    this.VideoFile = VideoFile;
    this.URL = this.rootFile + '?token=' + this.VideoFile + "&wmode=transparent"
    this.queryParams = escape( newQueryParams );
    this.affiliateId = newAffiliateId;
    this.videoUrl = newVideoUrl;
    this.previewBid = newPreviewBid;

    this.domainUrl = null;
    this.rootUrl = null;

    /**
     * covers the params:
     * => width
     * => height
     * => target
     * => rootFile
     */
    if ( this.isValueSet( config ) ) {
        var args = config.split( ',' );
        for ( var i = 0; i < args.length; i++ ) {
            var param = args[ i ].split( '=' );
            eval( "this." + param[ 0 ] + " = '" + param[ 1 ] + "'" );
        }
    }

    this.setWidth = function( newWidth ) {
        this.width = newWidth;
    }

    this.setHeight = function( newHeight ) {
        this.height = newHeight;
    }

    this.setTarget = function( newTarget ) {
        this.target = newTarget;
    }

    this.getFlashPlayerEmbed = function( ) {
        var output = '<embed src="'+this.URL+'" '
            + 'quality="best" '
            + 'bgcolor="" '
            + 'menu="true" '
            + 'width="' + this.width + '" '
            + 'height="' + this.height + '" '
            + 'name="root" '
            + 'id="root" '
            + 'align="middle" '
            + 'scaleMode="noScale" '
            + 'allowScriptAccess="always" '
            + 'allowFullScreen="true" '
            + 'wmode="transparent"'
            + 'type="application/x-shockwave-flash" '
            + 'pluginspage="http://www.macromedia.com/go/getflashplayer">'
            + '</embed>';

        return output;
    }

    this.writeBlogFlashPlayer = function() {
        var output = this.getFlashPlayerEmbed(  );
	    var div_block = document.getElementById( this.target );
        
	    div_block.value = output;
    }

    this.writeFlashPlayer = function() {
        var output = this.getFlashPlayerEmbed(  );
	    var div_block = document.getElementById( this.target );

	    div_block.innerHTML = output;
    }
}

VMPlayer.prototype.isValueSet = function( value ) {
    if ( value == null
      || value == 'null'
      || typeof( value ) == 'undefined'
      || value == '' )
    {
        return false;
    }
    return true;
}

function getDomain() {
    if ( this.domainUrl == null ) {
        this.domainUrl = String( document.location );
        while ( this.domainUrl.lastIndexOf( '/' ) > 8 ) {
            this.domainUrl = this.domainUrl.substring( 0, this.domainUrl.lastIndexOf( '/' ) );
        }
    }
    return this.domainUrl;
}
