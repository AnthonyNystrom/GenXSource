/* ------------------------------------------------
 * AskServiceTests.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Next2Friends.WebServices.Tests.AskService;

namespace Next2Friends.WebServices.Tests
{
    [TestClass]
    public class AskServiceTests
    {
        private AskServiceSoapClient _client;

        [TestInitialize]
        public void SetUp()
        {
            _client = new AskServiceSoapClient();
        }

        [TestMethod]
        public void AddCommentTest()
        {
            var newCommentID = _client.AddComment(
                "eisernWolf",
                "0000",
                new AskComment()
                {
                    AskQuestionID = 736,
                    DTCreated = DateTime.Now.Ticks.ToString(),
                    Nickname = "eisernWolf",
                    Text = "First-level comment"
                });
            newCommentID = _client.AddComment(
                "eisernWolf",
                "0000",
                new AskComment()
                {
                    AskQuestionID = 736,
                    DTCreated = DateTime.Now.Ticks.ToString(),
                    Nickname = "eisernWolf",
                    Text = "Second-level comment",
                    InReplyToCommentID = newCommentID
                });
            _client.AddComment(
                "eisernWolf",
                "0000",
                new AskComment()
                {
                    AskQuestionID = 736,
                    DTCreated = DateTime.Now.Ticks.ToString(),
                    Nickname = "eisernWolf",
                    Text = "Another second-level comment",
                    InReplyToCommentID = newCommentID
                });
            _client.AddComment(
                "eisernWolf",
                "0000",
                new AskComment()
                {
                    AskQuestionID = 736,
                    DTCreated = DateTime.Now.Ticks.ToString(),
                    Nickname = "eisernWolf",
                    Text = "Another first-level comment."
                });
        }
    }
}
