using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Genetibase.Network.Sockets
{
  /*
   * The contents of the simulation file behave like normal C# string.
   * Characters are converted to bytes using System.Text.Encoding.ASCII
   * Examples:
   *    contents of file (between " and ")    HEX value
   *    "\r\n"                                ODOA
   *    "BlaBla\tBla"                         426C61426C6109426C61
   */
  public class CSharpTextDataBlockReader: SocketSimulator.IDataBlockReader
  {
    private StringReader _DataReader;
    public void Initialize(XmlNode dataNode)
    {
      _DataReader = new StringReader(dataNode.ChildNodes[0].Value);
    }

    public void Close()
    {
      _DataReader.Close();
      _DataReader = null;
      GC.SuppressFinalize(this);
    }

    public byte[] ReadBlock()
    {
      char[] Block = new char[SocketSimulator.DataBlockSize];
      using (MemoryStream ms = new MemoryStream())
      {
        int dataRead = _DataReader.Read(Block, 0, SocketSimulator.DataBlockSize);
        Array.Resize<char>(ref Block, dataRead);
        string BlockString = new String(Block);

        while (BlockString.IndexOf('\r') != -1)
        {
          BlockString = BlockString.Remove(BlockString.IndexOf('\r'), 1);
        }
        while (BlockString.IndexOf('\n') != -1)
        {
          BlockString = BlockString.Remove(BlockString.IndexOf('\n'), 1);
        }
        while (BlockString.IndexOf('\t') != -1)
        {
          BlockString = BlockString.Remove(BlockString.IndexOf('\t'), 1);
        }
        while (BlockString.Length > 0)
        {
          if (BlockString[0] != '\\') // single backslash
          {
            int indexOfBackslash = BlockString.IndexOf('\\'); // single backslash
            if (indexOfBackslash == -1)
            {
              indexOfBackslash = BlockString.Length;
            }
            ms.Write(Encoding.ASCII.GetBytes(BlockString.Substring(0, indexOfBackslash)), 0, indexOfBackslash);
            BlockString = BlockString.Remove(0, indexOfBackslash);
          }
          else
          {
            if (BlockString.Length > 1)
            {
              switch (BlockString[1])
              {
                case 'r':
                  {
                    ms.WriteByte((byte)'\r');
                    BlockString = BlockString.Remove(0, 2);
                    break;
                  }
                case 'n':
                  {
                    ms.WriteByte((byte)'\n');
                    BlockString = BlockString.Remove(0, 2);
                    break;
                  }
                case 't':
                  {
                    ms.WriteByte((byte)'\t');
                    BlockString = BlockString.Remove(0, 2);
                    break;
                  }
                case '\\':
                  {
                    ms.WriteByte((byte)'\\');
                    BlockString = BlockString.Remove(0, 2);
                    break;
                  }
                case 'u':
                  {
                    if (BlockString.Length > 5)
                    {
                      ms.WriteByte(Convert.ToByte(Int32.Parse(BlockString.Substring(2, 4), NumberStyles.HexNumber)));
                      BlockString = BlockString.Remove(0, 4);
                    }
                    BlockString = BlockString.Remove(0, 2);
                    break;
                  }
                default:
                  {
                    BlockString.Remove(0, 2);
                    break;
                  }
              }
            }
          }
        }
        return (ms.ToArray());
      }
    }
  }
}