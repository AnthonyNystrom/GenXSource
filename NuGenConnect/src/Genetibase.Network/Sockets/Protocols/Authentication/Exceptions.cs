using System;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Sockets.Protocols.Authentication
{
  public class AlreadyRegisteredAuthenticationMethodException: IndyException
  {
    public AlreadyRegisteredAuthenticationMethodException(string MethodTypeName):
      base(String.Format(ResourceStrings.AuthenticationMethodAlreadyRegistered, MethodTypeName))
    {
    }
  }
  
  public class InvalidAlgorithmException: IndyException
  {
    public InvalidAlgorithmException():base(ResourceStrings.InvalidHashMethod)
    {
    }
  }
}
