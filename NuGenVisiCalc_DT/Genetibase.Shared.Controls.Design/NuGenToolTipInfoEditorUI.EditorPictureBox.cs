/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenToolTipInfoEditorUI
	{
		private sealed class EditorPictureBox : PictureBox
		{
			private NuGenCustomTypeEditorServiceContext _serviceContext;

			protected override void OnDoubleClick(EventArgs e)
			{
				base.OnDoubleClick(e);

				UITypeEditor imageEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
				_serviceContext.SetInstance(this, TypeDescriptor.GetProperties(this)["Image"]);
				this.Image = imageEditor.EditValue(_serviceContext, _serviceContext, this.Image) as Image;
			}

			public EditorPictureBox(NuGenCustomTypeEditorServiceContext serviceContext)
			{
				if (serviceContext == null)
				{
					throw new ArgumentNullException("serviceContext");
				}

				_serviceContext = serviceContext;

				this.BackColor = SystemColors.Window;
				this.BorderStyle = BorderStyle.FixedSingle;
				this.Dock = DockStyle.Fill;
				this.SizeMode = PictureBoxSizeMode.CenterImage;
			}
		}
	}
}
