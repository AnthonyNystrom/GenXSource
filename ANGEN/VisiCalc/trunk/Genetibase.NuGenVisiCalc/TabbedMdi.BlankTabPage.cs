/* -----------------------------------------------
 * TabbedMdi.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared.Controls;
using Genetibase.SmoothControls;

namespace Genetibase.NuGenVisiCalc
{
    partial class TabbedMdi
    {
        private sealed class BlankTabPage : NuGenSmoothTabPage
        {
            public BlankTabPage()
            {
                TabButtonImage = Resources.Blank;
                Text = Resources.Text_Blank;
            }
        }
    }
}
