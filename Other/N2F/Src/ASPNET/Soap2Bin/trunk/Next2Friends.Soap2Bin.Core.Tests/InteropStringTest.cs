using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    [TestClass]
    public class InteropStringTest
    {
        [TestMethod]
        public void ImplicitStringConversion()
        {
            InteropString interopString = "String";
            Assert.AreEqual<String>("String", interopString);
        }

        [TestMethod]
        public void EqualsTest()
        {
            InteropString interopString = "String";
            InteropString interopString2 = "String";
            Assert.IsTrue(interopString.Equals(interopString2));
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            InteropString interopString = "String";
            InteropString interopString2 = "String2";
            Assert.IsFalse(interopString.Equals(interopString2));
        }

        [TestMethod]
        public void LengthTest()
        {
            InteropString interopString = "";
            Assert.AreEqual(0, interopString.Length);
            interopString = "abc";
            Assert.AreEqual(3, interopString.Length);
        }
    }
}
