/* -----------------------------------------------
 * SampleDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SampleFramework
{
	internal sealed class SampleDescriptor
	{
		/*
		 * SamplePath
		 */

		private string _samplePath;

		public string SamplePath
		{
			get
			{
				return _samplePath;
			}
			set
			{
				_samplePath = value;
			}
		}

		/*
		 * Description
		 */

		public bool DescriptionIsAvailable
		{
			get
			{
				return File.Exists(this.DescriptionPath);
			}
		}

		private string _descriptionPath;

		public string DescriptionPath
		{
			get
			{
				return _descriptionPath;
			}
			set
			{
				_descriptionPath = value;
			}
		}

		/*
		 * Image
		 */

		public bool ImageIsAvailable
		{
			get
			{
				return File.Exists(this.ImagePath);
			}
		}

		private string _imagePath;

		public string ImagePath
		{
			get
			{
				return _imagePath;
			}
			set
			{
				_imagePath = value;
			}
		}

		/*
		 * Exe
		 */

		public bool ExeIsAvailable
		{
			get
			{
				return File.Exists(this.ExePath);
			}
		}

		private string _exePath;

		public string ExePath
		{
			get
			{
				return _exePath;
			}
			set
			{
				_exePath = value;
			}
		}

		/*
		 * CS Project
		 */

		public bool CsProjectIsAvailable
		{
			get
			{
				return File.Exists(this.CsProjectPath);
			}
		}

		private string _csProjectPath;

		public string CsProjectPath
		{
			get
			{
				return _csProjectPath;
			}
			set
			{
				_csProjectPath = value;
			}
		}

		/*
		 * VB Project
		 */

		public bool VbProjectIsAvailable
		{
			get
			{
				return File.Exists(this.VbProjectPath);
			}
		}

		private string _vbProjectPath;

		public string VbProjectPath
		{
			get
			{
				return _vbProjectPath;
			}
			set
			{
				_vbProjectPath = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SampleDescriptor"/> class.
		/// </summary>
		/// <param name="samplePath"></param>
		/// <param name="descriptionPath">Can be <see langword="null"/>.</param>
		/// <param name="imagePath">Can be <see langword="null"/>.</param>
		/// <param name="exePath">Can be <see langword="null"/>.</param>
		/// <param name="csProjectPath">Can be <see langword="null"/>.</param>
		/// <param name="vbProjectPath">Can be <see langword="null"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="samplePath"/> is <see langword="null"/> or empty.</para>
		/// </exception>
		public SampleDescriptor(
			string samplePath
			, string descriptionPath
			, string imagePath
			, string exePath
			, string csProjectPath
			, string vbProjectPath
			)
		{
			if (string.IsNullOrEmpty(samplePath))
			{
				throw new ArgumentNullException("samplePath");
			}

			_samplePath = samplePath;
			_descriptionPath = descriptionPath;
			_imagePath = imagePath;
			_exePath = exePath;
			_csProjectPath = csProjectPath;
			_vbProjectPath = vbProjectPath;
		}
	}
}
