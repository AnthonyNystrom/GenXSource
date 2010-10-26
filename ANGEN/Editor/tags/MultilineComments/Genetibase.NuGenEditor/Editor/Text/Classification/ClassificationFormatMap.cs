/* -----------------------------------------------
 * ClassificationFormatMap.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace Genetibase.Windows.Controls.Editor.Text.Classification
{
	internal class ClassificationFormatMap : IClassificationFormatMap
	{
		private Dictionary<String, TextFormattingRunProperties> _lookupDictionary = new Dictionary<String, TextFormattingRunProperties>();

		public ClassificationFormatMap()
		{
			this.FillLookupDictionary();
		}

		private void FillLookupDictionary()
		{
			_lookupDictionary["comment"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Times New Roman"), FontStyles.Italic, FontWeights.Normal, FontStretches.Normal), 14, Colors.Gray);
			_lookupDictionary["identifier"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Colors.Black);
			_lookupDictionary["keyword"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Color.FromRgb(0, 0x80, 0xff));
			_lookupDictionary["whitespace"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Colors.Black);
			_lookupDictionary["operator"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Colors.Teal);
			_lookupDictionary["literal"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Colors.Brown);
			_lookupDictionary["String"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Color.FromRgb(0xff, 0x40, 0));
			_lookupDictionary["other"] = new TextFormattingRunProperties(new Typeface("Lucida Console"), 12, Colors.Black);
			_lookupDictionary["bold_comment"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Times New Roman"), FontStyles.Italic, FontWeights.Bold, FontStretches.Normal), 14, Colors.Gray);
			_lookupDictionary["bold_identifier"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Colors.Black);
			_lookupDictionary["bold_keyword"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Color.FromRgb(0, 0xa8, 0xff));
			_lookupDictionary["bold_whitespace"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Colors.Black);
			_lookupDictionary["bold_operator"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Colors.Teal);
			_lookupDictionary["bold_literal"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Colors.Brown);
			_lookupDictionary["bold_string"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Color.FromRgb(0xff, 0x40, 0));
			_lookupDictionary["bold_other"] = new TextFormattingRunProperties(new Typeface(new FontFamily("Lucida Console"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal), 12, Colors.Black);
		}

		public TextFormattingRunProperties GetTextProperties(String classification)
		{
			TextFormattingRunProperties defaultProperties;
			if (classification == null)
			{
				throw new ArgumentNullException("classification");
			}
			if (!_lookupDictionary.TryGetValue(classification, out defaultProperties))
			{
				classification = classification.Trim().ToLowerInvariant();
				if (!_lookupDictionary.TryGetValue(classification, out defaultProperties))
				{
					defaultProperties = TextFormattingRunProperties.DefaultProperties;
				}
			}
			return defaultProperties;
		}

		public IEnumerable<String> ClassificationsList
		{
			get
			{
				return _lookupDictionary.Keys;
			}
		}
	}
}
