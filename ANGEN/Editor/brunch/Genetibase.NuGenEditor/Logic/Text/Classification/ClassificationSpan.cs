/* -----------------------------------------------
 * ClassificationSpan.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Logic.Text.Classification
{
	/// <summary>
	/// </summary>
	public class ClassificationSpan : VersionedTextSpan
	{
		private String _classification;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassificationSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="classification"/> is <see langword="null"/>.</para></exception>
		public ClassificationSpan(TextBuffer buffer, Int32 start, Int32 length, String classification)
			: base(buffer, start, length)
		{
			if (classification == null)
			{
				throw new ArgumentNullException("classification");
			}
			_classification = classification;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassificationSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="classification"/> is <see langword="null"/>.</para></exception>
		public ClassificationSpan(TextBuffer buffer, ITextVersion version, Int32 start, Int32 length, String classification)
			: base(buffer, version, start, length)
		{
			if (classification == null)
			{
				throw new ArgumentNullException("classification");
			}

			_classification = classification;
		}

		/// <summary>
		/// </summary>
		public String Classification
		{
			get
			{
				return _classification;
			}
		}
	}
}
