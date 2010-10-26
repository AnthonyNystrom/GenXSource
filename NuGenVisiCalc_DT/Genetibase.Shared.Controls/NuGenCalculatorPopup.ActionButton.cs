/* -----------------------------------------------
 * NuGenCalculatorPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.CalculatorInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenCalculatorPopup
	{
		private sealed class ActionButton : NuGenButton
		{
			private IAction _buttonAction;

			public IAction ButtonAction
			{
				get
				{
					return _buttonAction;
				}
			}

			public ActionButton(INuGenServiceProvider serviceProvider, IAction buttonAction, string text)
				: base(serviceProvider)
			{
				if (buttonAction == null)
				{
					throw new ArgumentNullException("buttonAction");
				}

				_buttonAction = buttonAction;

				this.Text = text;
			}
		}
	}
}
