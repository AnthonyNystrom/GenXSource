using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    /// <summary>
    /// Summary description for InteropInt16Test
    /// </summary>
    [TestClass]
    public class InteropInt16Test
    {
        public TestContext TestContext { get; set; }

        private static readonly Int16[] _int16Data = new Int16[] { -32768, -1, 0, 1, 32767 };
        private static readonly Int32[] _int32Data = new Int32[] { -32768, -1, 0, 1, 32767 };
        private static readonly Int32[] _int32OverflowData = new Int32[] { -32769, 32768 };

        [TestMethod]
        public void ImplicitInt16Conversion()
        {
            Assert.IsTrue(
                _int16Data.All(
                    value =>
                    {
                        InteropInt16 interopInt16 = value;
                        return value == interopInt16;
                    }));
        }

        [TestMethod]
        public void ImplicitInt32Conversion()
        {
            Assert.IsTrue(
                _int32Data.All(
                    value =>
                    {
                        InteropInt16 interopInt16 = value;
                        return value == interopInt16;
                    }));
        }

        [TestMethod]
        public void ImplicitInt32OverflowConversion()
        {
            Assert.IsTrue(
                _int32OverflowData.All(
                    value =>
                    {
                        try { InteropInt16 interopInt16 = value; return false; }
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
                InteropInt16 interopInt16 = data[i, 0];
                InteropInt16 interopInt162 = data[i, 1];
                Assert.IsTrue(interopInt16.Equals(interopInt162));
            }
        }
    }
}
