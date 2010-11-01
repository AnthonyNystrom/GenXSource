using System;
using System.Collections;
using System.Reflection;
using System.Xml;

namespace UseCaseMakerLibrary
{
	public interface IXMLNodeSerializable
	{
		XmlNode	XmlSerialize(XmlDocument document, object instance, string propertyName, bool deep);
		void	XmlDeserialize(XmlNode fromNode, object instance);
	}

	public class XMLSerializeIgnoreAttribute : System.Attribute
	{
		public XMLSerializeIgnoreAttribute()
		{
		}
	}

	public class XMLSerializeAsAttributeAttribute : System.Attribute
	{
		Boolean getOnly = false;

		public XMLSerializeAsAttributeAttribute()
		{
			this.getOnly = false;
		}

		public XMLSerializeAsAttributeAttribute(Boolean getOnly)
		{
			this.getOnly = getOnly;
		}

		public Boolean HasGetOnly
		{
			get
			{
				return this.getOnly;
			}
		}
	}

	public class XMLSerializeCollectionKeyAttribute : System.Attribute
	{
		public XMLSerializeCollectionKeyAttribute()
		{
		}
	}

	public class XmlSerializerException : Exception
	{
		string message = string.Empty;

		public XmlSerializerException(string message)
		{
			this.message = message;		
		}

		public new string Message
		{
			get
			{
				return this.message;
			}
		}
	}

	/// <summary>
	/// Descrizione di riepilogo per XMLNodeRetriever.
	/// </summary>
	public class XmlSerializer
	{
		internal XmlSerializer()
		{
		}

		static public XmlDocument XmlSerialize(
			string mainNodeName,
			string namespaceURI,
			string version,
			object instance,
			bool deep)
		{
			XmlDocument document = new XmlDocument();
			XmlDeclaration decl = document.CreateXmlDeclaration("1.0","UTF-8","");
			XmlElement mainNode = document.CreateElement(string.Empty, mainNodeName, namespaceURI);
			XmlAttribute attr = document.CreateAttribute("Version");
			attr.Value = version;
			mainNode.SetAttributeNode(attr);
			document.AppendChild(mainNode);
			document.InsertBefore(decl,document.DocumentElement);
			XmlNode node = XmlSerializer.XmlSerialize(document,instance,string.Empty,true);
			mainNode.AppendChild(document.ImportNode(node,true));
			return document;
		}

		static internal XmlNode XmlSerialize(XmlDocument document, object instance, string propertyName, bool deep)
		{
			string namespaceURI = document.DocumentElement.NamespaceURI;

			XmlElement mainNode;
			int i;

			if(propertyName == string.Empty)
			{
				mainNode = document.CreateElement(string.Empty,instance.GetType().Name,namespaceURI);
			}
			else
			{
				mainNode = document.CreateElement(string.Empty,propertyName,namespaceURI);
			}
			XmlAttribute instanceType = document.CreateAttribute("Type");
			instanceType.Value = instance.GetType().ToString();
			mainNode.SetAttributeNode(instanceType);

			PropertyInfo[] pi = instance.GetType().GetProperties();

			for(i = 0; i < pi.Length; i++)
			{
				if(!pi[i].IsDefined(typeof(XMLSerializeIgnoreAttribute),false))
				{
					XmlElement propertyNode = document.CreateElement(string.Empty,pi[i].Name,namespaceURI);
					if(pi[i].PropertyType.GetInterface("IXMLNodeSerializable") != null
						&& pi[i].GetValue(instance,null) != null)
					{
						if(deep)
						{
							XmlNode methodNode = XmlSerialize(document,pi[i].GetValue(instance,null),pi[i].Name,deep);
							propertyNode = (XmlElement)document.ImportNode(methodNode,true);
							mainNode.AppendChild(propertyNode);
						}
						if(pi[i].PropertyType.GetInterface("ICollection") != null)
						{
							IEnumerator ie = ((ICollection)pi[i].GetValue(instance,null)).GetEnumerator();
							while(ie.MoveNext())
							{
								object element = null;
								Type [] parameters = new Type[1];
								parameters[0] = ie.Current.GetType();
								PropertyInfo Item = pi[i].PropertyType.GetProperty("Item",parameters);
								if(Item == null)
								{
									element = ie.Current;
								}
								else
								{
									MethodInfo GetItem = Item.GetGetMethod(false);
									object [] GetItemParameters = new object[1];
									GetItemParameters[0] = ie.Current;
									element = GetItem.Invoke(pi[i].GetValue(instance,null),GetItemParameters);
								}
								if(element.GetType().GetInterface("IXMLNodeSerializable") != null)
								{
									XmlNode methodNode = XmlSerialize(document,element,string.Empty,deep);
									propertyNode.AppendChild((XmlElement)document.ImportNode(methodNode,true));
									mainNode.AppendChild(propertyNode);
								}
							}
						}
					}
					else
					{
						if(pi[i].IsDefined(typeof(XMLSerializeAsAttributeAttribute),false))
						{
							XmlAttribute xmlAttribute = document.CreateAttribute(pi[i].Name);
							xmlAttribute.Value = pi[i].GetValue(instance,null).ToString();
							mainNode.SetAttributeNode(xmlAttribute);
						}
						else
						{
							propertyNode.InnerText = (pi[i].GetValue(instance,null) == null)
								? string.Empty : pi[i].GetValue(instance,null).ToString();
							XmlAttribute propertyType = document.CreateAttribute("Type");
							propertyType.Value = pi[i].PropertyType.ToString();
							propertyNode.SetAttributeNode(propertyType);
							mainNode.AppendChild(propertyNode);
						}
					}
				}
			}
			
			return mainNode;
		}

