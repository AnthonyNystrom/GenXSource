/* -----------------------------------------------
 * OperatorLoaderTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.NuGenVisiCalc.Operators;
using NUnit.Framework;

namespace Genetibase.NuGenVisiCalc.Tests
{
	[TestFixture]
	public sealed class OperatorLoaderTests
	{
		[OperatorFixture]
		private abstract class AbstractOperators
		{
			[Operator("Abstract", "abstract")]
			public abstract Double Abstract(Double x, Double y);
		}

		[OperatorFixture]
		private class Operators
		{
			[Operator("Add", "+")]
			public Double Add(Double x, Double y)
			{
				return x + y;
			}

			[Operator("Substract", "-")]
			public Double Substract(Double x, Double y)
			{
				return x - y;
			}

			[Operator("Multiply", "*")]
			private static Double Multiply(Double x, Double y)
			{
				return x * y;
			}

			[Operator("Divide", "/")]
			private Double Divide(Double x, Double y)
			{
				return x / y;
			}
		}

		[Test]
		public void LoadOperatorsFromAssemblyTest()
		{
			Dictionary<String, OperatorDescriptor> operators = OperatorLoader.LoadOperatorsFromAssembly(Assembly.GetExecutingAssembly());
			LoadOperatorsInternalTest(operators);
		}

		[Test]
		public void LoadOperatorsFromTypeTest()
		{
			Dictionary<String, OperatorDescriptor> operators = OperatorLoader.LoadOperatorsFromType(typeof(Operators));
			LoadOperatorsInternalTest(operators);
		}

		[Test]
		public void DescriptorTest()
		{
			Dictionary<String, OperatorDescriptor> operators = OperatorLoader.LoadOperatorsFromType(typeof(Operators));

			OperatorDescriptor addOperator = operators["+"];
			Assert.AreEqual("Add", addOperator.Name);
			Assert.AreEqual("+", addOperator.StringRepresentation);
			ParameterInfo[] paramsInfo = addOperator.GetInputParameters();
			Assert.AreEqual(addOperator.InputParameterCount, paramsInfo.Length);
			Assert.AreEqual(2, addOperator.InputParameterCount);

			OperatorDescriptor substractOperator = operators["-"];
			Assert.AreEqual("Substract", substractOperator.Name);
			Assert.AreEqual("-", substractOperator.StringRepresentation);
		}

		private void LoadOperatorsInternalTest(Dictionary<String, OperatorDescriptor> operators)
		{
			Assert.AreEqual(2, operators.Count);

			OperatorDescriptor addOperator = operators["+"];
			OperatorDescriptor substractOperator = operators["-"];

			Object result = addOperator.Invoke(25.0, 34.0);
			Assert.AreEqual(59, result);

			result = substractOperator.Invoke(25.0, 34.0);
			Assert.AreEqual(-9, result);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadOperatorsFromTypeArgumentNullExceptionTest()
		{
			OperatorLoader.LoadOperatorsFromType(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadOperatorsFromAssemblyArgumentNullExceptionTest()
		{
			OperatorLoader.LoadOperatorsFromAssembly(null);
		}

		[Test]
		public void OperatorDescriptorCtorArgumentNullException()
		{
			OperatorDescriptor descriptor = null;
			MethodInfo mi = typeof(Object).GetMethod("ToString");
			Assert.IsNotNull(mi);

			try
			{
				descriptor = new OperatorDescriptor(mi, null, "stringRepresentation", 0, PrimitiveOperator.No);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				descriptor = new OperatorDescriptor(mi, "", "stringRepresentation", 0, PrimitiveOperator.No);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				descriptor = new OperatorDescriptor(mi, "name", null, 0, PrimitiveOperator.No);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				descriptor = new OperatorDescriptor(mi, "name", "", 0, PrimitiveOperator.No);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				descriptor = new OperatorDescriptor(null, "name", "representation", 0, PrimitiveOperator.No);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}
	}
}
