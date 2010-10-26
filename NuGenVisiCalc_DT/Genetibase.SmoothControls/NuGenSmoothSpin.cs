/* -----------------------------------------------
 * NuGenSmoothSpin.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothSpin), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenSmoothSpin : NuGenSpin
	{
		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/*
		 * BackgroundImageLayout
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSpin"/> class.
		/// </summary>
		public NuGenSmoothSpin()
			: this(NuGenSmoothServiceManager.SpinServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenInt32ValueConverter"/></para>
		/// 	<para><see cref="INuGenSpinRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
