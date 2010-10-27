package
{
	import flash.display.*;
	import flash.events.*;
	import flash.filesystem.FileStream;
	import flash.net.*;
	import flash.utils.ByteArray;
	
	public class Exif //extends Sprite {
	{
//		private var image:URLLoader = null;
				
/*		public function Exif( path:String = "YOUR_DEFAULT_TEST_FILE" ) {						
			var request:URLRequest = new URLRequest( path );
			
			image = new URLLoader();
			image.dataFormat = DataFormat.BINARY;
			image.addEventListener( EventType.COMPLETE, parse );
			image.load( request );
		}		
		
		public function parse( event:Event ):Void {*/
		public static function Parse( stream : Object ) : ByteArray
		{
			var contents:Array = null;
			var dump:Array = null;
			var ifd:Array = null;
			var data:ByteArray = new ByteArray();
			var exif:uint = 0;
			var interop:uint = 0;
			var length:uint = 0;
			var tiff:TIFF = null;

			stream.readBytes( data, 0, 12 );

			if( !isJpeg( data ) ) {
//				trace( "Not a JPEG." );
				return null;	
			}

			if ( data[6] == 0x4a && data[7] == 0x46 && data[8] == 0x49 && data[9] == 0x46 )
			{
				// HACK: Skip JFIF header
				stream.position += 6;
				stream.readBytes( data, 0, 12 );
			}

			if( !hasExif( data ) ) {
//				trace( "No EXIF data found." );
				return null;	
			}
			
			data.position = 4;
			length = data.readUnsignedShort();
//			trace( "EXIF header length: " + length + " bytes" );

			stream.readBytes( data, 0, length - 8 );
			
/*			if( data[0] == 73 ) {
				trace( "Intel format" );	
			} else {
				trace( "Motorola format" );
			}*/
			
			tiff = new TIFF( data );
			ifd = tiff.list();
			
			for( var i:int = 0; i < ifd.length; i++ ) {
//				trace( "IFD " + i );
				
/*				if( i == 0 ) {
					trace( "Main image" );	
				} else if( i == 1 ) {
					trace( "Thumbnail image" );
				}*/
				
//				trace( "At offset: " + ifd[i] );
				
				contents = tiff.dump( ifd[i] );
				tiff.print( contents, TIFF.EXIF_TAGS );
				
				exif = 0;

				var thumb : Object = new Object();

				for( var t:int = 0; t < contents.length; t++ )
				{
					var tag : Number = contents[ t ].getTag();

					switch ( tag )
					{
						case 0x201: thumb.position = contents[ t ].getValues()[ 0 ]; break;
						case 0x202: thumb.length = contents[ t ].getValues()[ 0 ]; break;
						case 34665: exif = contents[ t ].getValues()[ 0 ]; break;
					}
				}

				if ( thumb.position && thumb.length )
				{
					var result : ByteArray = new ByteArray();
					data.position = thumb.position;
					data.readBytes( result, 0, thumb.length );
					return result;					
				}

				if( exif != 0 ) {
//					trace( "Exif SubIFD at offset " + exif );	
					contents = tiff.dump( exif );
					tiff.print( contents, TIFF.EXIF_TAGS );
					
					interop = 0;
					
					for( var s:int = 0; s < contents.length; s++ ) {
						if( contents[s].getTag() == 40965 ) {
							interop = contents[s].getValues()[0];
						}	
					}

					if( interop != 0 ) {
//						trace( "Exif Interoperability SubSubIFD at offset " + interop );
						contents = tiff.dump( interop );
						tiff.print( contents, TIFF.INTEROP_TAGS );	
					}					
				}
			}

			return null;
		}
		
		public static function isJpeg( data:ByteArray ):Boolean {
			var jpeg:Boolean = false;
			
			if( data[0] == 255 && data[1] == 216 /*&&
				data[2] == 255 && data[3] == 225*/ ) {
				jpeg = true;	
			}
			
			return jpeg;
		}
		
		public static function hasExif( data:ByteArray ):Boolean {
			var exif:Boolean = false;

			if( data[6] == 69 && data[7] == 120 &&
				data[8] == 105 && data[9] == 102 ) {
				exif = true;		
			}
			
			return exif;	
		}
	}
}
