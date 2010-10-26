/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.TabControlInternals;
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
		private sealed class ColorTabControl : NuGenTabControl
		{
			#region Declarations.Fields

			private StdColorListBox _stdColorListBox;
			private WebColorListBox _webColorListBox;
			private CustomColorsPanel _customColorsPanel;
			private Dictionary<IColorCollection, NuGenTabPage> _colorCollections;

			#endregion

			#region Events

			public event EventHandler<NuGenColorEventArgs> ColorSelected
			{
				add
				{
					_stdColorListBox.ColorSelected += value;
					_webColorListBox.ColorSelected += value;
					_customColorsPanel.ColorSelected += value;
				}
				remove
				{
					_stdColorListBox.ColorSelected -= value;
					_webColorListBox.ColorSelected -= value;
					_customColorsPanel.ColorSelected -= value;
				}
			}

			#endregion

			#region Methods.Public

			public void SetSelectedColor(Color value)
			{
				foreach (IColorCollection colorCollection in _colorCollections.Keys)
				{
					if (colorCollection.SetSelectedColor(value))
					{
						this.SelectedTab = _colorCollections[colorCollection];
						break;
					}
				}
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="ColorTabControl"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// Requires:<para/>
			/// <see cref="INuGenButtonStateTracker"/><para/>
			/// <see cref="INuGenControlStateTracker"/><para/>
			/// <see cref="INuGenTabRenderer"/><para/>
			/// <see cref="INuGenTabStateTracker"/><para/>
			/// <see cref="INuGenTabLayoutManager"/><para/>
			/// <see cref="INuGenPanelRenderer"/><para/>
			/// <see cref="INuGenListBoxRenderer"/><para/>
			/// <see cref="INuGenImageListService"/><para/>
			/// <see cref="INuGenColorsProvider"/><para/>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="serviceProvider"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public ColorTabControl(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				_customColorsPanel = new CustomColorsPanel(serviceProvider);
				_stdColorListBox = new StdColorListBox(serviceProvider);
				_webColorListBox = new WebColorListBox(serviceProvider);

				this.CloseButtonOnTab = false;
				this.Dock = DockStyle.Fill;

				NuGenTabPage customColorsPage = new NuGenTabPage(serviceProvider);
				customColorsPage.Text = Resources.Text_ColorBoxPopup_Custom;
				customColorsPage.Controls.Add(_customColorsPanel);

				NuGenTabPage stdColorsPage = new NuGenTabPage(serviceProvider);
				stdColorsPage.Text = Resources.Text_ColorBoxPopup_System;
				stdColorsPage.Controls.Add(_stdColorListBox);

				NuGenTabPage webColorsPage = new NuGenTabPage(serviceProvider);
				webColorsPage.Text = Resources.Text_ColorBoxPopup_Web;
				webColorsPage.Controls.Add(_webColorListBox);

				this.TabPages.Add(customColorsPage);
				this.TabPages.Add(webColorsPage);
				this.TabPages.Add(stdColorsPage);

				_colorCollections = new Dictionary<IColorCollection, NuGenTabPage>();

				_colorCollections.Add(_stdColorListBox, stdColorsPage);
				_colorCollections.Add(_webColorListBox, webColorsPage);
				_colorCollections.Add(_customColorsPanel, customColorsPage);
			}

			#endregion
		}
	}
}
