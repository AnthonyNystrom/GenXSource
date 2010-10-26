/* -----------------------------------------------
 * NuGenLinkLabel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="LinkLabel"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenLinkLabel), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenLinkLabelDesigner")]
	[NuGenSRDescription("Description_LinkLabel")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenLinkLabel : NuGenLabel
	{
		#region Properties.Appearance

		/*
		 * ActiveLinkColor
		 */

		private Color _activeLinkColor = Color.Empty;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_LinkLabel_ActiveLinkColor")]
		public Color ActiveLinkColor
		{
			get
			{
				if (_activeLinkColor == Color.Empty)
				{
					return this.DefaultActiveLinkColor;
				}

				return _activeLinkColor;
			}
			set
			{
				if (_activeLinkColor != value)
				{
					_activeLinkColor = value;
					this.OnActiveLinkColorChanged(EventArgs.Empty);
				}
			}
		}

		private void ResetActiveLinkColor()
		{
			this.ActiveLinkColor = this.DefaultActiveLinkColor;
		}

		private bool ShouldSerializeActiveLinkColor()
		{
			return this.ActiveLinkColor != this.DefaultActiveLinkColor;
		}

		/// <summary>
		/// </summary>
		protected virtual Color DefaultActiveLinkColor
		{
			get
			{
				return SystemColors.GradientActiveCaption;
			}
		}

		private static readonly object _activeLinkColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ActiveLinkColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_LinkLabel_ActiveLinkColorChanged")]
		public event EventHandler ActiveLinkColorChanged
		{
			add
			{
				this.Events.AddHandler(_activeLinkColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_activeLinkColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ActiveLinkColorChanged"/> event.
		/// </summary>
		protected virtual void OnActiveLinkColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_activeLinkColorChanged, e);
		}

		/*
		 * LinkColor
		 */

		private Color _linkColor = Color.Empty;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_LinkLabel_LinkColor")]
		public Color LinkColor
		{
			get
			{
				if (_linkColor == Color.Empty)
				{
					return this.DefaultLinkColor;
				}

				return _linkColor;
			}
			set
			{
				if (_linkColor != value)
				{
					_linkColor = value;
					this.OnLinkColorChanged(EventArgs.Empty);
				}
			}
		}

		private void ResetLinkColor()
		{
			this.LinkColor = this.DefaultLinkColor;
		}

		private bool ShouldSerializeLinkColor()
		{
			return this.LinkColor != this.DefaultLinkColor;
		}

		/// <summary>
		/// </summary>
		protected virtual Color DefaultLinkColor
		{
			get
			{
				return SystemColors.ActiveCaption;
			}
		}

		private static readonly object _linkColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="LinkColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_LinkLabel_LinkColorChanged")]
		public event EventHandler LinkColorChanged
		{
			add
			{
				this.Events.AddHandler(_linkColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_linkColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="LinkColorChanged"/> event.
		/// </summary>
		protected virtual void OnLinkColorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_linkColorChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Target
		 */

		private string _target;

		/// <summary>
		/// Gets or sets the target to open when the control is clicked. If the target is invalid, an exception will be thrown on click.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_LinkLabel_Target")]
		public string Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (_target != value)
				{
					_target = value;
					this.OnTargetChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _targetChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Target"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_LinkLabel_TargetChanged")]
		public event EventHandler TargetChanged
		{
			add
			{
				this.Events.AddHandler(_targetChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_targetChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TargetChanged"/> event.
		/// </summary>
		protected virtual void OnTargetChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_targetChanged, e);
		}

		#endregion

		#region Properties.Public

		/*
		 * PreferredSize
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size PreferredSize
		{
			get
			{
				return new Size(this.PreferredWidth, this.PreferredHeight);
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * ForeColor
		 */

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The foreground <see cref="T:System.Drawing.Color"></see> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnClick
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnClick(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Target))
			{
				Process.Start(this.Target);
			}

			base.OnClick(e);
		}

		/*
		 * OnMouseEnter
		 */

		/// <summary>
		/// Raises the mouse enter event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			base.Cursor = Cursors.Hand;
			this.Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Underline);
			this.ForeColor = this.ActiveLinkColor;
		}

		/*
		 * OnMouseLeave
		 */

		/// <summary>
		/// Raises the mouse leave event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			base.Cursor = Cursors.Default;
			this.Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Regular);
			this.ForeColor = this.LinkColor;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinkLabel"/> class.
		/// </summary>
		public NuGenLinkLabel()
			: this(NuGenServiceManager.LabelServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinkLabel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenLabelRenderer"/></para>
		/// <para><see cref="INuGenLabelLayoutManager"/></para>
		/// <para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenLinkLabel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.ForeColor = this.LinkColor;
		}
	}
}
