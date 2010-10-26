/* -----------------------------------------------
 * NuGenMiniBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenMiniBar), "Resources.NuGenIcon.png")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenMinibarDesigner")]
	[DefaultEvent("ButtonClick")]
	[NuGenSRDescription("Description_MiniBar")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenMiniBar : UserControl
	{
		/// <summary>
		/// </summary>
		public event EventHandler<NuGenTargetEventArgsT<NuGenMiniBar, NuGenMiniBarControl>> ButtonClick;

		#region Properties.Public

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public NuGenMiniBarControlCollection Buttons
		{
			get
			{
				return _coll;
			}
		}

		/// <summary>
		/// </summary>
		[DefaultValue(false)]
		public new bool AutoSize
		{
			get
			{
				return _autosize;
			}
			set
			{
				_autosize = value;
				this.OnSizeChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public int InternalBarWidth
		{
			get
			{
				return w;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Ruft ab, ob die NuGenMiniBar von einem Klick getroffen wird oder ob sie
		/// ein MouseDown-Ereignis erhält
		/// </summary>
		public bool GetHit(Point pt)
		{
			return this.ClientRectangle.Contains(pt);
		}
		/// <summary>
		/// Findet das Element, auf dem die angegebene Mausposition liegt
		/// </summary>
		public int FindButtonIndex(Point mouse)
		{
			for (int i = 0; i < _coll.Count; i++)
				if (_coll[i].ClientRectangle.Contains(mouse))
					return i;
			return -1;
		}

		/// <summary>
		/// </summary>
		public void MeasureButtons()
		{
			_btnStates = new NuGenMiniBarButtonState[_coll.Count];
			w = 0;
			for (int i = 0; i < _coll.Count; i++)
			{
				_coll[i].Location = new Point(w, 0);
				w += _coll[i].ClientRectangle.Width;
				_btnStates[i] = _coll[i].NState;
			}
			this.OnSizeChanged(EventArgs.Empty);
		}

		/// <summary>
		/// </summary>
		public void UpdateButtons(NuGenMiniBarUpdateAction act)
		{
			NuGenMiniBarButtonState st;
			Point pt = this.PointToClient(Control.MousePosition);
			MouseButtons but = Control.MouseButtons;
			int i = 0;
			foreach (NuGenMiniBarControl btn in _coll)
			{
				st = btn.Action(pt, but, act);
				if (st != _btnStates[i])
				{
					_btnStates[i] = st;
					this.Invalidate(Rectangle.Inflate(btn.ClientRectangle, 1, 1));
					this.Update();
				}
				i++;
			}
		}

		#endregion

		#region Methods.Internal

		internal void ClickButton(NuGenMiniBarControl btn)
		{
			if (ButtonClick != null && btn != null)
				ButtonClick(this, new NuGenTargetEventArgsT<NuGenMiniBar, NuGenMiniBarControl>(this, btn));
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (!_autosize)
				return;
			this.ClientSize = new Size(Math.Max(4, w + 1), 14);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			int i = 0;
			foreach (NuGenMiniBarControl ctl in _coll)
			{
				if (ctl is NuGenMiniBarButton)
				{
					NuGenMiniBarButton btn = ctl as NuGenMiniBarButton;
					DrawButtonStyle(e.Graphics, btn.ClientRectangle, _btnStates[i]);
					if (btn.Glyph != null)
						e.Graphics.DrawImage(btn.Glyph, btn.ClientRectangle);
				}
				else if (ctl is NuGenMiniBarLabel)
				{
					NuGenMiniBarLabel lbl = ctl as NuGenMiniBarLabel;
					e.Graphics.DrawString(lbl.Text, SystemInformation.MenuFont,
						Brushes.Black, lbl.ClientRectangle, _fmt);
				}
				i++;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this.DesignMode)
				return;
			UpdateButtons(NuGenMiniBarUpdateAction.MouseDown);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.DesignMode)
				return;
			UpdateButtons(NuGenMiniBarUpdateAction.MouseMove);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (this.DesignMode)
				return;
			UpdateButtons(NuGenMiniBarUpdateAction.MouseUp);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.DesignMode)
				return;
			UpdateButtons(NuGenMiniBarUpdateAction.MouseMove);
		}

		#endregion

		#region Methods.Private

		private void DrawButtonStyle(Graphics gr, Rectangle bounds, NuGenMiniBarButtonState state)
		{
			if (state != NuGenMiniBarButtonState.Normal)
			{
				brs.Color = Color.FromArgb(80 * (int)state, brs.Color);
				gr.FillRectangle(brs, bounds);
				gr.DrawRectangle(pen, bounds);
			}
		}

		#endregion

		private NuGenMiniBarControlCollection _coll;
		private NuGenMiniBarButtonState[] _btnStates;
		private Pen pen = new Pen(SystemColors.Highlight);
		private SolidBrush brs = new SolidBrush(SystemColors.Highlight);
		private int w;
		private StringFormat _fmt;
		private bool _autosize;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMiniBar"/> class.
		/// </summary>
		public NuGenMiniBar()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint, true);
			_coll = new NuGenMiniBarControlCollection(this);
			_fmt = new StringFormat();
			_fmt.LineAlignment = StringAlignment.Center;
		}		
	}
}
