using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrossPoster.Client.Tests
{
    [TestClass]
    public class DateStringTest
    {
        [TestMethod]
        public void DateToStringTest()
        {
            var expectedDate = new DateTime(2020, 2, 20, 2, 20, 0);
            var actualDate = DateTime.Parse("2020-02-20 02:20:00");
            Assert.AreEqual<DateTime>(expectedDate, actualDate);
        }
    }
}
