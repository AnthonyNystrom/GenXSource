using System;

namespace Genetibase.Network.Sockets.Protocols.Authentication
{
  [Flags]
  public enum AuthenticationSchemesEnum
  {
    Unknown,
    Base,
    Digest,
    NTLM
  }
  
  public enum AuthenticationWhatsNextEnum
  {
    AskTheProgram,
    DoRequest,
    Fail
  }
}
