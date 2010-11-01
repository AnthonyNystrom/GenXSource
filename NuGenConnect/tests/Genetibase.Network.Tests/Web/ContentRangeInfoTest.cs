using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Genetibase.Network.Web;

namespace Genetibase.Network.Tests.Web
{
    [TestFixture()]
    public class ContentRangeInfoTest
    {
        [Test()]
        public void ParseTest()
        {
            string[] values = new string[]
                        {
                            "bytes 500-999/1234"
                        };
            foreach (string value in values)
            {
                Assert.AreEqual(value, ContentRangeInfo.Parse(value).ToString());
            }
            
        }

    }
}
