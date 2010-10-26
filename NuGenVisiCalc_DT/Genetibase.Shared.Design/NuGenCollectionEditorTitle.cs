/* -----------------------------------------------
 * NuGenCollectionEditorTitle.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Design
{
	internal class NuGenCollectionEditorTitle : NuGenLabel
	{
		public NuGenCollectionEditorTitle()
		{
			this.AutoSize = false;
			this.Height = 24;
			this.TextAlign = ContentAlignment.TopLeft;
		}
	}
}
