/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-21 20:08:47 -0500 (Tue, 21 Mar 2006) $
* $Revision: 4678 $
*
* Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using Org.Jmol.Util;
using NuGenJmol;

namespace Org.Jmol.G3d
{
	/// <summary><p>
	/// Implements a color index model using a colix as a
	/// <strong>COLor IndeX</strong>.
	/// </p>
	/// <p>
	/// A colix is a color index represented as a short int.
	/// </p>
	/// <p>
	/// The value 0 is considered a null value ... for no color. In Jmol this
	/// generally means that the value is inherited from some other object.
	/// </p>
	/// <p>
	/// The value 1 is used to indicate TRANSLUCENT, but with the color
	/// coming from the parent. The value 2 indicates OPAQUE, but with the
	/// color coming from the parent.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	sealed class Colix
	{
		private static int colixMax;
		private static int[] argbs = new int[128];
		private static int[] argbsGreyscale;
		private static int[][] ashades = new int[128][];
		private static int[][] ashadesGreyscale;
		private static readonly Int2IntHash colixHash = new Int2IntHash();
		
		public static short getColix(int argb)
		{
			if (argb == 0)
				return 0;
			int translucentMask = 0;
			if ((argb & unchecked((int)0xFF000000)) != unchecked((int)0xFF000000))
			{
				if ((argb & unchecked((int) 0xFF000000)) == 0)
				{
					System.Console.Out.WriteLine("zero alpha channel + non-zero rgb not supported");
					throw new System.IndexOutOfRangeException();
				}
				argb |= unchecked((int) 0xFF000000);
				translucentMask = NuGraphics3D.TRANSLUCENT_MASK;
			}
			int c = colixHash.get(argb);
			if (c > 0)
			{
				return (short) (c | translucentMask);
			}
			return (short) (allocateColix(argb) | translucentMask);
		}
		
		private static int allocateColix(int argb)
		{
			lock (typeof(Colix))
			{
				// double-check to make sure that someone else did not allocate
				// something of the same color while we were waiting for the lock
				if ((argb & unchecked((int) 0xFF000000)) != unchecked((int)0xFF000000))
					throw new System.IndexOutOfRangeException();
				for (int i = colixMax; --i >= NuGraphics3D.SPECIAL_COLIX_MAX; )
					if (argb == argbs[i])
						return (short) i;
				if (colixMax == argbs.Length)
				{
					int oldSize = argbs.Length;
					int newSize = oldSize * 2;
					int[] t0 = new int[newSize];
					Array.Copy(argbs, 0, t0, 0, oldSize);
					argbs = t0;
					
					if (argbsGreyscale != null)
					{
						t0 = new int[newSize];
						Array.Copy(argbsGreyscale, 0, t0, 0, oldSize);
						argbsGreyscale = t0;
					}
					
					int[][] t2 = new int[newSize][];
					Array.Copy(ashades, 0, t2, 0, oldSize);
					ashades = t2;
					
					if (ashadesGreyscale != null)
					{
						t2 = new int[newSize][];
						Array.Copy(ashadesGreyscale, 0, t2, 0, oldSize);
						ashadesGreyscale = t2;
					}
				}
				argbs[colixMax] = argb;
				if (argbsGreyscale != null)
					argbsGreyscale[colixMax] = NuGraphics3D.calcGreyscaleRgbFromRgb(argb);
				colixHash.put(argb, colixMax);
				return colixMax++;
			}
		}
		
		private static void  calcArgbsGreyscale()
		{
			lock (typeof(Colix))
			{
				if (argbsGreyscale == null)
				{
					argbsGreyscale = new int[argbs.Length];
					for (int i = argbsGreyscale.Length; --i >= 0; )
						argbsGreyscale[i] = NuGraphics3D.calcGreyscaleRgbFromRgb(argbs[i]);
				}
			}
		}
		
		public static int getArgb(short colix)
		{
			return argbs[colix & NuGraphics3D.OPAQUE_MASK];
		}
		
		public static int getArgbGreyscale(short colix)
		{
			if (argbsGreyscale == null)
				calcArgbsGreyscale();
			return argbsGreyscale[colix & NuGraphics3D.OPAQUE_MASK];
		}
		
		public static bool isTranslucent(short colix)
		{
            return (colix & NuGraphics3D.TRANSLUCENT_MASK) != 0;
		}
		
		public static int[] getShades(short colix)
		{
            colix &= NuGraphics3D.OPAQUE_MASK;
			int[] shades = ashades[colix];
			if (shades == null)
                shades = ashades[colix] = Shade3D.getShades(argbs[colix], false);
			return shades;
		}
		
		public static int[] getShadesGreyscale(short colix)
		{
            colix &= NuGraphics3D.OPAQUE_MASK;
			if (ashadesGreyscale == null)
				ashadesGreyscale = new int[ashades.Length][];
			int[] shadesGreyscale = ashadesGreyscale[colix];
			if (shadesGreyscale == null)
				shadesGreyscale = ashadesGreyscale[colix] = Shade3D.getShades(argbs[colix], true);
			return shadesGreyscale;
		}
		
		public static void flushShades()
		{
			for (int i = colixMax; --i >= 0; )
				ashades[i] = null;
		}

        public static readonly Int2IntHash hashMix2 = new Int2IntHash(32);
		
		public static short getColixMix(short colixA, short colixB)
		{
			if (colixA == colixB)
				return colixA;
			if (colixA <= 0)
				return colixB;
			if (colixB <= 0)
				return colixA;
            int translucentMask = colixA & colixB & NuGraphics3D.TRANSLUCENT_MASK;
            colixA &= (short)~NuGraphics3D.TRANSLUCENT_MASK;
            colixB &= (short)~NuGraphics3D.TRANSLUCENT_MASK;
			int mixId = ((colixA < colixB)?(((ushort)colixA << 16) | (ushort)colixB):(((ushort)colixB << 16) | (ushort)colixA));
			int mixed = hashMix2.get(mixId);
			if (mixed == System.Int32.MinValue)
			{
				int argbA = argbs[colixA];
				int argbB = argbs[colixB];
				int r = (((argbA & 0x00FF0000) + (argbB & 0x00FF0000)) >> 1) & 0x00FF0000;
				int g = (((argbA & 0x0000FF00) + (argbB & 0x0000FF00)) >> 1) & 0x0000FF00;
				int b = (((argbA & 0x000000FF) + (argbB & 0x000000FF)) >> 1);
				int argbMixed = unchecked((int) 0xFF000000) | r | g | b;
				mixed = getColix(argbMixed);
				hashMix2.put(mixId, mixed);
			}
			return (short) (mixed | translucentMask);
		}

		static Colix()
		{
            colixMax = NuGraphics3D.SPECIAL_COLIX_MAX;
		}
	}
}