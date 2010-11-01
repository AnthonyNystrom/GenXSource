using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Genetibase.Network.Sockets {
	/// <summary>
	/// This class is the base class for all socket implementations. A Socket handles all raw 
	/// reading and writing for a single connection.
	/// </summary>
	public abstract class Socket {

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Socket"/> class.
		/// </summary>
		public Socket()
			: base() {
		}
		
		/// <summary>
		/// The encoding used by this instance.
		/// </summary>
		public Encoding Encoding = Encoding.ASCII;
		/// <summary>
		/// The default value of <see cref="P:Socket.MaxCapturedLines"/>.
		/// </summary>
		/// <value>The default value is -1.</value>
		public const int Default_MaxCapturedLines = -1;
		/// <summary>
		/// The <see cref="T:Buffer"/> instance used to keep track of received bytes.
		/// </summary>
		protected internal Buffer mInputBuffer;
		/// <summary>
		/// The timeout of read operations
		/// </summary>
		protected int mReadTimeout = -1;			
		/// <summary>
		/// Specified whether the connection has been closed gracefully or not.
		/// </summary>
		protected bool mClosedGracefully = false;

		/// <summary>
		/// The size of the <see cref="InputBuffer"/>. Default value is 16384.
		/// </summary>
		protected int mRecvBufferSize = 16 * 1024;
		/// <summary>
		/// The size of the send buffer. Default value is 16384.
		/// </summary>
		protected int mSendBufferSize = 16 * 1024;
		/// <summary>
		/// The maximum amount of lines captured by <see cref="Capture"/>. For default value, see
		/// <see cref="Default_MaxCapturedLines"/>.
		/// </summary>
		protected int mMaxCapturedLines = Default_MaxCapturedLines;

		/// <summary>
		/// Closes this instance.
		/// </summary>
		public virtual void Close() {
			if (mIntercept != null) {
				mIntercept.Disconnect();
			}
			GC.ReRegisterForFinalize(mInputBuffer);
		}

		/// <summary>
		/// Gets the input buffer.
		/// </summary>
		/// <value>The input buffer.</value>
		public Buffer InputBuffer {
			get {
				return mInputBuffer;
			}
		}

		private ConnectionIntercept mIntercept;

		/// <summary>
		/// Gets or sets the intercept.
		/// </summary>
		/// <value>The intercept.</value>
		public ConnectionIntercept Intercept {
			get {
				return mIntercept;
			}
			set {
				mIntercept = value;
			}
		}

		/// <summary>
		/// Opens this instance.
		/// </summary>
		public virtual void Open() {
			mInputBuffer = new Buffer();
			mInputBuffer.Encoding = Encoding;
			if (mIntercept != null) {
				mIntercept.Connect(this);
			}
			GC.SuppressFinalize(mInputBuffer);
		}

		/// <summary>
		/// Transmits the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public virtual void Transmit(ref byte[] data) {
			if (!Connected()) {
				throw new ClosedSocketException("Socket is closed!");
			}
			if (mIntercept != null) {
				mIntercept.Send(ref data);
			}
		}

		/// <summary>
		/// Writes the file.
		/// </summary>
		/// <param name="file">The file.</param>
		public void WriteFile(string file) {
			if (!File.Exists(file)) {
				throw new IndyException("File doesn't exist!");
			}
			Stream LStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			try {
				Write(LStream);
			} finally {
				LStream.Close();
			}
		}

		/// <summary>
		/// Writes the specified A stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public void Write(Stream stream) {
			Write(stream, 0, false);
		}

		/// <summary>
		/// Writes the specified stream. Only the first <paramref name="size"/> number of bytes.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="size">The size.</param>
		public void Write(Stream stream, long size) {
			Write(stream, size, false);
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void Write(long value) {
			Write(value, true);
		}

		/// <summary>
		/// Writes the specified value, optionally converting the value to Network order.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="convert">Whether or not to convert. Conversion is done using <see cref="System.Net.IPAddres.HostToNetworkOrder"/>.</param>
		public void Write(long value, bool convert) {
			if (convert) {
				value = (long)System.Net.IPAddress.HostToNetworkOrder(value);
			}
			Write(BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the specified stream.
		/// </summary>
		/// <param name="stream">The stream of the contents should be send.</param>
		/// <param name="size">The size of the chunk of data to be send.</param>
		/// <param name="writeByteCount">Tf set to <see langword="true"/>, the amount of bytes is first send using <see cref="Write(long)"/>.</param>
		public void Write(Stream stream, long size, bool writeByteCount) {
			byte[] LBuffer = new byte[mSendBufferSize];
			long TempLong;
			int LBufSize;
			if (size < 0) {
				TempLong = stream.Position;
				size = stream.Length;
				stream.Position = TempLong;
				size = size - TempLong;
			} else {
				if (size == 0) {
					size = stream.Length;
					stream.Position = 0;
				}
			}
			if (writeByteCount) {
				Write(size);
			}
#warning TODO: Implement Work
			//      BeginWork(WorkModeEnum.Write, ASize);
			while (size > 0) {
				LBufSize = (int)Math.Min(size, mSendBufferSize);
				Array.Resize<byte>(ref LBuffer, LBufSize);
				LBufSize = stream.Read(LBuffer, 0, LBufSize);
				if (LBufSize == 0) {
					throw new Exception("No data to read");
				}
				//        Work(WorkModeEnum.Write, LBufSize);
				Array.Resize<byte>(ref LBuffer, LBufSize);
				Write(LBuffer);
				size -= LBufSize;
			}
			//      EndWork(WorkModeEnum.Write);
		}

		/// <summary>
		/// Writes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public virtual void Write(byte[] data) {
			Transmit(ref data);
		}

		/// <summary>
		/// Returns whether this instance is connected or not.
		/// </summary>
		/// <returns></returns>
		public virtual bool Connected() {
			CheckForDisconnect(false);
			return (((mClosedGracefully == false) & (mInputBuffer != null)) | ((mInputBuffer != null && mInputBuffer.Size > 0))) & IsOpen();
		}

		/// <summary>
		/// Reads all bytes up to and including the specified <paramref name="endMarker"/>.
		/// </summary>
		/// <param name="endMarker">The end marker.</param>
		/// <returns>The bytes read.</returns>
		public byte[] ReadTo(byte[] endMarker) {
			// MtW: add timeout support
			int mInputBufferIndex = 0;
			while ((mInputBufferIndex = mInputBuffer.IndexOf(endMarker)) == -1) {
				ReadFromSource(false);
				CheckForDisconnect(true, true);
			}
			return mInputBuffer.ExtractToByteArray(mInputBufferIndex + endMarker.Length);
		}

		/// <summary>
		/// Reads the <see cref="System.Int64"/> value and converts it to Host order using <see cref="System.Net.IPAddress.NetworkToHostOrder"/>.
		/// </summary>
		/// <returns>The <see cref="System.Int64"/> value read.</returns>
		public long ReadLong() {
			return ReadLong(true);
		}

		/// <summary>
		/// Reads the <see cref="System.Int64"/> value, optionaly converting it to Host order using <see cref="System.Net.IPAddress.NetworkToHostOrder"/>.
		/// </summary>
		/// <param name="convert">If set to <see langword="true"/>, the result is being converted to Host order.</param>
		/// <returns>The <see cref="System.Int64"/> value read.</returns>
		public long ReadLong(bool convert) {
			byte[] LBytes = ReadBytes(8);
			long TempResult = BitConverter.ToInt64(LBytes, 0);
			if (convert) {
				return (long)System.Net.IPAddress.NetworkToHostOrder(TempResult);
			}
			return TempResult;
		}

		/// <summary>
		/// Reads a certain amount of bytes into the <paramref name="stream"/>. The amount of bytes
		/// is determined by <see cref="M:Socket.ReadLong()"/>.
		/// </summary>
		/// <param name="stream">The <see cref="System.IO.Stream"/> instance to write to.</param>
		public void ReadStream(Stream stream) {
			ReadStream(stream, -1, false);
		}

		/// <summary>
		/// Reads the amount of bytes specified by <paramref name="byteCount"/> into the <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="System.IO.Stream"/> instance to write to.</param>
		/// <param name="byteCount">The amount of bytes to read.</param>
		public void ReadStream(Stream stream, long byteCount) {
			ReadStream(stream, byteCount, false);
		}

		private void AdjustStreamSize(Stream AStream, long ASize) {
			long LStreamPos = AStream.Position;
			AStream.SetLength(ASize);
			AStream.Position = LStreamPos;
		}

		/// <summary>
		/// Reads the amount of bytes specified by <paramref name="byteCount"/>
		/// </summary>
		/// <param name="AStream">The A stream.</param>
		/// <param name="AByteCount">The A byte count.</param>
		/// <param name="AReadUntilDisconnect">if set to <see langword="true"/> [A read until disconnect].</param>
		public virtual void ReadStream(Stream stream, long byteCount, bool readUntilDisconnect) {
			byte[] LBuffer = new byte[0];
			long LBufSize;
			long LWorkCount;
			if (byteCount == -1
				&& !readUntilDisconnect) {
				byteCount = ReadLong();
			}
			if (byteCount > -1) {
				AdjustStreamSize(stream, stream.Position + byteCount);
			}
#warning TODO: add support for Work
			if (readUntilDisconnect) {
				LWorkCount = Int64.MaxValue;
				//        BeginWork(WorkModeEnum.Read);
			} else {
				LWorkCount = byteCount;
				//        BeginWork(WorkModeEnum.Read, LWorkCount);
			}
			try {
				if (InputBuffer.Size > 0) {
					long i = Math.Min((long)mInputBuffer.Size, LWorkCount);
					mInputBuffer.ExtractToStream(stream, (int)i);
					LWorkCount -= i;
				}
				LBufSize = Math.Min(LWorkCount, mRecvBufferSize);
				while (Connected()
						&& LWorkCount > 0) {
					int i = (int)Math.Min(LWorkCount, LBufSize);
					try {
						try {
							Array.Resize<byte>(ref LBuffer, i);
							ReadBytes(ref LBuffer, i, false);
						} catch (Exception E) {
							i = Math.Min(i, InputBuffer.Size);
							LBuffer = InputBuffer.ExtractToByteArray(i);
							if (E is NotEnoughDataInBufferException & byteCount == -1) {
								break;
							}
							if (!(E is Genetibase.Network.Sockets.ConnectionClosedGracefullyException)
								|| !readUntilDisconnect) {
								throw E;
							}
						}
					} finally {
						if (i > 0) {
							stream.Write(LBuffer, 0, i);
							LWorkCount -= i;
						}
					}
				}
			} finally {
				//        EndWork(WorkModeEnum.Read);
				if (stream.Length > stream.Position) {
					stream.SetLength(stream.Position);
				}
				LBuffer = null;
			}
		}

		public virtual void CloseGracefully() {
			mClosedGracefully = true;
		}

		protected virtual void InterceptReceive(ref byte[] data) {
			if (mIntercept != null) {
				mIntercept.Receive(ref data);
			}
		}

		protected void ThrowConnClosedGracefully() {
			/* ************************************************************* 
			------ If you receive an exception here, please read. ----------

			If this is a SERVER
			-------------------
			The client has disconnected the socket normally and this exception is used to notify the
			server handling code. This exception is normal and will not happen while your program is 
			running in realtime.

			If this is a CLIENT
			-------------------
			The server side of this connection has disconnected normaly but your client has attempted
			to read or write to the connection.
  
			// ************************************************************* */
			throw new ConnectionClosedGracefullyException();
		}


		//Kudzu
		// The idea behind IOMethods is this. The Socket has the raw access ones. Ones that can be overridden
		// and implemented by particular socket implementations. Really depending on the implementation this means
		// different things, but it certainly includes all raw IO of chunks of data that happen in one piece. This
		// is especially important for queued handlers like SuperGenetibase.Network.Sockets. Layered methods (read and write) or 
		// methods that interact with external classes should go in a client or in cases of TCPClient itself
		// (as opposed to descendants) an external class that operates on the socket directly. This allows reuse 
		// from clients and servers without forcing them to share a common base.
		//
		// Begin IO Methods
		#region ReadFromSource
		protected abstract int ReadFromSource(
			bool aThrowIfDisconnected
			, int aTimeOut
			, bool AThrowIfTimeout
		);

		protected int ReadFromSource() {
			return ReadFromSource(true, Global.InfiniteTimeOut, true);
		}

		protected int ReadFromSource(bool throwExceptionIfDisconnected) {
			return ReadFromSource(throwExceptionIfDisconnected, Global.InfiniteTimeOut, true);
		}

		protected int ReadFromSource(bool throwExceptionIfDisconnected, int timeOut) {
			return ReadFromSource(throwExceptionIfDisconnected, timeOut, true);
		}
		#endregion

		#region CheckForDisconnect
		public abstract void CheckForDisconnect(
			bool throwExceptionIfDisconnected
			, bool ignoreBuffer
		);

		public void CheckForDisconnect() {
			CheckForDisconnect(true, false);
		}

		public void CheckForDisconnect(
				bool throwExceptionIfDisconnected
			) {
			CheckForDisconnect(throwExceptionIfDisconnected, false);
		}
		#endregion

		#region CheckForDataOnSource
		public abstract void CheckForDataOnSource(
			int aTimeOut
		);

		public void CheckForDataOnSource() {
			CheckForDataOnSource(Global.InfiniteTimeOut);
		}
		#endregion

		public void Write(string text) {
			byte[] data = Encoding.GetBytes(text);
			Transmit(ref data);
		}

		public void WriteLn(string text) {
			Write(text + Global.EOL);
		}

		public virtual string ReadLn() {
                                   
            
            string lResult = "";
			byte[] lBuffer = new byte[1];
			while (true) {
				ReadBytes(ref lBuffer, 1, false);

				if (Encoding.GetString(lBuffer) == "\n") {
					if (lResult.EndsWith("\r")) {
						lResult = lResult.Substring(0, lResult.Length - 1);
					}
					return lResult;
				} else {
					lResult += Encoding.GetString(lBuffer);
				}
			}
		}

		public byte[] ReadBytes(int byteCount) {
			byte[] TempResult = new byte[byteCount];
			ReadBytes(ref TempResult, byteCount, false);
			return TempResult;
		}

		public void ReadBytes(ref byte[] theBuffer, int byteCount, bool append) {
			try {
				if (byteCount > 0) {
					try {
						// Read from stack until we have enough data
						while (mInputBuffer.Size < byteCount) {
							ReadFromSource(false);
							CheckForDisconnect(true, true);
						}
					} catch (ConnectionClosedGracefullyException) {
					}
					mInputBuffer.ExtractToByteArray(ref theBuffer, byteCount, append);
				} else {
					if (byteCount == -1) {
						ReadFromSource(false, mReadTimeout, false);
						CheckForDisconnect(true, true);
						mInputBuffer.ExtractToByteArray(ref theBuffer, -1, append);
					}
				}
			} catch (ConnectionClosedGracefullyException) {
			}
		}

		public void Capture(Stream ADest) {
			int LLineCount = 0;
			PerformCapture(ADest, ref LLineCount, ".", true);
		}

		public void Capture(Stream ADest, string ADelim) {
			Capture(ADest, ADelim, true);
		}

		public void Capture(Stream ADest, string ADelim, bool AIsRFCMessage) {
			int LLineCount = 0;
			PerformCapture(ADest, ref LLineCount, ADelim, AIsRFCMessage);
		}

		public void Capture(Stream ADest, out int VLineCount) {
			Capture(ADest, out VLineCount, ".", true);
		}

		public void Capture(Stream ADest, out int VLineCount, string ADelim) {
			Capture(ADest, out VLineCount, ADelim, true);
		}

		public void Capture(Stream ADest, out int VLineCount, string ADelim, bool AIsRFCMessage) {
			VLineCount = 0;
			PerformCapture(ADest, ref VLineCount, ADelim, AIsRFCMessage);
		}

		public void Capture(IList<string> ADest) {
			int LLineCount = 0;
			PerformCapture(ADest, ref LLineCount, ".", true);
		}

		public void Capture(IList<string> ADest, string ADelim) {
			Capture(ADest, ADelim, true);
		}

		public void Capture(IList<string> ADest, string ADelim, bool AIsRFCMessage) {
			int LLineCount = 0;
			PerformCapture(ADest, ref LLineCount, ADelim, AIsRFCMessage);
		}

		public void Capture(IList<string> ADest, out int VLineCount) {
			Capture(ADest, out VLineCount, ".", true);
		}

		public void Capture(IList<string> ADest, out int VLineCount, string ADelim) {
			Capture(ADest, out VLineCount, ADelim, true);
		}

		public void Capture(IList<string> ADest, out int VLineCount, string ADelim, bool AIsRFCMessage) {
			VLineCount = 0;
			PerformCapture(ADest, ref VLineCount, ADelim, AIsRFCMessage);
		}

		protected virtual void PerformCapture(object ADest, ref int VLineCount, string ADelim, bool AIsRFCMessage) {
			if (ADest == null) {
				return;
			}
			string s = "";
			Stream LStream = null;
			IList<string> LStrings = null;
			VLineCount = 0;
			try {
				if (ADest is IList<string>) {
					LStrings = (IList<string>)ADest;
				} else {
					if (ADest is Stream) {
						LStream = (Stream)ADest;
					} else {
						throw new IndyException(ResourceStrings.ObjectTypeNotSupported);
					}
				}
#warning TODO: add Work support
				//        BeginWork(WorkModeEnum.Read);
				try {
					do {
						s = ReadLn();
						if (s.Equals(ADelim)) {
							return;
						} else {
							string TempString = ADelim + Global.CR;
							if (s == TempString) {
								return;
							}
						}
						if (mMaxCapturedLines > 0) {
							if (VLineCount > mMaxCapturedLines) {
								throw new IndyException(ResourceStrings.MaximumNumberOfCaptureLineExceeded);
							}
						}
						if (AIsRFCMessage
							&& s.Substring(0, 2) == "..") {
							s.Remove(0, 1);
						}
						VLineCount++;
						if (LStrings != null) {
							LStrings.Add(s);
						} else {
							if (LStream != null) {
								LStream.Write(Encoding.GetBytes(s), 0, Encoding.GetByteCount(s));
							}
						}
					}
					while (true);
				} finally {
					//          EndWork(WorkModeEnum.Read);
				}
			} finally {
				if (LStream != null) {
					LStream.Flush();
				}
			}
		}

		public abstract bool IsOpen();

		#region EndPoint Property
		protected string mEndPoint;
		public string EndPoint {
			get {
				return mEndPoint;
			}
		}
		#endregion;

		//WriteBuffering

		public int MaxCapturedLines {
			get {
				return mMaxCapturedLines;
			}
			set {
				mMaxCapturedLines = value;
			}
		}
	}
}
