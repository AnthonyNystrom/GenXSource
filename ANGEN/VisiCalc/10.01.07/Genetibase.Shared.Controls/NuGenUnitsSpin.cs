/* -----------------------------------------------
 * NuGenUnitsSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a <see cref="NuGenSpin"/> with support for measure units.
	/// </summary>
	[ToolboxItem(false)]
	[NuGenSRDescription("Description_UnitsSpin")]
	public class NuGenUnitsSpin : NuGenSpin
	{
		#region Properties.MeasureUnits

		/*
		 * Factor
		 */

		/// <summary>
		/// Gets or sets the step for measure units. E.g. 1000 g = 1 kg. Then the factor will be 1000.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(1000)]
		[NuGenSRCategory("Category_MeasureUnits")]
		[NuGenSRDescription("Description_UnitsSpin_Factor")]
		public Int32 Factor
		{
			get
			{
				return this.UnitsValueConverter.Factor;
			}
			set
			{
				this.UnitsValueConverter.Factor = value;
			}
		}

		private static readonly Object _factorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="Factor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_UnitsSpin_FactorChanged")]
		public event EventHandler FactorChanged
		{
			add
			{
				this.Events.AddHandler(_factorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_factorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FactorChanged"/> event.
		/// </summary>
		protected virtual void OnFactorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_factorChanged, e);
		}

		/*
		 * MeasureUnits
		 */

		/// <summary>
		/// Gets or sets the list of available measure units. For weight it can be g, kg, T.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_MeasureUnits")]
		[NuGenSRDescription("Description_UnitsSpin_MeasureUnits")]
		public String[] MeasureUnits
		{
			get
			{
				return this.UnitsValueConverter.MeasureUnits;
			}
			set
			{
				this.UnitsValueConverter.MeasureUnits = value;
			}
		}

		private static readonly object _measureUnitsChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="MeasureUnits"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_UnitsSpin_MeasureUnitsChanged")]
		public event EventHandler MeasureUnitsChanged
		{
			add
			{
				this.Events.AddHandler(_measureUnitsChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_measureUnitsChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MeasureUnitsChanged"/> event.
		/// </summary>
		protected virtual void OnMeasureUnitsChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_measureUnitsChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets the amount to icrement or decrement on each button click.
		/// </summary>
		/// <value></value>
		/// <exception cref="ArgumentException">Only non-negative values are accepted.</exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Int32 Increment
		{
			get
			{
				return base.Increment;
			}
			set
			{
				base.Increment = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler IncrementChanged
		{
			add
			{
				base.IncrementChanged += value;
			}
			remove
			{
				base.IncrementChanged -= value;
			}
		}

		#endregion

		#region Properties.Services

		private INuGenMeasureUnitsValueConverter _unitsValueConverter;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenMeasureUnitsValueConverter UnitsValueConverter
		{
			get
			{
				if (_unitsValueConverter == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_unitsValueConverter = this.ServiceProvider.GetService<INuGenMeasureUnitsValueConverter>();

					if (_unitsValueConverter == null)
					{
						throw new NuGenServiceNotFoundException<INuGenMeasureUnitsValueConverter>();
					}
				}

				return _unitsValueConverter;
			}
		}

		#endregion

		#region EventHandlers.UnitsValueConverter

		private void _unitsValueConverter_FactorChanged(Object sender, EventArgs e)
		{
			this.OnFactorChanged(e);
		}

		private void _unitsValueConverter_IncrementChanged(Object sender, EventArgs e)
		{
			base.Increment = this.UnitsValueConverter.Increment;
		}

		private void _unitsValueConverter_MeasureUnitsChanged(Object sender, EventArgs e)
		{
			this.OnMeasureUnitsChanged(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenUnitsSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenInt32ValueConverter"/></para>
		///		<para><see cref="INuGenMeasureUnitsValueConverter"/></para>
		/// 	<para><see cref="INuGenSpinRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenUnitsSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.UnitsValueConverter.Factor = 1000;

			this.UnitsValueConverter.FactorChanged += new EventHandler(_unitsValueConverter_FactorChanged);
			this.UnitsValueConverter.IncrementChanged += new EventHandler(_unitsValueConverter_IncrementChanged);
			this.UnitsValueConverter.MeasureUnitsChanged += new EventHandler(_unitsValueConverter_MeasureUnitsChanged);

			this.Value = this.Minimum;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_unitsValueConverter != null)
				{
					_unitsValueConverter.FactorChanged -= _unitsValueConverter_FactorChanged;
					_unitsValueConverter.IncrementChanged -= _unitsValueConverter_IncrementChanged;
					_unitsValueConverter.MeasureUnitsChanged -= _unitsValueConverter_MeasureUnitsChanged;
					_unitsValueConverter.Dispose();
					_unitsValueConverter = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
