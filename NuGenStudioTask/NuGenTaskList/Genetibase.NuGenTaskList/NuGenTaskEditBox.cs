/* -----------------------------------------------
 * NuGenTaskEditBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents a <see cref="T:TextBox"/> to edit a task.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTaskEditBox : TextBox, INuGenDEHClient
	{
		#region INuGenDEHClient Members

		private static readonly object eventToBeDelayed = new object();

		public event NuGenDEHEventHandler EventToBeDelayed
		{
			add
			{
				this.Events.AddHandler(eventToBeDelayed, value);
			}
			remove
			{
				this.Events.RemoveHandler(eventToBeDelayed, value);
			}
		}

		/// <summary>
		/// Bubbles <see cref="E:EventToBeDelayed"/> event.
		/// </summary>
		protected virtual void OnEventToBeDelayed(INuGenDEHEventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventToBeDelayed(eventToBeDelayed, e);
		}

		/// <summary>
		/// </summary>
		public void HandleDelayedEvent(object sender, INuGenDEHEventArgs e)
		{
			if (e is NuGenTaskAddedEventArgs)
			{
				this.TaskAdded((NuGenTaskAddedEventArgs)e);
			}
			else if (e is NuGenSelectedTaskChangedEventArgs)
			{
				this.SelectedTaskChanged((NuGenSelectedTaskChangedEventArgs)e);
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * Initiator
		 */

		private NuGenDEHEventInitiator initiator = null;

		protected virtual NuGenDEHEventInitiator Initiator
		{
			get
			{
				if (this.initiator == null)
				{
					this.initiator = new NuGenDEHEventInitiator(this, this.Events);
				}

				return this.initiator;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenTaskEditBox"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 100);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:TextChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			this.OnEventToBeDelayed(new NuGenSelectedTaskChangedEventArgs(this.Text));
		}

		#endregion

		#region DelayedEventHandlers

		private void TaskAdded(NuGenTaskAddedEventArgs e)
		{
			this.SelectedTaskChanged(e);

			if (this.IsHandleCreated)
			{
				this.BeginInvoke(
					new MethodInvoker(
						delegate
						{
							this.Focus();
							this.SelectAll();
						}
					)
				);
			}
		}

		private void SelectedTaskChanged(NuGenSelectedTaskChangedEventArgs e)
		{
			if (this.IsHandleCreated)
			{
				this.BeginInvoke(
					new MethodInvoker(
						delegate
						{
							this.Enabled = !e.IsTaskTextReadonly;
							this.Text = e.TaskText != null ? e.TaskText : "";
						}
					)
				);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskEditBox"/> class.
		/// </summary>
		public NuGenTaskEditBox()
		{
			this.Multiline = true;

			Color previousBackColor = this.BackColor;
			this.Enabled = false;
			this.BackColor = previousBackColor;
			
			this.ScrollBars = ScrollBars.Vertical;
			this.WordWrap = true;
		}

		#endregion
	}
}
