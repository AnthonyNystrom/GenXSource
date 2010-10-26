/* -----------------------------------------------
 * SlideShowForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenPresenter.Properties;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Genetibase.NuGenPresenter
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal sealed class SlideShowForm : FullScreenForm
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_closeTimer.Start();
			_slideTimer.Start();
		}

		private Image _currentSlide;

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;
			bool stretch = Settings.Default.StretchSlide;

			if (_currentSlide != null)
			{
				if (stretch)
				{
					g.DrawImage(_currentSlide, bounds);
				}
				else
				{
					bool random = Settings.Default.RandomSlideAlignment;
					Size initialImageSize;

					if (Settings.Default.SlideFitScreen)
					{
						initialImageSize = NuGenControlPaint.ScaleToFit(this.ClientRectangle, _currentSlide.Size).Size;
					}
					else
					{
						initialImageSize = _currentSlide.Size;
					}


					ContentAlignment align;

					if (random)
					{
						align = (ContentAlignment)_random.Next(0, 8);
					}
					else
					{
						align = Settings.Default.SlideAlignment;
					}

					Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(
						initialImageSize, bounds, align
					);

					g.DrawImage(_currentSlide, imageBounds);
				}
			}
		}

		private Image GetNextImage()
		{
			string path;
			int counter = _slideCounter;
			
			do
			{
				if (counter < _pathCollection.Count)
				{
					path = _pathCollection[counter++];
				}
				else
				{
					return null;
				}
			} while (!File.Exists(path));

			_slideCounter = _pathCollection.IndexOf(path) + 1;

			if (_slideCounter == _pathCollection.Count)
			{
				_slideCounter = 0;
			}

			return Image.FromFile(path);
		}

		private void PopulatePathCollection(FileInfo[] fileInfoCollection)
		{
			Debug.Assert(fileInfoCollection != null, "fileInfoCollection != null");

			foreach (FileInfo fi in fileInfoCollection)
			{
				_pathCollection.Add(fi.FullName);
			}
		}

		private void _closeTimer_Tick(object sender, EventArgs e)
		{
			if (++_minutes >= Settings.Default.Interval)
			{
				_closeTimer.Stop();
				this.Close();
			}
		}

		private int _slideCounter;

		private void _slideTimer_Tick(object sender, EventArgs e)
		{
			_currentSlide = this.GetNextImage();
			this.Invalidate();
		}

		private Timer _closeTimer;
		private Timer _slideTimer;
		private int _minutes;
		private StringCollection _pathCollection;
		private Random _random;

		public SlideShowForm()
		{
			_pathCollection = new StringCollection();
			_random = new Random();

			if (Directory.Exists(Settings.Default.SelectedSlidePath))
			{
				DirectoryInfo dirInfo = new DirectoryInfo(Settings.Default.SelectedSlidePath);

				this.PopulatePathCollection(dirInfo.GetFiles("*.gif"));
				this.PopulatePathCollection(dirInfo.GetFiles("*.jpg"));
				this.PopulatePathCollection(dirInfo.GetFiles("*.jpeg"));
				this.PopulatePathCollection(dirInfo.GetFiles("*.png"));
				this.PopulatePathCollection(dirInfo.GetFiles("*.bmp"));
				this.PopulatePathCollection(dirInfo.GetFiles("*.tiff"));
			}

			_closeTimer = new Timer();
			_closeTimer.Interval = 60000;
			_closeTimer.Tick += _closeTimer_Tick;

			_slideTimer = new Timer();
			_slideTimer.Interval = Math.Max(1, Math.Min(999, Settings.Default.SlideLatency)) * 1000;
			_slideTimer.Tick += _slideTimer_Tick;

			this.BackColor = Settings.Default.SlideBackgroundColor;

			_currentSlide = this.GetNextImage();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_closeTimer != null)
				{
					_closeTimer.Tick -= _closeTimer_Tick;
					_closeTimer.Dispose();
					_closeTimer = null;
				}

				if (_slideTimer != null)
				{
					_slideTimer.Tick -= _slideTimer_Tick;
					_slideTimer.Dispose();
					_slideTimer = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
