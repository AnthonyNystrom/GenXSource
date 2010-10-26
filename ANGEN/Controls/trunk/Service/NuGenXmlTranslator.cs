/* -----------------------------------------------
 * NuGenXmlTranslator.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Xml;

namespace Genetibase.Controls.Service
{
	/// <summary>
	/// Translates the input string from XHTML into RTF.
	/// </summary>
	class NuGenXmlTranslator : XmlDocument
	{
		#region Declarations

		private NuGenRtfDocument rtfDocument;
		private XmlTextReader reader;

		#endregion
		
		#region Properties.Public
		
		/// <summary>
		/// Determines the list of errors encountered during XHTML to RTF translation.
		/// </summary>
		private ArrayList errors = new ArrayList();

		/// <summary>
		/// Gets the list of errors encountered during XHTML to RTF translation.
		/// </summary>
		public ArrayList Errors
		{
			get { return errors; }
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// If there is a body element, then all XML code outside of it is removed.
		/// </summary>
		/// <param name="xmlText">XML code to be trimmed.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="xmlText"/> is <see langword="null"/>.</exception>
		public string TrimBody(string xmlText)
		{
			if (xmlText == null)
			{
				throw new ArgumentNullException("xmlText");
			}

			string lower = xmlText.ToLower();
			int bodyStart = lower.IndexOf("<body");
			
			if (bodyStart > 0)
			{
				xmlText = xmlText.Substring(bodyStart);
			}
			
			lower = xmlText.ToLower();
			
			int bodyEnd = lower.LastIndexOf("</body>") + 7;
			
			if (bodyEnd > 7)
			{
				xmlText = xmlText.Substring(0, bodyEnd);
			}

			return xmlText;
		}

		/// <summary>
		/// Initiates translation from XML to RTF (Rich Text Format)
		/// Returns an object representing a RTF document.
		/// </summary>
		/// <returns></returns>
		public NuGenRtfDocument ToRtfDocument()
		{
			this.rtfDocument = new NuGenRtfDocument();
			
			// Starting at the placeholding element XmlTranslator_root,
			// the primary translation method is called recursively on all
			// of the nodes in the XML DOM.
			try
			{
				foreach (XmlNode child in this.DocumentElement.ChildNodes)
				{
					this.rtfDocument.AppendText("{\\pard ");
					TranslateXmlNodeIntoRtfGroup(child, 3);
					this.rtfDocument.AppendText("}");
				}
			}
			catch (Exception e)
			{
				// Catch any other exceptions during translation.
				errors.Add("Error during translation. " + e.Message);
			}

			return this.rtfDocument;
		}

		/// <summary>
		/// The primary method responsible for translating XML into RTF code.
		/// It recursively walks the XML DOM tree, building the RTF document
		/// as it goes.
		/// </summary>
		/// <param name="x">The current node to be translated.</param>
		/// <param name="baseFontSize">The XHTML font size (1-7) of the parent element.</param>
		public void TranslateXmlNodeIntoRtfGroup(XmlNode x, int baseFontSize)
		{
			if (x != null)
			{
				// If a node of this type is reached, then its content should be added
				// to the RTF document, and it has no children to recursively translate.
				if (x.Name.ToLower().ToLower() == "#text")
				{
					this.rtfDocument.AppendText("{" + x.InnerText + "}");
				}
				// Otherwise this node is a tag of some sort, potentially with children
				// that need to be translated. The supported tags and their attributes 
				// are deciphered, and the equivalent RTF control word(s) is(are) added 
				// to the RTF document.
				else
				{
					bool eval = true; // Evaluate this node's children?
					this.rtfDocument.AppendText("{"); // Begin new RTF group

					if (x.Name.ToLower() == "body")
					{
						foreach (XmlAttribute attribute in x.Attributes)
						{
							if (attribute.Name.ToLower() == "text")
							{
								int index = 0;
								index = this.rtfDocument.UseColor(TranslateXhtmlColorIntoColorFromArgb(attribute.Value));
								this.rtfDocument.AppendText("\\cf" + index + " ");
							}
						}
					}
					else if (x.Name.ToLower() == "br")
					{
						this.rtfDocument.AppendText("{\\par}");
					}
					else if (x.Name.ToLower() == "h1")
					{
						this.rtfDocument.AppendText("\\b\\fs48\\par ");
						baseFontSize = 6;
					}
					else if (x.Name.ToLower() == "h2")
					{
						this.rtfDocument.AppendText("\\b\\fs36\\par ");
						baseFontSize = 5;
					}
					else if (x.Name.ToLower() == "h3")
					{
						this.rtfDocument.AppendText("\\b\\fs27\\par ");
						baseFontSize = 4;
					}
					else if (x.Name.ToLower() == "h4")
					{
						this.rtfDocument.AppendText("\\b\\fs24\\par ");
						baseFontSize = 3;
					}
					else if (x.Name.ToLower() == "h5")
					{
						this.rtfDocument.AppendText("\\b\\fs20\\par ");
						baseFontSize = 2;
					}
					else if (x.Name.ToLower() == "h6")
					{
						this.rtfDocument.AppendText("\\b\\fs15\\par ");
						baseFontSize = 1;
					}
					else if ((x.Name.ToLower() == "b") || (x.Name.ToLower() == "strong"))
					{
						this.rtfDocument.AppendText("\\b ");
					}
					else if ((x.Name.ToLower() == "i") 
						|| (x.Name.ToLower() == "em") 
						|| (x.Name.ToLower() == "cite")
						|| (x.Name.ToLower() == "dfn")
						|| (x.Name.ToLower() == "var"))
					{
						this.rtfDocument.AppendText("\\i ");
					}
					else if (x.Name.ToLower() == "u")
					{
						this.rtfDocument.AppendText("\\ul ");
					}
					else if ((x.Name.ToLower() == "s") || (x.Name.ToLower() == "strike"))
					{
						this.rtfDocument.AppendText("\\strike ");
					}
					else if ((x.Name.ToLower() == "tt")
						|| (x.Name.ToLower() == "pre")
						|| (x.Name.ToLower() == "code")
						|| (x.Name.ToLower() == "samp"))
					{
						int index = rtfDocument.UseFont("Courier New");
						rtfDocument.AppendText("\\f" + index + "\\fs20 ");
						baseFontSize = 2;
					}
					else if (x.Name.ToLower() == "big")
					{
						baseFontSize++;
						this.rtfDocument.AppendText("\\fs"
							+ TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize) + " ");
					}
					else if (x.Name.ToLower() == "small")
					{
						baseFontSize--;
						this.rtfDocument.AppendText("\\fs"
							+ TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize) + " ");
					}
					else if (x.Name.ToLower() == "a")
					{
						int index = rtfDocument.UseColor(Color.FromArgb(0, 0, 255));
						this.rtfDocument.AppendText("\\cf" + index + "\\ul ");
					}
					else if (x.Name.ToLower() == "ul")
					{
						this.rtfDocument.AppendText("\\pn\\pnlvlblt ");
					}
					else if (x.Name.ToLower() == "li")
					{
						this.rtfDocument.AppendText("\\pnlvlcont\\par\\pnlvlblt ");
					}
					else if (x.Name.ToLower() == "basefont")
					{
						foreach (XmlAttribute attribute in x.Attributes)
						{
							if (attribute.Name.ToLower() == "size")
							{
								baseFontSize = Int32.Parse(attribute.Value);
								this.rtfDocument.AppendText("\\fs"
									+ TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize));
							}
						}
					}
					else if (x.Name.ToLower() == "font")
					{
						foreach (XmlAttribute attribute in x.Attributes)
						{
							if (attribute.Name.ToLower() == "face")
							{
								int commaIndex = attribute.Value.IndexOf(',');
								int index = 0;
								if (commaIndex > 0)
								{
									index = this.rtfDocument.UseFont(attribute.Value.Substring(0, commaIndex));
								}
								else
								{
									index = this.rtfDocument.UseFont(attribute.Value);
								}
								this.rtfDocument.AppendText("\\f" + index + " ");
							}
							else if (attribute.Name.ToLower() == "size")
							{
								if (attribute.Value.StartsWith("+"))
								{
									baseFontSize += Int32.Parse(attribute.Value);
									this.rtfDocument.AppendText("\\fs"
										+ TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize) + " ");
								}
								else if (attribute.Value.StartsWith("-"))
								{
									baseFontSize -= Int32.Parse(attribute.Value);
									this.rtfDocument.AppendText("\\fs"
										+ TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize) + " ");
								}
								else
								{
									baseFontSize = Int32.Parse(attribute.Value);
									this.rtfDocument.AppendText("\\fs"
										+ TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize) + " ");
								}
							}
							else if (attribute.Name.ToLower() == "color")
							{
								int index = 0;
								index = this.rtfDocument.UseColor(TranslateXhtmlColorIntoColorFromArgb(attribute.Value));
								this.rtfDocument.AppendText("\\cf" + index + " ");
							}
						}
					}
					// Head elements should not be found, but this is just for extra robustness.
					// Comments should not be evaluated.
					else if ((x.Name.ToLower() == "head")
						|| (x.Name.ToLower() == "#comment"))
					{
						eval = false;
					}
					
					if (eval)
					{
						// All known and unknown tags may have the style attribute,
						// which in turn may contain certain properties which will
						// affect the format of descendent elements. This for loop
						// parses the style attribute if one exists on the current node.
						// token[0] == property name; token[1] == property value
						foreach (XmlAttribute attribute in x.Attributes)
						{
							if (attribute.Name.ToLower() == "style")
							{
								char[] semi = {';'};
								string[] tokens = attribute.Value.Split(semi);
								
								for (int i = 0; i < tokens.Length; i++)
								{
									char[] colon = {':'};
									string[] token = tokens[i].Split(colon);

									if (token[0].ToLower().Trim() == "font-family")
									{
										int commaIndex = token[1].Trim().IndexOf(',');
										int index = 0;
										if (commaIndex > 0)
										{
											index = this.rtfDocument.UseFont(token[1].Substring(0, commaIndex).Trim());
										}
										else
										{
											index = this.rtfDocument.UseFont(token[1].Trim());
										}
										this.rtfDocument.AppendText("\\f" + index + " ");
									}
									else if (token[0].ToLower().Trim() == "font-size")
									{
										int rtfFontSize = TranslateStyleFontSizeIntoRtfFontSize(token[1].Trim(), baseFontSize);
										baseFontSize = TranslateRtfFontSizeIntoXhtmlFontSize(rtfFontSize);
										this.rtfDocument.AppendText("\\fs" + rtfFontSize + " ");
									}
									else if (token[0].ToLower().Trim() == "font-style")
									{
										if (token[1].ToLower().Trim() == "normal")
										{
											this.rtfDocument.AppendText("\\i0 ");
										}
										else if ((token[1].ToLower().Trim() == "italic")
											|| (token[1].ToLower().Trim() == "oblique"))
										{
											this.rtfDocument.AppendText("\\i ");
										}
									}
									else if (token[0].ToLower().Trim() == "font-weight")
									{
										if ((token[1].ToLower().Trim() == "normal")
											|| (token[1].ToLower().Trim() == "lighter"))
										{
											this.rtfDocument.AppendText("\\b0 ");
										}
										else
										{
											this.rtfDocument.AppendText("\\b ");
										}
									}
									else if (token[0].ToLower().Trim() == "text-align")
									{
										if (token[1].ToLower().Trim() == "right")
										{
											this.rtfDocument.AppendText("\\qr ");
										}
										else if (token[1].ToLower().Trim() == "center")
										{
											this.rtfDocument.AppendText("\\qc ");
										}
										else if (token[1].ToLower().Trim() == "justify")
										{
											this.rtfDocument.AppendText("\\qj ");
										}
										else
										{
											this.rtfDocument.AppendText("\\ql ");
										}
									}
									else if (token[0].ToLower().Trim() == "text-decoration")
									{
										if (token[1].ToLower().Trim() == "none")
										{
											this.rtfDocument.AppendText("\\ul0\\strike0 ");
										}
										else if (token[1].ToLower().Trim() == "underline")
										{
											this.rtfDocument.AppendText("\\ul ");
										}
										else if (token[1].ToLower().Trim() == "line-through")
										{
											this.rtfDocument.AppendText("\\strike ");
										}
									}
									else if (token[0].ToLower().Trim() == "color")
									{
										int index = 0;
										index = this.rtfDocument.UseColor(TranslateXhtmlColorIntoColorFromArgb(token[1].ToLower().Trim()));
										this.rtfDocument.AppendText("\\cf" + index + " ");
									}
									else if (token[0].ToLower().Trim() == "line-height")
									{
										int rtfFontSize = TranslateStyleFontSizeIntoRtfFontSize(token[1].Trim(), baseFontSize);
										this.rtfDocument.AppendText("\\sl" + (10 * rtfFontSize)  + "\\slmult1 ");
									}
								}
							}
						}

						// Recursively translates descendent nodes
						foreach (XmlNode child in x.ChildNodes)
						{
							TranslateXmlNodeIntoRtfGroup(child, baseFontSize);
						}
					}

					// Because of peculiarities of the RTF format, some control
					// words are placed at the end of their groups.
					if (x.Name.ToLower() == "p")
					{
						foreach (XmlAttribute attribute in x.Attributes)
						{
							if (attribute.Name.ToLower() == "align")
							{
								if (attribute.Value == "right")
								{
									this.rtfDocument.AppendText("\\qr ");
								}
								else if (attribute.Value == "center")
								{
									this.rtfDocument.AppendText("\\qc ");
								}
								else if (attribute.Value == "justify")
								{
									this.rtfDocument.AppendText("\\qj ");
								}
								else
								{
									this.rtfDocument.AppendText("\\ql ");
								}
							}
						}

						this.rtfDocument.AppendText("\\par");
					}
					else if ((x.Name.ToLower() == "h1")
						|| (x.Name.ToLower() == "h2")
						|| (x.Name.ToLower() == "h3")
						|| (x.Name.ToLower() == "h4")
						|| (x.Name.ToLower() == "h5")
						|| (x.Name.ToLower() == "h6"))
					{
						this.rtfDocument.AppendText("\\par");
					}
					
					this.rtfDocument.AppendText("}");

					// This terminates the effects of a ul "bullet" tag.
					if (x.Name.ToLower() == "ul")
					{
						this.rtfDocument.AppendText("\\pard");
					}
				}
			}
		}

		#endregion

		#region Methods.Private

		/// <summary>
		/// Translates an XHTML font size (1-7) into an RTF font size in half-points.
		/// Any errors result in a font size of 12 pts.
		/// </summary>
		private int TranslateXhtmlFontSizeIntoRtfFontSize(int xhtmlFontSize)
		{
			switch (xhtmlFontSize)
			{
				case 1: return 15;
				case 2: return 20;
				case 3: return 24;
				case 4: return 27;
				case 5: return 36;
				case 6: return 48;
				case 7: return 72;
				default: errors.Add("Unrecognized XHTML font size. Defaulting to 12 pt."); 
					return 24;
			}
		}

		/// <summary>
		/// This inverts the previous method.
		/// </summary>
		private int TranslateRtfFontSizeIntoXhtmlFontSize(int rtfFontSize)
		{
			if (rtfFontSize <= 15)
			{
				return 1;
			}
			else if (rtfFontSize <= 20)
			{
				return 2;
			}
			else if (rtfFontSize <= 24)
			{
				return 3;
			}
			else if (rtfFontSize <= 27)
			{
				return 4;
			}
			else if (rtfFontSize <= 36)
			{
				return 5;
			}
			else if (rtfFontSize <= 48)
			{
				return 6;
			}
			else
			{
				return 7;
			}
		}

		/// <summary>
		/// Translates a style sheet font size, which may have one of several different font
		/// measurements, into an RTF font size in half-points. The baseFontSize is required
		/// for measurements given in a form relative to the current font size. Any errors
		/// result in a font size of 12 pts.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="styleFontSize"/> is <see langword="null"/>.
		/// </exception>
		private int TranslateStyleFontSizeIntoRtfFontSize(string styleFontSize, int baseFontSize)
		{
			if (styleFontSize == null)
			{
				throw new ArgumentNullException("styleFontSize");
			}

			Font convertSize;

			if (styleFontSize.ToLower() == "xx-small")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(1);
			}
			else if (styleFontSize.ToLower() == "x-small")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(2);
			}
			else if (styleFontSize.ToLower() == "small")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(3);
			}
			else if (styleFontSize.ToLower() == "medium")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(4);
			}
			else if (styleFontSize.ToLower() == "large")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(5);
			}
			else if (styleFontSize.ToLower() == "x-large")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(6);
			}
			else if (styleFontSize.ToLower() == "xx-large")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(7);
			}
			else if (styleFontSize.ToLower() == "smaller")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize - 1);
			}
			else if (styleFontSize.ToLower() == "bigger")
			{
				return TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize + 1);
			}
			else
			{
				try
				{
					if (styleFontSize.ToLower().EndsWith("%"))
					{
						return (int)((TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize)
							* Int32.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("%")))) 
							/ 100.0);
					}
					else if (styleFontSize.ToLower().EndsWith("em"))
					{
						return (int)(TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize)
							* Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("em"))));
					}
					else if (styleFontSize.ToLower().EndsWith("ex"))
					{
						return (int)((TranslateXhtmlFontSizeIntoRtfFontSize(baseFontSize) / 2.0)
							* Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("ex"))));
					}
					else if (styleFontSize.ToLower().EndsWith("pt"))
					{
						return (int)(2.0
							* Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("pt"))));
					}
					else if (styleFontSize.ToLower().EndsWith("in"))
					{
						convertSize = new Font("", 
							(float)(Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("in")))),
							System.Drawing.GraphicsUnit.Inch);
						return (int)(2.0 * convertSize.SizeInPoints);
					}
					else if (styleFontSize.ToLower().EndsWith("cm"))
					{
						convertSize = new Font("", 
							(float)((Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("cm")))) / 10),
							System.Drawing.GraphicsUnit.Millimeter);
						return (int)(2.0 * convertSize.SizeInPoints);
					}
					else if (styleFontSize.ToLower().EndsWith("mm"))
					{
						convertSize = new Font("", 
							(float)(Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("mm")))),
							System.Drawing.GraphicsUnit.Millimeter);
						return (int)(2.0 * convertSize.SizeInPoints);
					}
					else if (styleFontSize.ToLower().EndsWith("pc"))
					{
						convertSize = new Font("", 
							(float)((Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("pc")))) * 12),
							System.Drawing.GraphicsUnit.Point);
						return (int)(2.0 * convertSize.SizeInPoints);
					}
					else if (styleFontSize.ToLower().EndsWith("px"))
					{
						convertSize = new Font("", 
							(float)(Double.Parse(styleFontSize.Substring(0, styleFontSize.ToLower().IndexOf("px")))),
							System.Drawing.GraphicsUnit.Pixel);
						return (int)(2.0 * convertSize.SizeInPoints);
					}
					else
					{
						errors.Add("Invalid font unit. Defaulting to 12 pt.");
						return 24;
					}
				}
				catch
				{
					errors.Add("Invalid font size. Defaulting to 12 pt.");
					return 24;
				}
			}
		}

		/// <summary>
		/// Translates color arguments into Color object representations.
		/// The argument may either be a hexidecimal number #RRGGBB,
		/// one of sixteen predefined, case-sensitive names (these are 
		/// used by the font element with the color attribute).
		/// NOTE: Predefined color names for style sheets are NOT supported.
		/// Style sheet colors must either be entered in as a hex number or
		/// in the form of: rgb float float float, where each float is a value
		/// between 0 and 1, representing relative amounts of red, green, and blue,
		/// respectively. Any errors result in a default of black (0, 0, 0).
		/// </summary>
		/// <param name="xhtmlColor"></param>
		/// <returns></returns>
		private Color TranslateXhtmlColorIntoColorFromArgb(string xhtmlColor)
		{
			if (xhtmlColor == "Black")
			{
				return Color.FromArgb(0, 0, 0);
			}
			else if (xhtmlColor == "Silver")
			{
				return Color.FromArgb(192, 192, 192);
			}
			else if (xhtmlColor == "Gray")
			{
				return Color.FromArgb(128, 128, 128);
			}
			else if (xhtmlColor == "White")
			{
				return Color.FromArgb(255, 255, 255);
			}
			else if (xhtmlColor == "Maroon")
			{
				return Color.FromArgb(128, 0, 0);
			}
			else if (xhtmlColor == "Red")
			{
				return Color.FromArgb(255, 0, 0);
			}
			else if (xhtmlColor == "Purple")
			{
				return Color.FromArgb(128, 0, 128);
			}
			else if (xhtmlColor == "Fuchsia")
			{
				return Color.FromArgb(255, 0, 255);
			}
			else if (xhtmlColor == "Green")
			{
				return Color.FromArgb(0, 128, 0);
			}
			else if (xhtmlColor == "Lime")
			{
				return Color.FromArgb(0, 255, 0);
			}
			else if (xhtmlColor == "Olive")
			{
				return Color.FromArgb(128, 128, 0);
			}
			else if (xhtmlColor == "Yellow")
			{
				return Color.FromArgb(255, 255, 0);
			}
			else if (xhtmlColor == "Navy")
			{
				return Color.FromArgb(0, 0, 128);
			}
			else if (xhtmlColor == "Blue")
			{
				return Color.FromArgb(0, 0, 255);
			}
			else if (xhtmlColor == "Teal")
			{
				return Color.FromArgb(0, 128, 128);
			}
			else if (xhtmlColor == "Aqua")
			{
				return Color.FromArgb(0, 255, 255);
			}
			else if (xhtmlColor.StartsWith("#"))
			{
				try
				{
					int color = Int32.Parse(xhtmlColor.Substring(1), 
						System.Globalization.NumberStyles.HexNumber);
					byte red = (byte)((color & 16711680) / 65536);
					byte green = (byte)((color & 65280) / 256);
					byte blue = (byte)(color & 255);
					return Color.FromArgb(red, green, blue);
				}
				catch
				{
					errors.Add("Invalid color value. Defaulting to black.");
					return Color.FromArgb(0, 0, 0);
				}
			}
			else if (xhtmlColor.StartsWith("rgb"))
			{
				try
				{
					char[] space = {' '};
					string[] tokens = xhtmlColor.Split(space);

					int i = 0;

					while (tokens[++i] == "") { }
                    byte red = (byte)(Double.Parse(tokens[i].Trim()) * 255);

					while (tokens[++i] == "") { }
					byte green = (byte)(Double.Parse(tokens[i].Trim()) * 255);

					while (tokens[++i] == "") { }
					byte blue = (byte)(Double.Parse(tokens[i].Trim()) * 255);

					return Color.FromArgb(red, green, blue);
				}
				catch
				{
					errors.Add("Invalid color value. Defaulting to black.");
					return Color.FromArgb(0, 0, 0);
				}
			}
			else
			{
				errors.Add("Invalid color format. Defaulting to black.");
				return Color.FromArgb(0, 0, 0);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenXmlTranslator"/> class.
		/// </summary>
		public NuGenXmlTranslator() : base() { }		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenXmlTranslator"/> class.
		/// </summary>
		/// <param name="xmlText">XML code to be translated.</param>
		public NuGenXmlTranslator(string xmlText) : base()
		{
			try
			{
				// All XML code except that contained within the body element is removed.
				xmlText = TrimBody(xmlText);

				// A space is added after all carriage returns to prevent word squishes
				// across line wraps from the input string.
				xmlText = xmlText.Replace("\n", " \n");

				// In case the input string is an XML fragment that contains multiple
				// roots, an additional element is placed around the input string to serve
				// as the undisputed root. Otherwise, the DOM creation would fail.
				reader = new XmlTextReader(
					new System.IO.StringReader("<XmlTranslator_root>" + xmlText + "</XmlTranslator_root>"));

				// All whitespace is preserved.
				reader.WhitespaceHandling = WhitespaceHandling.All;
				this.Load(reader);
			}
			catch (Exception e)
			{
				// If the XML is not well-formed, there may be an exception thrown
				// during creation of the DOM. The DOM will be empty and an error
				// will be added to the Errors property.
				errors.Add("Error creating DOM. " + e.Message);
			}
		}

		#endregion
	}
}
