using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.Lwo
{
    class lwio
    {
        public static void revbytes(ref byte[] bp, int elsize, int elcount)
        {
            int clip = elcount / 2;
            for (int el = 0; el < elcount; el++)
            {
                int start = elcount * el;
                int end = start + elcount;
                for (int b = 0; b < clip; b++)
                {
                    byte temp = bp[start + b];
                    bp[start + b] = bp[end - b];
                    bp[end - b] = temp;
                }
            }
        }

        public static byte[] getbytes(BinaryReader fp, int size, ref int flen)
        {
            if (flen == -1)
                return null;
            if (size < 0)
            {
                flen = -1;
                return null;
            }
            byte[] data = new byte[size];
            if (fp.Read(data, 0, size) != size)
            {
                flen = -1;
                return null;
            }
            flen += size;
            return data;
        }

        public static int getI1(BinaryReader fp, ref int flen)
        {
            int i;
            if (flen == -1)
                return 0;
            try
            {
                i = fp.ReadSByte();
            }
            catch
            {
                flen = -1;
                return 0;
            }
            flen += 1;
            return i;
        }

        public static short getI2(BinaryReader fp, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[2];
            if (fp.Read(b, 0, 2) != 2)
            {
                flen = -1;
                return 0;
            }
            revbytes(ref b, 2, 1);
            flen += 2;
            return BitConverter.ToInt16(b, 0);
        }

        public static int getI4(BinaryReader fp, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[4];
            if (fp.Read(b, 0, 4) != 4)
            {
                flen = -1;
                return 0;
            }
            revbytes(ref b, 4, 1);
            flen += 4;
            return BitConverter.ToInt32(b, 0);
        }

        public static byte getU1(BinaryReader fp, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte i;
            try
            {
                i = fp.ReadByte();
            }
            catch
            {
                flen = -1;
                return 0;
            }
            flen += 1;
            return i;
        }

        public static ushort getU2(BinaryReader fp, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[2];
            if (fp.Read(b, 0, 2) != 2)
            {
                flen = -1;
                return 0;
            }
            revbytes(ref b, 2, 1);
            flen += 2;
            return BitConverter.ToUInt16(b, 0);
        }

        public static uint getU4(BinaryReader fp, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[4];
            if (fp.Read(b, 0, 4) != 4)
            {
                flen = -1;
                return 0;
            }
            revbytes(ref b, 4, 1);
            flen += 4;
            return BitConverter.ToUInt32(b, 0);
        }

        public static int getVX(BinaryReader fp, ref int flen)
        {
            if (flen == -1)
                return 0;
            try
            {
                byte c = fp.ReadByte();
                int i;
                if (c != 0xFF)
                {
                    i = c << 8;
                    c = fp.ReadByte();
                    i |= c;
                    flen += 2;
                }
                else
                {
                    c = fp.ReadByte();
                    i = c << 16;
                    c = fp.ReadByte();
                    i |= c << 8;
                    c = fp.ReadByte();
                    i |= c;
                    flen += 4;
                }
                return i;
            }
            catch
            {
                flen = -1;
                return 0;
            }
        }

        public static float getF4(BinaryReader fp, ref int flen)
        {
            float f;
            if (flen == -1)
                return 0;
            byte[] b = new byte[4];
            if (fp.Read(b, 0, 4) != 4)
            {
                flen = -1;
                return 0;
            }
            revbytes(ref b, 4, 1);
            flen += 4;
            return BitConverter.ToSingle(b, 0);
        }

        public static string getS0(BinaryReader fp, ref int flen)
        {
            string s;
            int i, c, len, pos;

            if (flen == -1)
                return null;

            pos = (int)fp.BaseStream.Position;
            try
            {
                for (i = 1; ; i++)
                {
                    c = fp.ReadChar();
                    if (c <= 0)
                        break;
                }
            }
            catch
            {
                flen = -1;
                return null;
            }

            if (i == 1)
            {
                if (fp.BaseStream.Seek(pos + 2, SeekOrigin.Begin) == 0)
                    flen = -1;
                else
                    flen += 2;
                return null;
            }

            len = i + (i & 1);

            fp.BaseStream.Seek(pos, SeekOrigin.Begin);
            s = new string(fp.ReadChars(len));

            flen += len;
            return s;
        }

        public static int sgetI1(byte[] bp, ref int index, ref int flen)
        {
            int i;
            if (flen == -1)
                return 0;
            i = bp[index];
            if (i > 127)
                i -= 256;
            flen++;
            index++;
            return i;
        }

        public static short sgetI2(byte[] bp, ref int index, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[2];
            Array.Copy(bp, index, b, 0, 2);
            revbytes(ref b, 2, 1);
            flen += 2;
            index += 2;
            return BitConverter.ToInt16(b, 0);
        }

        public static int sgetI4(byte[] bp, ref int index, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[4];
            Array.Copy(bp, index, b, 0, 4);
            revbytes(ref b, 4, 1);
            flen += 4;
            index += 4;
            return BitConverter.ToInt32(b, 0);
        }

        public static byte sgetU1(byte[] bp, ref int index, ref int flen)
        {
            if (flen == -1)
                return 0;
            flen += 1;
            index++;
            return bp[index - 1];
        }

        public static ushort sgetU2(byte[] bp, ref int index, ref int flen)
        {
            if (flen == -1)
                return 0;
            ushort i = (ushort)((bp[index] << 8) | bp[index + 1]);
            flen += 2;
            index += 2;
            return i;
        }

        public static uint sgetU4(byte[] bp, ref int index, ref int flen)
        {
            if (flen == -1)
                return 0;
            byte[] b = new byte[4];
            Array.Copy(bp, index, b, 0, 4);
            revbytes(ref b, 4, 1);
            flen += 4;
            index += 4;
            return BitConverter.ToUInt32(b, 0);
        }

        public static int sgetVX(byte[] bp, ref int index, ref int flen)
        {
            int i;
            if (flen == -1)
                return 0;

            if (bp[index] != 0xFF)
            {
                i = bp[index] << 8 | bp[index + 1];
                flen += 2;
                index += 2;
            }
            else
            {
                i = (bp[index + 1] << 16) | (bp[index + 2] << 8) | bp[index + 3];
                flen += 4;
                index += 4;
            }
            return i;
        }

        public static float sgetF4(byte[] bp, ref int index, ref int flen)
        {
            float f;
            if (flen == -1)
                return 0;
            byte[] b = new byte[4];
            Array.Copy(bp, index, b, 0, 4);
            revbytes(ref b, 4, 1);
            flen += 4;
            index += 4;
            return BitConverter.ToSingle(b, 0);
        }

        public static string sgetS0(byte[] bp, ref int index, ref int flen)
        {
            if (flen == -1)
                return null;

            string s = BitConverter.ToString(bp, index);
            if (s == null || s.Length == 0)
            {
                flen += 2;
                index += 2;
                return null;
            }
            flen += s.Length;
            index += s.Length;
            return s;
        }
    }
}
