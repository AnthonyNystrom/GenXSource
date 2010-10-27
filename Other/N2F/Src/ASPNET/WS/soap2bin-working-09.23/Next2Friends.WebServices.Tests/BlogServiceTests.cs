/* ------------------------------------------------
 * BlogServiceTests.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Next2Friends.WebServices.Tests.BlogService;

namespace Next2Friends.WebServices.Tests
{
    [TestClass]
    public class BlogServiceTests
    {
        private BlogServiceSoapClient _client;

        [TestInitialize]
        public void SetUp()
        {
            _client = new BlogServiceSoapClient();
        }

        [TestMethod]
        public void AddComment_SingleLevel()
        {
            var blogEntry = _client.GetEntryFromUrl("eisernWolf", "0000", "/blog.aspx?m=OTE0N2Vk&b=MmRiMGQz");
            _client.AddComment(
                "eisernWolf",
                "0000",
                new BlogComment()
                {
                    BlogEntryID = blogEntry.ID,
                    Body = "Test comment. 1st level."
                });
        }

        [TestMethod]
        public void AddComments_TwoLevels()
        {
            var blogEntry = _client.GetEntryFromUrl("eisernWolf", "0000", "/blog.aspx?m=OTE0N2Vk&b=MmRiMGQz");
            var newCommentID = _client.AddComment(
                "eisernWolf",
                "0000",
                new BlogComment()
                {
                    BlogEntryID = blogEntry.ID,
                    Body = "Test comment. 1st level."
                });
            newCommentID = _client.AddComment(
                "eisernWolf",
                "0000",
                new BlogComment()
                {
                    BlogEntryID = blogEntry.ID,
                    Body = "Test comment. 2nd level.",
                    InReplyToCommentID = newCommentID
                });
            _client.AddComment(
                "eisernWolf",
                "0000",
                new BlogComment()
                {
                    BlogEntryID = blogEntry.ID,
                    Body = "Test comment. 3rd level.",
                    InReplyToCommentID = newCommentID
                });
        }
    }
}
