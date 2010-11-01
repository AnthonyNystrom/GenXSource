/* -----------------------------------------------
 * NuGenDesignerUtils.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Design
{
	/// <summary>
	/// Provides helper methods for <see cref="T:System.Windows.Forms.Design.ComponentDesigner"/> inheritors.
	/// </summary>
	public static class NuGenDesignerUtils
	{
		#region Properties.Public.Static

		/*
		 * AnyMiddleAlignment
		 */

		/// <summary>
		/// ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft
		/// </summary>
		public static ContentAlignment AnyMiddleAlignment
		{
			get
			{
				return 0
					| ContentAlignment.MiddleRight
					| ContentAlignment.MiddleCenter
					| ContentAlignment.MiddleLeft
					;
			}
		}
		
		/*
		 * AnyTopAlignment
		 */

		/// <summary>
		/// ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft
		/// </summary>
		public static ContentAlignment AnyTopAlignment
		{
			get
			{
				return 0
					| ContentAlignment.TopRight
					| ContentAlignment.TopCenter
					| ContentAlignment.TopLeft
					;
			}
		}

		#endregion

		#region Methods.Public.Static

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
			
			if ((alignment & NuGenDesignerUtils.AnyTopAlignment) != ((ContentAlignment)0))
			{
				return (ctrlRect.Top + ascent);
			}
			
			if ((alignment & NuGenDesignerUtils.AnyMiddleAlignment) != ((ContentAlignment)0))
			{
				return (((ctrlRect.Top + (ctrlRect.Height / 2)) - (height / 2)) + ascent);
			}
			
			return ((ctrlRect.Bottom - height) + ascent);
		}

		#endregion
	}
}
