/* $RCSfile$
* $Author: tohel $
* $Date: 2006-07-05 16:46:29 +0200 (Wed, 05 Jul 2006) $
* $Revision: 6592 $
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
using Support;
using Org.OpenScience.CDK.Math;

namespace Org.OpenScience.CDK.IO.Formats
{
	/// <summary> See <a href="http://www.hyper.com/"></a>
	/// 
	/// </summary>
	/// <cdk.module>  io </cdk.module>
	/// <cdk.set>     io-formats </cdk.set>
	public class HINFormat : IChemFormatMatcher
	{
		virtual public System.String FormatName
		{
			get
			{
				return "HyperChem HIN";
			}
			
		}
		virtual public System.String MIMEType
		{
			get
			{
				return null;
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
				return new System.String[]{"hin"};
			}
			
		}
		virtual public System.String ReaderClassName
		{
			get
			{
				return "Org.OpenScience.CDK.IO.HINReader";
			}
			
		}
		virtual public System.String WriterClassName
		{
			get
			{
				return "Org.OpenScience.CDK.IO.HINWriter";
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
				return RequiredDataFeatures | DataFeatures.HAS_GRAPH_REPRESENTATION;
			}
			
		}
		virtual public int RequiredDataFeatures
		{
			get
			{
				return DataFeatures.HAS_3D_COORDINATES | DataFeatures.HAS_ATOM_PARTIAL_CHARGES | DataFeatures.HAS_ATOM_ELEMENT_SYMBOL;
			}
			
		}
		
		public HINFormat()
		{
		}
		
		public virtual bool matches(int lineNumber, System.String line)
		{
			if (line.StartsWith("atom ") && (line.EndsWith(" s") || line.EndsWith(" d") || line.EndsWith(" t") || line.EndsWith(" a")))
			{
				SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(line, " ");
				if (MathTools.isOdd(tokenizer.Count))
				{
					return true;
				}
			}
			return false;
		}
	}
}