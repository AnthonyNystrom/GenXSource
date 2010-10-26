/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.Properties;
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
		private sealed class CustomColorsPanel : UserControl, IColorCollection
		{
			#region Declarations.Fields

			private IContainer _components;
			private FlowLayoutPanel _flowLayoutPanel;
			private TableLayoutPanel _tableLayoutPanel;
			private NuGenButton _otherButton;
			private ColorDialog _colorDialog;

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

			private void OnColorSelected(NuGenColorEventArgs e)
			{
				EventHandler<NuGenColorEventArgs> handler = this.Events[_colorSelected] as EventHandler<NuGenColorEventArgs>;

				if (handler != null)
				{
					handler(this, e);
				}
			}

			public bool SetSelectedColor(Color color)
			{
				return true;
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
			private INuGenColorsProvider ColorsProvider
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

			/*
			 * ServiceProvider
			 */

			private INuGenServiceProvider _serviceProvider;

			/// <summary>
			/// </summary>
			private INuGenServiceProvider ServiceProvider
			{
				get
				{
					return _serviceProvider;
				}
			}

			#endregion

			#region Methods.Private

			/*
			 * InitializeColorDialog
			 */

			private void InitializeColorDialog(ColorDialog colorDialog)
			{
				Debug.Assert(colorDialog != null, "colorDialog != null");
				colorDialog.FullOpen = true;
				Debug.Assert(_components != null, "_components != null");
				_components.Add(colorDialog);
			}

			/*
			 * InitializeFlowLayoutPanel
			 */

			private void InitializeFlowLayoutPanel(FlowLayoutPanel layoutPanel)
			{
				Debug.Assert(layoutPanel != null, "layoutPanel != null");
				layoutPanel.BackColor = Color.Transparent;
				layoutPanel.Dock = DockStyle.Fill;
			}

			/*
			 * InitializeOtherButton
			 */

			private void InitializeOtherButton(Button otherButton)
			{
				Debug.Assert(otherButton != null, "otherButton != null");
				otherButton.Dock = DockStyle.Fill;
				otherButton.Text = Resources.Text_ColorBoxPopup_Other;
				otherButton.Click += _otherButton_Click;
			}

			/*
			 * InitializeTableLayoutPanel
			 */

			private void InitializeTableLayoutPanel(TableLayoutPanel layoutPanel)
			{
				Debug.Assert(layoutPanel != null, "layoutPanel != null");

				layoutPanel.BackColor = Color.Transparent;
				layoutPanel.Dock = DockStyle.Fill;
				layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
				layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
				layoutPanel.Parent = this;
			}

			#endregion

			#region EventHandlers

			private void _colorPanel_Click(object sender, EventArgs e)
			{
				this.OnColorSelected(new NuGenColorEventArgs(((ColorPanel)sender).DisplayColor));
			}

			private void _otherButton_Click(object sender, EventArgs e)
			{
				Debug.Assert(_colorDialog != null, "_colorDialog != null");
				
				if (_colorDialog.ShowDialog() == DialogResult.OK)
				{
					this.OnColorSelected(new NuGenColorEventArgs(_colorDialog.Color));
				}
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="CustomColorsPanel"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenButtonStateTracker"/></para>
			/// <para><see cref="INuGenControlStateTracker"/></para>
			/// <para><see cref="INuGenButtonRenderer"/></para>
			/// <para><see cref="INuGenPanelRenderer"/></para>
			/// <para><see cref="INuGenColorsProvider"/></para>
			/// </param>
			public CustomColorsPanel(INuGenServiceProvider serviceProvider)
			{
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}

				_serviceProvider = serviceProvider;

				this.Dock = DockStyle.Fill;

				_components = new Container();
				_colorDialog = new ColorDialog();
				_tableLayoutPanel = new TableLayoutPanel();
				_flowLayoutPanel = new FlowLayoutPanel();
				_otherButton = new NuGenButton(serviceProvider);

				this.InitializeColorDialog(_colorDialog);
				this.InitializeFlowLayoutPanel(_flowLayoutPanel);
				this.InitializeOtherButton(_otherButton);
				this.InitializeTableLayoutPanel(_tableLayoutPanel);

				_tableLayoutPanel.Controls.Add(_flowLayoutPanel, 0, 0);
				_tableLayoutPanel.Controls.Add(_otherButton, 0, 1);

				IList<Color> colors = null;
				this.ColorsProvider.FillWithCustomColors(out colors);
				Debug.Assert(colors != null, "colors != null");

				foreach (Color color in colors)
				{
					ColorPanel colorPanel = new ColorPanel(serviceProvider);
					colorPanel.Click += _colorPanel_Click;
					colorPanel.DisplayColor = color;
					colorPanel.Parent = _flowLayoutPanel;
				}
			}

			#endregion
		}
	}
}
