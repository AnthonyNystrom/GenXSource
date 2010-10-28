using System;
using System.ComponentModel;

namespace Genetibase.MathX.NugenCCalc
{


	/// <summary>
	/// <para>Provides the <b>abstract</b> base class for the all 2D function
	/// parameters.</para>
	/// </summary>
	[Serializable()]
	public abstract class Function2DParameters : FunctionParameters
	{
		#region Private Instance Fields

		protected bool _derivativeMode = false;
		protected int _orderOfDerivative = 1;

		#endregion

		#region Public Instance Properties

		/// <summary>Get or set derivative mode for current function</summary>
		[Description("Set derivative mode")]
		[DefaultValue(false)]
		public bool DerivativeMode
		{
			get
			{
				return _derivativeMode;
			}
			set
			{
				if (_derivativeMode != value)
				{
					_derivativeMode = value;
					_isDitry = true;
					NotifyChanged();
				}			
			}
		}


		/// <summary>Get or set order of derivative</summary>
		[Description("Order of derivative")]
		[DefaultValue(1)]
		public int OrderOfDerivative
		{
			get
			{
				return _orderOfDerivative;
			}
			set
			{
				if (_orderOfDerivative != value)
				{
					if (value < 1)
						throw new ArgumentException("Order of derivative cannot be less than 1");
					_orderOfDerivative = value;
					_isDitry = true;
					NotifyChanged();
				}
			}
		}


		#endregion
	}
}
