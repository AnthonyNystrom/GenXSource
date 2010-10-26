/* -----------------------------------------------
 * NuGenToolButton.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Text;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Controls
{
	/// <summary>
	/// Represents a toolbar button.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenToolButtonDesigner))]
	[ToolboxItem(true)]
	public class NuGenToolButton : Button
	{
		#region Properties.Behavior

		/*
		 * EatLine
		 */

		/// <summary>
		/// If <see langword="true"/> replaces the tail symbols which cannot be displayed due to the label width with "...".
		/// Takes affect only if WordWrap property is <see langword="false"/>.
		/// </summary>
		private bool internalEatLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot
		/// be displayed due to the label width.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("If true replaces the tail symbols which cannot be dispalyed with \"...\". Takes affect only if WordWrap property is false.")]
		public virtual bool EatLine
		{
			get { return this.internalEatLine; }
			set 
			{
				if (this.internalEatLine != value)
				{
					this.internalEatLine = value;
					this.OnEatLineChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenToolButton.EatLine"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the EatLine property changes.")]
		public event EventHandler EatLineChanged;

		/// <summary>
		/// Raises the <see cref="E:NuGenToolButton.EatLineChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnEatLineChanged(EventArgs e)
		{
			if (this.EatLineChanged != null)
				this.EatLineChanged(this, e);
		}

		/*
		 * Pushed
		 */

		/// <summary>
		/// Determines if this <see cref="T:NuGenToolButton"/> is currently in the pushed state.
		/// </summary>
		private bool internalPushed = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenToolButton"/> is currently
		/// in the pushed state.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Determines if the button is currently in the pushed state.")]
		public virtual bool Pushed
		{
			get { return this.internalPushed; }
			set 
			{
				if (this.internalPushed != value)
				{
					this.internalPushed = value;
					this.OnPushedChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenToolButton.Pushed"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the Pushed property changes.")]
		public event EventHandler PushedChanged;

		/// <summary>
		/// Raises the <see cref="E:NuGenToolButton.PushedChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnPushedChanged(EventArgs e)
		{
			if (this.PushedChanged != null)
				this.PushedChanged(this, e);
		}

		/*
		 * PushButton
		 */

		/// <summary>
		/// Indicates whether this <see cref="T:NuGenToolButton"/> acts like a push button.
		/// </summary>
		private bool internalPushButton = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenToolButton"/> acts like a push button.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates whether this button acts like a push button.")]
		public virtual bool PushButton
		{
			get { return this.internalPushButton; }
			set
			{
				if (this.internalPushButton != value)
				{
					this.internalPushButton = value;
					this.OnPushButtonChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenToolButton.PushButton"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the PushButton property changes.")]
		public event EventHandler PushButtonChanged;

		/// <summary>
		/// Raises the <see cref="E:NuGenToolButton.PushButtonChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnPushButtonChanged(EventArgs e)
		{
			if (this.PushButtonChanged != null)
				this.PushButtonChanged(this, e);
		}

		/*
		 * UnpushAllInGroup
		 */

		/// <summary>
		/// Indicates whether this <see cref="T:NuGenToolButton"/> unpushes all the other buttons in the group.
		/// </summary>
		private bool internalUnpushAllInGroup = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenToolButton"/> unpushes all the
		/// other buttons in the group.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates whether this button unpushes all the other buttons in the group.")]
		public virtual bool UnpushAllInGroup
		{
			get { return this.internalUnpushAllInGroup; }
			set
			{
				if (this.internalUnpushAllInGroup != value)
				{
					this.internalUnpushAllInGroup = value;
					this.OnUnpushAllInGroupChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Occurs when value of the <see cref="P:NuGenToolButton.UnpushAllInGroup"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the UnpushAllInGroup property changes.")]
		public event EventHandler UnpushAllInGroupChanged;

		/// <summary>
		/// Raises the <see cref="E:NuGenToolButton.UnpushAllInGroupChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnUnpushAllInGroupChanged(EventArgs e)
		{
			if (this.UnpushAllInGroupChanged != null)
				this.UnpushAllInGroupChanged(this, e);
		}

		/*
		 * WordWrap
		 */

		/// <summary>
		/// Indicates whether lines are automatically word-wrapped.
		/// </summary>
		private bool internalWordWrap = false;

		/// <summary>
		/// Gets or sets the value indicating whether lines are automatically word-wrapped.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates whether lines are automatically word-wrapped.")]
		public virtual bool WordWrap
		{
			get { return this.internalWordWrap; }
			set 
			{
				if (this.internalWordWrap != value)
				{
					this.internalWordWrap = value;
					this.OnWordWrapChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenToolButton.WordWrap"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the WordWrap property changes.")]
		public event EventHandler WordWrapChanged;

		/// <summary>
		/// Raises the <see cref="E:NuGenToolButton.WordWrapChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnWordWrapChanged(EventArgs e)
		{
			if (this.WordWrapChanged != null)
				this.WordWrapChanged(this, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { return; }
		}

		/// <summary>
		/// Occurs when the value of the BackColor property changes on the control.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add { base.BackColorChanged += value; }
			remove { base.BackColorChanged -= value; }
		}

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { return; }
		}

		/// <summary>
		/// Occurs when the value of the BackgroundImage property changes on the control.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]		
		public new event EventHandler BackgroundImageChanged
		{
			add { base.BackgroundImageChanged += value; }
			remove { base.BackgroundImageChanged -= value; }
		}

		/*
		 * FlatStyle
		 */

		/// <summary>
		/// Gets or sets the flat style appearance of the button control.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.FlatStyle"/> values.</exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FlatStyle FlatStyle
		{
			get { return base.FlatStyle; }
			set { return; }
		}

		/*
		 * ImageAlign
		 */

		/// <summary>
		/// Gets or sets the alignment of the image on the button control.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment"/> values.</exception>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment ImageAlign
		{
			get { return base.ImageAlign; }
			set { base.ImageAlign = value; }
		}

		/*
		 * RightToLeft
		 */

		/// <summary>
		/// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"/> values.</exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get { return base.RightToLeft; }
			set { return; }
		}

		/// <summary>
		/// Occurs when the value of the RightToLeft property changes on the control.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add { base.RightToLeftChanged += value; }
			remove { base.RightToLeftChanged -= value; }
		}

		/*
		 * TabIndex
		 */

		/// <summary>
		/// Gets or sets the tab order of the control within its container.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get { return base.TabIndex; }
			set { return; }
		}

		/// <summary>
		/// Occurs when the value of the TabIndex property changes on the control.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabIndexChanged
		{
			add { base.TabStopChanged += value; }
			remove { base.TabStopChanged -= value; }
		}

		/*
		 * TabStop
		 */

		/// <summary>
		/// Gets or sets a value indicating whether the user can give the focus to
		/// this control using the TAB key.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get { return base.TabStop; }
			set { return; }
		}

		/// <summary>
		/// Occurs when the value of the TabStop property changes on the control.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]		
		public new event EventHandler TabStopChanged
		{
			add { base.TabStopChanged += value; }
			remove { base.TabStopChanged -= value; }
		}

		/*
		 * TextAlign
		 */

		/// <summary>
		/// Gets or sets the alignment of the text on the button control.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Drawing.ContentAlignment"/> values.</exception>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContentAlignment TextAlign
		{
			get { return base.TextAlign; }
			set { base.TextAlign = value; }
		}

		#endregion

		#region Properties.Protected

		/*
		 * IsMouseOver
		 */

		/// <summary>
		/// Determines if the mouse is over this <see cref="T:NuGenToolButton"/>.
		/// </summary>
		private bool internalIsMouseOver = false;

		/// <summary>
		/// Gets or sets the value indicating whether the mouse is over this <see cref="T:NuGenToolButton"/>.
		/// </summary>
		protected bool IsMouseOver
		{
			get { return this.internalIsMouseOver; }
			set 
			{
				if (this.internalIsMouseOver != value)
				{
					this.internalIsMouseOver = value;
					this.Invalidate();
				}
			}
		}

		/*
		 * IsPressed
		 */

		/// <summary>
		/// Determines if this <see cref="T:NuGenToolButton"/> is pressed.
		/// </summary>
		private bool internalIsPressed = false;

		/// <summary>
		/// Gets or sets the value indicating whether this <see cref="T:NuGenToolButton"/> is pressed.
		/// </summary>
		protected bool IsPressed
		{
			get { return this.internalIsPressed; }
			set 
			{
				if (this.internalIsPressed != value)
				{
					this.internalIsPressed = value;
					this.Invalidate();
				}
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenToolButton"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 24);
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		private INuGenStringProcessor stringProcessor = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenStringProcessor StringProcessor
		{
			get
			{
				if (this.stringProcessor == null)
				{
					this.stringProcessor = new NuGenStringProcessor();
				}

				return this.stringProcessor;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left) 
			{
				this.IsPressed = true;

				if (this.PushButton)
				{
					this.Pushed = true;

					if (this.UnpushAllInGroup && this.Parent != null)
					{
						foreach (Control ctrl in this.Parent.Controls)
						{
							if (ctrl is NuGenToolButton && ctrl != this)
							{
								(ctrl as NuGenToolButton).Pushed = false;
								(ctrl as NuGenToolButton).IsPressed = false;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.IsMouseOver = true;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.IsMouseOver = false;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.IsPressed = false;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			/*
			 * Graphics initialization.
			 */

			Graphics g = e.Graphics;

			/*
			 * Container bounds.
			 */

			Rectangle tweakedRect = new Rectangle(
				this.ClientRectangle.X + 1,
				this.ClientRectangle.Y + 1,
				this.ClientRectangle.Width - 2,
				this.ClientRectangle.Height - 2
				);

			Rectangle borderRect = new Rectangle(
				this.ClientRectangle.X + 1,
				this.ClientRectangle.Y + 1,
				this.ClientRectangle.Width - 2,
				this.ClientRectangle.Height - 2
				);

			/*
			 * Background
			 */

			if (this.Pushed && !this.IsMouseOver)
			{
				using (SolidBrush sb = new SolidBrush(SystemColors.ControlDark))
				{
					g.FillRectangle(sb, tweakedRect);
				}
			}
			else if (this.IsPressed)
			{
				using (SolidBrush sb = new SolidBrush(SystemColors.ControlDark))
				{
					g.FillRectangle(sb, tweakedRect);
				}
			}
			else
			{
				using (SolidBrush sb = new SolidBrush(SystemColors.Control))
				{
					g.FillRectangle(sb, tweakedRect);
				}
			}

			/*
			 * Image
			 */

			Image image = null;

			/* Try to work via Image property. */
			if (this.Image != null)
			{
				image = this.Image;
			}
			/* Try to work via ImageList property. */
			else if (this.ImageList != null && this.ImageIndex >= 0 && this.ImageIndex < this.ImageList.Images.Count)
			{
				image = this.ImageList.Images[this.ImageIndex];
			}

			Rectangle imageRect = Rectangle.Empty;
			
			/* The image is found and we will draw it. */
			if (image != null)
			{
				int xOffset = 4;
				int yOffset = tweakedRect.Height / 2 - image.Size.Height / 2 + 1;

				imageRect = new Rectangle(
					new Point(xOffset, yOffset),
					image.Size
					);

				if (this.IsPressed || this.Pushed)
				{
					imageRect.Offset(1, 1);
				}

				/* The button is enabled. */
				if (this.Enabled) 
				{
					g.DrawImage(image, imageRect);
				}
				/* The button is disabled. */
				else
				{
					ControlPaint.DrawImageDisabled(
						g,
						image,
						imageRect.X,
						imageRect.Y,
						Color.Transparent
						);
				}
			}

			/*
			 * Text
			 */

			if (this.Text != null && this.Text != "")
			{
				Rectangle textRect = Rectangle.Empty;

				if (imageRect.IsEmpty)
				{
					textRect = new Rectangle(
						tweakedRect.X + 4,
						tweakedRect.Y + 4,
						tweakedRect.Width - 8,
						tweakedRect.Height - 8
						);
				}
				else
				{
					textRect = new Rectangle(
						imageRect.Right + 4,
						tweakedRect.Y + 4,
						tweakedRect.Width - imageRect.Width - 8,
						tweakedRect.Height - 8
						);
				}

				if (this.IsPressed || this.Pushed)
				{
					textRect.Offset(0, 1);
				}

				StringFormat sf = new StringFormat();

				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;

				if (this.WordWrap == false) 
				{
					sf.FormatFlags = StringFormatFlags.NoWrap;
				}

				string bufferString = "";

				if (this.WordWrap == false && this.EatLine == true) 
				{
					Debug.Assert(this.StringProcessor != null, "this.StringProcessor != null.");
					bufferString = this.StringProcessor.EatLine(this.Text, this.Font, textRect.Width, g);
				}
				else
				{
					bufferString = this.Text;
				}

				if (this.Enabled)
				{
					using (SolidBrush sb = new SolidBrush(this.ForeColor)) 
					{
						g.DrawString(bufferString, this.Font, sb, textRect, sf);
					}
				}
				else
				{
					using (SolidBrush sb = new SolidBrush(SystemColors.GrayText))
					{
						g.DrawString(bufferString, this.Font, sb, textRect, sf);
					}
				}
			}

			/*
			 * Border
			 */
				
			/* The button is pressed or pushed down. */
			if (this.IsPressed || this.Pushed)
			{
				/* Left and top borders are dark. */
				using (Pen pen = new Pen(SystemColors.ControlDark))
				{
					g.DrawLine(
						pen,
						NuGenControlPaint.RectBLCorner(borderRect),
						NuGenControlPaint.RectTLCorner(borderRect)
						);
					g.DrawLine(
						pen,
						NuGenControlPaint.RectTLCorner(borderRect),
						NuGenControlPaint.RectTRCorner(borderRect)
						);
				}

				/* Right and bottom borders are light. */
				using (Pen pen = new Pen(SystemColors.ControlLight))
				{
					g.DrawLine(
						pen,
						NuGenControlPaint.RectTRCorner(borderRect),
						NuGenControlPaint.RectBRCorner(borderRect)
						);
					g.DrawLine(
						pen,
						NuGenControlPaint.RectBLCorner(borderRect),
						NuGenControlPaint.RectBRCorner(borderRect)
						);
				}
			}
			/* The button is not pushed or pressed and the mouse is over the button. */
			else if (this.IsMouseOver && !this.IsPressed)
			{
				/* Left and top borders are light. */
				using (Pen pen = new Pen(SystemColors.ControlLight))
				{
					g.DrawLine(
						pen,
						NuGenControlPaint.RectBLCorner(borderRect),
						NuGenControlPaint.RectTLCorner(borderRect)
						);
					g.DrawLine(
						pen,
						NuGenControlPaint.RectTLCorner(borderRect),
						NuGenControlPaint.RectTRCorner(borderRect)
						);
				}

				/* Right and bottom borders are dark. */
				using (Pen pen = new Pen(SystemColors.ControlDark))
				{
					g.DrawLine(
						pen,
						NuGenControlPaint.RectTRCorner(borderRect),
						NuGenControlPaint.RectBRCorner(borderRect)
						);
					g.DrawLine(
						pen,
						NuGenControlPaint.RectBLCorner(borderRect),
						NuGenControlPaint.RectBRCorner(borderRect)
						);
				}
			}
			/* The button is in its defalt state. */
			else
			{
				/* We don't need borders in the default state. */
				using (Pen pen = new Pen(SystemColors.Control))
				{
					g.DrawRectangle(pen, borderRect);
				}
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolButton"/> class.
		/// </summary>
		public NuGenToolButton()
		{
			NuGenControlPaint.SetStyle(this, ControlStyles.Opaque | ControlStyles.Selectable, false);
			NuGenControlPaint.SetStyle(this, ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

			this.BackColor = Color.Transparent;
			this.TabStop = false;
		}

		#endregion
	}
}
