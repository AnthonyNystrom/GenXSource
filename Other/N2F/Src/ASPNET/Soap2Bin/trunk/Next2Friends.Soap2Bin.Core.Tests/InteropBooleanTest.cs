using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Next2Friends.Soap2Bin.Core.Tests
{
    [TestClass]
    public class InteropBooleanTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ConversionTrue()
        {
            ConversionTest(true);
        }

        [TestMethod]
        public void ConversionFalse()
        {
            ConversionTest(false);
        }

        [TestMethod]
        public void EqualsTest()
        {
            InteropBoolean interopBoolean = true;
            InteropBoolean interopBoolean2 = true;
            Assert.IsTrue(interopBoolean.Equals(interopBoolean2));
        }

        [TestMethod]
        public void EqualsTest2()
        {
            InteropBoolean interopBoolean = true;
            Assert.IsTrue(interopBoolean.Equals(interopBoolean));
        }

        [TestMethod]
        public void EqualsFalseTest()
        {
            InteropBoolean interopBoolean = true;
            Assert.IsFalse(interopBoolean.Equals(1));
        }

        [TestMethod]
        public void EqualsFalseTest2()
        {
            InteropBoolean interopBoolean = false;
            InteropBoolean interopBoolean2 = true;
            Assert.IsFalse(interopBoolean.Equals(interopBoolean2));
        }

        private static void ConversionTest(Boolean value)
        {
            InteropBoolean interopBoolean = value;
            Assert.AreEqual<Boolean>(value, interopBoolean);
        }
    }
}
