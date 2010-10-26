/* -----------------------------------------------
 * SampleFolder.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SampleFramework
{
	internal sealed class SampleFolderDescriptor : ISampleFolderDescriptor
	{
		private string _csProjectExtension;

		public string CSProjectExtension
		{
			get
			{
				return _csProjectExtension;
			}
			set
			{
				_csProjectExtension = value;
			}
		}

		private string _csProjectFolderName;

		public string CSProjectFolderName
		{
			get
			{
				return _csProjectFolderName;	
			}
			set
			{
				_csProjectFolderName = value;
			}
		}

		private string _descriptionFileName;

		public string DescriptionFileName
		{
			get
			{
				return _descriptionFileName;
			}
			set
			{
				_descriptionFileName = value;
			}
		}

		private string _path;

		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}

		private string _screenshotFileName;

		public string ScreenshotFileName
		{
			get
			{
				return _screenshotFileName;
			}
			set
			{
				_screenshotFileName = value;
			}
		}

		private string _vbProjectExtension;

		public string VBProjectExtension
		{
			get
			{
				return _vbProjectExtension;
			}
			set
			{
				_vbProjectExtension = value;
			}
		}

		private string _vbProjectFolderName;

		public string VBProjectFolderName
		{
			get
			{
				return _vbProjectFolderName;
			}
			set
			{
				_vbProjectFolderName = value;
			}
		}

		public SampleFolderDescriptor()
		{
		}
	}
}
