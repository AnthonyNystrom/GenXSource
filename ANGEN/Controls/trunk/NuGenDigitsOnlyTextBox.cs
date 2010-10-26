/* -----------------------------------------------
 * NuGenDigitsOnlyTextBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.WinApi;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Represents a text box that allows only digit input.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenDigitsOnlyTextBoxDesigner))]
	[ToolboxItem(true)]
	public class NuGenDigitsOnlyTextBox : TextBox
	{
		#region Declarations

		/// <summary>
		/// Contains the previous valid text.
		/// </summary>
		private string bufferText = "";

		/// <summary>
		/// Contains the cursor position for the previous valid text.
		/// </summary>
		private int bufferIndex = 0;

		/// <summary>
		/// Used to filter input.
		/// </summary>
		private const string PATTERN = @"^\d*$";

		#endregion

		#region Properties.Hidden

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
			}
		}

		/*
		 * Multiline
		 */

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// is a multiline text box control.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}
			set
			{
				return;
			}
		}

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenDigitsOnlyTextBox.Multiline"/> property changes.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler MultilineChanged
		{
			add { base.MultilineChanged += value; }
			remove { base.MultilineChanged -= value; }
		}

		/*
		 * ScrollBars
		 */

		/// <summary>
		/// Gets or sets which scroll bars should
		/// appear in a multiline <see cref="T:System.Windows.Forms.TextBox"/>
		/// control.
		/// </summary>
		/// <value></value>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A value that is not within the range of valid values for the enumeration was assigned to the property.</exception>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new object ScrollBars
		{
			get { return base.ScrollBars; }
			set { return; }
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The message to process.</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WinUser.WM_USER + 7441)
			{
				if (this.Text.Length > 0) 
				{
					if (Regex.IsMatch(this.Text, PATTERN))
					{
						this.bufferText = this.Text;
						
						POINT p;
						User32.GetCaretPos(out p);
						this.bufferIndex = (int)User32.SendMessage(
							this.Handle,
							WinUser.EM_CHARFROMPOS,
							IntPtr.Zero,
							Common.MakeLParam(p.x, p.y)
							);
					}
					else
					{
						this.Text = this.bufferText;
						this.SelectionStart = this.bufferIndex;
					}
				}
				else
				{
					this.bufferText = "";
					this.bufferIndex = 0;
				}
			}
			else 
			{
				base.WndProc(ref m);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDigitsOnlyTextBox"/> class.
		/// </summary>
		public NuGenDigitsOnlyTextBox()
		{
		}

		#endregion
	}
}
