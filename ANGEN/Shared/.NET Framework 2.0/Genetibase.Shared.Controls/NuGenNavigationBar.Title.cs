/* -----------------------------------------------
 * NuGenNavigationBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenNavigationBar
	{
		private sealed class Title : NuGenTitle
		{
			#region Properties.Services

			private INuGenNavigationBarLayoutManager _layoutManager;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenNavigationBarLayoutManager LayoutManager
			{
				get
				{
					if (_layoutManager == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_layoutManager = this.ServiceProvider.GetService<INuGenNavigationBarLayoutManager>();

						if (_layoutManager == null)
						{
							throw new NuGenServiceNotFoundException<INuGenNavigationBarLayoutManager>();
						}
					}

					return _layoutManager;
				}
			}

			#endregion

			#region Methods.Public

			/*
			 * SetSelectedButton
			 */

			/// <summary>
			/// </summary>
			/// <param name="selectedButton">Can be <see langword="null"/>.</param>
			public void SetSelectedButton(NuGenNavigationButton selectedButton)
			{
				if (selectedButton != null)
				{
					this.Text = selectedButton.Text;
				}
				else
				{
					this.Text = "";
				}
			}

			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="Title"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenControlStateTracker"/></para>
			/// <para><see cref="INuGenNavigationBarLayoutManager"/></para>
			/// <para><see cref="INuGenTitleRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public Title(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Height = this.LayoutManager.GetTitleHeight();
			}
		}
	}
}
