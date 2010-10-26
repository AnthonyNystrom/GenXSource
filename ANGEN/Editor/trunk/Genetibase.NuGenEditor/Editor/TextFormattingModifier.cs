/* -----------------------------------------------
 * TextFormattingModifier.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.TextFormatting;
using System.Windows;

namespace Genetibase.Windows.Controls.Editor
{
	internal class TextFormattingModifier : TextModifier
	{
		private TextRunProperties _properties;

		public TextFormattingModifier(TextRunProperties properties)
		{
			_properties = properties;
		}

		public override TextRunProperties ModifyProperties(TextRunProperties properties)
		{
			return _properties;
		}

		public override FlowDirection FlowDirection
		{
			get
			{
				return FlowDirection.LeftToRight;
			}
		}

		public override Boolean HasDirectionalEmbedding
		{
			get
			{
				return true;
			}
		}

		public override Int32 Length
		{
			get
			{
				return 1;
			}
		}

		public override TextRunProperties Properties
		{
			get
			{
				return _properties;
			}
		}
	}
}
