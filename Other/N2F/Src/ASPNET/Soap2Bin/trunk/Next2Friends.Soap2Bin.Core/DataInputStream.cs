using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Next2Friends.Soap2Bin.Core.Properties;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Next2Friends.Soap2Bin.Core
{
    /// <summary>
    /// Provides functionality to read big-endian stream.
    /// </summary>
    public sealed class DataInputStream : IDisposable
    {
        private Byte[] _buffer;
        private Stream _stream;

        /// <summary>
        /// Creates a new instance of the <code>DataInputStream</code> class
        /// </summary>
        /// <param name="input">Specifies the nderlying stream.</param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <code>input</code> is <code>null</code>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <code>input</code> stream is not readable, or the stream is already closed.
        /// </exception>
        public DataInputStream(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (!input.CanRead)
                throw new ArgumentException(Resources.Argument_StreamNotReadable);

            _stream = input;
            _buffer = new Byte[16];
        }

        /// <summary>
        /// Closes the current reader and the underlying stream.
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
        /// Reads the next byte from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// If the end of stream is reached.
        /// </exception>
        public InteropByte ReadByte()
        {
            return (SByte)ReadByte(_stream);
        }

        /// <summary>
        /// Reads an <code>InteropBoolean</code> value from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// If the end of stream is reached.
        /// </exception>
        public InteropBoolean ReadBoolean()
        {
            return ReadByte(_stream) == 1 ? true : false;
        }

        /// <summary>
        /// Reads an <code>InteropFloat</code> value from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns></returns>
        public InteropFloat ReadFloat()
        {
            FillBuffer(_buffer, _stream, 4);
            return new InteropFloat() { Value = (_buffer[0] | (_buffer[1] << 8) | (_buffer[2] << 0x10) | (_buffer[3] << 0x18)) };
        }

        /// <summary>
        /// Reads a 2-Byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// If the end of stream is reached.
        /// </exception>
        public InteropInt16 ReadInt16()
        {
            FillBuffer(_buffer, _stream, 2);
            return (Int16)(_buffer[0] | (_buffer[1] << 8));
        }

        /// <summary>
        /// Reads a 4-Byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">
        /// If the stream is closed.
        /// </exception>
        /// <exception cref="IOException">
        /// If an I/O error occurs.
        /// </exception>
        /// <exception cref="EndOfStreamException">
        /// If the end of stream is reached.
        /// </exception>
        public InteropInt32 ReadInt32()
        {
            FillBuffer(_buffer, _stream, 4);
            return _buffer[0] | (_buffer[1] << 8) | (_buffer[2] << 0x10) | (_buffer[3] << 0x18);
        }

        public InteropString ReadString()
        {
            if (_stream == null)
                throw new ObjectDisposedException(Resources.ObjectDisposed_FileClosed);
            
            var builder = new StringBuilder();
            var currentValue = -1;

            while (currentValue != 0)
            {
                currentValue = ReadInt16();
                if (currentValue != 0)
                    builder.Append(Convert.ToChar(currentValue));
            }

            return builder.ToString();
        }

        public DateTime ReadDateTime()
        {
            var dateTimeString = ReadString().ToString();

            if (String.IsNullOrEmpty(dateTimeString))
                return DateTime.Now;

            Int32 year, month, day, hour, minute, second;

            try
            {
                year = Int32.Parse(dateTimeString.Substring(0, 4));
                month = Int32.Parse(dateTimeString.Substring(4, 2));
                day = Int32.Parse(dateTimeString.Substring(6, 2));
                hour = Int32.Parse(dateTimeString.Substring(8, 2));
                minute = Int32.Parse(dateTimeString.Substring(10, 2));
                second = Int32.Parse(dateTimeString.Substring(12, 2));
            }
            catch (FormatException e)
            {
                throw new FormatException(String.Format(Resources.Format_DateTimeString, dateTimeString));
            }

            return new DateTime(year, month, day, hour, minute, second);
        }

        private void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                var stream = _stream;
                _stream = null;
                if (stream != null)
                    stream.Close();
            }

            _stream = null;
            _buffer = null;
        }

        private void FillBuffer(Byte[] buffer, Stream stream, Int32 numBytes)
        {
            for (var i = 0; i < numBytes; i++)
                buffer[i] = ReadByte(stream);
        }

        private static Byte ReadByte(Stream stream)
        {
            if (stream == null)
                throw new ObjectDisposedException(Resources.ObjectDisposed_FileClosed);
            var readByte = stream.ReadByte();
            if (readByte == -1)
                throw new EndOfStreamException(Resources.IO_ReadBeyondEOF);
            return (Byte)readByte;
        }
    }
}
