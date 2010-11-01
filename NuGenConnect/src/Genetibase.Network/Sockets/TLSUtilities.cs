using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
  public static class TLSUtilities {
    public static readonly UseTLSEnum[] ExplicitTLSValues = new UseTLSEnum[] {UseTLSEnum.UseRequireTLS, UseTLSEnum.UseExplicitTLS};
    public const UseTLSEnum DefaultUseTLSValue = UseTLSEnum.NoTLSSupport;
  }
}