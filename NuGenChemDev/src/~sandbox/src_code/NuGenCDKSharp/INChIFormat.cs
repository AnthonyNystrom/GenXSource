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
	/// <summary> See <a http://www.iupac.org/inchi/"></a>
	/// 
	/// </summary>
	/// <cdk.module>  io </cdk.module>
	/// <cdk.set>     io-formats </cdk.set>
	public class INChIFormat : IChemFormatMatcher
	{
		virtual public System.String FormatName
		{
			get
			{
				return "IUPAC-NIST Chemical Identifier (XML)";
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
				return new System.String[]{"inchi"};
			}
			
		}
		virtual public System.String ReaderClassName
		{
			get
			{
				return "Org.OpenScience.CDK.IO.INChIReader";
			}
			
		}
		virtual public System.String WriterClassName
		{
			get
			{
				return null;
			}
			
		}
		virtual public bool XMLBased
		{
			get
			{
				return true;
			}
			
		}
		virtual public int SupportedDataFeatures
		{
			get
			{
				return DataFeatures.NONE;
			}
			
		}
		virtual public int RequiredDataFeatures
		{
			get
			{
				return DataFeatures.NONE;
			}
			
		}
		
		public INChIFormat()
		{
		}
		
		public virtual bool matches(int lineNumber, System.String line)
		{
			if (line.IndexOf("<INChI") != - 1)
			{
				return true;
			}
			return false;
		}
	}
}