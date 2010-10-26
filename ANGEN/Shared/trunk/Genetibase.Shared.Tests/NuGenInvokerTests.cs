/* -----------------------------------------------
 * NuGenInvokerTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Genetibase.Shared;
using Genetibase.Shared.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public partial class NuGenInvokerTests
	{
		private DummyClass _dummyClass;
		private NuGenInvoker _invoker;
		private Int32 _fooValue = 10;

		[TestInitialize]
		public void SetUp()
		{
			_dummyClass = new DummyClass();
			_dummyClass.PrivateField = _fooValue;
			_invoker = new NuGenInvoker(_dummyClass);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorTest()
		{
			NuGenInvoker invoker = new NuGenInvoker(null);
		}

		[TestMethod]
		public void FieldAccessTest()
		{
			Int32 privateFieldValue = _invoker.Fields["_privateField"].GetValue<Int32>();
			Assert.AreEqual(_fooValue, privateFieldValue);

			ParentClass parentClass = new ParentClass();
			NuGenInvoker invoker = new NuGenInvoker(parentClass);

			NuGenFieldInfo privateFieldInfo = invoker.Fields["privateField"];
			Assert.IsNotNull(privateFieldInfo);
		}

		[TestMethod]
		public void EventSubscribtionTest()
		{
			Int32 SomeEventCalls = 0;
			_invoker.Fields["_eventsClass"].Events["SomeEvent"].AddHandler(new EventHandler(delegate
			{
				SomeEventCalls++;
			}));
			_invoker.Fields["_eventsClass"].Methods["InvokeSomeEvent"].Invoke();
			Assert.AreEqual(1, SomeEventCalls);
		}

		[TestMethod]
		public void PrivateEventTest()
		{
			NuGenInvoker invoker = new NuGenInvoker(new ParentClass());
			NuGenEventInfo info = invoker.Events["PrivateEvent"];
			Assert.IsNotNull(info);
		}

		[TestMethod]
		public void MethodTest()
		{
			_invoker.Methods["InvokeWithParameters"].Invoke(_fooValue);
			Int32 privateFieldValue = _invoker.Properties["PrivateField"].GetValue<Int32>();

			Assert.AreEqual(_fooValue, privateFieldValue);

			Int32 returnValue = _invoker.Methods["InvokeWithReturnValue"].Invoke<Int32>(_fooValue);
			Assert.AreEqual(_fooValue * 2, returnValue);
		}

		[TestMethod]
		public void PrivateMethodTest()
		{
			NuGenInvoker invoker = new NuGenInvoker(new ParentClass());
			NuGenMethodInfo methodInfo = invoker.Methods["PrivateMethod"];
			Assert.IsNotNull(methodInfo);
		}

		[TestMethod]
		public void PropertyTest()
		{
			_invoker.Fields["_eventsClass"].Properties["Property"].SetValue(_fooValue);
			NuGenPropertyInfo propertyInfo = _invoker.Fields["_eventsClass"].Properties["Property"];

			Int32 propertyValue = propertyInfo.GetValue<Int32>();
			Object typeAgnosticPropertyValue = propertyInfo.GetValue();

			Assert.AreEqual(_fooValue, propertyValue);
			Assert.AreEqual(_fooValue, typeAgnosticPropertyValue);
		}

		[TestMethod]
		public void PrivatePropertyTest()
		{
			NuGenInvoker invoker = new NuGenInvoker(new ParentClass());
			NuGenPropertyInfo propInfo = invoker.Properties["PrivateProperty"];
			Assert.IsNotNull(propInfo);
		}

		[TestMethod]
		public void StaticMethodTest()
		{
			Assert.AreEqual(
				_fooValue * 2,
				NuGenInvoker<StaticClass>.Methods["Method"].Invoke<Int32>(_fooValue)
			);
		}
	}
}
