/* ------------------------------------------------
 * UrlProcessorTests.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Next2Friends.WebServices.Utils;

namespace Next2Friends.WebServices.Tests
{
    [TestClass]
    public class UrlProcessorTests
    {
        [TestMethod]
        public void ExtractWebBlogEntryIDTest()
        {
            Assert.AreEqual("Yzc4MTk1", UrlProcessor.ExtractWebBlogEntryID("/blog.aspx?m=Y2ZjNDIzMDdiZDE1NDQxM2&b=Yzc4MTk1"));
            Assert.AreEqual("Mjk5NmU4", UrlProcessor.ExtractWebBlogEntryID("/blog.aspx?m=ZTZlNzE3Y2UzM2U1NGM5OD&b=Mjk5NmU4"));
        }

        [TestMethod]
        public void ExtractWebAskIDTest()
        {
            Assert.AreEqual("MzBhY2Fl", UrlProcessor.ExtractWebAskID("/ask/MzBhY2Fl"));
        }

        [TestMethod]
        public void ExtractWallNameTest()
        {
            Assert.AreEqual("shelton", UrlProcessor.ExtractWallName("/users/shelton/wall"));
        }

        [TestMethod]
        public void ExtractWebVideoIDTest()
        {
            Assert.AreEqual("YjgwMmI4", UrlProcessor.ExtractWebVideoID("/video/pork-and-beans/YjgwMmI4"));
            Assert.AreEqual("NjczZTRiN2U5MWUyNGFkYm", UrlProcessor.ExtractWebVideoID("/video/lawrence/NjczZTRiN2U5MWUyNGFkYm"));
            Assert.AreEqual("OTJmN2ZhNWE5OGQ0NGM1OD", UrlProcessor.ExtractWebVideoID("/video/london/OTJmN2ZhNWE5OGQ0NGM1OD"));
        }

        [TestMethod]
        public void ExtractWebPhotoCollectionIDTest()
        {
            Assert.AreEqual("YTRjODRi", UrlProcessor.ExtractWebPhotoCollectionID("/gallery/?g=YTRjODRi&m=MWI5OWQ1YmJjYjY5NDEwNm"));
        }

        [TestMethod]
        public void ExtractWebPhotoIDTest()
        {
            Assert.AreEqual("YTRjODRi", UrlProcessor.ExtractWebPhotoID("/gallery/?g=MGY1MWY3&m=NDMxZjA4YTAwOWQ1NDNlOD&wp=YTRjODRi"));
        }
    }
}
