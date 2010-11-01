/* -----------------------------------------------
 * NuGenXhtmlTextBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Service;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Enhanced <see cref="T:RichTextBox"/> that supports XHTML rendering.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	public class NuGenXhtmlTextBox : RichTextBox
	{
		#region Properties.NonBrowsable

		/// <summary>
		/// Determines the list of errors encountered during XHTML to RTF translation.
		/// </summary>
		private ArrayList errors = null;

		/// <summary>
		/// Gets the list of errors encountered during XHTML to RTF translation.
		/// </summary>
		[Browsable(false)]
		public ArrayList Errors
		{
			get
			{
				if (this.errors == null) 
				{
					this.errors = new ArrayList();
				}

				return errors;
			}
		}

		#endregion

		#region Properties.Public.Overriden

		/// <summary>
		/// Determines the text for this <see cref="T:NuGenXhtmlTextBox"/>.
		/// </summary>
		private string text = "";

		/// <summary>
		/// Gets or sets the text for this <see cref="T:NuGenXhtmlTextBox"/>.
		/// </summary>
		public override string Text
		{
			get { return this.text; }
			set
			{
				if (value == null)
				{
					value = "";
				}

				if (this.Text != value) 
				{
					this.text = value;
					this.OnTextChanged(EventArgs.Empty);

					NuGenXmlTranslator xmlTranslator = new NuGenXmlTranslator(value);
					this.Rtf = xmlTranslator.ToRtfDocument().ToString();
					this.errors = xmlTranslator.Errors;
				}
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.Refresh();
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenXhtmlTextBox"/> class.
		/// </summary>
		public NuGenXhtmlTextBox()
		{
			this.ReadOnly = true;
		}
		
		#endregion
	}
}
