/* -----------------------------------------------
 * NuGenToolStripControlHost.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a <see cref="ToolStripControlHost"/> that provides <see cref="INuGenEventInitiatorService"/>.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenToolStripControlHost : ToolStripControlHost
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolStripControlHost"/> class.
		/// </summary>
		public NuGenToolStripControlHost(Control ctrlToHost)
			: base(ctrlToHost)
		{
		}
	}
}
