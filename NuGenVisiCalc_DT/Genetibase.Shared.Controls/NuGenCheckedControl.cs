/* -----------------------------------------------
 * NuGenCheckedControl.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a <see cref="NuGenControl"/> that acts like a switch button.
	/// </summary>
	public abstract class NuGenCheckedControl : NuGenControl
	{
		#region Properties.Behavior

		/*
		 * Checked
		 */

		private bool _checked;

		/// <summary>
		/// Indicates whether the <see cref="NuGenCheckedControl"/> is in checked state.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_CheckedControl_Checked")]
		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				if (_checked != value)
				{
					_checked = value;
					this.OnCheckedChanged(EventArgs.Empty);

					if (_checked)
					{
						this.ButtonStateTracker.MouseDown();
					}
					else
					{
						if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
						{
							this.ButtonStateTracker.MouseUp();
						}
						else
						{
							this.ButtonStateTracker.MouseLeave();
						}
					}

					this.Invalidate();
				}
			}
		}

		private static readonly object _checkedChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Checked"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CheckedControl_CheckedChanged")]
		public event EventHandler CheckedChanged
		{
			add
			{
				this.Events.AddHandler(_checkedChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_checkedChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitchButton.CheckedChanged"/> event.
		/// </summary>
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_checkedChanged, e);
		}

		/*
		 * CheckOnClick
		 */

		private bool _checkOnClick;

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="NuGenCheckedControl"/> automatically
		/// appears checked or unchecked when clicked.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_CheckedControl_CheckOnClick")]
		public bool CheckOnClick
		{
			get
			{
				return _checkOnClick;
			}
			set
			{
				if (_checkOnClick != value)
				{
					_checkOnClick = value;
					this.OnCheckOnClickChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _checkOnClickChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="CheckOnClick"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CheckedControl_CheckOnClick")]
		public event EventHandler CheckOnClickChanged
		{
			add
			{
				this.Events.AddHandler(_checkOnClickChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_checkOnClickChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitchButton.CheckOnClickChanged"/> event.
		/// </summary>
		protected virtual void OnCheckOnClickChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_checkOnClickChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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

		#endregion

		#region Properties.Services

		private INuGenButtonStateTracker _buttonStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					INuGenButtonStateService buttonStateService = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (buttonStateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_buttonStateTracker = buttonStateService.CreateStateTracker();
				}

				return _buttonStateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			if (_checkOnClick)
			{
				this.Checked = !this.Checked;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			this.ButtonStateTracker.Enabled(this.Enabled);
			this.Invalidate();
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			if (!_checked)
			{
				this.ButtonStateTracker.MouseEnter();
				this.Invalidate();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (!_checked)
			{
				this.ButtonStateTracker.MouseLeave();
				this.Invalidate();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.Invalidate();
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCheckedControl"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenCheckedControl(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
