/* -----------------------------------------------
 * NuGenSerializer.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Globalization;
using Genetibase.Shared.Properties;
using Genetibase.Shared.Reflection;
using Genetibase.Shared.Xml;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Serializes and deserializes objects into and from XML documents.
	/// </summary>
	public class NuGenSerializer : NuGenEventInitiator
	{
		#region Events

		/*
		 * Deserializing
		 */

		/// <summary>
		/// The <see cref="E:NuGenSerializer.Deserializing"/> event identifier.
		/// </summary>
		private static readonly object _deserializing = new object();

		/// <summary>
		/// Occurs when the deserialization process is invoked for this <see cref="T:NuGenSerializer"/>.
		/// </summary>
		public event EventHandler Deserializing
		{
			add
			{
				this.Events.AddHandler(_deserializing, value);
			}
			remove
			{
				this.Events.RemoveHandler(_deserializing, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenSerializer.Deserializing"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnDeserializing(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[_deserializing];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * Serializing
		 */

		/// <summary>
		/// The <see cref="E:NuGenSerializer.Serializing"/> event identifier.
		/// </summary>
		private static readonly object _serializing = new object();

		/// <summary>
		/// Occurs when the serialization process is invoked for this <see cref="T:NuGenSerializer"/>.
		/// </summary>
		public event EventHandler Serializing
		{
			add
			{
				this.Events.AddHandler(_serializing, value);
			}
			remove
			{
				this.Events.RemoveHandler(_serializing, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenSerializer.Serializing"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnSerializing(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[_serializing];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Properties.Public

		/*
		 * CheckSerializable
		 */

		private bool _checkSerializable;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenSerializer"/> should check
		/// if the serializable object supports <see cref="T:INuGenSerializable"/> interface.
		/// </summary>
		public virtual bool CheckSerializable
		{
			[DebuggerStepThrough]
			get
			{
				return _checkSerializable;
			}
			[DebuggerStepThrough]
			set
			{
				_checkSerializable = value;
			}
		}

		/*
		 * Name
		 */

		/// <summary>
		/// Gets this <see cref="T:NuGenSerializer"/> name.
		/// </summary>
		public virtual string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this.GetType().Name;
			}
		}

		/*
		 * SortProperties
		 */

		private bool _sortProperties = true;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenSerializer"/> should sort
		/// the properties alphabetically.
		/// </summary>
		public virtual bool SortProperties
		{
			[DebuggerStepThrough]
			get
			{
				return _sortProperties;
			}
			[DebuggerStepThrough]
			set
			{
				_sortProperties = value;
			}
		}

		/*
		 * Version
		 */

		/// <summary>
		/// Gets this <see cref="T:NuGenSerializer"/> version.
		/// </summary>
		public virtual string Version
		{
			[DebuggerStepThrough]
			get
			{
				return Properties.Resources.Serializer_Version;
			}
		}

		#endregion

		#region Properties.Public.Static

		/*
		 * PropertyToString
		 */

		private static Hashtable _propertyToString = new Hashtable();

		/// <summary>
		/// Gets or sets a list of properties with custom property names associated.
		/// </summary>
		public static Hashtable PropertyToString
		{
			[DebuggerStepThrough]
			get
			{
				return _propertyToString;
			}
			[DebuggerStepThrough]
			set
			{
				_propertyToString = value;
			}
		}

		/*
		 * StringToProperty
		 */

		private static Hashtable _stringToProperty = new Hashtable();

		/// <summary>
		/// Gets or sets a list of custom property names with properties associated.
		/// </summary>
		public static Hashtable StringToProperty
		{
			[DebuggerStepThrough]
			get
			{
				return _stringToProperty;
			}
			[DebuggerStepThrough]
			set
			{
				_stringToProperty = value;
			}
		}

		/*
		 * StringToType
		 */

		private static Hashtable _stringToType = new Hashtable();

		/// <summary>
		/// Gets or sets a list of cutom type names with types associated.
		/// </summary>
		public static Hashtable StringToType
		{
			[DebuggerStepThrough]
			get
			{
				return _stringToType;
			}
			[DebuggerStepThrough]
			set
			{
				_stringToType = value;
			}
		}

		/*
		 * TypeToString
		 */

		private static Hashtable _typeToString = new Hashtable();

		/// <summary>
		/// Gets or sets a list of types with custom type names associated.
		/// </summary>
		public static Hashtable TypeToString
		{
			[DebuggerStepThrough]
			get
			{
				return _typeToString;
			}
			[DebuggerStepThrough]
			set
			{
				_typeToString = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * IsContent
		 */

		/// <summary>
		/// Determines whether the value of the specified object should be serialized rather than the object
		/// itself.
		/// </summary>
		/// <param name="obj">Specifies the object to test.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="obj"/>is <see langword="null"/>.
		/// </exception>
		public bool IsContent(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			Type type = obj.GetType();

			/*
			 * If the specified object type is already contained within this.contentHashtable.
			 */

			object contentValue = _contentHashtable[type];

			if (contentValue != null)
			{
				return (bool)contentValue;
			}

			/* Otherwise retrieve the type attributes. */
			object[] attributes = type.GetCustomAttributes(typeof(NuGenSerializationVisibilityAttribute), true);

			foreach (object attribute in attributes)
			{
				if (((NuGenSerializationVisibilityAttribute)attribute).Visibility == NuGenSerializationVisibility.Content)
				{
					_contentHashtable.Add(type, true);
					return true;
				}
			}

			_contentHashtable.Add(type, false);
			return false;
		}

		/*
		 * SetReferenceDeserializing
		 */

		/// <summary>
		/// Deserializes referenced objects.
		/// </summary>
		public void SetReferenceDeserializing()
		{
			int count = 0;

			foreach (NuGenReference reference in _references)
			{
				object obj = _graph[reference.PropertyInfo.ReferenceCode];

				if (obj != null)
				{
					NuGenSerializer.SetProperty(reference.StandardPropertyInfo, reference.Object, obj);
				}

				count++;
			}
		}

		/*
		 * SetReferenceSerializing
		 */

		/// <summary>
		/// Serializes referenced objects.
		/// </summary>
		public void SetReferenceSerializing()
		{
			foreach (NuGenReference reference in _references)
			{
				object propertyValue = reference.PropertyInfo.Value;

				if (propertyValue != null)
				{
					int referenceCode = _graph[propertyValue];

					if (referenceCode == -1)
					{
						reference.PropertyInfo.IsReference = false;
						reference.PropertyInfo.Value = null;
					}
					else
					{
						reference.PropertyInfo.ReferenceCode = referenceCode;
					}
				}
			}
		}

		#endregion

		#region Methods.Public.Static

		/*
		 * AddStringProperty
		 */

		/// <summary>
		/// Determines a new name for the specified property name used for serialization.
		/// </summary>
		/// <param name="property">Specifies the property name to assign a new name for.</param>
		/// <param name="str">Specifies a new property name.</param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="property"/> is <see langword="null"/>.
		/// </para>
		///	-or-
		///	<para>
		///		<paramref name="property"/> is <see langword="null"/>.
		/// </para>
		///	-or-
		///	<para>
		///		<paramref name="str"/> is <see langword="null"/>.
		/// </para>
		///	-or-
		///	<para>
		///		<paramref name="str"/> is <see langword="null"/>.
		/// </para>
		///	</exception>
		public static void AddStringProperty(string property, string str)
		{
			if (string.IsNullOrEmpty(property))
			{
				throw new ArgumentNullException("property");
			}

			if (string.IsNullOrEmpty(str))
			{
				throw new ArgumentNullException("str");
			}

			NuGenSerializer.StringToProperty.Add(str, property);
			NuGenSerializer.PropertyToString.Add(property, str);
		}

		/*
		 * AddStringType
		 */

		/// <summary>
		/// Determines a new name for the specified type used for serialization.
		/// </summary>
		/// <param name="type">Specifies the <see cref="Type"/> to assign a new name for.</param>
		/// <param name="str">Specifies a new type name.</param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="type"/> is <see langword="null"/>.
		/// </para>
		///	-or-
		///	<para>
		///		<paramref name="str"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="str"/> is an empty string.
		/// </para>
		/// </exception>
		public static void AddStringType(Type type, string str)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (string.IsNullOrEmpty(str))
			{
				throw new ArgumentNullException("str");
			}

			NuGenSerializer.StringToType.Add(str, type);
			NuGenSerializer.TypeToString.Add(type, str);
		}

		/// <summary>
		/// Clears custom property names.
		/// </summary>
		public static void ClearPropertyString()
		{
			NuGenSerializer.StringToProperty.Clear();
			NuGenSerializer.PropertyToString.Clear();
		}

		/*
		 * GetDefaultValue
		 */

		/// <summary>
		/// Gets the default value for the specified member.
		/// </summary>
		/// <param name="member">Specifies the member to retrieve the default value for.</param>
		/// <returns>If the specified member is marked with the <see cref="DefaultValueAttribute"/> attribute,
		/// returns the value of the attribute; otherwise, <see langword="null"/>.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="member"/> is <see langword="null"/>.
		/// </exception>
		public static object GetDefaultValue(MemberDescriptor member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}

			DefaultValueAttribute attribute = (DefaultValueAttribute)member.Attributes[typeof(DefaultValueAttribute)];

			if (attribute == null)
			{
				return attribute;
			}
			else
			{
				return attribute.Value;
			}
		}

		/*
		 * GetPropertyFromString
		 */

		/// <summary>
		/// Gets the property name by a previously assigned custom name.
		/// </summary>
		/// <param name="str">Specifies the custom name for the property.</param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="str"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="str"/> is an empty string.
		/// </para>
		/// </exception>
		public static string GetPropertyFromString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				throw new ArgumentNullException("str");
			}

			string propertyName = NuGenSerializer.StringToProperty[str] as string;
			return propertyName == null ? str : propertyName;
		}

		/*
		 * GetStringFromProperty
		 */

		/// <summary>
		/// Gets the previously assigned custom name for the specified property.
		/// </summary>
		/// <param name="propertyName">Specifies the property name.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///	<paramref name="propertyName"/> is <see langword="null"/>.
		///	</exception>
		public static string GetStringFromProperty(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			string str = (string)NuGenSerializer.PropertyToString[propertyName];
			return str == null ? propertyName : str;
		}

		/*
		 * GetTypeOfArrayElement
		 */

		/// <summary>
		/// Retrieves the type of elements in the specified array. Can be <see langword="null"/>.
		/// </summary>
		/// <param name="array">Specifies the array to retrieve the type of elements for.</param>
		/// <returns>If the specified array is <see langword="null"/>, returns <see cref="Object"/>.</returns>
		public static Type GetTypeOfArrayElement(object array)
		{
			if (array == null)
			{
				return typeof(object);
			}

			Type arrayType = array.GetType();

			if (arrayType.GetElementType() != null)
			{
				return arrayType.GetElementType();
			}

			MethodInfo[] arrayMethods = arrayType.GetMethods();

			foreach (MethodInfo method in arrayMethods)
			{
				if (method.Name == "get_Item")
				{
					return method.ReturnType;
				}
			}

			string arrayFullName = array.GetType().FullName;

			return NuGenTypeFinder.GetType(
				arrayFullName.Substring(0, arrayFullName.Length - 2)
			);
		}

		/*
		 * SetProperty
		 */

		/// <summary>
		/// Sets the specified value of the property for the specified <see cref="Object"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the property to set the value for.</param>
		/// <param name="obj">Specifies the <see cref="Object"/> to set the property value for.</param>
		/// <param name="value">Specifies the value to set for the property.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="propertyInfo"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="obj"/> is <see langword="null"/>.
		/// </exception>
		public static void SetProperty(PropertyInfo propertyInfo, object obj, object value)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}

			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (propertyInfo.CanWrite)
			{
				propertyInfo.SetValue(obj, value, null);
			}
		}

		#endregion

		#region Methods.Deserialize

		/// <summary>
		/// Deserializes the specified <see cref="Object"/> from the specified path.
		/// </summary>
		/// <param name="obj">Specifies the <see cref="Object"/> to deserialize.</param>
		/// <param name="path">Specifies the path to deserialize from.</param>
		/// <param name="application">Specifies the name of the application that serialized the specified <see cref="T:Object"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="path"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="path"/> is an empty string.
		/// </para>
		/// </exception>
		public void Deserialize(object obj, string path, string application)
		{
			if (string.IsNullOrEmpty("path"))
			{
				throw new ArgumentNullException("path");
			}

			FileStream fs = null;

			try
			{
				fs = new FileStream(path, FileMode.Open, FileAccess.Read);
				this.Deserialize(obj, fs, application);
			}
			catch (MissingMethodException)
			{
			}
			catch
			{
				throw;
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
				}
			}
		}

		/// <summary>
		/// Deserializes the specified <see cref="Object"/> from the specified path.
		/// </summary>
		/// <param name="obj">Specifies the <see cref="Object"/> to deserialize. Can be <see langword="null"/>.</param>
		/// <param name="stream">Specifies the <see cref="Stream"/> to deserialize from.</param>
		/// <param name="application">Specifies the name of the application that serialized the specified <see cref="T:Object"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="stream"/> is <see langword="null"/>.
		/// </exception>
		public void Deserialize(object obj, Stream stream, string application)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			CultureInfo currentCulture = Application.CurrentCulture;
			NuGenXmlTextReader xmlTextReader = null;

			try
			{
				Application.CurrentCulture = NuGenCulture.enUS;
				xmlTextReader = new NuGenXmlTextReader(stream);

				while (xmlTextReader.Read())
				{
					if (xmlTextReader.IsStartElement() && xmlTextReader.Name == this.Name)
					{
						_graph = new NuGenGraph();
						_references = new NuGenReferenceCollection();

						_graph.Add(obj);

						string applicationName = xmlTextReader.GetAttribute(Resources.XmlAttribute_Application);

						if (applicationName.Equals(application))
						{
							this.DeserializeObject(obj, this.DeserializeObject(xmlTextReader));
							this.SetReferenceDeserializing();
						}

						return;
					}

					xmlTextReader.Read();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				Application.CurrentCulture = currentCulture;

				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
			}
		}

		#endregion

		#region Methods.DeserializeObject

		private NuGenPropertyInfoCollection DeserializeObject(XmlTextReader xmlTextReader)
		{
			Debug.Assert(xmlTextReader != null, "xmlTextReader != null");

			NuGenPropertyInfoCollection propertyInfoCollection = new NuGenPropertyInfoCollection();
			xmlTextReader.Read();

			while (!xmlTextReader.EOF)
			{
				if (xmlTextReader.IsStartElement())
				{
					this.OnDeserializing(EventArgs.Empty);

					string propertyString = xmlTextReader.Name;

					bool isKey = xmlTextReader.GetAttribute(Resources.XmlAttribute_IsKey) == Resources.XmlValue_True;
					bool isRef = xmlTextReader.MoveToAttribute(Resources.XmlAttribute_IsRef);
					bool isList = xmlTextReader.MoveToAttribute(Resources.XmlAttribute_IsList);

					NuGenPropertyInfo propertyInfo = new NuGenPropertyInfo(
						XmlConvert.DecodeName(NuGenSerializer.GetPropertyFromString(propertyString)),
						null,
						isKey,
						isRef,
						isList
						);

					if (xmlTextReader.GetAttribute(Resources.XmlAttribute_IsNull) == Resources.XmlValue_True)
					{
						propertyInfo.Value = null;
					}
					else if (xmlTextReader.GetAttribute(Resources.XmlAttribute_IsSer) != null)
					{
						string refAttrValue = xmlTextReader.GetAttribute(Resources.XmlAttribute_Ref);

						if (refAttrValue != null)
						{
							propertyInfo.ReferenceCode = int.Parse(refAttrValue, CultureInfo.InvariantCulture);
						}

						string typeAttrValue = xmlTextReader.GetAttribute(Resources.XmlAttribute_Type);

						Type typeFromAttr = NuGenSerializer.StringToType[typeAttrValue] as Type;

						if (typeFromAttr == null)
						{
							propertyInfo.Value = NuGenActivator.CreateObject(typeAttrValue);
						}
						else
						{
							propertyInfo.Value = NuGenActivator.CreateObject(typeFromAttr);
						}

						propertyInfo.IsSerializable = true;
						INuGenSerializable serializable = (INuGenSerializable)propertyInfo.Value;
						serializable.Deserialize(_converter, xmlTextReader);

						_graph.Add(serializable, propertyInfo.ReferenceCode);
					}
					else if (propertyInfo.IsReference)
					{
						propertyInfo.ReferenceCode = int.Parse(xmlTextReader.GetAttribute(Resources.XmlAttribute_IsRef), CultureInfo.InvariantCulture);
					}
					else if (propertyInfo.IsList)
					{
						propertyInfo.Count = int.Parse(xmlTextReader.GetAttribute(Resources.XmlAttribute_Count), CultureInfo.InvariantCulture);

						if ((propertyInfo.Count > 0) && !xmlTextReader.IsEmptyElement)
						{
							propertyInfo.Properties = this.DeserializeObject(xmlTextReader);
						}
					}
					else if (!propertyInfo.IsKey)
					{
						if (xmlTextReader.GetAttribute(Resources.XmlAttribute_IsImage) == Resources.XmlValue_True)
						{
							int bufferLength = int.Parse(xmlTextReader.GetAttribute(Resources.XmlAttribute_Length), CultureInfo.InvariantCulture);
							byte[] buffer = new byte[bufferLength];
							xmlTextReader.ReadBase64(buffer, 0, bufferLength);
							propertyInfo.Value = NuGenImageConverter.BytesToImage(buffer);
						}
						else if (xmlTextReader.GetAttribute(Resources.XmlAttribute_IsEmfImage) == Resources.XmlValue_True)
						{
							int bufferLength = int.Parse(xmlTextReader.GetAttribute(Resources.XmlAttribute_Length), CultureInfo.InvariantCulture);
							byte[] buffer = new byte[bufferLength];
							xmlTextReader.ReadBase64(buffer, 0, bufferLength);
							propertyInfo.Value = NuGenMetafileConverter.BytesToMetafile(buffer);
						}
						else
						{
							propertyInfo.Value = xmlTextReader.ReadString();
						}
					}
					else if (propertyInfo.IsKey)
					{
						string attrRef = xmlTextReader.GetAttribute(Resources.XmlAttribute_Ref);

						if (attrRef != null)
						{
							propertyInfo.ReferenceCode = int.Parse(attrRef, CultureInfo.InvariantCulture);
						}

						string typeAttr = xmlTextReader.GetAttribute(Resources.XmlAttribute_Type);
						Type typeFromAttr = NuGenSerializer.StringToType[typeAttr] as Type;

						if (typeFromAttr == null)
						{
							propertyInfo.Value = NuGenActivator.CreateObject(typeAttr);
						}
						else
						{
							propertyInfo.Value = NuGenActivator.CreateObject(typeFromAttr);
						}

						if (!xmlTextReader.IsEmptyElement)
						{
							propertyInfo.Properties = this.DeserializeObject(xmlTextReader);
						}
					}

					propertyInfoCollection.Add(propertyInfo);
				}
				else if (xmlTextReader.NodeType == XmlNodeType.EndElement)
				{
					return propertyInfoCollection;
				}

				xmlTextReader.Read();
			}

			return propertyInfoCollection;
		}

		/// <summary>
		/// Deserializes the specified <see cref="Object"/> from the specified <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="obj">Specifies the <see cref="Object"/> to serialize.</param>
		/// <param name="properties">Specifies the list of properties to deserialize.</param>
		private void DeserializeObject(object obj, NuGenPropertyInfoCollection properties)
		{
			foreach (NuGenPropertyInfo property in properties)
			{
				this.OnDeserializing(EventArgs.Empty);

				PropertyInfo propertyInfo = null;

				if (obj != null)
				{
					propertyInfo = obj.GetType().GetProperty(property.Name);
				}

				if (!property.IsReference && (property.ReferenceCode != -1))
				{
					_graph.Add(property.Value, property.ReferenceCode);
				}

				if (property.IsKey)
				{
					NuGenSerializer.SetProperty(propertyInfo, obj, property.Value);

					if (propertyInfo != null)
					{
						this.DeserializeObject(propertyInfo.GetValue(obj, null), property.Properties);
						continue;
					}

					this.DeserializeObject(null, property.Properties);
				}
				else
				{
					if (property.IsList)
					{
						int count = property.Count;

						IList list = null;
						object propertyValue = null;

						if (propertyInfo != null)
						{
							list = propertyInfo.GetValue(obj, null) as IList;

							if (list != null)
							{
								Type listElementType = NuGenSerializer.GetTypeOfArrayElement(list);

								if (list is Array)
								{
									list = Array.CreateInstance(listElementType, count);
									NuGenSerializer.SetProperty(propertyInfo, obj, list);
								}
								else
								{
									list.Clear();
								}

								int listPropertyCount = 0;

								foreach (NuGenPropertyInfo listProperty in property.Properties)
								{
									this.OnDeserializing(EventArgs.Empty);

									if (listProperty.IsReference && (listProperty.ReferenceCode != -1))
									{
										listProperty.Value = _graph[listProperty.ReferenceCode];
										propertyValue = listProperty.Value;
									}
									else if ((listElementType == typeof(string)) || !(listProperty.Value is string))
									{
										propertyValue = listProperty.Value;
									}
									else
									{
										try
										{
											propertyValue = _converter.StringToObject(
												(string)listProperty.Value,
												listElementType
												);
										}
										catch
										{
											propertyValue = (string)listProperty.Value;
										}
									}

									if (!listProperty.IsReference && (listProperty.ReferenceCode != -1))
									{
										_graph.Add(listProperty.Value, listProperty.ReferenceCode);
									}

									if (list is Array)
									{
										object elementValue = _converter.StringToObject((string)propertyValue, listElementType);
										((Array)list).SetValue(elementValue, listPropertyCount++);
									}
									else
									{
										list.Add(propertyValue);

										if (!listProperty.IsReference && !listProperty.IsSerializable)
										{
											this.DeserializeObject(propertyValue, listProperty.Properties);
										}
									}
								}
							}
						}

						continue;
					}

					if (property.IsReference)
					{
						object propertyValue = _graph[property.ReferenceCode];

						NuGenSerializer.SetProperty(propertyInfo, obj, propertyValue);

						if (propertyValue == null)
						{
							_references.Add(
								property,
								propertyInfo,
								obj
							);
						}
					}
					else
					{
						if (property.Value == null)
						{
							NuGenSerializer.SetProperty(propertyInfo, obj, null);
							continue;
						}

						if (propertyInfo != null)
						{
							object propertyValue = property.Value;

							if ((property.Value is string) && (propertyInfo.PropertyType != typeof(object)))
							{
								propertyValue = _converter.StringToObject((string)property.Value, propertyInfo.PropertyType);
							}

							NuGenSerializer.SetProperty(propertyInfo, obj, propertyValue);
						}
					}
				}
			}
		}

		#endregion

		#region Methods.Serialize

		/// <summary>
		/// Serializes the specified <see cref="Object"/> to the specified path.
		/// </summary>
		/// <param name="obj">Specifies the <see cref="Object"/> to serialize.</param>
		/// <param name="path">Specifies the path to serialize to.</param>
		/// <param name="application">Specifies the name of the application that calls this <see cref="NuGenSerializer"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="path"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="path"/> is an empty string.
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public void Serialize(object obj, string path, string application)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			FileStream fs = null;

			try
			{
				fs = new FileStream(path, FileMode.Create, FileAccess.Write);
				this.Serialize(obj, fs, application);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
				}
			}
		}

		/// <summary>
		/// Serializes the specified <see cref="Object"/> to the specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="obj">Specifies the <see cref="Object"/> to serialize.</param>
		/// <param name="stream">Specifies the <see cref="Stream"/> to serialize to.</param>
		/// <param name="application">Specifies the name of the application that calls this <see cref="NuGenSerializer"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="stream"/> is <see langword="null"/>.
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public void Serialize(object obj, Stream stream, string application)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			CultureInfo currentCulture = Application.CurrentCulture;

			try
			{
				Application.CurrentCulture = NuGenCulture.enUS;

				NuGenXmlTextWriter writer = new NuGenXmlTextWriter(stream);

				writer.WriteStartDocument(true);
				writer.WriteStartElement(this.Name);
				writer.WriteAttributeString(Resources.XmlAttribute_Version, this.Version);
				writer.WriteAttributeString(Resources.XmlAttribute_Version, application);

				_graph = new NuGenGraph();
				_references = new NuGenReferenceCollection();

				NuGenPropertyInfoCollection propertyInfoCollection = this.SerializeObject(obj);

				this.SetReferenceSerializing();

				this.SerializeObject(writer, propertyInfoCollection);

				writer.WriteEndElement();
				writer.Flush();
			}
			catch
			{
				throw;
			}
			finally
			{
				Application.CurrentCulture = currentCulture;
			}
		}

		#endregion

		#region Methods.SerializeList

		/// <summary>
		/// Serializes the specified list.
		/// </summary>
		/// <param name="properties">Specifies the <see cref="NuGenPropertyInfoCollection"/> to be filled
		/// with the elements of the specified list.</param>
		/// <param name="list">Specifies the list to serialize.</param>
		/// <returns>Returns the number of elements in the specified list.</returns>
		private int SerializeList(NuGenPropertyInfoCollection properties, object list)
		{
			if (list == null)
			{
				return 0;
			}

			Debug.Assert(properties != null, "properties != null");
			Debug.Assert(list is IList, "list is IList");

			int count = 0;

			foreach (object item in (IList)list)
			{
				this.OnSerializing(EventArgs.Empty);

				if (
					(item.GetType().IsPrimitive)
					|| (item is string)
					|| (this.IsContent(item))
					)
				{
					properties.Add(
						new NuGenPropertyInfo(Resources.XmlAttribute_Value, item, false, false, false)
					);
				}
				else
				{
					int index = _graph[item];

					if (index == -1)
					{
						_graph.Add(item);

						NuGenPropertyInfo propertyInfo = null;

						if (this.CheckSerializable && (item is INuGenSerializable))
						{
							propertyInfo = new NuGenPropertyInfo(
								string.Format(CultureInfo.InvariantCulture, "{0}{1}", Resources.XmlTag_Item, (count + 1).ToString(CultureInfo.InvariantCulture)),
								item,
								true,
								false,
								false
							);

							propertyInfo.IsSerializable = true;
						}
						else
						{
							string propertyName = Resources.XmlTag_Item;

							PropertyInfo namePropertyInfo = item.GetType().GetProperty("Name");

							if (namePropertyInfo != null)
							{
								string namePropertyValue = (string)namePropertyInfo.GetValue(item, null);

								if (!string.IsNullOrEmpty(namePropertyValue))
								{
									propertyName = namePropertyValue;
								}
							}

							propertyInfo = new NuGenPropertyInfo(
								string.Format(CultureInfo.InvariantCulture, "{0}{1}", propertyName, (count + 1).ToString(CultureInfo.InvariantCulture)),
								item,
								true,
								false,
								false
							);

							propertyInfo.Properties.AddRange(this.SerializeObject(item));
						}

						Debug.Assert(propertyInfo != null, "propertyInfo != null");

						propertyInfo.ReferenceCode = _graph[item];
						properties.Add(propertyInfo);
					}
					else
					{
						NuGenPropertyInfo propertyInfo = new NuGenPropertyInfo(
							Resources.XmlTag_Item,
							item,
							true,
							true,
							false
						);

						propertyInfo.ReferenceCode = index;
						properties.Add(propertyInfo);
					}
				}

				count++;
			}

			return count;
		}

		#endregion

		#region Methods.SerializeObject

		/// <summary>
		/// Serializes the specified <see cref="Object"/>. Can be <see langword="null"/>.
		/// </summary>
		/// <param name="obj">Specifies the <see cref="Object"/> to serialize.</param>
		/// <returns></returns>
		public NuGenPropertyInfoCollection SerializeObject(object obj)
		{
			if (obj == null)
			{
				return null;
			}

			if (_graph[obj] == -1)
			{
				_graph.Add(obj);
			}

			NuGenPropertyInfoCollection properties = new NuGenPropertyInfoCollection();
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(obj);

			/* If no public properties found return null. */

			if ((descriptors == null) || (descriptors.Count == 0))
			{
				return null;
			}

			if (this.SortProperties)
			{
				descriptors = descriptors.Sort();
			}

			/*
			 * Serialize public properties.
			 */

			for (int i = 0; i < descriptors.Count; i++)
			{
				this.OnSerializing(EventArgs.Empty);

				PropertyDescriptor descriptor = descriptors[i];

				NuGenSerializationVisibilityAttribute attribute = (NuGenSerializationVisibilityAttribute)descriptor.Attributes[typeof(NuGenSerializationVisibilityAttribute)];

				/* This is the default value for the property that is not marked with
				 * NuGenSerializationVisibilityAttribute attribute. */
				NuGenSerializationVisibility visibility = NuGenSerializationVisibility.Content;

				object propertyDefaultValue = NuGenSerializer.GetDefaultValue(descriptor);
				string propertyName = descriptor.Name;
				object propertyValue = descriptor.GetValue(obj);

				/* If the property is marked with the NuGenSerializationVisibilityAttribute attribute... */
				if (attribute != null)
				{
					/* Skip properties that are not visible for the serializer. */
					if (attribute.Visibility == NuGenSerializationVisibility.Hidden)
					{
						continue;
					}

					visibility = attribute.Visibility;
				}
				/* If the property is not marked with the NuGenSerializationVisibilityAttribute attribute... */
				else
				{
					if (
						(descriptor.SerializationVisibility == DesignerSerializationVisibility.Content) &&
						!(propertyValue is IList) &&
						!(descriptor.PropertyType.IsEnum)
						)
					{
						visibility = NuGenSerializationVisibility.Reference;
					}
					else if (
						!(descriptor.ShouldSerializeValue(obj)) ||
						(descriptor.SerializationVisibility == DesignerSerializationVisibility.Hidden)
						)
					{
						continue;
					}
				}

				NuGenPropertyInfo propertyInfo = null;

				if (propertyValue == null)
				{
					propertyInfo = new NuGenPropertyInfo(
						propertyName,
						propertyValue,
						propertyDefaultValue,
						true,
						false,
						false
					);
				}
				else
				{
					switch (visibility)
					{
						case NuGenSerializationVisibility.Reference:
						{
							propertyInfo = new NuGenPropertyInfo(
								propertyName,
								propertyValue,
								propertyDefaultValue,
								true,
								true,
								false
							);
							_references.Add(propertyInfo);
							break;
						}
						case NuGenSerializationVisibility.Content:
						{

							/*
							 * Automatically determine if the marked property is a collection.
							 */

							if (propertyValue is IList)
							{
								propertyInfo = new NuGenPropertyInfo(
									propertyName,
									propertyValue,
									propertyDefaultValue,
									false,
									false,
									true
								);
								propertyInfo.Count = this.SerializeList(
									propertyInfo.Properties,
									propertyValue
								);
							}
							else
							{
								propertyInfo = new NuGenPropertyInfo(
									propertyName,
									propertyValue,
									propertyDefaultValue,
									false,
									false,
									false
								);
							}

							break;
						}
						case NuGenSerializationVisibility.Class:
						{
							if (!propertyValue.GetType().IsClass)
							{
								throw new InvalidCastException(
									string.Format(CultureInfo.InvariantCulture, Properties.Resources.InvalidCast_NotClass, propertyName)
								);
							}

							NuGenReferenceIgnoreAttribute[] ignoreAttributes = (NuGenReferenceIgnoreAttribute[])descriptor.PropertyType.GetCustomAttributes(typeof(NuGenReferenceIgnoreAttribute), false);

							int valueInitial = _graph[propertyValue];

							if ((valueInitial == -1) || (ignoreAttributes.Length > 0))
							{
								_graph.Add(propertyValue);

								int valueAfterAdd = _graph[propertyValue];

								if (
									(this.CheckSerializable)
									&& (propertyValue is INuGenSerializable)
									)
								{
									propertyInfo = new NuGenPropertyInfo(
										propertyName,
										propertyValue,
										propertyDefaultValue,
										true,
										false,
										false
									);

									propertyInfo.IsSerializable = true;
								}
								else
								{
									propertyInfo = new NuGenPropertyInfo(
										propertyName,
										propertyValue,
										propertyDefaultValue,
										true,
										false,
										false
									);

									NuGenPropertyInfoCollection innerProperties = this.SerializeObject(propertyValue);

									if (innerProperties != null)
									{
										propertyInfo.Properties.AddRange(innerProperties);
									}
								}

								propertyInfo.ReferenceCode = valueAfterAdd;
							}
							else
							{
								propertyInfo = new NuGenPropertyInfo(
									propertyName,
									propertyValue,
									propertyDefaultValue,
									true,
									true,
									false
								);

								propertyInfo.ReferenceCode = valueInitial;
							}

							break;
						}
					}
				}

				if (propertyInfo != null)
				{
					properties.Add(propertyInfo);
				}
			}

			return properties;
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		private void SerializeObject(XmlTextWriter xmlTextWriter, NuGenPropertyInfoCollection properties)
		{
			if (properties != null)
			{
				foreach (NuGenPropertyInfo property in properties)
				{
					this.OnSerializing(EventArgs.Empty);

					if (
						(property.DefaultValue == null) ||
						!(object.Equals(property.DefaultValue, property.Value))
						)
					{
						this.SerializeProperty(xmlTextWriter, property);
					}
				}
			}
		}

		#endregion

		#region Methods.SerializeProperty

		[SecurityPermission(SecurityAction.LinkDemand)]
		private void SerializeProperty(XmlTextWriter xmlTextWriter, NuGenPropertyInfo propertyInfo)
		{
			string propertyName = XmlConvert.EncodeName(
				NuGenSerializer.GetStringFromProperty(propertyInfo.Name)
				);

			if (propertyInfo.Value == null)
			{
				if (propertyInfo.DefaultValue != null)
				{
					xmlTextWriter.WriteStartElement(propertyName);
					xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsNull, Resources.XmlValue_True);
					xmlTextWriter.WriteEndElement();
				}
			}
			else if (propertyInfo.IsKey)
			{
				if (propertyInfo.IsReference)
				{
					xmlTextWriter.WriteStartElement(propertyName);
					xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsRef, propertyInfo.ReferenceCode.ToString(CultureInfo.InvariantCulture));
					xmlTextWriter.WriteEndElement();
				}
				else
				{
					xmlTextWriter.WriteStartElement(propertyName);

					if (propertyInfo.ReferenceCode != -1)
					{
						xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_Ref, propertyInfo.ReferenceCode.ToString(CultureInfo.InvariantCulture));
					}

					if (propertyInfo.Value != null)
					{
						string typeString = (string)NuGenSerializer.TypeToString[propertyInfo.Value.GetType()];

						if (typeString == null)
						{
							typeString = propertyInfo.Value.GetType().FullName;
						}

						xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_Type, typeString);
					}

					if (propertyInfo.IsSerializable)
					{
						xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsSer, Resources.XmlValue_True);

						INuGenSerializable serializable = (INuGenSerializable)propertyInfo.Value;
						serializable.Serialize(_converter, xmlTextWriter);

						_graph.Add(serializable, propertyInfo.ReferenceCode);
					}
					else
					{
						xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsKey, Resources.XmlValue_True);
						this.SerializeObject(xmlTextWriter, propertyInfo.Properties);
					}

					xmlTextWriter.WriteEndElement();
				}
			}
			else if (propertyInfo.IsList)
			{
				xmlTextWriter.WriteStartElement(propertyName);
				xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsList, Resources.XmlValue_True);

				if (propertyInfo.Value != null)
				{
					xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_Count, propertyInfo.Count.ToString(CultureInfo.InvariantCulture));
				}

				this.SerializeObject(xmlTextWriter, propertyInfo.Properties);
				xmlTextWriter.WriteEndElement();
			}
			else if (propertyInfo.Value is Metafile)
			{
				xmlTextWriter.WriteStartElement(propertyName);

				Metafile metafile = (Metafile)propertyInfo.Value;
				byte[] buffer = NuGenMetafileConverter.MetafileToBytes(metafile);

				xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsEmfImage, Resources.XmlValue_True);
				xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_Length, buffer.Length.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteBase64(buffer, 0, buffer.Length);

				xmlTextWriter.WriteEndElement();
			}
			else if (propertyInfo.Value is Image)
			{
				xmlTextWriter.WriteStartElement(propertyName);

				Image image = (Image)propertyInfo.Value;
				byte[] buffer = NuGenImageConverter.ImageToBytes(image);

				xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_IsImage, Resources.XmlValue_True);
				xmlTextWriter.WriteAttributeString(Resources.XmlAttribute_Length, buffer.Length.ToString(CultureInfo.InvariantCulture));
				xmlTextWriter.WriteBase64(buffer, 0, buffer.Length);

				xmlTextWriter.WriteEndElement();
			}
			else
			{
				string objectString = _converter.ObjectToString(propertyInfo.Value);

				if (objectString != null)
				{
					xmlTextWriter.WriteStartElement(propertyName);
					xmlTextWriter.WriteString(objectString);
					xmlTextWriter.WriteEndElement();
				}
			}
		}

		#endregion

		private Hashtable _contentHashtable = new Hashtable();
		private INuGenObjectStringConverter _converter;
		private NuGenGraph _graph;
		private NuGenReferenceCollection _references;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSerializer"/> class.
		/// </summary>
		public NuGenSerializer()
			: this(new NuGenObjectStringConverter())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSerializer"/> class.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
		public NuGenSerializer(INuGenObjectStringConverter converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}

			_converter = converter;
		}
	}
}
