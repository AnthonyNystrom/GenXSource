/* -----------------------------------------------
 * NuGenCommandManager2Ex.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Handles commands associated with menus, toolbars, status bars and context menus.
    /// Supports .NET 2.0 ToolStrip based menus, toolbars etc. and our own toolstrip extended controls.
    /// </summary>
    [ToolboxItem(true)]
	[NuGenSRDescription("Description_CommandManagerEx")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenCommandManager2Ex : NuGenCommandManager2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManager2Ex"/> class. 
        /// </summary>
        public NuGenCommandManager2Ex()
            : this(null)
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommandManager2Ex"/> class. 
        /// </summary>
        /// <param name="container"></param>
        public NuGenCommandManager2Ex(IContainer container)
            : base(container)
        {
            UIItemAdapter = new NuGenUIItemAdapter2Ex(this);
        }
    }
}
