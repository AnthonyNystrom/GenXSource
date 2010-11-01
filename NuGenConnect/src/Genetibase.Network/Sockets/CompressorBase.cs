using System;
using System.IO;

namespace Genetibase.Network.Sockets {
	public abstract class CompressorBase {
		public abstract void DecompressDeflateStream(Stream stream);
		public abstract void DecompressGZipStream(Stream stream);
	}
}