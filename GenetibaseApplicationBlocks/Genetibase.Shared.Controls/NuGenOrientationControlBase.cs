/* -----------------------------------------------
 * NuGenOrientationControlBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides basic functionality for the controls that support horizontal and vertical orientation.
	/// </summary>
	public abstract class NuGenOrientationControlBase : NuGenControlBase
	{
		#region Properties.Appearance

		/*
		 * Orientation
		 */

		private NuGenOrientationStyle _orientation;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Orientation")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public NuGenOrientationStyle Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				if (_orientation != value)
				{
					_orientation = value;

					int bufferWidth = this.Width;
					this.Width = this.Height;
					this.Height = bufferWidth;
					this.Invalidate();

					this.OnOrientationChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _orientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Orientation"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_OrientationChanged")]
		public event EventHandler OrientationChanged
		{
			add
			{
				this.Events.AddHandler(_orientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_orientationChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenOrientationControlBase.OrientationChanged"/> event.
		/// </summary>
		protected virtual void OnOrientationChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_orientationChanged, e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOrientationControlBase"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenOrientationControlBase(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
