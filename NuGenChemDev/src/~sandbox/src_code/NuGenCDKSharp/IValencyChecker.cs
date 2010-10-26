/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-04-15 21:58:22 +0200 (Sat, 15 Apr 2006) $
*  $Revision: 5940 $
*
*  Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/

using Org.OpenScience.CDK.Interfaces;
namespace Org.OpenScience.CDK.Tools
{
	
	/// <summary> A common interface for SaturationChecker and ValencyChecker. Mainly created
	/// to be able to have HydrogenAdder use both.
	/// 
	/// </summary>
	/// <author>          Egon Willighagen
	/// </author>
	/// <cdk.created>     2004-01-08 </cdk.created>
	/// <summary> 
	/// </summary>
	/// <cdk.module>      valencycheck </cdk.module>
	public interface IValencyChecker
	{
		
		bool isSaturated(IAtomContainer ac);
		bool isSaturated(IAtom atom, IAtomContainer container);
		int calculateNumberOfImplicitHydrogens(IAtom atom, IAtomContainer container);
	}
}