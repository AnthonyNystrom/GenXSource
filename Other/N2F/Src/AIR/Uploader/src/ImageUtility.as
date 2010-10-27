package
{
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	import flash.utils.ByteArray;
	
	import mx.controls.Image;
	import mx.graphics.codec.JPEGEncoder;

	public class ImageUtility
	{
		public static function Resize( bitmap : Bitmap, width : int, height : int ) : Bitmap
		{
			var maxSize : Number = Math.sqrt( width * height );
			var size : Number = Math.sqrt( bitmap.width * bitmap.height );

			var scale : Number = Math.min( maxSize / size, 1 );

			width = bitmap.width * scale;
			height = bitmap.height * scale;

			var matrix : Matrix = new Matrix( scale, 0, 0, scale );

			var bitmapData : BitmapData = new BitmapData( width, height, false, 0xF0F0F0 );
			bitmapData.draw( bitmap, matrix, null, null, null, true );

			return new Bitmap( bitmapData, "auto", true );
		}

		public static function Rotate( bitmap : Bitmap, angle : Number ) : Bitmap
		{
			var matrix : Matrix = new Matrix();
			matrix.rotate( angle ); 

			var sizes : Array = [
				new Point( bitmap.width, 0 ),
				new Point( bitmap.width, bitmap.height ),
				new Point( 0, bitmap.height )
			];

			var bounds : Object = { minX: 0, maxX: 0, minY: 0, maxY: 0 };

			for ( var i : int = 0; i < sizes.length; ++ i )
			{
				var point : Point = matrix.transformPoint( sizes[ i ] );
				if ( point.x < bounds.minX ) bounds.minX = point.x;
				if ( point.x > bounds.maxX ) bounds.maxX = point.x;
				if ( point.y < bounds.minY ) bounds.minY = point.y;
				if ( point.y > bounds.maxY ) bounds.maxY = point.y;
			}

			var bitmapData : BitmapData = new BitmapData(
				bounds.maxX - bounds.minX, bounds.maxY - bounds.minY, false, 0xF0F0F0 );
			bitmapData.draw( bitmap, matrix, null, null, null, true );

			return new Bitmap( bitmapData, "auto", true );
		}

		public static function Encode( image : Image, width : int, height : int ) : ByteArray
		{
			var encoder : JPEGEncoder = new JPEGEncoder( 80 );
			return encoder.encode( Resize( image.content as Bitmap, width, height ).bitmapData );
		}
	}
}