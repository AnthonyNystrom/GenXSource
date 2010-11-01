/* -----------------------------------------------
 * NuGenOpenFileSelector.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides GUI to select a file to open.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	public class NuGenOpenFileSelector : NuGenFileSelector
	{
		#region Properties.Protected.Overriden

		/*
		 * FileDialog
		 */

		private FileDialog fileDialog = null;

		/// <summary>
		/// Gets the <see cref="P:NuGenFileSelector.FileDialog"/> associated with this <see cref="T:NuGenFileSelector"/>.
		/// </summary>
		/// <value></value>
		protected override FileDialog FileDialog
		{
			get
			{
				if (this.fileDialog == null) 
				{
					this.fileDialog = new OpenFileDialog();
				}

				return this.fileDialog;
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOpenFileSelector"/> class.
		/// </summary>
		public NuGenOpenFileSelector()
		{
		}
		
		#endregion
	}
}
