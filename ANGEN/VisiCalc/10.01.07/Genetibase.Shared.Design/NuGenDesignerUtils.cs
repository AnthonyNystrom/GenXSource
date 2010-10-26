/* -----------------------------------------------
 * NuGenDesignerUtils.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides helper methods for <see cref="T:System.Windows.Forms.Design.ComponentDesigner"/> inheritors.
	/// </summary>
	public static class NuGenDesignerUtils
	{
		/*
		 * GetTextBaseline
		 */

		/// <summary>
		/// Retrieves text baseline for the specified <see cref="T:System.Windows.Forms.Control"/> with the
		/// specified <see cref="T:System.Drawing.ContentAlignment"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="T:System.Windows.Forms.Control"/> to retrieve text
		/// baseline for.</param>
		/// <param name="alignment">Specifies text alignment on the <paramref name="ctrl"/>.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="ctrl"/> is <see langword="null"/>.</exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static int GetTextBaseline(Control ctrl, ContentAlignment alignment)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			Rectangle ctrlRect = ctrl.ClientRectangle;
			
			int ascent = 0;
			int height = 0;
			
			using (Graphics grfx = ctrl.CreateGraphics())
			{
				IntPtr hDC = grfx.GetHdc();
				IntPtr hFont = ctrl.Font.ToHfont();
				
				try
				{
					IntPtr oldFont = Gdi32.SelectObject(hDC, hFont);
					TEXTMETRIC textMetric = new TEXTMETRIC();

					Gdi32.GetTextMetrics(hDC, textMetric);
					
					ascent = textMetric.tmAscent + 1;
					height = textMetric.tmHeight;
					
					Gdi32.SelectObject(hDC, oldFont);
				}
				finally
				{
					Gdi32.DeleteObject(hFont);
					grfx.ReleaseHdc(hDC);
				}
			}
			
			if ((alignment & NuGenControlPaint.AnyTop) != 0)
			{
				return (ctrlRect.Top + ascent);
			}
			
			if ((alignment & NuGenControlPaint.AnyMiddle) != 0)
			{
				return (((ctrlRect.Top + (ctrlRect.Height / 2)) - (height / 2)) + ascent);
			}
			
			return ((ctrlRect.Bottom - height) + ascent);
		}

		/*
		 * GetHostForm
		 */

		/// <summary>
		/// Retrieves the host form for a component.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="designerHost"/> is <see langword="null"/>.</para>
		/// </exception>
		public static Form GetHostForm(IDesignerHost designerHost)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}

			return (Form)designerHost.RootComponent;
		}
	}
}
