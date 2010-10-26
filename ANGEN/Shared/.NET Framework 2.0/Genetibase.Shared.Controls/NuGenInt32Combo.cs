/* -----------------------------------------------
 * NuGenInt32Combo.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a <see cref="NuGenComboBox"/> with a <see cref="Value"/> property that always returns
	/// the last successfully validated <see cref="Int32"/> user input.
	/// </summary>
	[ToolboxItem(false)]
	public class NuGenInt32Combo : NuGenComboBox
	{
		#region Properties.Appearance

		/// <summary>
		/// Gets or sets the current combo box value.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_IntCombo_Value")]
		public int Value
		{
			get
			{
				return this.Int32ValueConverter.Value;
			}
			set
			{
				this.Int32ValueConverter.Value = value;
			}
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_IntCombo_ValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenInt32Combo.ValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Data

		/*
		 * Minimum
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_IntCombo_Minimum")]
		public int Minimum
		{
			get
			{
				return this.Int32ValueConverter.Minimum;
			}
			set
			{
				this.Int32ValueConverter.Minimum = value;
			}
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_IntCombo_MinimumChanged")]
		public event EventHandler MinimumChanged
		{
			add
			{
				this.Events.AddHandler(_minimumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_minimumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenInt32Combo.MinimumChanged"/> event.
		/// </summary>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * Maximum
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(100)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_IntCombo_Maximum")]
		public int Maximum
		{
			get
			{
				return this.Int32ValueConverter.Maximum;
			}
			set
			{
				this.Int32ValueConverter.Maximum = value;
			}
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_IntCombo_MaximumChanged")]
		public event EventHandler MaximumChanged
		{
			add
			{
				this.Events.AddHandler(_maximumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maximumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenInt32Combo.MaximumChanged"/> event.
		/// </summary>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
		/// <value></value>
		/// <returns>The text associated with this control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		private INuGenInt32ValueConverter _int32ValueConverter;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenInt32ValueConverter Int32ValueConverter
		{
			get
			{
				if (_int32ValueConverter == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_int32ValueConverter = this.ServiceProvider.GetService<INuGenInt32ValueConverter>();

					if (_int32ValueConverter == null)
					{
						throw new NuGenServiceNotFoundException<INuGenInt32ValueConverter>();
					}
				}

				return _int32ValueConverter;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == System.Windows.Forms.Keys.Return || e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				this.Int32ValueConverter.Text = this.Text;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			this.Int32ValueConverter.Text = this.Text;
		}

		#endregion

		#region EventHandlers.Int32ValueConverter

		private void _int32ValueConverter_MaximumChanged(object sender, EventArgs e)
		{
			this.OnMaximumChanged(e);
		}

		private void _int32ValueConverter_MinimumChanged(object sender, EventArgs e)
		{
			this.OnMinimumChanged(e);
		}

		private void _int32ValueConverter_TextChanged(object sender, EventArgs e)
		{
			this.Text = this.Int32ValueConverter.Text;
		}

		private void _int32ValueConverter_ValueChanged(object sender, EventArgs e)
		{
			this.OnValueChanged(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInt32Combo"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		///		<para><see cref="INuGenInt32ValueConverter"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenInt32Combo(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.Int32ValueConverter.MaximumChanged += _int32ValueConverter_MaximumChanged;
			this.Int32ValueConverter.MinimumChanged += _int32ValueConverter_MinimumChanged;
			this.Int32ValueConverter.TextChanged += _int32ValueConverter_TextChanged;
			this.Int32ValueConverter.ValueChanged += _int32ValueConverter_ValueChanged;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ComboBox"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_int32ValueConverter != null)
				{
					_int32ValueConverter.MaximumChanged -= _int32ValueConverter_MaximumChanged;
					_int32ValueConverter.MinimumChanged -= _int32ValueConverter_MinimumChanged;
					_int32ValueConverter.TextChanged -= _int32ValueConverter_TextChanged;
					_int32ValueConverter.ValueChanged -= _int32ValueConverter_ValueChanged;
					_int32ValueConverter.Dispose();
					_int32ValueConverter = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
