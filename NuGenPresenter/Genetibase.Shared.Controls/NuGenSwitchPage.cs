/* -----------------------------------------------
 * NuGenSwitchPage.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenSwitchPageDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSwitchPage : UserControl
	{
		/*
		 * SwitchButtonImage
		 */

		private Image _switchButtonImage;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_SwitchPage_SwitchButtonImage")]
		public Image SwitchButtonImage
		{
			get
			{
				return _switchButtonImage;
			}
			set
			{
				if (_switchButtonImage != value)
				{
					_switchButtonImage = value;
					this.OnSwitchButtonImageChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _switchButtonImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonImage"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SwitchPage_SwitchButtonImageChanged")]
		public event EventHandler SwitchButtonImageChanged
		{
			add
			{
				this.Events.AddHandler(_switchButtonImageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonImageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SwitchButtonImageChanged"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_switchButtonImageChanged, e);
		}

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the text that the associated switch button displays.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_SwitchPage_Text")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changes.
		/// </summary>
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SwitchPage_TextChanged")]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

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
		/// Initializes a new instance of the <see cref="NuGenSwitchPage"/> class.
		/// </summary>
		public NuGenSwitchPage()
		{
			this.BackColor = Color.Transparent;
			this.Dock = DockStyle.Fill;
		}
	}
}
