/* -----------------------------------------------
 * NuGenDirectorySelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenDirectorySelector
	{
		private sealed class DropDownButton : NuGenWndLessUIControl
		{
			private INuGenDirectorySelectorRenderer _renderer;

			private INuGenDirectorySelectorRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenDirectorySelectorRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenDirectorySelectorRenderer>();
						}
					}

					return _renderer;
				}
			}

			/// <summary>
			/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.Paint"/> event will be raised.
			/// </summary>
			/// <param name="e"></param>
			protected override void OnPaint(PaintEventArgs e)
			{
				NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
				paintParams.Bounds = this.Bounds;
				paintParams.State = this.ButtonStateTracker.GetControlState();
				this.Renderer.DrawDropDownButton(paintParams);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="DropDownButton"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			///		<para><see cref="INuGenDirectorySelectorRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public DropDownButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}
		}
	}
}
