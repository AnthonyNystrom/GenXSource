/* -----------------------------------------------
 * WndServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SampleFramework
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenWindowStateTracker"/></para>
	/// <para><see cref="ISampleFolderDescriptor"/></para>
	/// <para><see cref="ISamplesManager"/></para>
	/// </summary>
	internal class WndServiceProvider : NuGenServiceProvider
	{
		private INuGenWindowStateTracker _wndStateTracker;

		protected virtual INuGenWindowStateTracker WndStateTracker
		{
			get
			{
				if (_wndStateTracker == null)
				{
					_wndStateTracker = new NuGenWindowStateTracker();
				}

				return _wndStateTracker;
			}
		}

		private ISampleFolderDescriptor _sampleFolderDescriptor;

		protected virtual ISampleFolderDescriptor SampleFolderDescriptor
		{
			get
			{
				if (_sampleFolderDescriptor == null)
				{
					_sampleFolderDescriptor = new SampleFolderDescriptor();
				}

				return _sampleFolderDescriptor;
			}
		}

		private ISamplesManager _samplesManager;

		protected virtual ISamplesManager SamplesManager
		{
			get
			{
				if (_samplesManager == null)
				{
					_samplesManager = new SamplesManager();
				}

				return _samplesManager;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenWindowStateTracker))
			{
				Debug.Assert(this.WndStateTracker != null, "this.WndStateTracker != null");
				return this.WndStateTracker;
			}
			else if (serviceType == typeof(ISampleFolderDescriptor))
			{
				Debug.Assert(this.SampleFolderDescriptor != null, "this.SampleFolderDescriptor != null");
				return this.SampleFolderDescriptor;
			}
			else if (serviceType == typeof(ISamplesManager))
			{
				Debug.Assert(this.SamplesManager != null, "this.SamplesManager != null");
				return this.SamplesManager;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WndServiceProvider"/> class.
		/// </summary>
		public WndServiceProvider()
		{
		}
	}
}
