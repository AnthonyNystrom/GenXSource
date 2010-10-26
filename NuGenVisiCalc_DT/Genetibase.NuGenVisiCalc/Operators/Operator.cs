/* -----------------------------------------------
 * Operator.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Shared.Reflection;
using System.Reflection;
using Genetibase.NuGenVisiCalc.ComponentModel;
using System.ComponentModel;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared;
using System.Text;
using System.Globalization;

namespace Genetibase.NuGenVisiCalc.Operators
{
	[Serializable]
	internal class Operator : NodeBase
	{
		#region Properties.Format

		/*
		 * DecimalPlaces
		 */

		private NuGenPositiveInt32 _decimalPlaces;

		private NuGenPositiveInt32 DecimalPlacesInternal
		{
			get
			{
				if (_decimalPlaces == null)
				{
					_decimalPlaces = new NuGenPositiveInt32();
					_decimalPlaces.Value = 2;
				}

				return _decimalPlaces;
			}
		}

		[Browsable(true)]
		[DefaultValue(2)]
		[NuGenSRCategory("Category_Format")]
		public Int32 DecimalPlaces
		{
			get
			{
				return DecimalPlacesInternal.Value;
			}
			set
			{
				DecimalPlacesInternal.Value = value;
			}
		}

		/*
		 * SignificantDigits
		 */

		private NuGenPositiveInt32 _significantDigits;

		private NuGenPositiveInt32 SignificantDigitsInternal
		{
			get
			{
				if (_significantDigits == null)
				{
					_significantDigits = new NuGenPositiveInt32();
					_significantDigits.Value = 10;
				}

				return _significantDigits;
			}
		}

		[Browsable(true)]
		[DefaultValue(10)]
		[NuGenSRCategory("Category_Format")]
		public Int32 SignificantDigits
		{
			get
			{
				return SignificantDigitsInternal.Value;
			}
			set
			{
				SignificantDigitsInternal.Value = value;
			}
		}

		#endregion

		#region Properties.Operation

		[Browsable(true)]
		[NuGenSRCategory("Category_Operation")]
		public String Operation
		{
			get
			{
				return _descriptor.Name;
			}
		}

		#endregion

		#region Properties.Result

		/*
		 * Result
		 */

		[Browsable(true)]
		[NuGenSRCategory("Category_Result")]
		public String Result
		{
			get
			{
				Object result = GetData();

				if (result == null)
				{
					return Resources.Message_ErrorPerformingCalculation;
				}

				return result.ToString();
			}
		}

		/*
		 * ResultCultureSpecific
		 */

		[Browsable(true)]
		[NuGenSRCategory("Category_Result")]
		public String ResultCultureSpecific
		{
			get
			{
				Object result = GetData();

				if (result == null)
				{
					return Resources.Message_ErrorPerformingCalculation;
				}

				StringBuilder formatBuilder = new StringBuilder();
				formatBuilder.Append("#,#.");

				for (Int32 i = 0; i < DecimalPlaces; i++)
				{
					formatBuilder.Append("#");
				}

				String format = formatBuilder.ToString();
				return Math.Round(Convert.ToDouble(result), DecimalPlaces, MidpointRounding.AwayFromZero).ToString(format, CultureInfo.CurrentUICulture);				
			}
		}

		/*
		 * ResultFixedPoint
		 */

		public String ResultFixedPoint
		{
			get
			{
				Object result = GetData();

				if (result == null)
				{
					return Resources.Message_ErrorPerformingCalculation;
				}

				return Math.Round(Convert.ToDouble(result), DecimalPlaces, MidpointRounding.AwayFromZero).ToString(CultureInfo.CurrentUICulture);
			}
		}

		/*
		 * ResultScientific
		 */

		[Browsable(true)]
		[NuGenSRCategory("Category_Result")]
		public String ResultScientific
		{
			get
			{
				Object result = GetData();

				if (result == null)
				{
					return Resources.Message_ErrorPerformingCalculation;
				}

				String format = "";

				if (SignificantDigits == 1)
				{
					format = "#0+E";
				}
				else
				{
					StringBuilder formatBuilder = new StringBuilder();
					formatBuilder.Append("#.");

					for (Int32 i = 1; i < SignificantDigits; i++)
					{
						formatBuilder.Append("#");
					}

					formatBuilder.Append("E+000");
					format = formatBuilder.ToString();
				}

				return Convert.ToDouble(result).ToString(format, CultureInfo.CurrentUICulture);
			}
		}

		#endregion

		#region Properties.NonBrowsable

		private OperatorDescriptor _descriptor;

		[Browsable(false)]
		public OperatorDescriptor OperatorDescriptor
		{
			get
			{
				return _descriptor;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * GetData
		 */

		public override Object GetData()
		{
			Object[] values = new Object[_descriptor.InputParameterCount];

			for (Int32 i = 0; i < _descriptor.InputParameterCount; i++)
			{
				values[i] = GetInputData(i);

				if (values[i] == null)
				{
					return null;
				}
			}

			return _descriptor.Invoke(values);
		}

		public override Object GetData(int index)
		{
			return GetData();
		}

		public override Object GetData(int index, params Object[] options)
		{
			return GetData();
		}

		public override Object GetData(params Object[] options)
		{
			return GetData();
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Operator"/> class.
		/// </summary>
		protected Operator()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Operator"/> class.
		/// </summary>
		/// <param name="descriptor">Can be <see langword="null"/>.</param>
		/// <param name="inputCount"></param>
		/// <param name="outputCount"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="descriptor"/> is <see langword="null"/>.</para>
		/// </exception>
		public Operator(OperatorDescriptor descriptor)
		{
			if (descriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}

			_descriptor = descriptor;
			Name = Header = _descriptor.Name;

			CreateOutputs(descriptor.OutputParameterCount);
			Object defaultOutputValue = NuGenActivator.CreateObject(descriptor.GetOutputParameter().ParameterType);

			SetOutput(0, "Result", defaultOutputValue);

			CreateInputs(descriptor.InputParameterCount);
			ParameterInfo[] inputParameters = descriptor.GetInputParameters();

			for (int i = 0; i < descriptor.InputParameterCount; i++)
			{
				Object defaultInputValue = NuGenActivator.CreateObject(inputParameters[i].ParameterType);
				SetInput(i, inputParameters[i].Name, defaultInputValue);
			}
		}
	}
}
