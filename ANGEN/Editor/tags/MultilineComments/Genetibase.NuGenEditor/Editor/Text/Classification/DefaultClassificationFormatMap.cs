/* -----------------------------------------------
 * DefaultClassificationFormatMap.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Editor.Text.Classification
{
	internal class DefaultClassificationFormatMap : IClassificationFormatMap
	{
		private IEnumerable<String> _classificationList;

		public TextFormattingRunProperties GetTextProperties(String classification)
		{
			if (classification == null)
			{
				throw new ArgumentNullException("classification");
			}
			return TextFormattingRunProperties.DefaultProperties;
		}

		public IEnumerable<String> ClassificationsList
		{
			get
			{
				if (_classificationList == null)
				{
					_classificationList = new List<String>().AsReadOnly();
				}
				return _classificationList;
			}
		}
	}
}
