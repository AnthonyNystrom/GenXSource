/* $RCSfile$
* $Author: egonw $
* $Date: 2005-11-10 10:52:44 -0500 (Thu, 10 Nov 2005) $
* $Revision: 4255 $
*
* Copyright (C) 2003-2005  The Jmol Development Team
*
* Contact: jmol-developers@lists.sf.net
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
using NuGenJmol;

namespace Org.Jmol.G3d
{
	/// <summary><p>
	/// Implements the shading of RGB values to support shadow and lighting
	/// highlights.
	/// </p>
	/// <p>
	/// Each RGB value has 64 shades. shade[0] represents ambient lighting.
	/// shade[63] is white ... a full specular highlight.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Miguel, miguel@jmol.org
	/// </author>
	sealed class Shade3D
	{
		public static bool Specular
		{
			get
			{
				return specularOn;
			}
			set
			{
				specularOn = value;
				dump();
			}
			
		}
        public static float LightsourceZ
		{
			set
			{
				zLightsource = value;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				magnitudeLight = (float) System.Math.Sqrt(xLightsource * xLightsource + yLightsource * yLightsource + zLightsource * zLightsource);
				dump();
			}
			
		}
        public static int SpecularPower
		{
			set
			{
				if (value >= 0)
					intenseFraction = value / 100f;
				else
					specularExponent = - value;
				dump();
			}
			
		}
        public static int AmbientPercent
		{
			set
			{
				ambientFraction = value / 100f;
				dump();
			}
			
		}
        public static int DiffusePercent
		{
			set
			{
				intensityDiffuse = value / 100f;
				dump();
			}
			
		}
        public static int SpecularPercent
		{
			set
			{
				intensitySpecular = value / 100f;
				dump();
			}
			
		}
		
		// there are 64 shades of a given color
		// 0 = ambient
		// 63 = brightest ... white
        public const int shadeMax = 64;
		//UPGRADE_NOTE: Final was removed from the declaration of 'shadeLast '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly int shadeLast = shadeMax - 1;

        public static sbyte shadeNormal = 52;
		
		// the light source vector
        public const float xLightsource = -1;
        public const float yLightsource = -1;
        public static float zLightsource = 2.5f;
		//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        public static float magnitudeLight = (float)System.Math.Sqrt(xLightsource * xLightsource + yLightsource * yLightsource + zLightsource * zLightsource);
		// the light source vector normalized
        public static float xLight = xLightsource / magnitudeLight;
        public static float yLight = yLightsource / magnitudeLight;
        public static float zLight = zLightsource / magnitudeLight;
		
		// the viewer vector is always 0,0,1
		
		// set specular on|off
		public static bool specularOn = true;
		// set specular 0-100
		public static float intensitySpecular = 0.22f;
		// set specpower -5
		public static int specularExponent = 6;
		// set specpower 0-100
		public static float intenseFraction = 0.4f;
		// set diffuse 0-100
		public static float intensityDiffuse = 0.84f;
		// set ambient 0-100
		public static float ambientFraction = 0.45f;
		
		public static int[] getShades(int rgb, bool greyScale)
		{
			int[] shades = new int[shadeMax];
			
			int red = (rgb >> 16) & 0xFF;
			int grn = (rgb >> 8) & 0xFF;
			int blu = rgb & 0xFF;
			
			float ambientRange = 1 - ambientFraction;
			
			shades[shadeNormal] = Shade3D.rgb(red, grn, blu);
			for (int i = 0; i < shadeNormal; ++i)
			{
				float fraction = ambientFraction + ambientRange * i / shadeNormal;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                shades[i] = Shade3D.rgb((int)(red * fraction + 0.5f), (int)(grn * fraction + 0.5f), (int)(blu * fraction + 0.5f));
			}
			
			int nSteps = shadeMax - shadeNormal - 1;
			float redRange = (255 - red) * intenseFraction;
			float grnRange = (255 - grn) * intenseFraction;
			float bluRange = (255 - blu) * intenseFraction;
			
			for (int i = 1; i <= nSteps; ++i)
			{
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                shades[shadeNormal + i] = Shade3D.rgb(red + (int)(redRange * i / nSteps + 0.5f), grn + (int)(grnRange * i / nSteps + 0.5f), blu + (int)(bluRange * i / nSteps + 0.5f));
			}
			if (greyScale)
				for (int i = shadeMax; --i >= 0; )
					shades[i] = NuGraphics3D.calcGreyscaleRgbFromRgb(shades[i]);
			return shades;
		}
		
		private static int rgb(int red, int grn, int blu)
		{
			return unchecked((int) 0xFF000000) | (red << 16) | (grn << 8) | blu;
		}
		
		public static System.String StringFromRgb(int rgb)
		{
			int red = (rgb >> 16) & 0xFF;
			int grn = (rgb >> 8) & 0xFF;
			int blu = rgb & 0xFF;
			
			return "[" + red + "," + grn + "," + blu + "]";
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'intensitySpecularSurfaceLimit '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly sbyte intensitySpecularSurfaceLimit = (sbyte) (shadeNormal + 4);
		
		public static sbyte calcIntensity(float x, float y, float z)
		{
			double magnitude = System.Math.Sqrt(x * x + y * y + z * z);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (sbyte) (calcFloatIntensityNormalized((float) (x / magnitude), (float) (y / magnitude), (float) (z / magnitude)) * shadeLast + 0.5f);
		}
		
		public static sbyte calcIntensityNormalized(float x, float y, float z)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (sbyte) (calcFloatIntensityNormalized(x, y, z) * shadeLast + 0.5f);
		}
		
		public static int calcFp8Intensity(float x, float y, float z)
		{
			double magnitude = System.Math.Sqrt(x * x + y * y + z * z);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (int) (calcFloatIntensityNormalized((float) (x / magnitude), (float) (y / magnitude), (float) (z / magnitude)) * shadeLast * (1 << 8));
		}
		
		public static float calcFloatIntensity(float x, float y, float z)
		{
			double magnitude = System.Math.Sqrt(x * x + y * y + z * z);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return calcFloatIntensityNormalized((float) (x / magnitude), (float) (y / magnitude), (float) (z / magnitude));
		}
		
		public static float calcFloatIntensityNormalized(float x, float y, float z)
		{
			float cosTheta = x * xLight + y * yLight + z * zLight;
			float intensity = 0; // ambient component
			if (cosTheta > 0)
			{
				intensity += cosTheta * intensityDiffuse; // diffuse component
				
				if (specularOn)
				{
					// this is the dot product of the reflection and the viewer
					// but the viewer only has a z component
					float dotProduct = z * 2 * cosTheta - zLight;
					if (dotProduct > 0)
					{
						for (int n = specularExponent; --n >= 0 && dotProduct > .0001f; )
							dotProduct *= dotProduct;
						// specular component
						intensity += dotProduct * intensitySpecular;
					}
				}
			}
			if (intensity > 1)
				return 1;
			return intensity;
		}
		
		public static sbyte calcDitheredNoisyIntensity(float x, float y, float z)
		{
			// add some randomness to prevent banding
			int fp8Intensity = calcFp8Intensity(x, y, z);
			int intensity = fp8Intensity >> 8;
			// this cannot overflow because the if the float intensity is 1.0
			// then intensity will be == shadeLast
			// but there will be no fractional component, so the next test will fail
			if ((fp8Intensity & 0xFF) > nextRandom8Bit())
				++intensity;
			int random16bit = seed & 0xFFFF;
			if (random16bit < 65536 / 3 && intensity > 0)
				--intensity;
			else if (random16bit > 65536 * 2 / 3 && intensity < shadeLast)
				++intensity;
			return (sbyte) intensity;
		}
		
		public static sbyte calcDitheredNoisyIntensity(float x, float y, float z, float r)
		{
			// add some randomness to prevent banding
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int fp8Intensity = (int) (calcFloatIntensityNormalized(x / r, y / r, z / r) * shadeLast * (1 << 8));
			int intensity = fp8Intensity >> 8;
			// this cannot overflow because the if the float intensity is 1.0
			// then intensity will be == shadeLast
			// but there will be no fractional component, so the next test will fail
			if ((fp8Intensity & 0xFF) > nextRandom8Bit())
				++intensity;
			int random16bit = seed & 0xFFFF;
			if (random16bit < 65536 / 3 && intensity > 0)
				--intensity;
			else if (random16bit > 65536 * 2 / 3 && intensity < shadeLast)
				++intensity;
			return (sbyte) intensity;
		}
		
		/*
		This is a linear congruential pseudorandom number generator,
		as defined by D. H. Lehmer and described by Donald E. Knuth in
		The Art of Computer Programming,
		Volume 2: Seminumerical Algorithms, section 3.2.1.
		
		static long seed = 1;
		static int nextRandom8Bit() {
		seed = (seed * 0x5DEECE66DL + 0xBL) & ((1L << 48) - 1);
		//    return (int)(seed >>> (48 - bits));
		return (int)(seed >>> 40);
		}
		*/
		
		// this doesn't really need to be synchronized
		// no serious harm done if two threads write seed at the same time
		private static int seed = 0x12345679; // turn lo bit on
		/// <summary><p>
		/// Implements RANDU algorithm for random noise in lighting/shading.
		/// </p>
		/// <p>
		/// RANDU is the classic example of a poor random number generator.
		/// But it is very cheap to calculate and is good enough for our purposes.
		/// </p>
		/// 
		/// </summary>
		/// <returns> Next random
		/// </returns>
		public static int nextRandom8Bit()
		{
			int t = seed;
			seed = t = ((t << 16) + (t << 1) + t) & 0x7FFFFFFF;
			return t >> 23;
		}
		
		public static void  dump()
		{
			System.Console.Out.WriteLine("\n ambientPercent=" + ambientFraction + "\n diffusePercent=" + intensityDiffuse + "\n specularOn=" + specularOn + "\n specularPercent=" + intensitySpecular + "\n specularPower=" + intenseFraction + "\n specularExponent=" + specularExponent + "\n zLightsource=" + zLightsource + "\n shadeNormal=" + shadeNormal);
		}
	}
}