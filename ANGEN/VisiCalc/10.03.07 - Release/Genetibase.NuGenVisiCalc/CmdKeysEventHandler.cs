/* -----------------------------------------------
 * CmdKeysEventHandler.cs
 * Copyright � 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.NuGenVisiCalc
{
	internal delegate bool CmdKeysEventHandler(Object sender, ref Message msg, Keys keyData);
}
