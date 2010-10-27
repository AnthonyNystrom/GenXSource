using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Next2Friends.WebServices.Fix;

namespace Next2Friends.WebServices.Tests
{
    [TestClass]
    public sealed class ResourceServiceTests
    {
        [TestMethod]
        public void CanGetNicknameFromPath()
        {
            var nickname = ResourceService.GetNickname("eisernWolf/pthmb/");
            Assert.AreEqual("eisernWolf", nickname);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNicknameChecksIfPathIsNull()
        {
            ResourceService.GetNickname(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNicknameChecksIfPathIsEmpty()
        {
            ResourceService.GetNickname("");
        }

        [TestMethod]
        public void CanGetMediumImagePath()
        {
            var path = ResourceService.GetMediumImagePath("eisernWolf/pthmb/");
            Assert.AreEqual(@"\\www\user\eisernWolf\pmed\", path);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMediumImagePathChecksIfThumbnailPathIsNull()
        {
            ResourceService.GetMediumImagePath(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMediumImagePathChecksIfThumbnailPathIsEmpty()
        {
            ResourceService.GetMediumImagePath("");
        }

        [TestMethod]
        public void CanGetThumbnailImagePath()
        {
            var path = ResourceService.GetThumbnailImageFullPath("eisernWolf/pthmb/");
            Assert.AreEqual(@"\\www\user\eisernWolf\pthmb\", path);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetThumbnailImageFullPathChecksIfPathIsNull()
        {
            ResourceService.GetThumbnailImageFullPath(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetThumbnailImageFullPathChecksIfPathIsEmpty()
        {
            ResourceService.GetThumbnailImageFullPath("");
        }
    }
}
