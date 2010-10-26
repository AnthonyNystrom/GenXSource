/* -----------------------------------------------
 * NuGenSmoothOptionSpin.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="System.Windows.Forms.DomainUpDown"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothOptionSpin), "Resources.NuGenIcon.png")]
	[DefaultProperty("Items")]
	public class NuGenSmoothOptionSpin : NuGenOptionSpin
	{
		#region Properties.Hidden

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

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(155, 20);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothOptionSpin"/> class.
		/// </summary>
		public NuGenSmoothOptionSpin()
			: this(NuGenSmoothServiceManager.SpinServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothOptionSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requries:<para/>
		/// <see cref="INuGenSpinRenderer"/><para/>
		/// <see cref="INuGenButtonStateTracker"/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		public NuGenSmoothOptionSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
