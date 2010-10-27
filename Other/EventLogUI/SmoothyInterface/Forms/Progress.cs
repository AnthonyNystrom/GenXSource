using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SmoothyInterface.Forms
{
	public partial class Progress : Form
	{
		#region Globals
				
		int count = 0;		
				
		#endregion

		public Progress()
		{
			InitializeComponent();		
		}
				
		public void IncrementValue()
		{
			lock (this)
			{
				count++;

				if (this.InvokeRequired)
				{
					this.BeginInvoke(new MethodInvoker(UpdateInfo));
				}
				else
				{
					UpdateInfo();
				}
			}
		}

		private void UpdateInfo()
		{
			lblLoadedCount.Text = count.ToString();
		}		
	}
}