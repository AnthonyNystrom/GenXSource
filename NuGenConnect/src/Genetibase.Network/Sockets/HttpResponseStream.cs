using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genetibase.Network.Sockets
{
    internal class HttpResponseStream : Stream
    {
        Socket _socket;
        bool _chunkedStream;
        int _chunkSize;
        bool _endStream;

        public HttpResponseStream(Socket socket, bool chunkedStream)
        {
            _socket = socket;
            _chunkedStream = chunkedStream;
            _endStream = false;
            _chunkSize = 0;
        }


        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {            
        }

        public override long Length
        {
            get { throw new NotSupportedException("The method or operation is not supported."); }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException("The method or operation is not supported.");
            }
            set
            {
                throw new NotSupportedException("The method or operation is not supported.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_chunkedStream) return ChunkRead(buffer, offset, count);
            
            byte[] temp = null;
            try
            {
                temp = _socket.ReadBytes(count);
            }
            catch (NotEnoughDataInBufferException)
            {
                temp = _socket.ReadBytes(_socket.mInputBuffer.Size);
            }
            temp.CopyTo(buffer, offset);            

            return temp.Length;
        }

        private int ChunkRead(byte[] buffer, int offset, int count)
        {
            if (_endStream) return 0;

            int result = 0;

            while (result < count)
            {
                if (_chunkSize <= 0)
                    if (!int.TryParse(_socket.ReadLn(), out _chunkSize) || _chunkSize <= 0)
                    {
                        _endStream = true;
                        break;
                    }

                int bytesToRead = count - result <= _chunkSize ? count - result : _chunkSize;

                byte[] temp = null;
                try
                {
                    temp = _socket.ReadBytes(bytesToRead);
                }
                catch (NotEnoughDataInBufferException)
                {
                    temp = _socket.ReadBytes(_socket.mInputBuffer.Size);
                }
                temp.CopyTo(buffer, offset);
                offset += temp.Length;
                result += temp.Length;
                _chunkSize -= temp.Length;

                if (temp.Length == 0) break;
            }

            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("The method or operation is not supported.");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("The method or operation is not supported.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("The method or operation is not supported.");
        }
    }
}
