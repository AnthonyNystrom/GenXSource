/* -----------------------------------------------
 * NuGenCheckBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="CheckBox"/>
	/// </summary>
	[Designer(typeof(NuGenCheckBoxDesigner))]
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCheckBox : CheckBox
	{
		#region Properties.Public.Overridden

		/*
		 * AutoSize
		 */

		/// <summary>
		/// Gets or sets a value that indicates whether the control resizes based on its contents.
		/// </summary>
		/// <value></value>
		/// <returns>true if the control automatically resizes based on its contents; otherwise, false. The default is true.</returns>
		[DefaultValue(true)]
		public override bool AutoSize
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

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color of the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> value representing the background color.</returns>
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

		#region Properties.Protected.Virtual

		/*
		 * CheckBoxBounds
		 */

		private static readonly int _checkBoxSize = 12;

		/// <summary>
		/// </summary>
		protected virtual Rectangle CheckBoxBounds
		{
			get
			{
				if (this.RightToLeft == RightToLeft.No)
				{
					return new Rectangle(
						this.ClientRectangle.Left,
						this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - _checkBoxSize / 2,
						_checkBoxSize,
						_checkBoxSize
					);
				}

				return new Rectangle(
					this.ClientRectangle.Right - _checkBoxSize - 1,
					this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - _checkBoxSize / 2,
					_checkBoxSize,
					_checkBoxSize
				);
			}
		}

		/*
		 * TextBounds
		 */

		/// <summary>
		/// </summary>
		protected virtual Rectangle TextBounds
		{
			get
			{
				if (this.RightToLeft == RightToLeft.No)
				{
					return new Rectangle(
						this.CheckBoxBounds.Right + 2,
						this.ClientRectangle.Top,
						this.ClientRectangle.Width - this.CheckBoxBounds.Right,
						this.ClientRectangle.Height
					);
				}

				return new Rectangle(
					this.ClientRectangle.Left,
					this.ClientRectangle.Top,
					this.CheckBoxBounds.Left - this.ClientRectangle.Left,
					this.ClientRectangle.Height
				);
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * ButtonStateTracker
		 */

		private INuGenButtonStateTracker _buttonStateTracker = null;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenButtonStateService stateService = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_buttonStateTracker = stateService.CreateStateTracker();
					Debug.Assert(_buttonStateTracker != null, "_buttonStateTracker != null");
				}

				return _buttonStateTracker;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenCheckBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenCheckBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenCheckBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenCheckBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * StateTracker
		 */

		private INuGenControlStateTracker _stateTracker = null;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlStateTracker StateTracker
		{
			get
			{
				if (_stateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenControlStateService stateService = this.ServiceProvider.GetService<INuGenControlStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlStateService>();
					}

					_stateTracker = stateService.CreateStateTracker();
					Debug.Assert(_stateTracker != null, "_stateTracker != null");
				}

				return _stateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.ButtonStateTracker.Enabled(this.Enabled);
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnGotFocus
		 */

		/// <summary>
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			this.ButtonStateTracker.GotFocus();
			this.StateTracker.GotFocus();

			base.OnGotFocus(e);
		}

		/*
		 * OnLostFocus
		 */

		/// <summary>
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			this.ButtonStateTracker.LostFocus();
			this.StateTracker.LostFocus();

			base.OnLostFocus(e);
		}

		/*
		 * OnMouseDown
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseDown();
			base.OnMouseDown(e);
		}

		/*
		 * OnMouseEnter
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseEnter();
			base.OnMouseEnter(e);
		}

		/*
		 * OnMouseLeave
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseLeave();
			base.OnMouseLeave(e);
		}

		/*
		 * OnMouseUp
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseUp();
			base.OnMouseUp(e);
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			Graphics g = e.Graphics;

			this.Renderer.DrawCheckBox(
				new NuGenPaintParams(
					this,
					g,
					this.CheckBoxBounds,
					this.ButtonStateTracker.GetControlState()
				)
			);

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(
				this,
				g,
				this.TextBounds,
				this.StateTracker.GetControlState(),
				this.Text
			);

			textPaintParams.Font = this.Font;
			textPaintParams.ForeColor = this.ForeColor;
			textPaintParams.TextAlign = this.TextAlign;

			this.Renderer.DrawText(textPaintParams);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCheckBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// <see cref="INuGenCheckBoxRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenCheckBox(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			this.SetStyle(ControlStyles.Opaque, false);

			this.AutoSize = true;
			this.BackColor = Color.Transparent;
			this.Width = this.DefaultSize.Width;
		}

		#endregion
	}
}
