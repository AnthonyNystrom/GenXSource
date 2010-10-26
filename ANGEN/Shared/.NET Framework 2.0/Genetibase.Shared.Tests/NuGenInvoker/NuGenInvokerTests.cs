/* -----------------------------------------------
 * NuGenInvokerTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;
using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenInvokerTests
	{
		private DummyClass _dummyClass;
		private EventSink _eventSink;
		private NuGenInvoker _invoker;
		private int _fooValue = 10;

		[SetUp]
		public void SetUp()
		{
			_dummyClass = new DummyClass();
			_dummyClass.PrivateField = _fooValue;
			_eventSink = new EventSink();
			_invoker = new NuGenInvoker(_dummyClass);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorTest()
		{
			NuGenInvoker invoker = new NuGenInvoker(null);
		}

		[Test]
		public void FieldAccessTest()
		{
			int privateFieldValue = _invoker.Fields["_privateField"].GetValue<int>();
			Assert.AreEqual(_fooValue, privateFieldValue);

			ParentClass parentClass = new ParentClass();
			NuGenInvoker invoker = new NuGenInvoker(parentClass);

			NuGenFieldInfo privateFieldInfo = invoker.Fields["privateField"];
			Assert.IsNotNull(privateFieldInfo);
		}

		[Test]
		public void EventSubscribtionTest()
		{
			_eventSink.ExpectedSomeEventCount = 1;
			
			_invoker.Fields["_eventsClass"].Events["SomeEvent"].AddHandler(new EventHandler(_eventSink.EventsClass_SomeEvent));
			_invoker.Fields["_eventsClass"].Methods["InvokeSomeEvent"].Invoke();
		}

		[Test]
		public void MethodTest()
		{
			_invoker.Methods["InvokeWithParameters"].Invoke(_fooValue);
			int privateFieldValue = _invoker.Properties["PrivateField"].GetValue<int>();

			Assert.AreEqual(_fooValue, privateFieldValue);

			int returnValue = _invoker.Methods["InvokeWithReturnValue"].Invoke<int>(_fooValue);
			Assert.AreEqual(_fooValue * 2, returnValue);
		}

		[Test]
		public void PropertyTest()
		{
			_invoker.Fields["_eventsClass"].Properties["Property"].SetValue(_fooValue);
			NuGenPropertyInfo propertyInfo = _invoker.Fields["_eventsClass"].Properties["Property"];

			int propertyValue = propertyInfo.GetValue<int>();
			object typeAgnosticPropertyValue = propertyInfo.GetValue();

			Assert.AreEqual(_fooValue, propertyValue);
			Assert.AreEqual(_fooValue, typeAgnosticPropertyValue);
		}

		[Test]
		public void StaticMethodTest()
		{
			Assert.AreEqual(
				_fooValue * 2,
				NuGenInvoker<StaticClass>.Methods["Method"].Invoke<int>(_fooValue)
			);
		}
	}
}
