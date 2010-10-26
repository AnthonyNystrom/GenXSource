/* -----------------------------------------------
 * ProgramInput.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.ComponentModel;
using Genetibase.NuGenVisiCalc.Operators;
using System.Drawing;
using Genetibase.Shared.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenVisiCalc.Types
{
	[Serializable]
	[Type("ProgramInput")]
	internal class ProgramInput : Operator
	{
		private Int32 _inputIndex;

		[Browsable(true)]
		[NuGenSRCategory("Category_Schema")]
		[ReadOnly(true)]
		public Int32 InputIndex
		{
			get
			{
				return _inputIndex;
			}
			set
			{
				_inputIndex = value;
			}
		}

		private static readonly SolidBrush _nodeBrush = new SolidBrush(Color.FromArgb(44, 0, 0, 0));

		public override void DrawNode(Graphics g, Single roundRadius, System.Boolean selected)
		{
			Int32 x = ClientRectangle.X;
			Int32 y = ClientRectangle.Y;
			Int32 width = ClientRectangle.Width;
			Int32 height = ClientRectangle.Height;

			Rectangle[] rectArray = new Rectangle[3] 
			{
				new Rectangle(x + 1, y + 1, width, height)
				, new Rectangle(x + 2, y + 2, width, height)
				, new Rectangle(x + 3, y + 3, width, height)
			};

			foreach (Rectangle rect in rectArray)
			{
				if (roundRadius > 0)
				{
					NuGenControlPaint.FillRoundRectangle(g, _nodeBrush, rect, roundRadius);
				}
				else
				{
					g.FillRectangle(_nodeBrush, rect);
				}
			}

			/* Header */

			using (LinearGradientBrush lgb = new LinearGradientBrush(HeaderRectangle, HeaderBackColor, Color.White, LinearGradientMode.Horizontal))
			{
				if (roundRadius > 0)
				{
					NuGenControlPaint.FillRoundRectangle(g, lgb, HeaderRectangle, roundRadius, NuGenRoundRectangleStyle.TopRound);
				}
				else
				{
					g.FillRectangle(lgb, HeaderRectangle);
				}
			}

			/* Header text. */

			using (SolidBrush sb = new SolidBrush(HeaderForeColor))
			{
				g.DrawString(Header, HeaderFont, sb, HeaderRectangle.X + 2, HeaderRectangle.Y + 2);
			}

			/* Body */

			using (LinearGradientBrush lgb = new LinearGradientBrush(BodyRectangle, BodyBackColor, Color.White, LinearGradientMode.Horizontal))
			{
				if (roundRadius > 0)
				{
					NuGenControlPaint.FillRoundRectangle(g, lgb, BodyRectangle, roundRadius, NuGenRoundRectangleStyle.BottomRound);
				}
				else
				{
					g.FillRectangle(lgb, BodyRectangle);
				}
			} 

			/* Inputs and Outputs */

			using (SolidBrush sb = new SolidBrush(BodyForeColor))
			{
				for (Int32 i = 0; i < OutputsLength; i++)
				{
					Rectangle currentRect = OutputRectCollection[i];
					g.DrawString("<", BodyFont, sb, currentRect.X, currentRect.Y);
				}

				for (Int32 i = 0; i < InputsLength; i++)
				{
					Rectangle currentRect = InputRectCollection[i];
					g.DrawString(">", BodyFont, sb, currentRect.X, currentRect.Y);
				}
			}

			/* Result */

			Object result = GetData();

			if (result != null)
			{
				using (SolidBrush sb = new SolidBrush(BodyForeColor))
				{
					Int32 resultX = BodyRectangle.X + BodyRectangle.Width / 2;
					Int32 resultY = InputRectCollection[0].Y;

					using (StringFormat sf = new StringFormat())
					{
						sf.Alignment = StringAlignment.Center;

						switch (DisplayFormat)
						{
							case ValueDisplayFormat.CultureSpecific:
							{
								g.DrawString(ResultCultureSpecific, BodyFont, sb, resultX, resultY, sf);
								break;
							}
							case ValueDisplayFormat.FixedPoint:
							{
								g.DrawString(ResultFixedPoint, BodyFont, sb, resultX, resultY, sf);
								break;
							}
							case ValueDisplayFormat.Scientific:
							{
								g.DrawString(ResultScientific, BodyFont, sb, resultX, resultY, sf);
								break;
							}
							default:
							{
								g.DrawString(Result, BodyFont, sb, resultX, resultY, sf);
								break;
							}
						}
					}
				}
			}

			/* Outline */

			if (selected)
			{
				using (Pen pen = new Pen(OutlineColor, 2))
				{
					if (roundRadius > 0)
					{
						NuGenControlPaint.DrawRoundRectangle(g, pen, ClientRectangle, roundRadius);
					}
					else
					{
						g.DrawRectangle(pen, ClientRectangle);
					}
				}
			}
			else
			{
				using (Pen pen = new Pen(Color.FromArgb(65, OutlineColor), 1))
				{
					if (roundRadius > 0)
					{
						NuGenControlPaint.DrawRoundRectangle(g, pen, ClientRectangle, roundRadius);
					}
					else
					{
						g.DrawRectangle(pen, ClientRectangle);
					}
				}
			}
		}

		public override Object GetData()
		{
			if (ContainingProgram.ParentNode != null)
			{
				return ContainingProgram.ParentNode.GetInputData(_inputIndex);
			}

			return null;
		}

		public ProgramInput()
		{
			Name = Header = "ProgramInput";
			CreateInputs(1);
			SetInput(0, "", 0.0);
			CreateOutputs(1);
			SetOutput(0, "", 0.0);
		}
	}
}
