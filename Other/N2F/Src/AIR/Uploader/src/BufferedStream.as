package
{
	import flash.filesystem.FileStream;
	import flash.net.URLStream;
	import flash.utils.ByteArray;
	
	public class BufferedStream
	{
		public var stream : Object;
		private var _position : uint;
		private var buffer : ByteArray;

		public function BufferedStream( stream : Object ) : void
		{
			this.stream = stream;
			this._position = ( stream is FileStream ) ? (stream as FileStream).position : 0;
			this.buffer = new ByteArray();
		}

		public function get position() : uint
		{
			return _position;
		}

		public function set position( position : uint ) : void
		{
			if ( stream is FileStream ) (stream as FileStream).position = position;
			else if ( position > _position )
			{
				var bytes : ByteArray = new ByteArray();
				stream.readBytes( bytes, 0, Math.min( position - buffer.position, stream.bytesAvailable ) );
				buffer.position = buffer.length;
				buffer.writeBytes( bytes, 0, bytes.length );
			}

			_position = position;
			buffer.position = position;
		}

		public function readBytes( bytes : ByteArray, offset : uint = 0, length : uint = 0 ) : void
		{
			if ( stream is FileStream )
			{
				(stream as FileStream).readBytes( bytes, offset, length );
			}
			else
			{
				position = position + offset + length;
				buffer.position = buffer.position - offset - length;
				buffer.readBytes( bytes, offset, Math.min( length, buffer.bytesAvailable ) );
			}
		}

		public function getBuffer() : ByteArray
		{
			var length : uint = position + stream.bytesAvailable;
			position = length;
			position = 0;

			var bytes : ByteArray = new ByteArray();
			readBytes( bytes, 0, length );

			return bytes;
		}

		public function close() : void
		{
			if ( stream != null ) stream.close();
			stream = null;
			position = 0;
			buffer = null;
		}
	}
}