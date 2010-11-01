/* -----------------------------------------------
 * NuGenToolTipInfoConverterTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.Design;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenToolTipInfoConverterTests
	{
		private NuGenToolTipInfo _tooltipInfo;
		private NuGenToolTipInfoConverter _converter;
		private Image _sampleImg;
		private string _sampleStr;

		[SetUp]
		public void SetUp()
		{
			_converter = new NuGenToolTipInfoConverter();
			_sampleImg = new Bitmap(1, 1);
			_sampleStr = "SampleStr";
		}

		[TearDown]
		public void TearDown()
		{
			_sampleImg.Dispose();
		}

		[Test]
		public void GetConstructorInfoTest()
		{
			_tooltipInfo = new NuGenToolTipInfo();
			object[] values;
			MemberInfo ctorInfo = this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(0, values.Length);

			_tooltipInfo.Header = _sampleStr;
			this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(3, values.Length);

			_tooltipInfo.Remarks = _sampleStr;
			this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(6, values.Length);

			_tooltipInfo = new NuGenToolTipInfo();
			_tooltipInfo.Image = _sampleImg;
			this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(3, values.Length);

			_tooltipInfo = new NuGenToolTipInfo();
			_tooltipInfo.RemarksImage = _sampleImg;
			this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(6, values.Length);

			_tooltipInfo = new NuGenToolTipInfo();
			_tooltipInfo.Text = _sampleStr;
			this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(3, values.Length);

			_tooltipInfo.RemarksHeader = _sampleStr;
			this.GetConstructorInfo(out values);
			Assert.IsNotNull(ctorInfo);
			Assert.AreEqual(6, values.Length);
		}

		private MemberInfo GetConstructorInfo(out object[] constructorValues)
		{
			return _converter.GetConstructorInfo(_tooltipInfo, out constructorValues);
		}
	}
}
