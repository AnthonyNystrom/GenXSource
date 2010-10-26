/* -----------------------------------------------
 * NuGenAlignSelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenAlignSelector
	{
		internal sealed class Switcher : NuGenRadioButton
		{
			private ContentAlignment _assoicatedAlignment;

			public ContentAlignment AssociatedAlignment
			{
				get
				{
					return _assoicatedAlignment;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="Switcher"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenRadioButtonRenderer"/></para>
			/// 	<para><see cref="INuGenRadioButtonLayoutManager"/></para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenControlStateService"/></para>
			/// </param>
			/// <param name="associatedAlignment"></param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public Switcher(INuGenServiceProvider serviceProvider, ContentAlignment associatedAlignment)
				: base(serviceProvider)
			{
				_assoicatedAlignment = associatedAlignment;
				this.CheckAlign = ContentAlignment.MiddleCenter;
				this.Dock = DockStyle.Fill;
			}
		}
	}
}
