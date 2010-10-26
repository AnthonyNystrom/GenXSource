/* -----------------------------------------------
 * NuGenOpenFileSelector.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides GUI to select a file to open.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenOpenFileSelector), "Resources.NuGenIcon.png")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenOpenFileSelectorDesigner")]
	[DefaultEvent("PathChanged")]
	[DefaultProperty("Title")]
	[NuGenSRDescription("Description_OpenFileSelector")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenOpenFileSelector : NuGenFileSelector
	{
		/*
		 * FileDialog
		 */

		private FileDialog _fileDialog;

		/// <summary>
		/// Gets the <see cref="P:NuGenFileSelector.FileDialog"/> associated with this <see cref="T:NuGenFileSelector"/>.
		/// </summary>
		/// <value></value>
		protected override FileDialog FileDialog
		{
			get
			{
				if (_fileDialog == null) 
				{
					_fileDialog = new OpenFileDialog();
				}

				return _fileDialog;
			}
		}
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOpenFileSelector"/> class.
		/// </summary>
		public NuGenOpenFileSelector()
		{
		}		
	}
}
