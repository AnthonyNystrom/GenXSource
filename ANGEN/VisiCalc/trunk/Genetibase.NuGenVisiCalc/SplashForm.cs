/* -----------------------------------------------
 * SplashForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed partial class SplashForm : Form
	{
		public SplashForm()
		{
			InitializeComponent();
			SetStyle(ControlStyles.Opaque, true);
		}
	}
}
