using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Genetibase.Network.Sockets {
	/// <summary>
	/// This class contains data for the <see cref="E:Buffer.BytesAdded"/> and <see cref="E:Buffer.BytesRemoved"/>
	/// events.
	/// </summary>
  public sealed class BufferEventArgs : EventArgs {
    private int mByteCount;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:BufferEventArgs"/> class.
		/// </summary>
		/// <param name="aCount">The amount of bytes.</param>
    public BufferEventArgs(int aCount) {
      mByteCount = aCount;
    }

		/// <summary>
		/// Gets the amount of bytes involved in the action on the <see cref="T:Buffer"/>.
		/// </summary>
		/// <value>The byte count.</value>
    public int ByteCount {
      get {
        return mByteCount;
      }
    }
  }

  public sealed class Buffer : IDisposable {
    private List<byte> bytes;
    private int growthFactor;
    private int headIndex;
    private EventHandler<BufferEventArgs> bytesRemoved = null;
    private EventHandler<BufferEventArgs> bytesAdded = null;
    private int size;
		private int mMaximumSize = Int32.MaxValue;
    private Encoding encoding = Encoding.ASCII;

    /// <summary>
    /// This event gets fired when there are bytes removed from this <see cref="T:Buffer"/> 
		/// instance.
    /// </summary>
    public event EventHandler<BufferEventArgs> BytesRemoved {
      add {
        bytesRemoved += value;
      }
      remove {
        bytesRemoved -= value;
      }
    }

		/// <summary>
		/// This event gets fired when there are bytes added to this <see cref="T:Buffer"/> 
		/// instance.
		/// </summary>
    public event EventHandler<BufferEventArgs> BytesAdded {
      add {
        bytesAdded += value;
      }
      remove {
        bytesAdded -= value;
      }
    }

		/// <summary>
		/// Gets or sets the encoding used when <see cref="T:System.String"/> to <see cref="T:System.Byte[]"/> conversions (or vice-versa) need to be done.
		/// </summary>
		/// <value>The encoding.</value>
    public Encoding Encoding {
      get {
        return encoding;
      }
      set {
        encoding = value;
      }
    }

    private void CheckSize(int byteCount) {
			int spaceLeft = mMaximumSize - byteCount;
      if (spaceLeft < Size) {
        throw new TooMuchDataInBufferException();
      }
    }

		private void CheckAvailableBytes(ref int byteCount) {
			if (byteCount == -1) {
				byteCount = Size;
			}
			if (byteCount > Size) {
				throw new NotEnoughDataInBufferException(byteCount, size);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buffer"/> class.
		/// </summary>
    public Buffer() {
      bytes = new List<byte>();
      GrowthFactor = 2048;
      Clear();
    }

    void IDisposable.Dispose() {
      Clear();
      GC.SuppressFinalize(this);
    }

		/// <summary>
		/// Clears this instance.
		/// </summary>
    public void Clear() {
      bytes = new List<byte>();
      headIndex = 0;
      size = 0;
    }

		/// <summary>
		/// Compacts this instance. Clearing means removing the bytes which have been read
		/// already.
		/// </summary>
    public void CompactHead() {
      if (headIndex > 0) {
        lock (bytes) {
          bytes = bytes.GetRange(headIndex, Size);
        }
        headIndex = 0;
      }
    }

		/// <summary>
		/// Extracts all bytes from this instance, and returns it as a string. The
		/// <see cref="T:System.Buffer"/> to <see cref="T:System.String"/> is done using the specified
		/// <see cref="Encoding"/>.
		/// </summary>
		/// <returns>The complete contents of this instance.</returns>
    public string Extract() {
      return Extract(-1);
    }

		/// <summary>
		/// Extracts the <paramref name="byteCount"/> from this instance, and returns it as a string.The
		/// <see cref="T:System.Buffer"/> to <see cref="T:System.String"/> is done using the specified
		/// <see cref="Encoding"/>.
		/// </summary>
		/// <param name="byteCount">The amount of bytes to read.</param>
		/// <returns>The bytes read from this instance, converted to a <see cref="T:System.String"/></returns>
    public string Extract(int byteCount) {
      byte[] LBytes = new byte[0];
      if (byteCount > 0
        || byteCount == -1) {
        ExtractToByteArray(ref LBytes, byteCount);
        return encoding.GetString(LBytes);
      }
      return "";
    }

		/// <summary>
		/// Extracts the complete contents of this instance to a stream
		/// </summary>
		/// <param name="stream">The stream to write the contents of this instance to.</param>
    public void ExtractToStream(Stream stream) {
      ExtractToStream(stream, -1);
    }

		/// <summary>
		/// Extracts the specified <paramref name="byteCount"/> to the specified <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream"/> to write to.</param>
		/// <param name="byteCount">The amount of bytes to extract.</param>
    public void ExtractToStream(Stream stream, int byteCount) {
			CompactHead();
			CheckAvailableBytes(ref byteCount);
			stream.Write(bytes.GetRange(0, byteCount).ToArray(), 0, byteCount);
			Remove(byteCount);
    }

		/// <summary>
		/// Extracts the complete contents of the current instance to the specified <paramref name="buffer"/>.
		/// </summary>
		/// <param name="buffer">The <see cref="T:Buffer"/> instance to write to.</param>
    public void ExtractToBuffer(Buffer buffer) {
			ExtractToBuffer(buffer, -1);
    }

		/// <summary>
		/// Extracts the specified <paramref name="byteCount"/> to the specified <paramref name="buffer"/>.
		/// </summary>
		/// <param name="buffer">The <see cref="T:Buffer"/> to write to.</param>
		/// <param name="byteCount">The amount of bytes to extract.</param>
		public void ExtractToBuffer(Buffer buffer, int byteCount) {
			CheckAvailableBytes(ref byteCount);
			byte[] TempBytes = new byte[byteCount];
			ExtractToByteArray(ref TempBytes, byteCount, false);
			buffer.Write(TempBytes);
    }

		/// <summary>
		/// Extracts the complete contents of this instance and returns it as a <see cref="T:System.Byte[]"/>.
		/// </summary>
		/// <returns>The bytes extracted.</returns>
    public byte[] ExtractToByteArray() {
      byte[] tempResult = new byte[0];
      ExtractToByteArray(ref tempResult);
      return tempResult;
    }

		/// <summary>
		/// Extracts the complete contents of this instance into the specified <paramref name="byteArray"/>.
		/// </summary>
		/// <param name="byteArray">The byte array in which to put the contents of this instance.</param>
    public void ExtractToByteArray(ref byte[] byteArray) {
			ExtractToByteArray(ref byteArray, -1, true);
    }

		/// <summary>
		/// Extracts the specified <paramref name="byteCount"/> and returns it as a byte array.
		/// </summary>
		/// <param name="byteCount">The amount of bytes to read.</param>
		/// <returns>The byte array containing the extracted contents.</returns>
    public byte[] ExtractToByteArray(int byteCount) {
      byte[] tempResult = new byte[byteCount];
      ExtractToByteArray(ref tempResult, byteCount, false);
      return tempResult;
    }

		/// <summary>
		/// Extracts the specified <paramref name="byteCount"/> into the specified <paramref name="byteArray"/>
		/// </summary>
		/// <param name="byteArray">The byte array to store the contents in</param>
		/// <param name="byteCount">The amount of bytes to read.</param>
    public void ExtractToByteArray(ref byte[] byteArray, int byteCount) {
			ExtractToByteArray(ref byteArray, byteCount, true);
    }

		/// <summary>
		/// Extracts the specified <paramref name="byteArray"/> into the specified <paramref name="byteArray"/>,
		/// optionally <paramref name="append"/>ing to the <paramref name="byteArray"/>.
		/// </summary>
		/// <param name="byteArray">The byte array to put the extracted contents in.</param>
		/// <param name="byteCount">The amount of bytes to extract.</param>
		/// <param name="append">Specifies whether to append to <paramref name="byteArray"/> or not.</param>
    public void ExtractToByteArray(ref byte[] byteArray, int byteCount, bool append) {
      int OldSize;
      CheckAvailableBytes(ref byteCount);
      if (byteCount > 0) {
				if (append) {
					OldSize = byteArray.Length;
        } else {
          OldSize = 0;
        }
        byte[] NewArray = new byte[OldSize + byteCount];
				Array.Copy(byteArray, 0, NewArray, 0, OldSize);
				byteArray = NewArray;
        lock (bytes) {
					bytes.CopyTo(headIndex, byteArray, OldSize, byteCount);
        }
        Remove(byteCount);
      }
    }

		/// <summary>
		/// Returns the position of the specified <paramref name="bytes"/>.
		/// </summary>
		/// <param name="bytes">The bytes to look for.</param>
		/// <returns>The position of the specified <paramref name="bytes"/>. This value is zero-based and returns -1 if the specified bytes are not found.</returns>
    public int IndexOf(byte[] bytes) {
			return IndexOf(bytes, 0);
    }

		/// <summary>
		/// Returns the position of the bytes in the specified <paramref name="bytesArray"/>, after the specified <paramref name="startPos"/>.
		/// </summary>
		/// <param name="bytesArray">The bytes to look for.</param>
		/// <param name="startPos">The position at which to start looking for the bytes.</param>
		/// <returns>The position of the specified <paramref name="bytes"/>. This value is zero based and returns -1 if the specified bytes are not found.</returns>
		public int IndexOf(byte[] bytesArray, int startPos) {
      int j = 0;
      bool xFound = true;
			if (bytesArray.Length == 0) {
        throw new IndyException(ResourceStrings.BufferMissingTerminator);
      }
      if (startPos == -1) {
        startPos = 0;
      }
      if (Size == 0) {
        return -1;
      }
      if (startPos < 0 || startPos >= Size) {
        throw new IndyException(ResourceStrings.BufferInvalidStartPos);
      }
			for (int i = headIndex + startPos; i <= bytes.Count - bytesArray.Length; i++) {
        xFound = true;
				for (j = 0; j < bytesArray.Length; j++) {
					if (bytesArray[j].CompareTo(bytes[i + j]) != 0) {
            xFound = false;
            break;
          }
        }
        if (xFound) {
          return i - headIndex;
        }
      }
      return -1;
    }

		/// <summary>
		/// Returns the position of the specified string <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The string to look for. The <see cref="System.String"/> to <see cref="System.Byte[]"/> conversion is done using <see cref="P:Encoding"/>.</param>
		/// <returns>The position of the specified <paramref name="value"/>. This value is zero-based and returns -1 if the specified bytes are not found.</returns>
    public int IndexOf(string value) {
			return IndexOf(value, -1);
    }

		/// <summary>
		/// Returns the position of the specified string <paramref name="value"/>, after the specified <paramref name="startPos"/>.
		/// </summary>
		/// <param name="value">The string to look for. The <see cref="System.String"/> to <see cref="System.Byte[]"/> conversion is done using <see cref="P:Encoding"/>.</param>
		/// <param name="startPos">The at which to start looking for the string.</param>
		/// <returns>The position f the specified <paramref name="value"/>. This value is zero-based and returns -1 if the specified string is not found.</returns>
    public int IndexOf(string value, int startPos) {
			return IndexOf(encoding.GetBytes(value), startPos);
    }

		/// <summary>
		/// Returns the value of the byte at the specified <paramref name="index"/> of this instance, but doesn't remove it from the instance.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The value of the byte at the specified <paramref name="index"/>, or -1 if this instance is empty.</returns>
    public int PeekByte(int index) {
      if (Size == 0) {
        return -1;
      }
      if (index < 0
        || index >= Size) {
        throw new ArgumentOutOfRangeException("index");
      }
      return bytes[headIndex + index];
    }

		/// <summary>
		/// Removes the specified byte count from this instance.
		/// </summary>
		/// <param name="byteCount">The byte count to remove from this instance.</param>
    public void Remove(int byteCount) {
			if (byteCount >= Size) {
        Clear();
      } else {
				headIndex += byteCount;
				size -= byteCount;
        if (headIndex > GrowthFactor) {
          CompactHead();
        }
      }
      if (bytesRemoved != null) {
				bytesRemoved(this, new BufferEventArgs(byteCount));
      }
    }

		/// <summary>
		/// Writes the specified the string to this instance. The <see cref="T:System.String"/> to <see cref="T:System.Byte[]"/> conversion
		/// is done using <see cref="P:Encoding"/>.
		/// </summary>
		/// <param name="theString">The string to write to this instance.</param>
    public void Write(string theString) {
      Write(encoding.GetBytes(theString));
    }

		/// <summary>
		/// Writes the specified bytes to this instance.
		/// </summary>
		/// <param name="theBytes">The bytes to write to this instance.</param>
		public void Write(byte[] theBytes) {
			int ByteLength = theBytes.Length;
			CheckSize(ByteLength);
			CompactHead();
			bytes.AddRange(theBytes);
			size += ByteLength;
			if (bytesAdded != null) {
				bytesAdded(this, new BufferEventArgs(ByteLength));
			}
		}

		/// <summary>
		/// Gets or sets the growth factor. The growth factor is the amount of bytes this instance will grow at once.
		/// </summary>
		/// <value>The growth factor. The default value is 2048.</value>
    public int GrowthFactor {
      get {
        return growthFactor;
      }
      set {
        growthFactor = value;
      }
    }

		/// <summary>
		/// Gets the size of this instance.
		/// </summary>
		/// <value>The size.</value>
    public int Size {
      get {
        return size;
      }
    }

		/// <summary>
		/// Gets or sets the maximum size of this instance.
		/// </summary>
		/// <value>The maximum size.</value>
		public int MaximumSize {
			get {
				return mMaximumSize;
			}
			set {
				mMaximumSize = value;
			}
		}
  }
}