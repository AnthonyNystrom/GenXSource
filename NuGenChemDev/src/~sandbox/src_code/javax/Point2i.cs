/*
* $RCSfile: Point2i.java,v $
*
* Copyright (c) 2006 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.3 $
* $Date: 2006/01/05 03:50:49 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 2-element point represented by signed integer x,y
	/// coordinates.
	/// 
	/// </summary>
	/// <since> vecmath 1.4
	/// </since>
	[Serializable]
	public class Point2i:Tuple2i
	{
		
		new internal const long serialVersionUID = 9208072376494084954L;
		
		/// <summary> Constructs and initializes a Point2i from the specified
		/// x and y coordinates.
		/// </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		public Point2i(int x, int y):base(x, y)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point2i from the array of length 2.</summary>
		/// <param name="t">the array of length 2 containing x and y in order.
		/// </param>
		public Point2i(int[] t):base(t)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point2i from the specified Tuple2i.</summary>
		/// <param name="t1">the Tuple2i containing the initialization x and y
		/// data.
		/// </param>
		public Point2i(Tuple2i t1):base(t1)
		{
		}
		
		
		/// <summary> Constructs and initializes a Point2i to (0,0).</summary>
		public Point2i():base()
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		override public System.Object Clone()
		{
			return null;
		}
	}
}