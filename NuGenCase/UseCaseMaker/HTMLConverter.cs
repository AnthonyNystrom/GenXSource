using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Net;
using System.Reflection;

namespace UseCaseMaker
{
	/// <summary>
	/// Descrizione di riepilogo per HTMLConverter.
	/// </summary>
	public class HTMLConverter
	{
		private string stylesheetFilesPath = string.Empty;
		private string htmlFilesPath = string.Empty;
		private Localizer localizer = null;

		public HTMLConverter(
			string stylesheetFilesPath,
			string htmlFilesPath,
			Localizer localizer)
		{
			this.stylesheetFilesPath = stylesheetFilesPath;
			this.htmlFilesPath = htmlFilesPath;
			this.localizer = localizer;
		}

		public void BuildNavigator(string modelFilePath)
		{
			XmlResolver resolver = new XmlUrlResolver();
			resolver.Credentials = CredentialCache.DefaultCredentials;
			XmlDocument doc = new XmlDocument();
			doc.XmlResolver = resolver;
			doc.Load(modelFilePath);
			XslTransform transform = new XslTransform();
			transform.Load(this.stylesheetFilesPath + Path.DirectorySeparatorChar + "ModelTree.xsl",resolver);
			StreamWriter sw = new StreamWriter(this.htmlFilesPath + Path.DirectorySeparatorChar + "ModelTree.htm",false);
			XsltArgumentList al = new XsltArgumentList();
			al.AddParam("modelBrowser","",this.localizer.GetValue("Globals","ModelBrowser"));
			al.AddParam("glossary","",this.localizer.GetValue("Globals","Glossary"));
			transform.Transform(doc,al,sw,null);
			sw.Close();

			transform.Load(this.stylesheetFilesPath + Path.DirectorySeparatorChar + "HomePage.xsl",resolver);
			sw = new StreamWriter(this.htmlFilesPath + Path.DirectorySeparatorChar + "main.htm",false);
			al = new XsltArgumentList();
			AssemblyName an = this.GetType().Assembly.GetName();
			al.AddParam("version","",an.Version.ToString(3));
			transform.Transform(doc,al,sw,null);
			sw.Close();
		}

		public void BuildPages(string modelFilePath)
		{
			XmlResolver resolver = new XmlUrlResolver();
			resolver.Credentials = CredentialCache.DefaultCredentials;
			XmlDocument doc = new XmlDocument();
			doc.XmlResolver = resolver;
			doc.Load(modelFilePath);
			XmlNode modelNode = doc.SelectSingleNode("//Model");
			this.RecurseNode(doc, resolver, modelNode,"Package.xsl");
		}

		private void RecurseNode(XmlDocument doc, XmlResolver resolver, XmlNode elementNode, string xsltName)
		{
			this.ElementToHTMLPage(doc,resolver,elementNode,xsltName);

			foreach(XmlNode childNode in elementNode)
			{
				if(childNode.Name == "Glossary")
				{
					this.ElementToHTMLPage(doc,resolver,childNode,"Glossary.xsl");
				}
				if(childNode.Name == "Packages")
				{
					foreach(XmlNode packageNode in childNode.ChildNodes)
					{
						this.RecurseNode(doc,resolver,packageNode,"Package.xsl");
					}
				}
				if(childNode.Name == "Actors")
				{
					foreach(XmlNode actorNode in childNode.ChildNodes)
					{
						this.RecurseNode(doc,resolver,actorNode,"Actor.xsl");
					}
				}
				if(childNode.Name == "UseCases")
				{
					foreach(XmlNode useCaseNode in childNode.ChildNodes)
					{
						this.RecurseNode(doc,resolver,useCaseNode,"UseCase.xsl");
					}
				}
			}
		}

