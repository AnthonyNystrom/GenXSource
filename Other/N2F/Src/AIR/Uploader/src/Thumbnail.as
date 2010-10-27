package
{
	import flash.filesystem.File;
	import flash.filesystem.FileMode;
	import flash.filesystem.FileStream;
	import flash.utils.ByteArray;
	import flash.utils.Endian;

	// Thumbnail extractor based on http://blog.kevinhoyt.org/2007/07/06/exif-thumbnails-and-the-air-bus-tour/
	public class Thumbnail
	{
		public static function Extract( file : File, listener : Function ) : void
		{
			var stream : FileStream = new FileStream();

			stream.open( file, FileMode.READ );

			try
			{
				listener.call( file, file, Exif.Parse( stream ) );
			}
			catch ( e : Error )
			{
				listener.call( file, file, null );
			}

			stream.close();
		}	
	}
}