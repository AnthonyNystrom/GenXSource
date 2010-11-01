using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets.Protocols.Encoding {
  public static class Coder {
    public static string EncodeString<TEncoder>(string input) where TEncoder: EncoderBase, new(){
      TEncoder encoder = new TEncoder();
      return encoder.Encode(input);
    }

    public static string DecodeString<TDecoder>(string input) where TDecoder: DecoderBase, new(){
      TDecoder decoder = new TDecoder();
      return decoder.DecodeString(input);
    }
  }
}