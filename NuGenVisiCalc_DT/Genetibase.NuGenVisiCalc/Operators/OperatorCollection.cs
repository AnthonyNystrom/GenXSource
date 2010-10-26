/* -----------------------------------------------
 * OperatorCollection.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Operators
{
	[OperatorFixture]
	internal class OperatorCollection
	{
		[Operator("Add", "+", 0, PrimitiveOperator.Add)]
		public Double Add(Double valueA, Double valueB)
		{
			return valueA + valueB;
		}

		[Operator("Substract", "-", 0, PrimitiveOperator.Sub)]
		public Double Sub(Double valueA, Double valueB)
		{
			return valueA - valueB;
		}

		[Operator("Multiply", "*", 1, PrimitiveOperator.Mul)]
		public Double Multiply(Double valueA, Double valueB)
		{
			return valueA * valueB;
		}

		[Operator("Divide", "/", 1, PrimitiveOperator.Div)]
		public Double Divide(Double valueA, Double valueB)
		{
			if (valueB == 0)
			{
				return Double.PositiveInfinity;
			}

			return valueA / valueB;
		}

		[Operator("Abs", "abs")]
		public Double Abs(Double value)
		{
			return Math.Abs(value);
		}

		[Operator("Min", "min")]
		public Double Min(Double valueA, Double valueB)
		{
			return Math.Min(valueA, valueB);
		}

		[Operator("Max", "max")]
		public Double Max(Double valueA, Double valueB)
		{
			return Math.Max(valueA, valueB);
		}

		[Operator("Pow", "pow")]
		public Double Pow(Double value, Double power)
		{
			return Math.Pow(value, power);
		}

		[Operator("Pow2", "pow2")]
		public Double Pow2(Double value)
		{
			return Math.Pow(value, 2.0);
		}

		[Operator("Pow3", "pow3")]
		public Double Pow3(Double value)
		{
			return Math.Pow(value, 3.0);
		}

		[Operator("Sqrt", "sqrt")]
		public Double Sqrt(Double value)
		{
			return Math.Sqrt(value);
		}

		[Operator("Sin", "sin")]
		public Double Sin(Double value)
		{
			return Math.Sin(value);
		}

		[Operator("Cos", "cos")]
		public Double Cos(Double value)
		{
			return Math.Cos(value);
		}

		[Operator("Tan", "tan")]
		public Double Tan(Double value)
		{
			return Math.Tan(value);
		}

		[Operator("Exp", "exp")]
		public Double Exp(Double value)
		{
			return Math.Exp(value);
		}

		[Operator("Log", "log")]
		public Double Log(Double value)
		{
			return Math.Log(value);
		}

		[Operator("Log10", "log10")]
		public Double Log10(Double value)
		{
			return Math.Log10(value);
		}

		[Operator("Not", "not")]
		public Boolean Not(Boolean value)
		{
			return !value;
		}

		[Operator("And", "and")]
		public Boolean And(Boolean valueA, Boolean valueB)
		{
			return valueA && valueB;
		}

		[Operator("Or", "or")]
		public Boolean Or(Boolean valueA, Boolean valueB)
		{
			return valueA || valueB;
		}

		[Operator("Xor", "xor")]
		public Boolean Xor(Boolean valueA, Boolean valueB)
		{
			return valueA ^ valueB;
		}

		[Operator("Equal", "equal")]
		public Boolean Equal(Double valueA, Double valueB)
		{
			return valueA == valueB;
		}

		[Operator("NotEqual", "not_equal")]
		public Boolean NotEqual(Double valueA, Double valueB)
		{
			return valueA != valueB;
		}

		[Operator("Less", "less")]
		public Boolean Less(Double valueA, Double valueB)
		{
			return valueA < valueB;
		}

		[Operator("LessEqual", "less_equal")]
		public Boolean LessEqual(Double valueA, Double valueB)
		{
			return valueA <= valueB;
		}

		[Operator("Greater", "greater")]
		public Boolean Greater(Double valueA, Double valueB)
		{
			return valueA > valueB;
		}

		[Operator("GreaterEqual", "greater_equal")]
		public Boolean GreaterEqual(Double valueA, Double valueB)
		{
			return valueA >= valueB;
		}

		[Operator("Mod", "mod")]
		public Double Mod(Double valueA, Double valueB)
		{
			return valueA % valueB;
		}
	}
}