		private void ElementToHTMLPage(
			XmlDocument src,
			XmlResolver resolver,
			XmlNode currentNode,
			string xslFileName
			)
		{
			XsltArgumentList al = new XsltArgumentList();
			al.AddParam("elementUniqueID","",currentNode.Attributes["UniqueID"].InnerText);
			if(currentNode.Name == "Glossary")
			{
				al.AddParam("glossary","",this.localizer.GetValue("Globals","Glossary"));
				al.AddParam("glossaryItem","",this.localizer.GetValue("Globals","GlossaryItem"));
				al.AddParam("description","",this.localizer.GetValue("Globals","Description"));
			}
			if(currentNode.Name == "Model" || currentNode.Name == "Package")
			{
				if(currentNode.Name == "Model")
				{
					al.AddParam("elementType","",this.localizer.GetValue("Globals","Model"));
				}
				else
				{
					al.AddParam("elementType","",this.localizer.GetValue("Globals","Package"));
				}
				al.AddParam("actors","",this.localizer.GetValue("Globals","Actors"));
				al.AddParam("useCases","",this.localizer.GetValue("Globals","UseCases"));
				al.AddParam("packages","",this.localizer.GetValue("Globals","Packages"));
				al.AddParam("description","",this.localizer.GetValue("Globals","Description"));
				al.AddParam("notes","",this.localizer.GetValue("Globals","Notes"));
				al.AddParam("relatedDocs","",this.localizer.GetValue("Globals","RelatedDocuments"));
				al.AddParam("requirements","",this.localizer.GetValue("Globals","Requirements"));
			}
			if(currentNode.Name == "Actor")
			{
				al.AddParam("elementType","",this.localizer.GetValue("Globals","Actor"));
				al.AddParam("description","",this.localizer.GetValue("Globals","Description"));
				al.AddParam("notes","",this.localizer.GetValue("Globals","Notes"));
				al.AddParam("relatedDocs","",this.localizer.GetValue("Globals","RelatedDocuments"));
				al.AddParam("goals","",this.localizer.GetValue("Globals","Goals"));
			}
			if(currentNode.Name == "UseCase")
			{
				al.AddParam("statusNodeSet","",this.localizer.GetNodeSet("cmbStatus","Item"));
				al.AddParam("levelNodeSet","",this.localizer.GetNodeSet("cmbLevel","Item"));
				al.AddParam("complexityNodeSet","",this.localizer.GetNodeSet("cmbComplexity","Item"));
				al.AddParam("implementationNodeSet","",this.localizer.GetNodeSet("cmbImplementation","Item"));
				al.AddParam("historyTypeNodeSet","",this.localizer.GetNodeSet("HistoryType","Item"));

				al.AddParam("elementType","",this.localizer.GetValue("Globals","UseCase"));
				al.AddParam("preconditions","",this.localizer.GetValue("Globals","Preconditions"));
				al.AddParam("postconditions","",this.localizer.GetValue("Globals","Postconditions"));
				al.AddParam("openIssues","",this.localizer.GetValue("Globals","OpenIssues"));
				al.AddParam("flowOfEvents","",this.localizer.GetValue("Globals","FlowOfEvents"));
				al.AddParam("prose","",this.localizer.GetValue("Globals","Prose"));
				al.AddParam("details","",this.localizer.GetValue("Globals","Details"));
				al.AddParam("priority","",this.localizer.GetValue("Globals","Priority"));
				al.AddParam("status","",this.localizer.GetValue("Globals","Status"));
				al.AddParam("level","",this.localizer.GetValue("Globals","Level"));
				al.AddParam("complexity","",this.localizer.GetValue("Globals","Complexity"));
				al.AddParam("implementation","",this.localizer.GetValue("Globals","Implementation"));
				al.AddParam("assignedTo","",this.localizer.GetValue("Globals","AssignedTo"));
				al.AddParam("release","",this.localizer.GetValue("Globals","Release"));
				al.AddParam("activeActors","",this.localizer.GetValue("Globals","ActiveActors"));
				al.AddParam("primary","",this.localizer.GetValue("Globals","Primary"));
				al.AddParam("history","",this.localizer.GetValue("Globals","History"));
				al.AddParam("description","",this.localizer.GetValue("Globals","Description"));
				al.AddParam("notes","",this.localizer.GetValue("Globals","Notes"));
				al.AddParam("relatedDocs","",this.localizer.GetValue("Globals","RelatedDocuments"));
			}

			XslTransform transform = new XslTransform();
			transform.Load(this.stylesheetFilesPath + Path.DirectorySeparatorChar + xslFileName,resolver);
			StreamWriter sw = new StreamWriter(htmlFilesPath + Path.DirectorySeparatorChar + currentNode.Attributes["UniqueID"].Value + ".htm",false);
			transform.Transform(src,al,sw,null);
			sw.Close();
		}
	}
}
