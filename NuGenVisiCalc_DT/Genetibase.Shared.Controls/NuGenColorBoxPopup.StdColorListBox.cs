/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenColorBoxPopup
	{
		private sealed class StdColorListBox : ColorListBox
		{
			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="StdColorListBox"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenControlStateTracker"/></para>
			/// 	<para><see cref="INuGenListBoxRenderer"/></para>
			/// 	<para><see cref="INuGenImageListService"/></para>
			///		<para><see cref="INuGenColorsProvider"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public StdColorListBox(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				IList<Color> colors = null;
				this.ColorsProvider.FillWithStandardColors(out colors);
				Debug.Assert(colors != null, "colors != null");
				this.InitializeColorBox(colors);
			}

			#endregion
		}
	}
}
