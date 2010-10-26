/* -----------------------------------------------
 * NuGenCalculatorPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.Shared.Controls.Properties.Resources;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CalculatorInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	public partial class NuGenCalculatorPopup : NuGenControl
	{
		private static readonly object _valueAccepted = new object();

		/// <summary>
		/// </summary>
		public event EventHandler ValueAccepted
		{
			add
			{
				this.Events.AddHandler(_valueAccepted, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueAccepted, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenCalculatorPopup.ValueAccepted"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueAccepted(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_valueAccepted, e);
		}

		private static readonly object _valueCanceled = new object();

		/// <summary>
		/// </summary>
		public event EventHandler ValueCanceled
		{
			add
			{
				this.Events.AddHandler(_valueCanceled, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueCanceled, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenCalculatorPopup.ValueCanceled"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueCanceled(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_valueCanceled, e);
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0.0)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Calculator_Value")]
		public double Value
		{
			get
			{
				return _engine.Value;
			}
			set
			{
				_engine.Value = value;
			}
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calculator_ValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenCalculatorPopup.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		private static readonly Size _defaultSize = new Size(220, 220);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		private INuGenPanelRenderer _panelRenderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPanelRenderer PanelRenderer
		{
			get
			{
				if (_panelRenderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_panelRenderer = this.ServiceProvider.GetService<INuGenPanelRenderer>();

					if (_panelRenderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPanelRenderer>();
					}
				}

				return _panelRenderer;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_editLayoutPanel.Select();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.DrawBorder = true;
			paintParams.State = this.StateTracker.GetControlState();

			this.PanelRenderer.DrawBackground(paintParams);
			this.PanelRenderer.DrawBorder(paintParams);

			base.OnPaint(e);
		}

		/// <summary>
		/// </summary>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			_hotKeys.Process(keyData);
			return true;
		}

		private string GetEngineValue()
		{
			return _engine.Value.ToString(CultureInfo.CurrentUICulture);
		}

		#region Operations

		private void D0()
		{
			_engine.ProcessAction(_d0Button.ButtonAction);
		}

		private void D1()
		{
			_engine.ProcessAction(_d1Button.ButtonAction);
		}

		private void D2()
		{
			_engine.ProcessAction(_d2Button.ButtonAction);
		}

		private void D3()
		{
			_engine.ProcessAction(_d3Button.ButtonAction);
		}

		private void D4()
		{
			_engine.ProcessAction(_d4Button.ButtonAction);
		}

		private void D5()
		{
			_engine.ProcessAction(_d5Button.ButtonAction);
		}

		private void D6()
		{
			_engine.ProcessAction(_d6Button.ButtonAction);
		}

		private void D7()
		{
			_engine.ProcessAction(_d7Button.ButtonAction);
		}

		private void D8()
		{
			_engine.ProcessAction(_d8Button.ButtonAction);
		}

		private void D9()
		{
			_engine.ProcessAction(_d9Button.ButtonAction);
		}

		private void Accept()
		{
			this.OnValueAccepted(EventArgs.Empty);
		}

		private void Cancel()
		{
			this.OnValueCanceled(EventArgs.Empty);
		}

		#endregion

		private void _actionButton_Click(object sender, EventArgs e)
		{
			try
			{
				_engine.ProcessAction(((ActionButton)sender).ButtonAction);
			}
			catch (DivideByZeroException)
			{
				_valueDisplayTextBox.Text = res.DivideByZero;
			}
		}

		private void _engine_ValueChanged(object sender, EventArgs e)
		{
			_valueDisplayTextBox.Text = this.GetEngineValue();
		}

		private Engine _engine;
		private NuGenHotKeys _hotKeys;
		private NuGenTextBox _valueDisplayTextBox;
		private ActionButton _backspaceButton;
		private ActionButton _cButton;
		private ActionButton _ceButton;
		private ActionButton _d0Button;
		private ActionButton _d1Button;
		private ActionButton _d2Button;
		private ActionButton _d3Button;
		private ActionButton _d4Button;
		private ActionButton _d5Button;
		private ActionButton _d6Button;
		private ActionButton _d7Button;
		private ActionButton _d8Button;
		private ActionButton _d9Button;
		private ActionButton _dotButton;
		private ActionButton _signButton;
		private ActionButton _divideButton;
		private ActionButton _minusButton;
		private ActionButton _multiplyButton;
		private ActionButton _plusButton;
		private ActionButton _evaluateButton;
		private ActionButton _divXButton;
		private ActionButton _percentButton;
		private ActionButton _sqrtButton;

		private TableLayoutPanel _actionButtonLayoutPanel;
		private TableLayoutPanel _editLayoutPanel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalculatorPopup"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenButtonRenderer"/></para>
		/// 	<para><see cref="INuGenButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		/// 	<para><see cref="INuGenTextBoxRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenCalculatorPopup(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.Padding = new Padding(3);

			_backspaceButton = new ActionButton(serviceProvider, ActionManager.Backspace, res.Text_Calc_Backspace);
			_backspaceButton.TabIndex = 20;

			_cButton = new ActionButton(serviceProvider, ActionManager.C, res.Text_Calc_C);
			_cButton.TabIndex = 40;

			_ceButton = new ActionButton(serviceProvider, ActionManager.CE, res.Text_Calc_CE);
			_ceButton.TabIndex = 30;
			
			_d0Button = new ActionButton(serviceProvider, ActionManager.D0, res.Text_Calc_D0);
			_d1Button = new ActionButton(serviceProvider, ActionManager.D1, res.Text_Calc_D1);
			_d2Button = new ActionButton(serviceProvider, ActionManager.D2, res.Text_Calc_D2);
			_d3Button = new ActionButton(serviceProvider, ActionManager.D3, res.Text_Calc_D3);
			_d4Button = new ActionButton(serviceProvider, ActionManager.D4, res.Text_Calc_D4);
			_d5Button = new ActionButton(serviceProvider, ActionManager.D5, res.Text_Calc_D5);
			_d6Button = new ActionButton(serviceProvider, ActionManager.D6, res.Text_Calc_D6);
			_d7Button = new ActionButton(serviceProvider, ActionManager.D7, res.Text_Calc_D7);
			_d8Button = new ActionButton(serviceProvider, ActionManager.D8, res.Text_Calc_D8);
			_d9Button = new ActionButton(serviceProvider, ActionManager.D9, res.Text_Calc_D9);
			_dotButton = new ActionButton(serviceProvider, ActionManager.Dot, res.Text_Calc_Dot);
			_signButton = new ActionButton(serviceProvider, ActionManager.Sign, res.Text_Calc_Sign);
			_divideButton = new ActionButton(serviceProvider, ActionManager.Divide, res.Text_Calc_Divide);
			_minusButton = new ActionButton(serviceProvider, ActionManager.Minus, res.Text_Calc_Minus);
			_multiplyButton = new ActionButton(serviceProvider, ActionManager.Multiply, res.Text_Calc_Multiply);
			_plusButton = new ActionButton(serviceProvider, ActionManager.Plus, res.Text_Calc_Plus);
			_evaluateButton = new ActionButton(serviceProvider, ActionManager.Evaluate, res.Text_Calc_Evaluate);
			_divXButton = new ActionButton(serviceProvider, ActionManager.DivX, res.Text_Calc_DivX);
			_percentButton = new ActionButton(serviceProvider, ActionManager.Percent, res.Text_Calc_Percent);
			_sqrtButton = new ActionButton(serviceProvider, ActionManager.Sqrt, res.Text_Calc_Sqrt);

			_engine = new Engine();
			_engine.ValueChanged += _engine_ValueChanged;

			_actionButtonLayoutPanel = new TableLayoutPanel();
			_actionButtonLayoutPanel.BackColor = Color.Transparent;

			for (int row = 0; row < 5; row++)
			{
				_actionButtonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
			}

			for (int column = 0; column < 4; column++)
			{
				_actionButtonLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
			}

			this.AddToRow0(_d7Button, 0);
			this.AddToRow0(_d8Button, 1);
			this.AddToRow0(_d9Button, 2);
			this.AddToRow0(_divideButton, 3);
			this.AddToRow0(_sqrtButton, 4);

			this.AddToRow1(_d4Button, 0);
			this.AddToRow1(_d5Button, 1);
			this.AddToRow1(_d6Button, 2);
			this.AddToRow1(_multiplyButton, 3);
			this.AddToRow1(_percentButton, 4);

			this.AddToRow2(_d1Button, 0);
			this.AddToRow2(_d2Button, 1);
			this.AddToRow2(_d3Button, 2);
			this.AddToRow2(_minusButton, 3);
			this.AddToRow2(_divXButton, 4);

			this.AddToRow3(_d0Button, 0);
			this.AddToRow3(_signButton, 1);
			this.AddToRow3(_dotButton, 2);
			this.AddToRow3(_plusButton, 3);
			this.AddToRow3(_evaluateButton, 4);

			foreach (ActionButton button in _actionButtonLayoutPanel.Controls)
			{
				button.Dock = DockStyle.Fill;
				button.Click += _actionButton_Click;
			}

			_actionButtonLayoutPanel.Dock = DockStyle.Fill;
			_actionButtonLayoutPanel.Parent = this;
			_actionButtonLayoutPanel.TabIndex = 10;

			_editLayoutPanel = new TableLayoutPanel();
			_editLayoutPanel.BackColor = Color.Transparent;

			_editLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
			_editLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
			_editLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
			_editLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
			_editLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));

			_editLayoutPanel.Controls.Add(_backspaceButton, 0, 1);
			_editLayoutPanel.Controls.Add(_ceButton, 1, 1);
			_editLayoutPanel.Controls.Add(_cButton, 2, 1);

			foreach (ActionButton button in _editLayoutPanel.Controls)
			{
				button.Dock = DockStyle.Fill;
				button.Click += _actionButton_Click;
			}

			_valueDisplayTextBox = new NuGenTextBox(serviceProvider);
			_valueDisplayTextBox.Dock = DockStyle.Fill;
			_valueDisplayTextBox.ReadOnly = true;
			_valueDisplayTextBox.RightToLeft = RightToLeft.Yes;
			_valueDisplayTextBox.TabIndex = 10;
			_valueDisplayTextBox.Text = this.GetEngineValue();

			_editLayoutPanel.Controls.Add(_valueDisplayTextBox, 0, 0);
			_editLayoutPanel.SetColumnSpan(_valueDisplayTextBox, 3);

			_editLayoutPanel.Dock = DockStyle.Top;
			_editLayoutPanel.Height = 66;
			_editLayoutPanel.Parent = this;
			_editLayoutPanel.TabIndex = 10;

			_hotKeys = new NuGenHotKeys();

			this.AddOperation(this.D0, Keys.D0);
			this.AddOperation(this.D1, Keys.D1);
			this.AddOperation(this.D2, Keys.D2);
			this.AddOperation(this.D3, Keys.D3);
			this.AddOperation(this.D4, Keys.D4);
			this.AddOperation(this.D5, Keys.D5);
			this.AddOperation(this.D6, Keys.D6);
			this.AddOperation(this.D7, Keys.D7);
			this.AddOperation(this.D8, Keys.D8);
			this.AddOperation(this.D9, Keys.D9);
			
			this.AddOperation(this.Accept, Keys.Enter);
			this.AddOperation(this.Cancel, Keys.Escape);
		}

		private void AddOperation(NuGenHotKeyOperationHandler handler, Keys hotKeys)
		{
			_hotKeys.Operations.Add(new NuGenHotKeyOperation(hotKeys.ToString(), handler, hotKeys));
		}

		private void AddToRow0(ActionButton button, int column)
		{
			_actionButtonLayoutPanel.Controls.Add(button, column, 0);
		}

		private void AddToRow1(ActionButton button, int column)
		{
			_actionButtonLayoutPanel.Controls.Add(button, column, 1);
		}

		private void AddToRow2(ActionButton button, int column)
		{
			_actionButtonLayoutPanel.Controls.Add(button, column, 2);
		}

		private void AddToRow3(ActionButton button, int column)
		{
			_actionButtonLayoutPanel.Controls.Add(button, column, 3);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_engine != null)
				{
					_engine.ValueChanged -= _engine_ValueChanged;
					_engine = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
