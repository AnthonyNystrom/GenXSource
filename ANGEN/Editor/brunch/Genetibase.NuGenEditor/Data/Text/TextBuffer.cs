/* -----------------------------------------------
 * TextBuffer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Genetibase.Windows.Controls.Data.Text
{
	/// <summary>
	/// </summary>
	public abstract class TextBuffer : ITextReader, IPropertyOwner
	{
		/// <summary>
		/// </summary>
		public event EventHandler<TextChangedEventArgs> Changed;

		#region Properties.Abstract

		/// <summary>
		/// </summary>
		/// <value></value>
		public abstract Char this[Int32 position]
		{
			get;
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		public abstract Int32 Length
		{
			get;
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		public abstract Int32 LineCount
		{
			get;
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		public abstract ITextVersion Version
		{
			get;
		}

		#endregion

		#region Properties

		/*
		 * DocumentType
		 */

		private String _documentType;

		/// <summary>
		/// </summary>
		/// <value></value>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="value"/> is <see langword="null"/>.</para>
		/// </exception>
		public virtual String DocumentType
		{
			get
			{
				return _documentType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				_documentType = value;
			}
		}

		/*
		 * Properties
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		public IEnumerable<KeyValuePair<Object, Object>> Properties
		{
			get
			{
				return _properties;
			}
		}

		#endregion

		#region Methods.Abstract

		/// <summary>
		/// </summary>
		public abstract void CopyTo(Int32 sourceIndex, Char[] destination, Int32 destinationIndex, Int32 count);

		/// <summary>
		/// </summary>
		public abstract ITextEdit CreateEdit();

		/// <summary>
		/// </summary>
		public abstract void Delete(Int32 startPosition, Int32 charsToDelete);

		/// <summary>
		/// </summary>
		public abstract ITextReader Freeze(ITextVersion version);

		/// <summary>
		/// </summary>
		public abstract Int32 GetEndOfLineFromPosition(Int32 position);

		/// <summary>
		/// </summary>
		public abstract Int32 GetLengthOfLineFromLineNumber(Int32 line);

		/// <summary>
		/// </summary>
		public abstract Int32 GetLineNumberFromPosition(Int32 position);

		/// <summary>
		/// </summary>
		public abstract Int32 GetLineOffsetFromPosition(Int32 position);

		/// <summary>
		/// </summary>
		public abstract Int32 GetStartOfLineFromLineNumber(Int32 line);

		/// <summary>
		/// </summary>
		public abstract Int32 GetStartOfLineFromPosition(Int32 position);

		/// <summary>
		/// </summary>
		public abstract Int32 GetStartOfNextLineFromPosition(Int32 position);

		/// <summary>
		/// </summary>
		public abstract Int32 GetStartOfPreviousLineFromPosition(Int32 position);

		/// <summary>
		/// </summary>
		public abstract String GetText();

		/// <summary>
		/// </summary>
		public abstract String GetText(Int32 startIndex);

		/// <summary>
		/// </summary>
		public abstract String GetText(Int32 startIndex, Int32 length);

		/// <summary>
		/// </summary>
		public abstract void Insert(Int32 position, String text);

		/// <summary>
		/// </summary>
		public abstract void Replace(Int32 startPosition, Int32 charsToReplace, String replaceWith);

		/// <summary>
		/// </summary>
		public abstract Char[] ToCharArray();

		/// <summary>
		/// </summary>
		public abstract Char[] ToCharArray(Int32 startIndex, Int32 length);

		/// <summary>
		/// </summary>
		public abstract void Write(TextWriter writer);

		/// <summary>
		/// </summary>
		public abstract void Write(TextWriter writer, Int32 startIndex, Int32 length);

		#endregion

		#region Methods

		/*
		 * AddProperty
		 */

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="property"></param>
		public void AddProperty(Object key, Object property)
		{
			if (_properties == null)
			{
				_properties = new Dictionary<Object, Object>();
			}

			_properties.Add(key, property);
		}

		/*
		 * CompareVersions
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Circularity detected in text buffer version.
		/// </exception>
		public static Int32 CompareVersions(ITextVersion version1, ITextVersion version2)
		{
			ITextVersion version = version1;
			ITextVersion version3 = version2;

			if (version1 == version2)
			{
				return 0;
			}

			if (version1 == null)
			{
				return 1;
			}

			if (version2 == null)
			{
				return -1;
			}

			while (true)
			{
				version1 = version1.Next;
				version2 = version2.Next;

				if ((version1 == null) || (version2 == version))
				{
					return 1;
				}

				if ((version1 == version3) || (version2 == null))
				{
					return -1;
				}

				if ((version1 == version) || (version2 == version3))
				{
					throw new InvalidOperationException("Circularity detected in text buffer version.");
				}
			}
		}

		/*
		 * GetLengthOfVersion
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Specified <paramref name="version"/> does not belong to buffer.
		/// </exception>
		public virtual Int32 GetLengthOfVersion(ITextVersion version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (!this.VersionBelongsToBuffer(version))
			{
				throw new ArgumentException("Version does not belong to buffer.");
			}
			Int32 num = 0;

			while (version.Change != null)
			{
				num += version.Change.Delta;
				version = version.Next;
			}

			return (this.Length - num);
		}

		/*
		 * RemoveProperty
		 */

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public Boolean RemoveProperty(Object key)
		{
			if (_properties == null)
			{
				return false;
			}

			return _properties.Remove(key);
		}

		/*
		 * TryGetProperty
		 */

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public Boolean TryGetProperty<TProperty>(Object key, out TProperty property)
		{
			if (_properties != null)
			{
				Object obj2;
				Boolean flag = _properties.TryGetValue(key, out obj2);
				property = flag ? ((TProperty)obj2) : default(TProperty);
				return flag;
			}

			property = default(TProperty);
			return false;
		}

		/*
		 * VersionBelongsToBuffer
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		public virtual Boolean VersionBelongsToBuffer(ITextVersion version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}

			while (version != this.Version)
			{
				version = version.Next;

				if (version == null)
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		private Dictionary<Object, Object> _properties;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextBuffer"/> class.
		/// </summary>
		protected TextBuffer()
			: this("text")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextBuffer"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="documentType"/> is <see langword="null"/>.</para>
		/// </exception>
		protected TextBuffer(String documentType)
		{
			if (documentType == null)
			{
				throw new ArgumentNullException("documentType");
			}

			_documentType = documentType;
		}
	}
}
