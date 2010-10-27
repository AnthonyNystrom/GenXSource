using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Next2Friends.Soap2Bin.Core.Properties;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Writes primitive types in binary to a stream and supports writing strings in Unicode.
    /// </summary>
    public sealed class DataOutputStream : IDisposable
    {
        private Byte[] _buffer;
        private Byte[] _largeBuffer;
        private Stream _output;
        private Encoding _encoding;
        private Encoder _encoder;
        private Int32 _maxChars;
        private const Int32 _largeBufferSize = 256;

        /// <summary>
        /// Creates a new instance of the <code>DataOutputStream</code> class.
        /// </summary>
        /// <param name="output">Specifies the underlying stream.</param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>output</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the stream does not support writing, or the stream is already closed.
        /// </exception>
        public DataOutputStream(Stream output)
        {
            if (output == null)
                throw new ArgumentNullException("output");
            if (!output.CanWrite)
                throw new ArgumentException(Resources.Argument_StreamNotWritable);
            _output = output;
            _buffer = new Byte[16];
            _encoding = Encoding.Unicode;
            _encoder = _encoding.GetEncoder();
            _maxChars = _largeBufferSize / _encoding.GetMaxCharCount(1);
        }

        /// <summary>
        /// Closes the current <code>DataOutputStream</code> and the underlying stream.
        /// </summary>
        public void Close()
        {
            Dispose(true);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Clears all buffers for the current writer and causes any buffered data to be written to the underlying device. 
        /// </summary>
        public void Flush()
        {
            _output.Flush();
        }

        /// <summary>
        /// Writes a one-byte <code>InteropBoolean</code> value to the current stream,
        /// with 0 representing <code>false</code> and <code>1</code> representing true.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public void WriteBoolean(InteropBoolean value)
        {
            _output.WriteByte(value ? (Byte)1 : (Byte)0);
        }

        /// <summary>
        /// Writes an <code>InteropByte</code> to the current stream and advances the stream position by one byte.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public void WriteByte(InteropByte value)
        {
            _output.WriteByte((Byte)value);
        }

        /// <summary>
        /// Writes an <code>InteropFloat</code> to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public void WriteFloat(InteropFloat value)
        {
            var num = value.Value;
            _buffer[0] = (Byte)num;
            _buffer[1] = (Byte)(num >> 8);
            _buffer[2] = (Byte)(num >> 0x10);
            _buffer[3] = (Byte)(num >> 0x18);
            _output.Write(_buffer, 0, 4);
        }

        /// <summary>
        /// Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public void WriteInt16(InteropInt16 value)
        {
            _buffer[0] = (Byte)value;
            _buffer[1] = (Byte)(value >> 8);
            _output.Write(_buffer, 0, 2);
        }

        /// <summary>
        /// Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        public void WriteInt32(InteropInt32 value)
        {
            _buffer[0] = (Byte)value;
            _buffer[1] = (Byte)(value >> 8);
            _buffer[2] = (Byte)(value >> 0x10);
            _buffer[3] = (Byte)(value >> 0x18);
            _output.Write(_buffer, 0, 4);
        }

        /// <summary>
        /// Writes a Unicode string to this stream, and advances the current position of the stream in accordance with the specific characters being written to the stream.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>value</code> is <code>null</code>.
        /// </exception>
        public unsafe void WriteString(InteropString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            var chars = value.ToString().ToCharArray();

            foreach (var singleChar in chars)
            {
                Int16 valueToWrite = (Int16)singleChar;
                WriteInt16(valueToWrite);
            }
            WriteInt16(0);
        }

        public void WriteDateTime(DateTime value)
        {
            WriteString(String.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second));
        }

        private void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_output != null)
                    _output.Close();
            }
        }
    }
}
