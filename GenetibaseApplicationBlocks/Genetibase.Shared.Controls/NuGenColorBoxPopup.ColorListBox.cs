/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
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
	partial class NuGenColorBoxPopup
	{
		private class ColorListBox : NuGenListBox, IColorCollection
		{
			#region Declarations.Fields

			private IContainer _components;

			#endregion

			#region IColorCollection Members

			private static readonly object _colorSelected = new object();

			public event EventHandler<NuGenColorEventArgs> ColorSelected
			{
				add
				{
					this.Events.AddHandler(_colorSelected, value);
				}
				remove
				{
					this.Events.RemoveHandler(_colorSelected, value);
				}
			}

			protected virtual void OnColorSelected(NuGenColorEventArgs e)
			{
				EventHandler<NuGenColorEventArgs> handler = this.Events[_colorSelected] as EventHandler<NuGenColorEventArgs>;

				if (handler != null)
				{
					handler(this, e);
				}
			}

			public bool SetSelectedColor(Color color)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (color == ((ColorListItem)this.Items[i]).DisplayColor)
					{
						this.SelectedIndex = i;
						return true;
					}
				}

				return false;
			}

			#endregion

			#region Properties.Services

			/*
			 * ColorsProvider
			 */

			private INuGenColorsProvider _colorsProvider;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			protected INuGenColorsProvider ColorsProvider
			{
				get
				{
					if (_colorsProvider == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_colorsProvider = this.ServiceProvider.GetService<INuGenColorsProvider>();

						if (_colorsProvider == null)
						{
							throw new NuGenServiceNotFoundException<INuGenColorsProvider>();
						}
					}

					return _colorsProvider;
				}
			}

			#endregion

			#region Methods.Protected

			/*
			 * InitializeImageList
			 */

			/// <summary>
			/// </summary>
			/// <param name="colors"></param>
			/// <exception cref="ArgumentNullException">
			///	<para>
			///		<paramref name="colors"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			protected void InitializeColorBox(List<Color> colors)
			{
				if (colors == null)
				{
					throw new ArgumentNullException("colors");
				}

				System.Windows.Forms.ImageList imageList = null;
				this.ColorsProvider.FillWithColorSamples(colors, out imageList);
				Debug.Assert(imageList != null, "imageList != null");
				_components.Add(imageList);
				this.ImageList = imageList;

				for (int i = 0; i < colors.Count; i++)
				{
					this.Items.Add(new ColorListItem(colors[i]));
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			protected override void OnClick(EventArgs e)
			{
				base.OnClick(e);

				Color _selectedColor = Color.Empty;

				if (this.SelectedItem != null)
				{
					_selectedColor = ((ColorListItem)this.SelectedItem).DisplayColor;
				}

				this.OnColorSelected(new NuGenColorEventArgs(_selectedColor));
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="ColorListBox"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenControlStateTracker"/></para>
			/// 	<para><see cref="INuGenListBoxRenderer"/></para>
			/// 	<para><see cref="INuGenImageListService"/></para>
			///		<para><see cref="INuGenColorsProvider"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public ColorListBox(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Dock = DockStyle.Fill;
				this.HotTrack = true;
				this.SelectedIndex = -1;

				_components = new Container();
			}

			#endregion

			#region Dispose

			/// <summary>
			/// </summary>
			/// <param name="disposing"></param>
			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_components != null)
					{
						_components.Dispose();
					}
				}

				base.Dispose(disposing);
			}

			#endregion
		}
	}
}
