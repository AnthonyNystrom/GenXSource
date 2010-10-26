/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-04 18:45:46 +0200 (Tue, 04 Jul 2006) $
* $Revision: 6586 $
*
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.OpenScience.CDK.Tools;

namespace Org.OpenScience.CDK.IO.Formats
{
	/// <summary> See <a href="http://www.mdl.com/downloads/public/ctfile/ctfile.jsp"></a>
	/// 
	/// </summary>
	/// <cdk.module>  io </cdk.module>
	/// <cdk.set>     io-formats </cdk.set>
	public class MDLFormat : IChemFormatMatcher
	{
		virtual public System.String FormatName
		{
			get
			{
				return "MDL Molfile";
			}
			
		}
		virtual public System.String MIMEType
		{
			get
			{
				return "chemical/x-mdl-molfile";
			}
			
		}
		virtual public System.String PreferredNameExtension
		{
			get
			{
				return NameExtensions[0];
			}
			
		}
		virtual public System.String[] NameExtensions
		{
			get
			{
				return new System.String[]{"mol"};
			}
			
		}
		virtual public System.String ReaderClassName
		{
			get
			{
				return "Org.OpenScience.CDK.IO.MDLReader";
			}
			
		}
		virtual public System.String WriterClassName
		{
			get
			{
				return "Org.OpenScience.CDK.IO.MDLWriter";
			}
			
		}
		virtual public bool XMLBased
		{
			get
			{
				return false;
			}
			
		}
		virtual public int SupportedDataFeatures
		{
			get
			{
				return DataFeatures.HAS_2D_COORDINATES | DataFeatures.HAS_3D_COORDINATES | DataFeatures.HAS_GRAPH_REPRESENTATION;
			}
			
		}
		virtual public int RequiredDataFeatures
		{
			get
			{
				return DataFeatures.NONE;
			}
			
		}
		
		public MDLFormat()
		{
		}
		
		public virtual bool matches(int lineNumber, System.String line)
		{
			if (lineNumber == 4 && (line.IndexOf("v2000") >= 0 || line.IndexOf("V2000") >= 0))
			{
				return true;
			}
			else if (line.StartsWith("M  END"))
			{
				return true;
			}
			else if (lineNumber == 4 && line.Length > 7)
			{
				// possibly a MDL mol file
				try
				{
					System.String atomCountString = line.Substring(0, (3) - (0)).Trim();
					System.String bondCountString = line.Substring(3, (6) - (3)).Trim();
					System.Int32.Parse(atomCountString);
					System.Int32.Parse(bondCountString);
					bool mdlFile = true;
					if (line.Length > 6)
					{
						System.String remainder = line.Substring(6).Trim();
						for (int i = 0; i < remainder.Length; ++i)
						{
							char c = remainder[i];
							if (!(System.Char.IsDigit(c) || System.Char.IsWhiteSpace(c)))
							{
								mdlFile = false;
							}
						}
					}
					// all tests succeeded, likely to be a MDL file
					if (mdlFile)
					{
						return true;
					}
				}
				catch (System.FormatException nfe)
				{
					// Integers not found on fourth line; therefore not a MDL file
				}
			}
			return false;
		}
	}
}