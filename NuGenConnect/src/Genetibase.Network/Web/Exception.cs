using System;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Web {
  public class HTTPServerException: IndyException {
    public HTTPServerException(string AMsg)
      : base(AMsg) {
    }
  }

  public class HTTPHeaderAlreadyWrittenException: HTTPServerException {
    public HTTPHeaderAlreadyWrittenException(string AMsg)
      : base(AMsg) {
    }
  }

  public class HTTPErrorParsingCommandException: HTTPServerException {
    public HTTPErrorParsingCommandException(string AMsg)
      : base(AMsg) {
    }
  }

  public class HTTPUnsupportedAuthorisationSchemeException: HTTPServerException {
    public HTTPUnsupportedAuthorisationSchemeException(string AMsg)
      : base(AMsg) {
    }
  }

  public class HTTPCannotSwitchSessionStateWhenActiveException: HTTPServerException {
    public HTTPCannotSwitchSessionStateWhenActiveException(string AMsg)
      : base(AMsg) {
    }
  }

  public class HttpUnknownProtocolException: IndyException {
    public HttpUnknownProtocolException(string AMsg)
      : base(AMsg) {
    }
  }

  public class InvalidObjectTypeException: IndyException {
    public InvalidObjectTypeException(string AMsg)
      : base(AMsg) {
    }
  }

  public class HttpClientProtocolException: IndyException {
    protected string mErrorMessage = "";
    protected string mReplyMessage;
    protected int mErrorCode;
    public HttpClientProtocolException(int AErrorCode, string AReplyMessage, string AErrorMessage)
      : base(AErrorMessage) {
      mErrorMessage = AErrorMessage;
      mReplyMessage = AReplyMessage;
      mErrorCode = AErrorCode;
    }

    public string ErrorMessage {
      get {
        return mErrorMessage;
      }
    }
  }
}
