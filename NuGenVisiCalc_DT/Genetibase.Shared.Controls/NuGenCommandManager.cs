/* -----------------------------------------------
 * NuGenCommandManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
    /// <summary>
    /// Handles commands associated with menus, toolbars, status bars and context menus.
    /// Supports .NET 1.x menus and toolbars etc.
    /// </summary>
    [ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenCommandManager), "Resources.NuGenIcon.png")]
    [NuGenSRDescription("Description_BasicCommandManager")]
	public partial class NuGenCommandManager : NuGenCommandManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManager"/> class. 
        /// </summary>
        public NuGenCommandManager()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenCommandManager"/> class. 
        /// </summary>
        /// <param name="container">Component container</param>
        public NuGenCommandManager(IContainer container)
            : base(container)
        {
            UIItemAdapter = new NuGenUIItemAdapter(this);
        }
    }
}
