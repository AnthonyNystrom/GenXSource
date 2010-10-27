using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    [TestClass]
    public class InteropByteTest
    {
        public TestContext TestContext { get; set; }

        private static readonly Int16[] _int16Data = new Int16[] { -128, -1, 0, 1, 127 };
        private static readonly Int16[] _int16OverflowData = new Int16[] { -129, 128 };
        private static readonly Int32[] _int32Data = new Int32[] { -128, -1, 0, 1, 127 };
        private static readonly Int32[] _int32OverflowData = new Int32[] { -129, 128 };

        [TestMethod]
        public void ImplicitInt16Conversion()
        {
            Assert.IsTrue(
                _int16Data.All(
                    value =>
                    {
                        InteropByte interopByte = value;
                        return value == interopByte;
                    }));
        }

        [TestMethod]
        public void ImplicitInt16OverflowConversion()
        {
            Assert.IsTrue(
                _int16OverflowData.All(
                    value =>
                    {
                        try { InteropByte interopByte = value; return false; }
                        catch (OverflowException) { return true; }
                    }));
        }

        [TestMethod]
        public void ImplicitInt32Conversion()
        {
            Assert.IsTrue(
                _int32Data.All(
                    value =>
                    {
                        InteropByte interopByte = value;
                        return value == interopByte;
                    }));
        }

        [TestMethod]
        public void ImplicitInt32OverflowConversion()
        {
            Assert.IsTrue(
                _int32OverflowData.All(
                    value =>
                    {
                        try { InteropByte interopByte = value; return false; }
                        catch (OverflowException) { return true; }
                    }));
        }

        [TestMethod]
        public void EqualsTest()
        {
            var data = new Int16[,] { { 0, 0 }, { -1, -1 }, { 1, 1 } };
            var length = data.GetLength(0);
            for (var i = 0; i < length; i++)
            {
                InteropByte interopByte = data[i, 0];
                InteropByte interopByte2 = data[i, 1];
                Assert.IsTrue(interopByte.Equals(interopByte2));
            }
        }
    }
}
