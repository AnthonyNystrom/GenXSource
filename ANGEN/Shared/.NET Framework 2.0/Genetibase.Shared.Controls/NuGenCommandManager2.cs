/* -----------------------------------------------
 * NuGenCommandManager2.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Handles commands associated with menus, toolbars, status bars and context menus.
    /// Supports .NET 2.0 Toolstrip based menus and toolbars etc.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(NuGenCommandManager2), "Resources.NuGenIcon.png")]
	[NuGenSRDescription("Description_BasicCommandManager2")]
	public partial class NuGenCommandManager2 : NuGenCommandManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManager2"/> class. 
        /// </summary>
        public NuGenCommandManager2()
            : this(null)
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommandManager2"/> class. 
        /// </summary>
        public NuGenCommandManager2(IContainer container)
            : base(container)
        {
            UIItemAdapter = new NuGenUIItemAdapter2(this);
        }
    }
}
