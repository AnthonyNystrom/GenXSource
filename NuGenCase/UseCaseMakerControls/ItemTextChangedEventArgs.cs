using System;
using System.Windows.Forms;

namespace UseCaseMakerControls
{
	/// <summary>
	/// Descrizione di riepilogo per ItemTextChangedEventArgs.
	/// </summary>
	public class ItemTextChangedEventArgs
	{
		private LinkEnabledRTB item = null;
		public ItemTextChangedEventArgs(LinkEnabledRTB item)
		{
			this.item = item;
		}

		public LinkEnabledRTB Item
		{
			get
			{
				return this.item;
			}
		}
	}
}
