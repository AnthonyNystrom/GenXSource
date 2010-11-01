using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Genetibase.Network.Sockets {
  public class TextWriterServerIntercept: ServerIntercept {
    private int mConnectionNumber = 0;
    private TextWriter mTextWriter = null;
    private Encoding mEncoding = Encoding.ASCII;

    public Encoding Encoding {
      get {
        return mEncoding;
      }
      set {
        mEncoding = value;
      }
    }

    protected override void DoInit() {
      base.DoInit();
      mConnectionNumber = 0;
    }

    public TextWriter TextWriter {
      get {
        return mTextWriter;
      }
      set {
        mTextWriter = value;
      }
    }

    protected override ConnectionIntercept DoAccept(object connection) {
      return new TextWriterIntercept(mTextWriter, mEncoding, Interlocked.Increment(ref mConnectionNumber).ToString());
    }
  }
}
