/*
* $RCSfile: Point3i.java,v $
*
* Copyright (c) 2006 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.4 $
* $Date: 2006/01/05 03:50:51 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 3 element point represented by signed integer x,y,z
	/// coordinates.
	/// 
	/// </summary>
	/// <since> vecmath 1.2
	/// </since>
	[Serializable]
	public class Point3i:Tuple3i
	{
		
		// Compatible with 1.2
		new internal const long serialVersionUID = 6149289077348153921L;
		
		/// <summary> Constructs and initializes a Point3i from the specified
		/// x, y, and z coordinates.
		/// </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public Point3i(int x, int y, int z):base(x, y, z)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3i from the array of length 3.</summary>
		/// <param name="t">the array of length 3 containing x, y, and z in order.
		/// </param>
		public Point3i(int[] t):base(t)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3i from the specified Tuple3i.</summary>
		/// <param name="t1">the Tuple3i containing the initialization x, y, and z
		/// data.
		/// </param>
		public Point3i(Tuple3i t1):base(t1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point3i to (0,0,0).</summary>
		public Point3i():base()
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		override public System.Object Clone()
		{
			return null;
		}
	}
}