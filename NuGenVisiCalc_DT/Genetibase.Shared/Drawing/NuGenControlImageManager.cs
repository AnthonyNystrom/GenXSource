/* -----------------------------------------------
 * INuGenControlImageManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Represents a collection of <see cref="NuGenControlImageDescriptor"/> instances.
	/// </summary>
	public sealed class NuGenControlImageManager : INuGenControlImageManager
	{
		#region INuGenControlImageManager Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public INuGenControlImageDescriptor CreateImageDescriptor()
		{
			return new NuGenControlImageDescriptor();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlImageManager"/> class.
		/// </summary>
		public NuGenControlImageManager()
		{

		}

		#endregion
	}
}
