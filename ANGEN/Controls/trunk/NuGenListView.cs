/* -----------------------------------------------
 * NuGenListView.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Extended <see cref="T:ListView"/>.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	public class NuGenListView : ListView
	{
		#region Properties.Appearance

		/*
		 * EmptyListViewMessage
		 */

		/// <summary>
		/// Determines the message that appears when this <see cref="T:NuGenListView"/> contains no items.
		/// </summary>
		private string emptyListViewMessage = "";

		/// <summary>
		/// Gets or sets the message that appears when this <see cref="T:NuGenListView"/> contains no items.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Description("Determines the message that appears when this list view contains no items.")]
		public string EmptyListViewMessage
		{
			get 
			{
				if (this.emptyListViewMessage == null)
				{
					this.emptyListViewMessage = "";
				}

				return this.emptyListViewMessage;
			}
			set
			{
				if (this.emptyListViewMessage != value)
				{
					this.emptyListViewMessage = value;
					this.OnEmptyListViewMessageChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenListView.EmptyListViewMessageChanged"/> event identifier.
		/// </summary>
		private static readonly object EventEmptyListViewMessageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenListView.EmptyListViewMessage"/> property changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the value of the EmptyListViewMessage property changes.")]
		public event EventHandler EmptyListViewMessageChanged
		{
			add { this.Events.AddHandler(EventEmptyListViewMessageChanged, value); }
			remove { this.Events.RemoveHandler(EventEmptyListViewMessageChanged, value); }
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenListView.EmptyListViewMessageChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnEmptyListViewMessageChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[EventEmptyListViewMessageChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">Specifies the message to process.</param>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch (m.Msg)
			{
				case WinUser.WM_PAINT:
					if (this.Items.Count == 0 && this.Columns.Count > 0) 
					{
						using (Graphics g = Graphics.FromHwnd(this.Handle)) 
						{
							using (SolidBrush sb = new SolidBrush(this.BackColor))
							{
								g.FillRectangle(sb, this.ClientRectangle);
							}

							using (SolidBrush sb = new SolidBrush(this.ForeColor)) 
							using (StringFormat sf = new StringFormat()) 
							{
								sf.Alignment = StringAlignment.Center;
								sf.LineAlignment = StringAlignment.Center;

								g.DrawString(
									this.EmptyListViewMessage,
									this.Font,
									sb,
									this.ClientRectangle,
									sf
									);
							}
						}
					}
					break;
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenListView"/> class.
		/// </summary>
		public NuGenListView()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}
		
		#endregion
	}
}
