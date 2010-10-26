/* -----------------------------------------------
 * NuGenPresenterForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ComponentModel;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.PresenterInternals
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal sealed partial class NuGenPresenterForm : Form
	{
		#region Properties.Public

		private Point _cursorPosition;

		public Point CursorPosition
		{
			get
			{
				return _cursorPosition;
			}
			set
			{
				_cursorPosition = value;

				if (!_isTransformFrozen && !_isVanishingOnTimer)
				{
					this.GetDesiredTransformFromCursorPosition();
				}
			}
		}

		public Color PenColor
		{
			get
			{
				return _sketchCanvas.PenColor;
			}
			set
			{
				_sketchCanvas.PenColor = value;
			}
		}

		public float PenWidth
		{
			get
			{
				return _sketchCanvas.PenWidth;
			}
			set
			{
				_sketchCanvas.PenWidth = value;
			}
		}

		private bool _zoomActive;

		public bool ZoomActive
		{
			get
			{
				return _zoomActive;
			}
			set
			{
				_zoomActive = value;

				if (value)
				{
					this.CaptureScreenImage();

					_currentScale = 1;
					_desiredScale = _desiredScaleCurrent;
					_currentTransform.SetValues(this.CursorPosition.X, this.CursorPosition.Y);
					_desiredTransform.SetValues(this.CursorPosition.X, this.CursorPosition.Y);
					this.IsTransformFrozen = false;
					this.Visible = true;
				}
				else
				{
					_timer.Stop();
					this.Visible = false;
				}
			}
		}

		private float _zoomDepth = -1;

		public float ZoomDepth
		{
			get
			{
				if (_zoomDepth < 0)
				{
					return 2;
				}

				return _zoomDepth;
			}
			set
			{
				_zoomDepth = value;
			}
		}

		private float _zoomSpeed = -1;

		public float ZoomSpeed
		{
			get
			{
				if (_zoomSpeed < 0)
				{
					return 0.65f;
				}

				return _zoomSpeed;
			}
			set
			{
				_zoomSpeed = value;
			}
		}

		#endregion

		#region Properties.Layout

		private bool _isTransformFrozen;

		private bool IsTransformFrozen
		{
			get
			{
				return _isTransformFrozen;
			}
			set
			{
				_isTransformFrozen = value;

				if (_isTransformFrozen)
				{
					_timer.Stop();
				}
				else
				{
					_timer.Start();
				}
			}
		}

		private int PrimaryScreenHeight
		{
			get
			{
				return Screen.PrimaryScreen.Bounds.Height;
			}
		}

		private int PrimaryScreenWidth
		{
			get
			{
				return Screen.PrimaryScreen.Bounds.Width;
			}
		}

		#endregion

		#region Properties.Logic

		private bool CanStartTimer
		{
			get
			{
				return !_timer.Enabled && !_isTransformFrozen;
			}
		}

		#endregion

		#region Properties.Services

		private INuGenServiceProvider _serviceProvider;

		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		private INuGenTempImageService _tempImageService;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		private INuGenTempImageService TempImageService
		{
			get
			{
				if (_tempImageService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_tempImageService = this.ServiceProvider.GetService<INuGenTempImageService>();

					if (_tempImageService == null)
					{
						throw new NuGenServiceNotFoundException<NuGenTempImageService>();
					}
				}

				return _tempImageService;
			}
		}

		#endregion

		#region Methods.Public

		public void BeginNewZoomIn(bool shouldZoomIn)
		{
			_isVanishingOnTimer = false;
			_desiredScaleCurrent = shouldZoomIn ? this.ZoomDepth : 1;
			this.ZoomActive = true;
		}

		#endregion

		#region Methods.Protected.Overridden

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_hotKeys.Process(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

			if (this.ZoomActive)
			{
				this.StartVanishingOnTimer();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			this.IsTransformFrozen = true;

			if (e.Button == MouseButtons.Right)
			{
				this.StartVanishingOnTimer();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.StartTimer();
			this.CursorPosition = e.Location;
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			if (e.Delta > 0)
			{
				this.ZoomIn();
			}
			else
			{
				this.ZoomOut();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (_zoomImage != null)
			{
				g.ScaleTransform(_scaleTransform.X, _scaleTransform.Y);
				g.TranslateTransform(_translateTransform.X, _translateTransform.Y);
				Transform offsetTransform = this.GetMaxTransform();
				g.TranslateTransform(-offsetTransform.X, -offsetTransform.Y);
				g.DrawImageUnscaled(_zoomImage, Point.Empty);
			}
		}

		#endregion

		#region Methods.Private

		private void CaptureScreenImage()
		{
			_spotGrab.SpotSize = new Size(this.PrimaryScreenWidth, this.PrimaryScreenHeight);
			_spotGrab.CursorPosition = new Point(this.PrimaryScreenWidth / 2, this.PrimaryScreenHeight / 2);
			_spotGrab.Capture();
			_zoomImage = _spotGrab.CurrentSpotBmp;
		}

		private void GetDesiredTransformFromCursorPosition()
		{
			_desiredTransform.SetValues(this.CursorPosition.X, this.CursorPosition.Y);
		}

		private Transform GetMaxTransform()
		{
			return new Transform(
				this.PrimaryScreenWidth * 0.5f * (_currentScale - 1) / _currentScale,
				this.PrimaryScreenHeight * 0.5f * (_currentScale - 1) / _currentScale
			);
		}

		private void StartVanishingOnTimer()
		{
			_sketchCanvas.Clear();
			this.IsTransformFrozen = false;
			_isVanishingOnTimer = true;
			_desiredScale = 1;
		}

		private void StartTimer()
		{
			if (this.CanStartTimer)
			{
				_timer.Start();
			}
		}

		private void UpdateTransform()
		{
			Transform transform = new Transform(
				this.PrimaryScreenWidth / 2 - _currentTransform.X,
				this.PrimaryScreenHeight / 2 - _currentTransform.Y
			);
			Transform maxTransform = this.GetMaxTransform();

			if (transform.X > maxTransform.X)
				transform.X = maxTransform.X;
			if (transform.X < -maxTransform.X)
				transform.X = -maxTransform.X;
			if (transform.Y > maxTransform.Y)
				transform.Y = maxTransform.Y;
			if (transform.Y < -maxTransform.Y)
				transform.Y = -maxTransform.Y;

			_translateTransform = transform;
			_scaleTransform.SetValues(_currentScale, _currentScale);

			this.Invalidate();
		}

		private void ZoomIn()
		{
			this.StartTimer();
			_desiredScale /= 0.75f;

			if (_desiredScale > 8)
				_desiredScale = 8;
		}

		private void ZoomOut()
		{
			this.StartTimer();
			_desiredScale *= 0.75f;

			if (_desiredScale < 1)
				_desiredScale = 1;
		}

		#endregion

		#region EventHandlers.Timer

		private void _timer_Tick(object sender, EventArgs e)
		{
			float zoomRate = this.ZoomSpeed;
			float zoomRateOnVanish = zoomRate * 0.9375f;
			float panRate = 0.6f;
			float currentZoomRate = _isVanishingOnTimer ? zoomRateOnVanish : zoomRate;

			if (
				(Math.Abs(_currentTransform.X - _desiredTransform.X) < 0.01
				&& Math.Abs(_currentTransform.Y - _desiredTransform.Y) < 0.01
				&& Math.Abs(_currentScale - _desiredScale) < 0.01)
				|| _isTransformFrozen
				)
			{
				_timer.Stop();
			}
			else if (!_isTransformFrozen)
			{
				_currentScale = _currentScale * currentZoomRate + _desiredScale * (1 - currentZoomRate);
				_currentTransform.X = _currentTransform.X * panRate + _desiredTransform.X * (1 - panRate);
				_currentTransform.Y = _currentTransform.Y * panRate + _desiredTransform.Y * (1 - panRate);
			}

			this.UpdateTransform();

			if (_isVanishingOnTimer && _currentScale < 1.01)
			{
				this.ZoomActive = false;
			}
		}

		#endregion

		#region Operations

		/* Clear */

		private NuGenHotKeyOperation _clearOperation;

		public NuGenHotKeyOperation ClearOperation
		{
			get
			{
				if (_clearOperation == null)
				{
					_clearOperation = new NuGenHotKeyOperation("Clear", this.ClearHandler, Keys.C);
				}

				return _clearOperation;
			}
		}

		private void ClearHandler()
		{
			_sketchCanvas.Clear();
		}

		/* Escape */

		private NuGenHotKeyOperation _escapeOperation;

		public NuGenHotKeyOperation EscapeOperation
		{
			get
			{
				if (_escapeOperation == null)
				{
					_escapeOperation = new NuGenHotKeyOperation("Escape", this.EscapeHandler, Keys.Escape);
				}

				return _escapeOperation;
			}
		}

		private void EscapeHandler()
		{
			this.StartVanishingOnTimer();
		}

		/* Save */

		private NuGenHotKeyOperation _saveOperation;

		public NuGenHotKeyOperation SaveOperation
		{
			get
			{
				if (_saveOperation == null)
				{
					_saveOperation = new NuGenHotKeyOperation("Save", this.SaveHandler, Keys.S);
				}

				return _saveOperation;
			}
		}

		private void SaveHandler()
		{
			this.TempImageService.SaveTempImage(_sketchCanvas.GetSketch());
		}

		/* LockTransform */

		private NuGenHotKeyOperation _lockTransform;

		public NuGenHotKeyOperation LockTransformOperation
		{
			get
			{
				if (_lockTransform == null)
				{
					_lockTransform = new NuGenHotKeyOperation("LockTransform", this.LockTransformHandler, Keys.L);
				}

				return _lockTransform;
			}
		}

		private void LockTransformHandler()
		{
			this.IsTransformFrozen = false;
			_sketchCanvas.Clear();
		}

		/* ShowPointer */

		private NuGenHotKeyOperation _showPointerOperation;

		public NuGenHotKeyOperation ShowPointerOperation
		{
			get
			{
				if (_showPointerOperation == null)
				{
					_showPointerOperation = new NuGenHotKeyOperation("ShowPointer", this.ShowPointerHandler, Keys.S);
				}

				return _showPointerOperation;
			}
		}

		private void ShowPointerHandler()
		{
			_sketchCanvas.ShowPointer();
		}

		/* ZoomIn */

		private NuGenHotKeyOperation _zoomInOperation;

		public NuGenHotKeyOperation ZoomInOperation
		{
			get
			{
				if (_zoomInOperation == null)
				{
					_zoomInOperation = new NuGenHotKeyOperation("ZoomIn", this.ZoomInHandler, Keys.Up);
				}

				return _zoomInOperation;
			}
		}

		private void ZoomInHandler()
		{
			this.ZoomIn();
		}

		/* ZoomOut */

		private NuGenHotKeyOperation _zoomOutOperation;

		public NuGenHotKeyOperation ZoomOutOperation
		{
			get
			{
				if (_zoomOutOperation == null)
				{
					_zoomOutOperation = new NuGenHotKeyOperation("ZoomOut", this.ZoomOutHandler, Keys.Down);
				}

				return _zoomOutOperation;
			}
		}

		private void ZoomOutHandler()
		{
			this.ZoomOut();
		}

		#endregion

		private Timer _timer;
		private NuGenSpotGrab _spotGrab;
		private NuGenSketchCanvas _sketchCanvas;
		private NuGenHotKeys _hotKeys;
		private Image _zoomImage;
		private bool _isVanishingOnTimer;
		private float _currentScale;
		private float _desiredScale = 1;
		private float _desiredScaleCurrent = 2;
		private Transform _currentTransform = new Transform(0, 0);
		private Transform _desiredTransform = new Transform(0, 0);
		private Transform _scaleTransform = new Transform(1, 1);
		private Transform _translateTransform = new Transform(0, 0);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPresenterForm"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		///	<para><see cref="INuGenButtonLayoutManager"/></para>
		///	<para><see cref="INuGenButtonRenderer"/></para>
		/// <para><see cref="INuGenComboBoxRenderer"/></para>
		/// <para><see cref="INuGenDirectorySelectorRenderer"/></para>
		/// <para><see cref="INuGenImageListService"/></para>
		/// <para><see cref="INuGenPanelRenderer"/></para>
		/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
		/// <para><see cref="INuGenProgressBarRenderer"/></para>
		/// <para><see cref="INuGenScrollBarRenderer"/></para>
		/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// <para><see cref="INuGenTempImageService"/></para>
		/// <para><see cref="INuGenTextBoxRenderer"/></para>
		/// <para><see cref="INuGenTrackBarRenderer"/></para>
		/// <para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// <para><see cref="INuGenThumbnailRenderer"/></para>
		/// <para><see cref="INuGenToolStripRenderer"/></para>
		/// <para><see cref="INuGenValueTrackerService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPresenterForm(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
			_hotKeys = new NuGenHotKeys();

			_hotKeys.Operations.Add(this.ClearOperation);
			_hotKeys.Operations.Add(this.EscapeOperation);
			_hotKeys.Operations.Add(this.SaveOperation);
			_hotKeys.Operations.Add(this.LockTransformOperation);
			_hotKeys.Operations.Add(this.ShowPointerOperation);
			_hotKeys.Operations.Add(this.ZoomInOperation);
			_hotKeys.Operations.Add(this.ZoomOutOperation);

			Point centerScreen = new Point(this.PrimaryScreenWidth / 2, this.PrimaryScreenHeight / 2);
			_spotGrab = new NuGenSpotGrab(centerScreen, this.PrimaryScreenWidth, this.PrimaryScreenHeight);

			_timer = new Timer();
			_timer.Interval = 50;
			_timer.Tick += _timer_Tick;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.WindowState = FormWindowState.Maximized;

			_sketchCanvas = new NuGenSketchCanvas(this.Handle, serviceProvider);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_zoomImage != null)
				{
					_zoomImage.Dispose();
				}

				if (_sketchCanvas != null)
				{
					_sketchCanvas.Dispose();
				}
			}

			base.Dispose(disposing);
		}
	}
}
