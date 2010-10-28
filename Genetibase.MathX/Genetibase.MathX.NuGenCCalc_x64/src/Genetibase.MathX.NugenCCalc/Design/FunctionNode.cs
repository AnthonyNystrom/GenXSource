using System;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Summary description for FunctionNode.
	/// </summary>
	public class FunctionNode : TreeNode
	{

        #region Private Instance Fields

		private FunctionParameters _item;

        #endregion Private Instance Fields


        #region Private Instance Methods

		private void BindEvents( )
		{
			this._item.Changed+=new ParametersChangeHandler(_item_Changed);
		}

		private void _item_Changed(FunctionParameters parameters, EventArgs e)
		{
			base.Text = parameters.ToString();
		}

        #endregion Private Instance Methods


        #region Public Instance Constructors


		public FunctionNode(FunctionParameters item) : base(item.ToString())
		{
			_item = item;
			//BindEvents();
		}

        #endregion Public Instance Constructors


        #region Public Instance Properties

		public FunctionParameters Item
		{
			get
			{
				return _item;
			}
		}

		#endregion Public Instance Properties


	}
}
