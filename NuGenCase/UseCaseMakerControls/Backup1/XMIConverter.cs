using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Net;

namespace UseCaseMaker
{
	/**
	 * @brief Descrizione di riepilogo per XMIConverter.
	 */
	public class XMIConverter
	{
		#region Enumerators and Constants
		// Public
		// Private
		// Protected
		#endregion

		#region Class Members
		// Public
		// Private
		private string stylesheetFilesPath = string.Empty;
		private string xmiFilesPath = string.Empty;
		// Protected
		#endregion

		#region Constructors
		/**
		 * @brief Costruttore di default per XMIConverter
		 */
		public XMIConverter(
			string stylesheetFilesPath,
			string xmiFilesPath)
		{
			this.stylesheetFilesPath = stylesheetFilesPath;
			this.xmiFilesPath = xmiFilesPath;
		}
		#endregion

		#region Public Properties
		#endregion

		#region Private Properties
		#endregion

		#region Protected Properties
		#endregion

		#region Public Methods
		public void Transform(string modelFilePath)
		{
			XmlResolver resolver = new XmlUrlResolver();
			resolver.Credentials = CredentialCache.DefaultCredentials;
			XmlDocument doc = new XmlDocument();
			doc.XmlResolver = resolver;
			doc.Load(modelFilePath);
			XslTransform transform = new XslTransform();
			transform.Load(this.stylesheetFilesPath + Path.DirectorySeparatorChar + "XMI11Export.xsl",resolver);
			StreamWriter sw = new StreamWriter(this.xmiFilesPath,false);
			transform.Transform(doc,null,sw,null);
			sw.Close();
		}
		#endregion

		#region Private Methods
		#endregion

		#region Protected Methods
		#endregion
	}
}
