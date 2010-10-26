/* -----------------------------------------------
 * Number.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.NuGenVisiCalc.Types
{
	[Serializable]
	[Type("Number")]
	internal class Number : Type
	{
		private System.Boolean _isRandom;

		public System.Boolean IsRandom
		{
			get
			{
				return _isRandom;
			}
			set
			{
				_isRandom = value;

				GetOutput(0).Data = 0.0;

				if (_isRandom)
				{
					GetOutput(0).Name = "Random";
				}
				else
				{
					GetOutput(0).Name = "0";
				}
			}
		}

		public Double Value
		{
			get
			{
				return (Double)GetOutput(0).Data;
			}
			set
			{
				GetOutput(0).Name = value.ToString(CultureInfo.CurrentCulture);
				GetOutput(0).Data = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Number"/> class.
		/// </summary>
		public Number()
			: this(0)
		{
		}

		public Number(Double initialValue)
		{
			Name = Header = "Number";
			CreateOutputs(1);
			SetOutput(0, initialValue.ToString(CultureInfo.CurrentCulture), initialValue);
		}
	}
}
