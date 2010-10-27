using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    [TestClass]
    public class InteropInt32Test
    {
        private static readonly Int32[] _int32Data = new [] { Int32.MinValue, -1, 0, 1, Int32.MaxValue };
        
        [TestMethod]
        public void ImplicitConversion()
        {
            Assert.IsTrue(
                _int32Data.All(
                    value =>
                    {
                        InteropInt32 interopInt32 = value;
                        return value == interopInt32;
                    }));
        }

        [TestMethod]
        public void EqualsTest()
        {
            var data = new Int32[,] { { 0, 0 }, { -1, -1 }, { 1, 1 } };
            var length = data.GetLength(0);
            for (var i = 0; i < length; i++)
            {
                InteropInt32 interopInt32 = data[i, 0];
                InteropInt32 interopInt322 = data[i, 1];
                Assert.IsTrue(interopInt32.Equals(interopInt322));
            }
        }
    }
}
