using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    /// <summary>
    /// Summary description for InteropFloatTest
    /// </summary>
    [TestClass]
    public class InteropFloatTest
    {
        private static readonly Int32[] _int32Data = new[] { Int16.MinValue, -1, 0, 1, Int16.MaxValue };
        private static readonly Int32[] _int32OverflowData = new[] { Int32.MinValue, Int32.MaxValue };
        private static readonly Single[] _floatData = new[] { -2.1024f, -1.2f, 0.0f, 1.2f, 2.1024f };

        [TestMethod]
        public void ImplicitInt32Conversion()
        {
            Assert.IsTrue(_int32Data.All(
                value =>
                {
                    InteropFloat interopFloat = value;
                    return value == interopFloat;
                }));
        }

        [TestMethod]
        public void ImplicitInt32OverflowConversion()
        {
            Assert.IsTrue(_int32OverflowData.All(
                value =>
                {
                    try { InteropFloat interopFloat = value; return false; }
                    catch (OverflowException) { return true; }
                }));
        }

        [TestMethod]
        public void ImplicitFloatConversion()
        {
            foreach (var value in _floatData)
            {
                InteropFloat interopFloat = value;
                Assert.AreEqual(value, interopFloat, 0.1f);
            }
        }

        [TestMethod]
        public void EqualsTest()
        {
            foreach (var value in _floatData)
            {
                InteropFloat interopFloat = value;
                InteropFloat interopFloat2 = value;
                Assert.IsTrue(interopFloat.Equals(interopFloat2));
            }
        }
    }
}
