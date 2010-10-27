using Next2Friends.Soap2Bin.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Data;
using System.IO;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    [TestClass]
    public class DataStreamTest
    {
        [TestMethod]
        public void ByteTest()
        {
            WriteReadTest<InteropByte>(
                new InteropByte[] { 0, 1, -1, 127, -128 },
                (o, v) => o.WriteByte(v),
                i => i.ReadByte());
        }

        [TestMethod]
        public void BooleanTest()
        {
            WriteReadTest<InteropBoolean>(
                new InteropBoolean[] { true, false },
                (o, v) => o.WriteBoolean(v),
                i => i.ReadBoolean());
        }

        [TestMethod]
        public void FloatTest()
        {
            WriteReadTest<InteropFloat>(
                new InteropFloat[] { -2.1024f, -1, 0, 1, 2.1024f },
                (o, v) => o.WriteFloat(v),
                i => i.ReadFloat());
        }

        [TestMethod]
        public void Int16Test()
        {
            WriteReadTest<InteropInt16>(
                new InteropInt16[] { 0, 1, -1, Int16.MaxValue, Int16.MinValue },
                (o, v) => o.WriteInt16(v),
                i => i.ReadInt16());
        }

        [TestMethod]
        public void Int32Test()
        {
            WriteReadTest<InteropInt32>(
                new InteropInt32[] { 0, 1, -1, Int32.MaxValue, Int32.MinValue },
                (o, v) => o.WriteInt32(v),
                i => i.ReadInt32());
        }

        [TestMethod]
        public void StringTest()
        {
            WriteReadTest<InteropString>(
                new InteropString[] { "String", "", "Русская строка" },
                (o, v) => o.WriteString(v),
                i => i.ReadString());
        }

        [TestMethod]
        public void DateTimeTest()
        {
            WriteReadTest<DateTime>(
                new DateTime[] { new DateTime(1987, 8, 13), new DateTime(1961, 5, 9) },
                (o, v) => o.WriteDateTime(v),
                i => i.ReadDateTime());
        }

        private delegate void WriteAction<T>(DataOutputStream output, T value);
        private delegate T ReadAction<T>(DataInputStream input);

        private static void WriteReadTest<T>(T[] data, WriteAction<T> write, ReadAction<T> read)
        {
            foreach (var value in data)
            {
                var underlyingStream = new MemoryStream();
                var output = new DataOutputStream(underlyingStream);
                write(output, value);
                underlyingStream.Seek(0, SeekOrigin.Begin);
                var input = new DataInputStream(underlyingStream);
                Assert.AreEqual<T>(value, read(input));
            }
        }
    }
}