		static public void XmlDeserialize(
			XmlDocument document,
			string mainNodeName,
			string namespaceURI,
			string version,
			object instance)
		{
			// Check document and instance
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
			nsmgr.AddNamespace(string.Empty, namespaceURI);
			XmlNode node = document.DocumentElement;
			if(node.Name != mainNodeName)
			{
				throw new XmlSerializerException("Invalid document!");
			}
			XmlAttribute attr = node.Attributes["Version"];
			if(attr == null)
			{
				throw new XmlSerializerException("Invalid document!");
			}
			else
			{
				string [] currentVersion = version.Split('.');
				string [] fileVersion = attr.Value.Split('.');
				if(fileVersion.Length != 2)
				{
					throw new XmlSerializerException("Invalid document!");
				}
				if(fileVersion[0].CompareTo(currentVersion[0]) > 0)
				{
					throw new XmlSerializerException("Incompatible version!");
				}
				if(fileVersion[0] == currentVersion[0])
				{
					if(fileVersion[1].CompareTo(currentVersion[1]) > 0)
					{
						throw new XmlSerializerException("Incompatible version!");
					}
				}
			}
			XmlDeserialize(node.FirstChild, instance);
		}

		static internal void XmlDeserialize(XmlNode fromNode, object instance)
		{
			if(fromNode.Attributes["Type"] == null)
			{
				throw new XmlSerializerException("Type attribute not found!");
			}

			foreach(XmlNode node in fromNode.ChildNodes)
			{
				if(node.Attributes["Type"] == null)
				{
					throw new XmlSerializerException("Type attribute not found!");
				}

				try
				{
					PropertyInfo pi = instance.GetType().GetProperty(node.Name,Type.GetType(node.Attributes["Type"].Value));
					if(pi.PropertyType.GetInterface("ICollection") != null)
					{
						foreach(XmlNode itemNode in node.ChildNodes)
						{
							if(itemNode.GetType() == typeof(XmlElement))
							{
								if(itemNode.Attributes["Type"] == null)
								{
									throw new XmlSerializerException("Type attribute not found!");
								}
								Type itemType = instance.GetType().Assembly.GetType(itemNode.Attributes["Type"].Value);
								ConstructorInfo ctor = itemType.GetConstructor(
									BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
									null,
									new Type[0],
									null);
								object item = ctor.Invoke(new Type[0]); // Default constructor
								XmlDeserialize(itemNode,item);
								PropertyInfo itemPiKey = null;
								foreach(PropertyInfo itemPi in item.GetType().GetProperties())
								{
									if(itemPi.IsDefined(typeof(XMLSerializeCollectionKeyAttribute),true))
									{
										itemPiKey = itemPi;
										break;
									}
								}
								if(itemPiKey != null)
								{
									MethodInfo add = pi.PropertyType.GetMethod("Add");
									object [] addParameters = new Object[2];
									addParameters[0] = itemPiKey.GetValue(item,null);
									addParameters[1] = item;
									add.Invoke(pi.GetValue(instance,null),addParameters);
								}
								else
								{
									MethodInfo add = pi.PropertyType.GetMethod("Add");
									object [] addParameters = new Object[1];
									addParameters[0] = item;
									add.Invoke(pi.GetValue(instance,null),addParameters);
								}
							}
						}
					}
					if(pi.PropertyType.GetInterface("IXMLNodeSerializable") == null)
					{
						foreach(XmlNode nodeValue in node.ChildNodes)
						{
							if(nodeValue.GetType() == typeof(XmlText) && nodeValue.Value != null)
							{
								if(pi.PropertyType.IsEnum)
								{
									pi.SetValue(instance,Enum.Parse(pi.PropertyType,nodeValue.Value,true),null);
								}
								else
								{
									pi.SetValue(instance,Convert.ChangeType(nodeValue.Value,pi.PropertyType),null);
								}
							}
						}
					}
					else
					{
						if(pi.GetValue(instance,null) == null)
						{
							pi.SetValue(instance,Convert.ChangeType(node.Value,pi.PropertyType),null);
						}
						if(node.HasChildNodes)
						{
							XmlDeserialize(node,pi.GetValue(instance,null));
						}
					}
				}
				catch(NullReferenceException)
				{
					return;
				}

				foreach(XmlAttribute attr in fromNode.Attributes)
				{
					try
					{
						PropertyInfo pi = instance.GetType().GetProperty(attr.Name);
						if(pi.IsDefined(typeof(XMLSerializeAsAttributeAttribute),true))
						{
							object [] pa = pi.GetCustomAttributes(typeof(XMLSerializeAsAttributeAttribute),false);
							PropertyInfo pai = pa[0].GetType().GetProperty("HasGetOnly");
							if((Boolean)pai.GetValue(pa[0],null) == false)
							{
								pi.SetValue(instance,Convert.ChangeType(attr.Value,pi.PropertyType),null);
							}
						}
					}
					catch(NullReferenceException)
					{
					}
				}
			}
		}
	}
}
