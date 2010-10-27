/* ------------------------------------------------
 * BorderContainer.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using Next2Friends.CrossPoster.Client.Controls.Design;

namespace Next2Friends.CrossPoster.Client.Controls
{
    [Designer(typeof(BorderContainerDesigner))]
    class BorderContainer : TextBox
    {
        /// <summary>
        /// Creates a new instance of the <code>BorderContainer</code> class.
        /// </summary>
        public BorderContainer()
        {
            Multiline = true;
        }

        [DefaultValue(true)]
        public override Boolean Multiline
        {
            get
            {
                return base.Multiline;
            }
            set
            {
                base.Multiline = value;
            }
        }
    }
}
