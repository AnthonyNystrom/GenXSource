/* -----------------------------------------------
 * BreakTimerForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenPresenter.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Genetibase.ApplicationBlocks;
using Genetibase.Shared.Timers;
using Genetibase.Shared.Drawing;

namespace Genetibase.NuGenPresenter
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal sealed class BreakTimerForm : FullScreenForm
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_countDownBlock.Start();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			using (SolidBrush sb = new SolidBrush(Settings.Default.PenColor))
			using (StringFormat sf = NuGenControlPaint.ContentAlignmentToStringFormat(ContentAlignment.MiddleCenter))
			{
				g.DrawString(
					_countDownBlock.CurrentSpan.ToString()
					, this.Font
					, sb
					, this.ClientRectangle
					, sf
				);
			}
		}

		private void _countDownBlock_Tick(object sender, EventArgs e)
		{
			this.Invalidate();
		}

		private NuGenCountDownBlock _countDownBlock;

		public BreakTimerForm()
		{
			this.BackColor = SystemColors.Window;
			this.Font = new Font("Verdana", 24, FontStyle.Bold);

			INuGenTimer timer = new NuGenTimer();
			timer.Interval = 1000;

			_countDownBlock = new NuGenCountDownBlock(
				new NuGenCountDownSpan(Math.Max(1, Math.Min(99, Settings.Default.Interval)), 0)
				, timer
			);

			_countDownBlock.Tick += _countDownBlock_Tick;
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"></see>.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Cursor.Show();

				if (_countDownBlock != null)
				{
					_countDownBlock.Tick -= _countDownBlock_Tick;
					_countDownBlock.Dispose();
					_countDownBlock = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
