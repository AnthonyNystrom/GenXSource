/* -----------------------------------------------
 * NuGenInterpreter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.MathX.FormulaInterpreter
{
	/// <summary>
	/// </summary>
	public static class NuGenInterpreter
	{
		private static string vars = "abcdefghijklmnopqrstuvwxyz";
		private static double[] values = new double[26];
		private static bool _error;
		private static NuGenAngleMode _angleMode = NuGenAngleMode.Deg;

		/// <summary>
		/// Returns the index of a variable, if possible.
		/// </summary>
		private static bool IsVariable(string expression, out int ind)
		{
			int i = vars.IndexOf(expression);

			if (i != -1)
			{
				ind = i;
				return true;
			}
			else
			{
				ind = -1;
				return false;
			}
		}

		/// <summary>
		/// Gets the index of a closing bracket.
		/// </summary>
		private static int GetClosingBracket(string expression, int start)
		{
			int res = start, bracket = 0;
			for (int i = start; i < expression.Length; i++)
			{
				switch (expression.Substring(i, 1))
				{
					case "(":
					bracket++;
					break;
					case ")":
					bracket--;
					break;
				}
				if (bracket == 0)
				{
					res = i;
					break;
				}
			}
			return res;
		}
		/// <summary>
		/// Parses an infix expression.
		/// </summary>
		private static NuGenFormulaElement ParseInfixInternal(string expression)
		{
			string ret;
			double res;
			int index;
			//no chars inside, error
			if (expression.Length == 0)
				goto error;
			//if the expression is wrapped in brackets, remove them
			if (expression.StartsWith("(") && GetClosingBracket(expression, 0) == expression.Length - 1)
				expression = expression.Substring(1, expression.Length - 2);
			//if the expression is a value itself, return it
			if (double.TryParse(expression, System.Globalization.NumberStyles.Float,
				null, out res))
				return new NuGenValue(res);
			//check for constants
			if (expression == "PI")
				return new NuGenConstantPI();
			else if (expression == "e")
				return new NuGenConstantE();
			//return a variable element, if expression is a variable
			if (expression.Length == 1 && IsVariable(expression, out index))
				return new NuGenVariable(index);
			//if the expression is a function, analyze it
			if (expression.Length > 4 && expression.Substring(3, 1) == "(")
			{
				//if the closing bracket isn't the last one, it maybe
				//an operator expression: sin(x)+cos(x+1)
				int closingbr = GetClosingBracket(expression, 3);
				if (closingbr == expression.Length - 1)
				{
					ret = expression.Substring(4, closingbr - 4);
					switch (expression.Substring(0, 3))
					{
						case "sqr":
						return new NuGenSqr(ParseInfixInternal(ret));
						case "sin":
						return new NuGenSin(ParseInfixInternal(ret));
						case "cos":
						return new NuGenCos(ParseInfixInternal(ret));
						case "tan":
						return new NuGenTan(ParseInfixInternal(ret));
						case "log":
						return new NuGenLog(ParseInfixInternal(ret));
						case "abs":
						return new NuGenAbs(ParseInfixInternal(ret));
						case "fac":
						return new NuGenFac(ParseInfixInternal(ret));
					}
				}
			}
			//expression contains operators,
			//calculate the lowest preference
			int pos = 0, preference = 6, bracket = 0;
			for (int i = expression.Length - 1; i > -1; i--)
			{
				switch (expression.Substring(i, 1))
				{
					case "(":
					bracket++;
					break;
					case ")":
					bracket--;
					break;
					case "+":
					if (bracket == 0 && preference > 0)
					{
						pos = i;
						preference = 0;
					}
					break;
					case "-":
					if (bracket == 0 && preference > 1)
					{
						pos = i;
						preference = 1;
					}
					break;
					case "*":
					if (bracket == 0 && preference > 2)
					{
						pos = i;
						preference = 2;
					}
					break;
					case "%":
					if (bracket == 0 && preference > 3)
					{
						pos = i;
						preference = 3;
					}
					break;
					case "/":
					if (bracket == 0 && preference > 4)
					{
						pos = i;
						preference = 4;
					}
					break;
					case "^":
					if (bracket == 0 && preference > 5)
					{
						pos = i;
						preference = 5;
					}
					break;
				}
			}
			//found no two sides
			if (pos == 0 || pos == expression.Length - 1)
				goto error;
			ret = expression.Substring(pos, 1);
			string leftside, rightside;
			leftside = expression.Substring(0, pos);
			rightside = expression.Substring(pos + 1, expression.Length - (pos + 1));
			switch (ret)
			{
				case "+":
				return new NuGenAddition(ParseInfixInternal(leftside), ParseInfixInternal(rightside));
				case "-":
				return new NuGenSubtraction(ParseInfixInternal(leftside), ParseInfixInternal(rightside));
				case "*":
				return new NuGenMultiplication(ParseInfixInternal(leftside), ParseInfixInternal(rightside));
				case "/":
				return new NuGenDivision(ParseInfixInternal(leftside), ParseInfixInternal(rightside));
				case "%":
				return new NuGenRemainder(ParseInfixInternal(leftside), ParseInfixInternal(rightside));
				case "^":
				return new NuGenPower(ParseInfixInternal(leftside), ParseInfixInternal(rightside));
			}
		//no usable expression, return error value
		error:
			_error = true;
			return new NuGenValue(0.0);
		}
		
		/// <summary>
		/// If there are variables in there, the value is used.
		/// </summary>
		public static double CalculateInfixExpression(string expression)
		{
			return ParseInfixExpression(expression).Value;
		}
		/// <summary>
		/// Parses an expression into a tree of <see cref="NuGenFormulaElement"/> instances.
		/// </summary>
		public static NuGenFormulaElement ParseInfixExpression(string expression)
		{
			if (expression == "" || expression == null)
			{
				_error = true;
				return new NuGenValue(0.0);
			}
			_error = false;
			return ParseInfixInternal(expression);
		}

		/// <summary>
		/// Evaluates if there were any errors
		/// in the last expression
		/// </summary>
		public static bool Error
		{
			get
			{
				return _error;
			}
		}

		/// <summary>
		/// Gets or sets the AngleMeasuringMode which is used for
		/// trignonometric functions
		/// </summary>
		public static NuGenAngleMode AngleMode
		{
			get
			{
				return _angleMode;
			}
			set
			{
				_angleMode = value;
			}
		}
		/// <summary>
		/// Converts an angle into radiants using the current <see cref="AngleMode"/>.
		/// </summary>
		public static double AngleToRadians(double angle)
		{
			switch (_angleMode)
			{
				case NuGenAngleMode.Deg:
				return Math.PI * angle / 180.0;
				case NuGenAngleMode.Grad:
				return Math.PI * angle / 200.0;
			}
			return angle;
		}
		
		/// <summary>
		/// Gets the value of a variable.
		/// </summary>
		/// <param name="index">The index of the variable 0-26.
		/// The index is a reference to a letter in the Latin alphabet.</param>
		/// <returns>The value of the variable, or 0 if index out of range.</returns>
		public static double GetVariableValue(int index)
		{
			return index > -1 && index < vars.Length ?
				values[index] :
				0.0;
		}
		
		/// <summary>
		/// Sets the value of a variable.
		/// </summary>
		/// <param name="index">The index of the variable 0-26.
		/// The index is a reference to a letter in the Latin alphabet.</param>
		/// <param name="val">The value to assign to the variable.</param>
		public static void SetVariableValue(int index, double val)
		{
			if (index > -1 && index < vars.Length)
				values[index] = val;
		}
		
		/// <summary>
		/// Gets the letter of a variable index.
		/// </summary>
		/// <param name="index">The index of the variable 0-26.
		/// The index is a reference to a letter in the Latin alphabet</param>
		/// <returns>The string containing the char referencing the variable,
		/// or "#name" if index out of range.</returns>
		public static string GetVariableGlyph(int index)
		{
			return (index > -1 && index < vars.Length) ?
				new string(vars[index], 1) :
				@"#name#";
		}
		
		/// <summary>
		/// Gets the index to reference a variable.
		/// </summary>
		/// <param name="c">The char of the variable</param>
		/// <returns>Index of the variable or -1, if there is no variable
		/// with this char</returns>
		public static int GetVariableIndex(char c)
		{
			int i = vars.IndexOf(c);
			return i;
		}

		/// <summary>
		/// States, if the index is in range for setting a variable.
		/// </summary>
		public static bool IsIndexInRange(int index)
		{
			return index > -1 && index < vars.Length;
		}
	}
}
