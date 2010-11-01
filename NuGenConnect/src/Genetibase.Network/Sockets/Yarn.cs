using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public class Yarn: IDisposable
  {
    public virtual void Dispose()
    {
      GC.SuppressFinalize(this);
    }
  }
}
