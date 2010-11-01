/* -----------------------------------------------
 * NuGenRadioButton.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="RadioButton"/>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenRadioButton : RadioButton
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
		 * RadioButtonBounds
		 */

		private static readonly int _radioButtonSize = 12;

		/// <summary>
		/// </summary>
		protected virtual Rectangle RadioButtonBounds
		{
			get
			{
				if (this.RightToLeft == RightToLeft.No)
				{
					return new Rectangle(
						this.ClientRectangle.Left,
						this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - _radioButtonSize / 2,
						_radioButtonSize,
						_radioButtonSize
					);
				}

				return new Rectangle(
					this.ClientRectangle.Right - _radioButtonSize - 1,
					this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - _radioButtonSize / 2,
					_radioButtonSize,
					_radioButtonSize
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
						this.RadioButtonBounds.Right + 2,
						this.ClientRectangle.Top,
						this.ClientRectangle.Width - this.RadioButtonBounds.Right,
						this.ClientRectangle.Height
					);
				}

				return new Rectangle(
					this.ClientRectangle.Left,
					this.ClientRectangle.Top,
					this.RadioButtonBounds.Left - this.ClientRectangle.Left + 1,
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

		private INuGenRadioButtonRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenRadioButtonRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = this.ServiceProvider.GetService<INuGenRadioButtonRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenRadioButtonRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider;

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
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
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
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnLostFocus(System.EventArgs)"></see> event.
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
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			this.Renderer.DrawRadioButton(
				new NuGenPaintParams(
					this,
					g,
					this.RadioButtonBounds,
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
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenRadioButtonRenderer"/><para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenRadioButton(INuGenServiceProvider serviceProvider)
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
