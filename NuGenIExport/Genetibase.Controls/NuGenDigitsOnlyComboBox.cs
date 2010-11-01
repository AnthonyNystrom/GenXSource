/* -----------------------------------------------
 * NuGenDigitsOnlyComboBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Always returns a string in the appropriate digit format.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenDigitsOnlyComboBoxDesigner))]
	public class NuGenDigitsOnlyComboBox : ComboBox
	{
		#region Declarations

		/// <summary>
		/// Used to verify user input.
		/// </summary>
		private const string PATTERN = @"^\d+$";

		/// <summary>
		/// Contains the last appropriate text.
		/// </summary>
		private string bufferString = "1";

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// Gets the digit value for this <see cref="T:NuGenDigitsOnlyComboBox"/>.
		/// </summary>
		[Browsable(false)]
		public int DigitValue
		{
			get 
			{
				this.Text = this.bufferString;
				return int.Parse(this.bufferString);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			if (Regex.IsMatch(this.Text, PATTERN))
			{
				this.bufferString = this.Text;
			}

			base.OnTextChanged(e);
		}

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDigitsOnlyComboBox"/> class.
		/// </summary>
		public NuGenDigitsOnlyComboBox()
		{
		}

		#endregion
	}
}
