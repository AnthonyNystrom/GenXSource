using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NuGenCodeLine
	{
		private int indentation = 0;
		public int Indentation
		{
			get
			{
				return indentation;
			}
			set
			{
				indentation = value;
			}
		}

		private string text = string.Empty;
		public string Text
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
				textLineNumber = -1;
			}
		}

		private int textLineNumber = -1;
		public int TextLineNumber
		{
			get
			{
				if (textLineNumber == -1)
				{
					textLineNumber = 0;

					foreach (char character in text)
					{
						if (character == '\n')
						{
							textLineNumber++;
						}
					}
				}

				return textLineNumber;
			}
		}

		public NuGenCodeLine()
		{
		}

		public NuGenCodeLine(int indentation, string text)
		{
			Indentation = indentation;
			Text = text;
		}
	}
}