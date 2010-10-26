/* -----------------------------------------------
 * Boolean.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.ComponentModel;
using System.Globalization;

namespace Genetibase.NuGenVisiCalc.Types
{
	[Serializable]
	[Type("Boolean")]
	internal class Boolean : Type
	{
		[NuGenSRCategory("Category_Data")]
		public System.Boolean Value
		{
			get
			{
				return (System.Boolean)GetOutput(0).Data;
			}
			set
			{
				GetOutput(0).Name = value.ToString(CultureInfo.CurrentCulture);
				GetOutput(0).Data = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Boolean"/> class.
		/// </summary>
		public Boolean()
			: this(false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Boolean"/> class.
		/// </summary>
		public Boolean(System.Boolean initialValue)
		{
			Name = Header = "Boolean";
			CreateOutputs(1);
			SetOutput(0, initialValue.ToString(CultureInfo.CurrentCulture), initialValue);
		}
	}
}
