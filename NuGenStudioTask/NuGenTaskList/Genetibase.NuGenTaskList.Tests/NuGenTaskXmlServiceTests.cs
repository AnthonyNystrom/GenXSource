/* -----------------------------------------------
 * NuGenTaskXmlServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Xml;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public class NuGenTaskXmlServiceTests
	{
		private INuGenTaskXmlService xmlService = null;
		private XmlDocument xmlDoc = null;
		private XmlNode xmlElement = null;

		private string xmlNodeName = "dummyXmlNode";
		private string xmlChildNodeName = "dummyXmlChildNode";
		private string xmlNodeContent = "dummyXmlNodeContent";
		private string xmlDefaultNodeText = "defaultText";

		[SetUp]
		public void SetUp()
		{
			this.xmlService = new NuGenTaskXmlService();
			this.xmlDoc = new XmlDocument();
			this.xmlElement = this.xmlDoc.CreateElement(this.xmlNodeName);
		}

		[Test]
		public void AppendChildTest()
		{
			this.xmlService.AppendChild(this.xmlElement, this.xmlChildNodeName, this.xmlNodeContent);
			Assert.AreEqual(1, this.xmlElement.ChildNodes.Count);

			XmlElement retrievedElement = this.xmlElement[this.xmlChildNodeName];
			Assert.AreEqual(this.xmlNodeContent, retrievedElement.InnerText);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AppendChildArgumentNullExceptionOnXmlNodeTest()
		{
			this.xmlService.AppendChild(null, this.xmlChildNodeName, this.xmlNodeContent);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AppendChildArgumentNullExceptionOnXmlNodeNameTest()
		{
			this.xmlService.AppendChild(this.xmlElement, null, "");
		}

		[Test]
		public void AppendChildNullContentTest()
		{
			this.xmlService.AppendChild(this.xmlElement, this.xmlChildNodeName, null);
			Assert.AreEqual(1, this.xmlElement.ChildNodes.Count);
			Assert.AreEqual("", this.xmlElement.ChildNodes[0].InnerText);
		}

		[Test]
		public void AppendChild2Test()
		{
			XmlNode childNode = this.xmlService.AppendChild(this.xmlElement, this.xmlChildNodeName);
			Assert.AreEqual(1, this.xmlElement.ChildNodes.Count);
			Assert.AreEqual(childNode, this.xmlElement[this.xmlChildNodeName]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AppendChild2ArgumentNullExceptionOnXmlNodeTest()
		{
			this.xmlService.AppendChild(null, "");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AppendChild2ArgumentNullExceptionOnXmlNodeTextTest()
		{
			this.xmlService.AppendChild(this.xmlElement, null);
		}

		[Test]
		public void GetChildTextTest()
		{
			XmlElement xmlElement = this.xmlDoc.CreateElement(xmlNodeName);

			this.xmlService.AppendChild(xmlElement, xmlChildNodeName, xmlNodeContent);
			
			Assert.AreEqual(
				xmlNodeContent,
				this.xmlService.GetChildText(xmlElement, xmlChildNodeName, xmlDefaultNodeText)
			);
			
			Assert.AreEqual(
				xmlDefaultNodeText,
				this.xmlService.GetChildText(xmlElement, "", xmlDefaultNodeText)
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetChildTextArgumentNullExceptionXmlElementTest()
		{
			this.xmlService.GetChildText(null, "", "");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetChildTextArgumentNullExceptionChildNameTest()
		{
			this.xmlService.GetChildText(this.xmlDoc.CreateElement(xmlNodeName), null, "");
		}
	}
}
