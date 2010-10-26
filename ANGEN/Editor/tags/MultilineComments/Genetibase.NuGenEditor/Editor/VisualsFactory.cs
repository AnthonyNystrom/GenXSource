/* -----------------------------------------------
 * VisualsFactory.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.TextFormatting;
using Genetibase.Windows.Controls.Editor.Text.Classification;
using Genetibase.Windows.Controls.Editor.Text.View;
using Genetibase.Windows.Controls.Logic.Text.Classification;
using System.Windows;
using System.Globalization;
using System.Windows.Media;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor
{
	internal class VisualsFactory
	{
		private IClassificationFormatMap _classificationFormatMap;
		private IClassifier _classifier;
		private Double _maxLineWidth;
		private TextFormattingParagraphProperties _paragraphProperties;
		private Int32 _tabSize = 4;
		private static TextFormatter _textFormatter;
		private ITextView _textView;

		public VisualsFactory(ITextView textView, IClassifier classifier, Double maxLineWidth)
		{
			_textView = textView;
			_classifier = classifier;
			_maxLineWidth = maxLineWidth;

			/* 
			 * DocumentType("text.xml") = XmlWordClassificationLookup
			 * DocumentType("C#"), DocumentType("VB"), DocumentType("text") = ClassificationFormatMap
			 * Else = DefaultClassificationFormatMap
			 */

			_classificationFormatMap = Connector.Assets.ClassificationFormatMapSelector.Invoke(_textView.TextBuffer.DocumentType);
			_paragraphProperties = new TextFormattingParagraphProperties();
		}

		public IList<TextLineVisual> CreateLineVisual(Int32 lineNumber)
		{
			return this.CreateLineVisual(lineNumber, null);
		}

		public IList<TextLineVisual> CreateLineVisual(Int32 lineNumber, IList<SpaceNegotiation> spaceNegotiations)
		{
			IList<ClassificationSpan> classificationSpans;
			Int32 startOfLineFromLineNumber = _textView.TextBuffer.GetStartOfLineFromLineNumber(lineNumber);
			Int32 startOfNextLineFromPosition = _textView.TextBuffer.GetStartOfNextLineFromPosition(startOfLineFromLineNumber);
			Int32 lengthOfLineFromLineNumber = _textView.TextBuffer.GetLengthOfLineFromLineNumber(lineNumber);
			Int32 length = 0;
			if (startOfNextLineFromPosition == startOfLineFromLineNumber)
			{
				length = _textView.TextBuffer.GetEndOfLineFromPosition(startOfLineFromLineNumber) - startOfLineFromLineNumber;
			}
			else
			{
				length = startOfNextLineFromPosition - startOfLineFromLineNumber;
			}
			if (length > 0)
			{
				classificationSpans = _classifier.GetClassificationSpans(new VersionedTextSpan(_textView.TextBuffer, startOfLineFromLineNumber, length));
			}
			else
			{
				classificationSpans = new List<ClassificationSpan>(0);
			}
			TextFormattingSource textSource = new TextFormattingSource(_textView.TextBuffer, _classificationFormatMap, startOfLineFromLineNumber, length, classificationSpans, spaceNegotiations);
			if (_textFormatter == null)
			{
				_textFormatter = TextFormatter.Create();
			}
			List<TextLine> textLines = new List<TextLine>();
			Int32 firstCharIndex = 0;
			TextLineBreak previousLineBreak = null;
			while (firstCharIndex <= length)
			{
				TextLine item = _textFormatter.FormatLine(textSource, firstCharIndex, this.MaxLineWidth, _paragraphProperties, previousLineBreak);
				firstCharIndex += item.Length;
				previousLineBreak = item.GetTextLineBreak();
				textLines.Add(item);
			}
			List<TextLineVisual> list3 = new List<TextLineVisual>();
			list3.Add(new TextLineVisual(_textView, textLines, new TextSpan(_textView.TextBuffer, startOfLineFromLineNumber, length), length - lengthOfLineFromLineNumber, textSource.VirtualCharacterPositions));
			return list3;
		}

		public Double MaxLineWidth
		{
			get
			{
				return _maxLineWidth;
			}
			set
			{
				_maxLineWidth = value;
			}
		}

		public Int32 TabSize
		{
			get
			{
				return _tabSize;
			}
			set
			{
				_tabSize = value;
				TextRunProperties textProperties = _classificationFormatMap.GetTextProperties("whitespace");
				if (textProperties == null)
				{
					textProperties = _paragraphProperties.DefaultTextRunProperties;
				}
				String textToFormat = new String(' ', _tabSize);
				FormattedText text = new FormattedText(textToFormat, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, textProperties.Typeface, textProperties.FontRenderingEmSize, Brushes.Black);
				Double widthIncludingTrailingWhitespace = text.WidthIncludingTrailingWhitespace;
				_paragraphProperties.SetTabSize(widthIncludingTrailingWhitespace);
			}
		}
	}
}
