/* -----------------------------------------------
 * NuGenComponent.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a component that supports <see cref="INuGenServiceProvider"/> model.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenComponent : Component
	{
		private INuGenEventInitiatorService _initiator;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenComponent"/> class.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenComponent(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
		}
	}
}
