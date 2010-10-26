/* -----------------------------------------------
 * NuGenToolStripLinkLabel.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a link label with custom renderer that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class NuGenToolStripLinkLabel : NuGenToolStripControlHost
	{
		#region Properties.Appearance

		/*
		 * ActiveLinkColor
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "GradientActiveCaption")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_LinkLabel_ActiveLinkColor")]
		public Color ActiveLinkColor
		{
			get
			{
				return this.LinkLabel.ActiveLinkColor;
			}
			set
			{
				this.LinkLabel.ActiveLinkColor = value;
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

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "ActiveCaption")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_LinkLabel_LinkColor")]
		public Color LinkColor
		{
			get
			{
				return this.LinkLabel.LinkColor;
			}
			set
			{
				this.LinkLabel.LinkColor = value;
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

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenToolStripLinkLabel"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_Image")]
		public new Image Image
		{
			get
			{
				return this.LinkLabel.Image;
			}
			set
			{
				this.LinkLabel.Image = value;
			}
		}

		/*
		 * ImageAlign
		 */

		/// <summary>
		/// Gets or sets the alignment of the image that will be displayed on the label.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_ImageAlign")]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return this.LinkLabel.ImageAlign;
			}
			set
			{
				this.LinkLabel.ImageAlign = value;
			}
		}

		/*
		 * TextAlign
		 */

		/// <summary>
		/// Gets or sets the alignment of the text that will be displayed on the label.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_TextAlign")]
		public new ContentAlignment TextAlign
		{
			get
			{
				return this.LinkLabel.TextAlign;
			}
			set
			{
				this.LinkLabel.TextAlign = value;
			}
		}

		private static readonly object _textAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="TextAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Label_TextAlignChanged")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				this.Events.AddHandler(_textAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textAlignChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TextAlignChanged"/> event.
		/// </summary>
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_textAlignChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Target
		 */

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
				return this.LinkLabel.Target;
			}
			set
			{
				this.LinkLabel.Target = value;
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

		#region Properties.Public.Overridden

		/// <summary>
		/// </summary>
		/// <value></value>
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

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets a value indicating whether the item is automatically sized.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Windows.Forms.ToolStripItem"></see> is automatically sized; otherwise, false. The default value is true.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Boolean AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is automatically sized.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Windows.Forms.ToolStripItem"></see> is automatically sized; otherwise, false. The default value is true.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		#region Properties.Protected

		/// <summary>
		/// </summary>
		protected NuGenLinkLabel LinkLabel
		{
			get
			{
				return (NuGenLinkLabel)this.Control;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Subscribes events from the hosted control.
		/// </summary>
		/// <param name="control">The control from which to subscribe events.</param>
		protected override void OnSubscribeControlEvents(Control control)
		{
			base.OnSubscribeControlEvents(control);

			NuGenLinkLabel linkLabel = control as NuGenLinkLabel;

			if (linkLabel != null)
			{
				linkLabel.ActiveLinkColorChanged += _linkLabel_ActiveLinkColorChanged;
				linkLabel.LinkColorChanged += _linkLabel_LinkColorChanged;
				linkLabel.TargetChanged += _linkLabel_TargetChanged;
				linkLabel.TextAlignChanged += _linkLabel_TextAlignChanged;
			}
		}

		/// <summary>
		/// Unsubscribes events from the hosted control.
		/// </summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			base.OnUnsubscribeControlEvents(control);

			NuGenLinkLabel linkLabel = control as NuGenLinkLabel;

			if (linkLabel != null)
			{
				linkLabel.ActiveLinkColorChanged -= _linkLabel_ActiveLinkColorChanged;
				linkLabel.LinkColorChanged -= _linkLabel_LinkColorChanged;
				linkLabel.TargetChanged -= _linkLabel_TargetChanged;
				linkLabel.TextAlignChanged -= _linkLabel_TextAlignChanged;
			}
		}

		#endregion

		#region EventHandlers

		private void _linkLabel_ActiveLinkColorChanged(object sender, EventArgs e)
		{
			this.OnActiveLinkColorChanged(e);
		}

		private void _linkLabel_LinkColorChanged(object sender, EventArgs e)
		{
			this.OnLinkColorChanged(e);
		}

		private void _linkLabel_TargetChanged(object sender, EventArgs e)
		{
			this.OnTargetChanged(e);
		}

		private void _linkLabel_TextAlignChanged(object sender, EventArgs e)
		{
			this.OnTextAlignChanged(e);
		}

		#endregion

		private static Control CreateControlInstance(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			NuGenLinkLabel linkLabel = new NuGenLinkLabel(serviceProvider);
			linkLabel.Size = new Size(100, 20);
			linkLabel.MinimumSize = new Size(10, 5);

			return linkLabel;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolStripLinkLabel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenLabelLayoutManager"/></para>
		/// <para><see cref="INuGenLabelRenderer"/></para>
		/// <para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenToolStripLinkLabel(INuGenServiceProvider serviceProvider)
			: base(CreateControlInstance(serviceProvider))
		{
			this.AutoSize = false;
		}
	}
}
