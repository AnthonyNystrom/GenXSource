/* -----------------------------------------------
 * OperatorDescriptor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.Shared.Reflection;

namespace Genetibase.NuGenVisiCalc.Operators
{
	[Serializable]
	internal class OperatorDescriptor : IDescriptor
	{
		private Int32 _inputParameterCount;

		public Int32 InputParameterCount
		{
			get
			{
				return _inputParameterCount;
			}
		}

		private String _name;

		public String Name
		{
			get
			{
				return _name;
			}
		}

		private Int32 _precedence;

		public Int32 Precedence
		{
			get
			{
				return _precedence;
			}
		}

		private PrimitiveOperator _primitiveOperator;

		public PrimitiveOperator PrimitiveOperator
		{
			get
			{
				return _primitiveOperator;
			}
		}

		private Int32 _outputParameterCount;

		public Int32 OutputParameterCount
		{
			get
			{
				return _outputParameterCount;
			}
		}

		private String _stringRepresentation;

		public String StringRepresentation
		{
			get
			{
				return _stringRepresentation;
			}
		}

		public NodeBase CreateNode(Canvas canvas)
		{
			return new Operator(this);
		}

		public ParameterInfo[] GetInputParameters()
		{
			return _operatorEvaluateMethod.GetParameters();
		}

		public ParameterInfo GetOutputParameter()
		{
			return _operatorEvaluateMethod.ReturnParameter;
		}

		public Object Invoke(params Object[] operandList)
		{
			if (_operatorDeclaringTypeInstance == null)
			{
				Type declaringType = _operatorEvaluateMethod.DeclaringType;
				_operatorDeclaringTypeInstance = NuGenActivator.CreateObject(declaringType);
			}

			Object[] parameters = new Object[_inputParameterCount];

			for (Int32 i = 0; i < parameters.Length; i++)
			{
				parameters[i] = operandList[i];
			}

			return _operatorEvaluateMethod.Invoke(_operatorDeclaringTypeInstance, parameters);
		}

		public override String ToString()
		{
			return Name;
		}

		private Object _operatorDeclaringTypeInstance;
		private MethodInfo _operatorEvaluateMethod;

		/// <summary>
		/// Initializes a new instance of the <see cref="OperatorDescriptor"/> class.
		/// </summary>
		/// <param name="operatorEvaluateMethod">Specifies the operator evaluation method.</param>
		/// <param name="name">The name of the operator (appears in Toolbox window).</param>
		/// <param name="stringRepresentation">Specifies how the operator looks in an expression (e.g. "+" for Add operator).</param>
		/// <param name="precedence">Specifies the operator precedence (e.g. "+" operation should be performed after "*", etc.)</param>
		/// <param name="primitiveOperator">Specifies whether the operator is primitive (like "add", "substract", "multiply", or "divide").</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="operatorEvaluateMethod"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="name"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="name"/> is an empty string.</para>
		/// -or-
		/// <para><paramref name="stringRepresentation"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="stringRepresentation"/> is an empty string.</para>
		/// </exception>
		public OperatorDescriptor(
			MethodInfo operatorEvaluateMethod
			, String name
			, String stringRepresentation
			, Int32 precedence
			, PrimitiveOperator primitiveOperator
			)
		{
			if (operatorEvaluateMethod == null)
			{
				throw new ArgumentNullException("operatorEvaluateMethod");
			}

			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (String.IsNullOrEmpty(stringRepresentation))
			{
				throw new ArgumentNullException("stringRepresentation");
			}

			_operatorEvaluateMethod = operatorEvaluateMethod;
			_name = name;
			_stringRepresentation = stringRepresentation;

			ParameterInfo[] operatorEvaluateMethodParams = operatorEvaluateMethod.GetParameters();
			_inputParameterCount = operatorEvaluateMethodParams.Length;
			_outputParameterCount = 1;
			_precedence = precedence;
			_primitiveOperator = primitiveOperator;
		}
	}
}
