/* -----------------------------------------------
 * ISampleFolderDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SampleFramework
{
	internal interface ISampleFolderDescriptor
	{
		string CSProjectExtension
		{
			get;
			set;
		}

		string CSProjectFolderName
		{
			get;
			set;
		}

		string DescriptionFileName
		{
			get;
			set;
		}

		string Path
		{
			get;
			set;
		}

		string ScreenshotFileName
		{
			get;
			set;
		}

		string VBProjectExtension
		{
			get;
			set;
		}

		string VBProjectFolderName
		{
			get;
			set;
		}
	}
}
