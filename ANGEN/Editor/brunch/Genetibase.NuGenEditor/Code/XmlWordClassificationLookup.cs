/* -----------------------------------------------
 * XmlWordClassificationLookup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using Genetibase.Windows.Controls.Code.Documents;
using Genetibase.Windows.Controls.Editor;
using Genetibase.Windows.Controls.Editor.Text.Classification;

namespace Genetibase.Windows.Controls.Code
{
	internal class XmlWordClassificationLookup : IClassificationFormatMap
	{
		private static TextFormattingRunProperties[] map = new TextFormattingRunProperties[7];

		static XmlWordClassificationLookup()
		{
			Typeface typeface = new Typeface(CodeEditor.FontName);
			double fontSize = CodeEditor.FontSize;
			map[0] = new TextFormattingRunProperties(typeface, fontSize, Colors.Blue);
			map[1] = new TextFormattingRunProperties(typeface, fontSize, Colors.DarkRed);
			map[2] = new TextFormattingRunProperties(typeface, fontSize, Colors.Green);
			map[3] = new TextFormattingRunProperties(typeface, fontSize, Colors.Red);
			map[4] = new TextFormattingRunProperties(typeface, fontSize, Colors.Black);
			map[5] = new TextFormattingRunProperties(typeface, fontSize, Colors.DarkGoldenrod);
			map[6] = new TextFormattingRunProperties(typeface, fontSize, Colors.White);
		}

		public TextFormattingRunProperties GetTextProperties(string c)
		{
			if ((c[0] >= '0') && (c[0] <= '9'))
			{
				return map[c[0] - '0'];
			}
			return map[map.Length - 1];
		}

		public IEnumerable<string> ClassificationsList
		{
			get
			{
				return new string[] { "0", "1", "2", "3", "4", "5" };
			}
		}
	}
}
