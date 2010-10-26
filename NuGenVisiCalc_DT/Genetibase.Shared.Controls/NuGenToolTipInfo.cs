/* -----------------------------------------------
 * NuGenToolTipInfo.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.Serialization;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Encapsulates data associated with a tooltip for a specific control.
	/// </summary>
	[TypeConverter(typeof(NuGenToolTipInfoConverter))]
	public sealed class NuGenToolTipInfo
	{
		#region Properties.Public

		/*
		 * CustomSize
		 */

		private Size _customSize;

		/// <summary>
		/// </summary>
		public Size CustomSize
		{
			get
			{
				return _customSize;
			}
			set
			{
				_customSize = value;
			}
		}

		/*
		 * Header
		 */

		private string _header;

		/// <summary>
		/// </summary>
		public string Header
		{
			get
			{
				return _header;
			}
			set
			{
				_header = value;
			}
		}

		/*
		 * Image
		 */

		private Image _image;

		/// <summary>
		/// </summary>
		[DefaultValue(null)]
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
			}
		}

		/*
		 * Text
		 */

		private string _text;

		/// <summary>
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		/*
		 * RemarksHeader
		 */

		private string _remarksHeader;

		/// <summary>
		/// </summary>
		public string RemarksHeader
		{
			get
			{
				return _remarksHeader;
			}
			set
			{
				_remarksHeader = value;
			}
		}

		/*
		 * RemarksImage
		 */

		private Image _remarksImage;

		/// <summary>
		/// </summary>
		[DefaultValue(null)]
		public Image RemarksImage
		{
			get
			{
				return _remarksImage;
			}
			set
			{
				_remarksImage = value;
			}
		}

		/*
		 * Remarks
		 */

		private string _remarks;

		/// <summary>
		/// </summary>
		public string Remarks
		{
			get
			{
				return _remarks;
			}
			set
			{
				_remarks = value;
			}
		}

		#endregion

		#region Properties.Internal

		internal bool IsCustomSize
		{
			get
			{
				return _customSize != Size.Empty;
			}
		}

		internal bool IsHeaderVisible
		{
			get
			{
				return NuGenToolTipInfo.CheckStringVisible(_header);
			}
		}

		internal bool IsImageVisible
		{
			get
			{
				return NuGenToolTipInfo.CheckImageVisible(_image);
			}
		}

		internal bool IsTextVisible
		{
			get
			{
				return NuGenToolTipInfo.CheckStringVisible(_text);
			}
		}

		internal bool IsRemarksHeaderVisible
		{
			get
			{
				return NuGenToolTipInfo.CheckStringVisible(_remarksHeader);
			}
		}

		internal bool IsRemarksImageVisible
		{
			get
			{
				return NuGenToolTipInfo.CheckImageVisible(_remarksImage);
			}
		}

		internal bool IsRemarksVisible
		{
			get
			{
				return NuGenToolTipInfo.CheckStringVisible(_remarks);
			}
		}

		#endregion

		#region Methods.Private

		private static bool CheckImageVisible(Image value)
		{
			return value != null;
		}

		private static bool CheckStringVisible(string value)
		{
			return !string.IsNullOrEmpty(value);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolTipInfo"/> class.
		/// </summary>
		public NuGenToolTipInfo()
			: this(null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolTipInfo"/> class.
		/// </summary>
		public NuGenToolTipInfo(
			string header,
			Image image,
			string text
			)
			: this(header, image, text, null, null, null, Size.Empty)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolTipInfo"/> class.
		/// </summary>
		/// <param name="header"></param>
		/// <param name="image"></param>
		/// <param name="text"></param>
		/// <param name="remarksHeader"></param>
		/// <param name="remarksImage"></param>
		/// <param name="remarks"></param>
		/// <param name="customSize">Specify <c>Size.Empty</c> if the tooltip size should be auto-calculated.</param>
		public NuGenToolTipInfo(
			string header,
			Image image,
			string text,
			string remarksHeader,
			Image remarksImage,
			string remarks,
			Size customSize
			)
		{
			_header = header;
			_image = image;
			_text = text;
			_remarksHeader = remarksHeader;
			_remarksImage = remarksImage;
			_remarks = remarks;
			_customSize = customSize;
		}
	}
}
