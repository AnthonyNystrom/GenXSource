/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.TabControlInternals;
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
	/// <summary>
	/// Represents a popup control that represents a range of available colors.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenColorBoxPopup : NuGenControl
	{
		#region Events

		private static readonly object _colorSelected = new object();

		/// <summary>
		/// Occurs when the user selectes a color on this color box popup window.
		/// </summary>
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

		/// <summary>
		/// Raises the color selected event.
		/// </summary>
		/// <param name="e">The <see cref="Genetibase.Shared.Controls.ColorBoxInternals.NuGenColorEventArgs"/> instance containing the event data.</param>
		protected virtual void OnColorSelected(NuGenColorEventArgs e)
		{
			EventHandler<NuGenColorEventArgs> handler = this.Events[_colorSelected] as EventHandler<NuGenColorEventArgs>;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(185, 250);

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

		#region Methods.Public

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		public void SetSelectedColor(Color value)
		{
			_colorTabControl.SetSelectedColor(value);
		}

		#endregion

		private ColorTabControl _colorTabControl;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorBoxPopup"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateTracker"/></para>
		/// <para><see cref="INuGenButtonLayoutManager"/></para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenTabRenderer"/></para>
		/// <para><see cref="INuGenTabStateTracker"/></para>
		/// <para><see cref="INuGenTabLayoutManager"/></para>
		/// <para><see cref="INuGenListBoxRenderer"/></para>
		/// <para><see cref="INuGenButtonRenderer"/></para>
		/// <para><see cref="INuGenImageListService"/></para>
		/// <para><see cref="INuGenColorsProvider"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenColorBoxPopup(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_colorTabControl = new ColorTabControl(serviceProvider);
			_colorTabControl.ColorSelected += delegate(object sender, NuGenColorEventArgs e)
			{
				this.OnColorSelected(e);
			};
			_colorTabControl.Parent = this;
			this.BackColor = Color.Transparent;
		}
	}
}
