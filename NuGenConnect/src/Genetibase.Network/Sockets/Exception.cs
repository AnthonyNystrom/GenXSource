using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
  public class IndyException : Exception {
    public IndyException(string message) : base(message) {}
  }

  public class TooMuchDataInBufferException : IndyException {
    public TooMuchDataInBufferException()
      : base(ResourceStrings.TooMuchDataInBuffer)
    {
    }
  }

  public class NotEnoughDataInBufferException: IndyException
  {
    public NotEnoughDataInBufferException(int byteCount, int size)
      : base(String.Format(ResourceStrings.NotEnoughDataInBuffer, byteCount, size))
    {
    }

  }

  public class ConnectionClosedGracefullyException: IndyException {
    public ConnectionClosedGracefullyException() : base(ResourceStrings.ConnectionClosedGracefully) { }
  }

  public class ReadTimeoutException: IndyException {
    public ReadTimeoutException() : base(ResourceStrings.ReadTimeout) { }
  }

  public class ClosedSocketException: IndyException {
    public ClosedSocketException(string message) : base(message) { }
  }
}