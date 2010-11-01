using System;
using System.Windows.Forms;

namespace UseCaseMakerControls
{
	/// <summary>
	/// Descrizione di riepilogo per MouseOverTokenEventArgs.
	/// </summary>
	public class MouseOverTokenEventArgs : MouseEventArgs
	{
		private string token = string.Empty;
		private LinkEnabledRTB item = null;
		private bool overToken = false;
		
		public MouseOverTokenEventArgs(
			MouseButtons button,
			int clicks,
			int x,
			int y,
			int delta,
			LinkEnabledRTB item,
			string token,
			bool overToken)
			: base(button,clicks,x,y,delta)
		{
			this.item = item;
			this.token = token;
			this.overToken = overToken;
		}

		public string Token
		{
			get
			{
				return this.token;
			}
		}

		public LinkEnabledRTB Item
		{
			get
			{
				return this.item;
			}
		}

		public bool OverToken
		{
			get
			{
				return this.overToken;
			}
		}
	}
}
