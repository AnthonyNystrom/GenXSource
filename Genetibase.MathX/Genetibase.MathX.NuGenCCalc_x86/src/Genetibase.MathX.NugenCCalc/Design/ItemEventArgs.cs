using System;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Event argument carrying an item
	/// </summary>
	public delegate void ItemActionEventHandler (object sender, ItemEventArgs e);
	public class ItemEventArgs : EventArgs
	{
		#region Private Instance Fields

		private FunctionParameters _item;
		private string _action = "";

		#endregion


		#region Public Instance Constructors

		/// <summary>
		/// Initializes a new instance of the ItemEventArgs class 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="action"></param>
		public ItemEventArgs(FunctionParameters item, string action) : this (item)
		{
			this._action = action;
		}

		/// <summary>
		/// Initializes a new instance of the ItemEventArgs class 
		/// </summary>
		/// <param name="item"></param>
		public ItemEventArgs(FunctionParameters item) : base ()
		{
			this._item = item;
		}

		#endregion


		#region Public Instance Properties

		/// <summary>
		/// Get Item
		/// </summary>
		public FunctionParameters Item
		{
			get
			{
				return this._item;
			}
		}



		/// <summary>
		/// Get string action
		/// </summary>
		public string Action
		{
			get
			{
				return this._action;
			}
		}

		#endregion
	}
}
