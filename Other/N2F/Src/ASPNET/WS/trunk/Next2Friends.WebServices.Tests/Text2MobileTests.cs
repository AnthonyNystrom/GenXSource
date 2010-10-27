/* ------------------------------------------------
 * Text2MobileTests.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Next2Friends.WebServices.Utils;

namespace Next2Friends.WebServices.Tests
{
    [TestClass]
    public sealed class Text2MobileTests
    {
        private static Dictionary<String, String> _brTagList;
        private static Dictionary<String, String> _aTagList;
        private static Dictionary<String, String> _specialCharsList;

        static Text2MobileTests()
        {
            _brTagList = new Dictionary<String, String>();
            _brTagList.Add("<br />", " ");
            _brTagList.Add("Thank you I am glad you like it.<br /><br /><br />.", "Thank you I am glad you like it.   .");
            _brTagList.Add("that dude is naked<br /><br />genital jerkin", "that dude is naked  genital jerkin");

            _aTagList = new Dictionary<string, string>();
            _aTagList.Add("You might wanna check <a href=\"http://www.next2friends.com/view.aspx?v=ZGJmMzkyODZlOGE3NGE3Y2\">Kissy Baby</a>", "You might wanna check Kissy Baby");
            _aTagList.Add("I am now selling car parts such as: <a href=\"http://www.racepages.com/parts/blower_motor.html\">Blower Motor Accessories</a><br> <a href=\"http://www.racepages.com/parts/engine_mount/mitsubishi.html\">Mitsubishi Engine Mount</a><br> <a href=\"http://www.racepages.com/parts/tail_light_covers/jeep.html\">Jeep tail Light Covers</a><br>", "I am now selling car parts such as: Blower Motor Accessories  Mitsubishi Engine Mount  Jeep tail Light Covers ");

            _specialCharsList = new Dictionary<String, String>();
            _specialCharsList.Add("now you are hiding, and I am just showing off. That&#39;s such coincidence or what!", "now you are hiding, and I am just showing off. That's such coincidence or what!");
            _specialCharsList.Add("Did he say &quot;Crazy like cloning???&quot;", "Did he say \"Crazy like cloning???\"");
            _specialCharsList.Add("Astrophysicist --&gt; Fascinating!!!", "Astrophysicist --> Fascinating!!!");
        }

        [TestMethod]
        public void FilterTest_NullText()
        {
            Assert.IsNull(Text2Mobile.Filter(null));
        }

        [TestMethod]
        public void FilterTest_EmptyString()
        {
            Assert.AreEqual("", Text2Mobile.Filter(""));
        }

        [TestMethod]
        public void FilterTest_SimpleText()
        {
            Assert.AreEqual("Some text", Text2Mobile.Filter("Some text"));
        }

        [TestMethod]
        public void FilterTest_SimpleTagBr()
        {
            Assert.AreEqual(" ", Text2Mobile.Filter("<br />"));
        }

        [TestMethod]
        public void FilterTest_ListTagBr()
        {
            foreach (var input in _brTagList.Keys)
                Assert.AreEqual(_brTagList[input], Text2Mobile.Filter(input));
        }

        [TestMethod]
        public void FilterTest_SimpleTagA()
        {
            Assert.AreEqual("link", Text2Mobile.Filter("<a href=\"http://null.com\">link</a>"));
        }

        [TestMethod]
        public void FilterTest_ListTagA()
        {
            foreach (var input in _aTagList.Keys)
                Assert.AreEqual(_aTagList[input], Text2Mobile.Filter(input));
        }

        [TestMethod]
        public void FilterTest_ListSpecialChars()
        {
            foreach (var input in _specialCharsList.Keys)
                Assert.AreEqual(_specialCharsList[input], Text2Mobile.Filter(input));
        }
    }
}
